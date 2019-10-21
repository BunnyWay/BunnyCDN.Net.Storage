using System;

namespace BunnyCDN.Net.Storage
{
    /// <summary>
    /// An exception thrown by BunnyCDNStorage
    /// </summary>
    public class BunnyCDNStorageException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the BunnyCDNStorageException class
        /// </summary>
        /// <param name="message"></param>
        public BunnyCDNStorageException(string message) : base(message)
        {
        }
    }
}
