using System;
using System.Collections.Generic;
using System.Text;

namespace BunnyCDN.Net.Storage.Models
{
    public class StorageObject
    {
        /// <summary>
        /// The unique GUID of the file
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// The ID of the BunnyCDN user that holds the file
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// The date when the file was created
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// The date when the file was last modified
        /// </summary>
        public DateTime LastChanged { get; set; }
        /// <summary>
        /// The name of the storage zone to which the file is linked
        /// </summary>
        public string StorageZoneName { get; set; }
        /// <summary>
        /// The path to the object
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// The name of the object
        /// </summary>
        public string ObjectName { get; set; }
        /// <summary>
        /// The total of the object in bytes
        /// </summary>
        public long Length { get; set; }
        /// <summary>
        /// True if the object is a directory
        /// </summary>
        public bool IsDirectory { get; set; }
        /// <summary>
        /// The ID of the storage server that the file resides on
        /// </summary>
        public int ServerId { get; set; }
        /// <summary>
        /// The ID of the storage zone that the object is linked to
        /// </summary>
        public long StorageZoneId { get; set; }

        /// <summary>
        /// Gets the full path to the file
        /// </summary>
        public string FullPath => Path + ObjectName;
    }
}
