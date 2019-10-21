namespace BunnyCDN.Net.Storage
{
    /// <summary>
    /// An exception thrown by BunnyCDNStorage
    /// </summary>
    public class BunnyCDNStorageFileNotFoundException : BunnyCDNStorageException
    {
        /// <summary>
        /// Initialize a new instance of the BunnyCDNStorageFileNotFoundException class
        /// </summary>
        /// <param name="path">The path that is not found</param>
        public BunnyCDNStorageFileNotFoundException(string path) : base($"Could not find part of the object path: ${path}")
        {

        }
    }
}
