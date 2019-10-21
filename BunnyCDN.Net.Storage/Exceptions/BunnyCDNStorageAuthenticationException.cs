namespace BunnyCDN.Net.Storage
{
    /// <summary>
    /// An exception thrown by BunnyCDNStorage caused by authentication failure
    /// </summary>
    public class BunnyCDNStorageAuthenticationException : BunnyCDNStorageException
    {
        /// <summary>
        /// Initialize a new instance of the BunnyCDNStorageAuthenticationException class
        /// </summary>
        /// <param name="path">The path that is not found</param>
        public BunnyCDNStorageAuthenticationException(string storageZoneName, string accessKey) : base($"Authentication failed for storage zone '{storageZoneName}' with access key '{accessKey}'.")
        {

        }
    }
}
