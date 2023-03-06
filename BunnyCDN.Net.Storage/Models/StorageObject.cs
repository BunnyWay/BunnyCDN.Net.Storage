using System;

namespace BunnyCDN.Net.Storage.Models
{
    public class StorageObject
    {
        /// <summary>
        /// The unique GUID of the file
        /// </summary>
        public Guid Guid { get; set; }
        /// <summary>
        /// The ID of the BunnyCDN user that holds the file
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// The date when the file was created
        /// </summary>
        public DateTime DateCreated { get; set; }
        /// <summary>
        /// Content type of the stoage object
        /// </summary>
        public string ContentType { get; set; }
        public int ArrayNumber { get; set; }
        /// <summary>
        /// File checksum for integrity validation
        /// </summary>
        public string Checksum { get; set; }

        /// <summary>
        /// The date when the file was last modified
        /// </summary>
        public DateTime LastChanged { get; set; }
        /// <summary>
        /// The name of the storage zone to which the file is linked
        /// </summary>
        public string StorageZoneName { get; set; }
        /// <summary>
        /// Replicated zone names
        /// </summary>
        public string ReplicatedZones { get; set; }
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
        public ulong Length { get; set; }
        /// <summary>
        /// True if the object is a directory
        /// </summary>
        public bool IsDirectory { get; set; }
        /// <summary>
        /// The ID of the storage server that the file resides on
        /// </summary>
        public uint ServerId { get; set; }
        /// <summary>
        /// The ID of the storage zone that the object is linked to
        /// </summary>
        public ulong StorageZoneId { get; set; }

        /// <summary>
        /// Gets the full path to the file
        /// </summary>
        public string FullPath => Path + ObjectName;
    }
}
