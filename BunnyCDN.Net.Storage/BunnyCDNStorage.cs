using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

#if NETSTANDARD2_0
using Newtonsoft.Json;
#else
using System.Text.Json;
#endif

using BunnyCDN.Net.Storage.Models;

namespace BunnyCDN.Net.Storage
{
    public class BunnyCDNStorage
    {
        /// <summary>
        /// The API access key used for authentication
        /// </summary>
        public string ApiAccessKey { get; private set; }

        /// <summary>
        /// The name of the storage zone we are working on
        /// </summary>
        public string StorageZoneName { get; private set; }

        /// <summary>
        /// The HTTP Client used for the API communication
        /// </summary>
        private HttpClient _http = null;

        /// <summary>
        /// Initializes a new instance of the BunnyCDNStorage class 
        /// </summary>
        /// <param name="storageZoneName">The name of the storage zone to connect to</param>
        /// <param name="apiAccessKey">The API key to authenticate with</param>
        public BunnyCDNStorage(string storageZoneName, string apiAccessKey)
        {
            this.ApiAccessKey = apiAccessKey;
            this.StorageZoneName = storageZoneName;

            // Initialize the HTTP Client
            _http = new HttpClient();
            _http.Timeout = new TimeSpan(0, 0, 120);
            _http.DefaultRequestHeaders.Add("AccessKey", this.ApiAccessKey);
            _http.BaseAddress = new Uri($"https://storage.bunnycdn.com/");
        }

        #region Delete
        /// <summary>
        /// Delete an object at the given path. If the object is a directory, the contents will also be deleted.
        /// </summary>
        public async Task DeleteObjectAsync(string path)
        {
            var normalizedPath = this.NormalizePath(path);
            try
            {
                await _http.DeleteAsync(normalizedPath);
            }
            catch(WebException ex)
            {
                throw MapResponseToException((HttpStatusCode)(int)ex.Status, path);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Get the list of storage objects on the given path
        /// </summary>
        public async Task<List<StorageObject>> GetStorageObjectsAsync(string path)
        {
            var normalizedPath = NormalizePath(path, true);
            var response = await _http.GetAsync(normalizedPath);

            if(response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
#if NETSTANDARD2_0
                return JsonConvert.DeserializeObject<List<StorageObject>>(responseJson);
#else
                return JsonSerializer.Deserialize<List<StorageObject>>(responseJson);
#endif
            }
            else
            {
                throw this.MapResponseToException(response.StatusCode, normalizedPath);
            }
        }
        #endregion

        #region Upload
        /// <summary>
        /// Upload an object from a stream
        /// </summary>
        public async Task UploadAsync(Stream stream, string path)
        {
            var normalizedPath = NormalizePath(path, false);
            using (var content = new StreamContent(stream))
            {
                var response = await _http.PutAsync(normalizedPath, content);
                if(!response.IsSuccessStatusCode)
                {
                    throw this.MapResponseToException(response.StatusCode, normalizedPath);
                }
            }
        }

        /// <summary>
        /// Upload a local file to the storage
        /// </summary>
        public async Task UploadAsync(string localFilePath, string path)
        {
            var normalizedPath = NormalizePath(path, false);
            using (var fileStream = new FileStream(localFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 64))
            {
                using (var content = new StreamContent(fileStream))
                {
                    var response = await _http.PutAsync(normalizedPath, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw this.MapResponseToException(response.StatusCode, normalizedPath);
                    }
                }
            }
        }
        #endregion

        #region Download
        /// <summary>
        /// Return a stream with the contents of the object
        /// </summary>
        /// <param name="path">path</param>
        /// <returns></returns>
        public async Task DownloadObject(string path, string localFilePath)
        {

            var normalizedPath = this.NormalizePath(path);
            try
            {
                using (var stream = await this.DownloadObjectAsStreamAsync(normalizedPath))
                {
                    // Create a buffered stream to speed up the download
                    using (var bufferedStream = new BufferedStream(stream, 1024 * 64))
                    {
                        using (var fileStream = new FileStream(localFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, 1024 * 64))
                        {
                            bufferedStream.CopyTo(fileStream, 1024 * 64);
                        }
                    }
                }
            }
            catch(WebException ex)
            {
                throw MapResponseToException((HttpStatusCode)(int)ex.Status, path);
            }
        }

        /// <summary>
        /// Return a stream with the contents of the object
        /// </summary>
        /// <param name="path">path</param>
        /// <returns></returns>
        public async Task<Stream> DownloadObjectAsStreamAsync(string path)
        {
            try
            {
                var normalizedPath = this.NormalizePath(path, false);
                return await _http.GetStreamAsync(normalizedPath);
            }
            catch (WebException ex)
            {
                throw MapResponseToException((HttpStatusCode)(int)ex.Status, path);
            }
        }
        #endregion

        #region Utils
        /// <summary>
        /// Map the API response to the correct BunnyCDNStorageExecption
        /// </summary>
        /// <param name="statusCode">The StatusCode returned by the API</param>
        /// <param name="path">The called path</param>
        private BunnyCDNStorageException MapResponseToException(HttpStatusCode statusCode, string path)
        {
            if (statusCode == HttpStatusCode.NotFound)
            {
                return new BunnyCDNStorageFileNotFoundException(path);
            }
            else if (statusCode == HttpStatusCode.Unauthorized)
            {
                return new BunnyCDNStorageAuthenticationException(this.StorageZoneName, this.ApiAccessKey);
            }
            else
            {
                return new BunnyCDNStorageException("An unknown error has occured during the request.");
            }
        }

        /// <summary>
        /// Normalize a path string
        /// </summary>
        /// <returns></returns>
        private string NormalizePath(string path, bool? isDirectory = null)
        {
            if (!path.StartsWith($"/{this.StorageZoneName}/") && !path.StartsWith($"{this.StorageZoneName}/"))
            {
                throw new BunnyCDNStorageException($"Path validation failed. File path must begin with /{this.StorageZoneName}/.");
            }

            path = path.Replace("\\", "/");
            if (isDirectory != null)
            {
                if (isDirectory.Value)
                {
                    if (!path.EndsWith("/"))
                    {
                        path = path + "/";
                    }
                }
                else
                {
                    if (path.EndsWith("/") && path != "/")
                    {
                        throw new BunnyCDNStorageException("The requested path is invalid.");
                    }
                }
            }
            while (path.Contains("//"))
            {
                path.Replace("//", "/");
            }
            if(path.StartsWith("/"))
            {
                path = path.Remove(0, 1);
            }

            return path;
        }
        #endregion
    }
}
