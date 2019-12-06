using BunnyCDN.Net.Storage.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

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
            _http.BaseAddress = new Uri("https://storage.bunnycdn.com/");
        }

        #region Delete
        /// <summary>
        /// Delete an object at the given path. If the object is a directory, the contents will also be deleted.
        /// </summary>
        /// <param name="path">Path to delete</param>
        /// <returns>Deletion success</returns>
        public async Task<bool> DeleteObjectAsync(string path)
        {
            var normalizedPath = NormalizePath(path);
            try
            {
                var response = await _http.DeleteAsync(normalizedPath);
                return response.IsSuccessStatusCode;
            }
            catch(WebException ex)
            {
                throw this.MapResponseToException((HttpStatusCode)(int)ex.Status, path);
            }
        }
        #endregion

        #region List
        /// <summary>
        /// Get the list of storage objects on the given path
        /// </summary>
        /// <param name="path">Path to retrieve objects from</param>
        /// <returns>Path's storage objects</returns>
        public async Task<List<StorageObject>> GetStorageObjectsAsync(string path)
        {
            var normalizedPath = this.NormalizePath(path, true);
            var response = await _http.GetAsync(normalizedPath);

            if(response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<StorageObject>>(responseJson);
            }
            else
            {
                throw this.MapResponseToException(response.StatusCode, normalizedPath);
            }
        }
        #endregion

        #region Upload
        /// <summary>
        /// Upload an object from a stream (missing path will be created)
        /// </summary>
        /// <param name="stream">Stream containing file contents</param>
        /// <param name="path">Destination path</param>
        public async Task UploadAsync(Stream stream, string path)
        {
            var normalizedPath = this.NormalizePath(path, false);
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
        /// <param name="localFilePath">Local path of file to upload</param>
        /// <param name="path">Destination path</param>
        public async Task UploadAsync(string localFilePath, string path)
        {
            var normalizedPath = this.NormalizePath(path, false);
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
        /// Download the object to a local file
        /// </summary>
        /// <param name="path">Source path to download from</param>
        /// <param name="localFilePath">Local path to download file to</param>
        public async Task DownloadObjectAsync(string path, string localFilePath)
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
                throw this.MapResponseToException((HttpStatusCode)(int)ex.Status, path);
            }
        }

        /// <summary>
        /// Return a stream with the contents of the object
        /// </summary>
        /// <param name="path">Source path to download from</param>
        /// <returns>Stream containing the file contents</returns>
        public async Task<Stream> DownloadObjectAsStreamAsync(string path)
        {
            try
            {
                var normalizedPath = this.NormalizePath(path, false);
                return await _http.GetStreamAsync(normalizedPath);
            }
            catch (WebException ex)
            {
                throw this.MapResponseToException((HttpStatusCode)(int)ex.Status, path);
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
            switch (statusCode)
            {
                case HttpStatusCode.NotFound:
                    return new BunnyCDNStorageFileNotFoundException(path);
                case HttpStatusCode.Unauthorized:
                    return new BunnyCDNStorageAuthenticationException(StorageZoneName, ApiAccessKey);
                default:
                    return new BunnyCDNStorageException("An unknown error has occured during the request.");
            }
        }

        /// <summary>
        /// Normalize a path string
        /// </summary>
        /// <returns>Recognizable, valid string for use against API calls</returns>
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
            // Remove double slashes
            while (path.Contains("//"))
            {
                path.Replace("//", "/");
            }
            // Remove the starting slash
            if(path.StartsWith("/"))
            {
                path = path.Remove(0, 1);
            }

            return path;
        }
        #endregion
    }
}
