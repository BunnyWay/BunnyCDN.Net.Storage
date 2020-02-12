namespace BunnyCDN.Net.Storage
{
    public class BunnyCDNStorageChecksumException : BunnyCDNStorageException
    {
        /// <summary>
        /// Initialize a new instance of the BunnyCDNStorageAuthenticationException class
        /// </summary>
        /// <param name="path">The path that is not found</param>
        public BunnyCDNStorageChecksumException(string path, string checksum)
            : base($"Upload checksum verification failed for '{path}' using checksum '{checksum}'.") {}
    }
}