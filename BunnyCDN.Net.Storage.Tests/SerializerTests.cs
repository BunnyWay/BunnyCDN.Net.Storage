using System;
using Xunit;
using BunnyCDN.Net.Storage.Models;

namespace BunnyCDN.Net.Storage.Tests
{
    public class SerializerTests
    {
        [Fact]
        public void Deserialize_ShouldRetunCorrectObject()
        {
            // Arrange
            var input = "{\"Guid\":\"338d2080-bbbf-407f-b756-048c2eb0838f\",\"StorageZoneName\":\"testStorageZone\",\"Path\":\"/testStorageZone/\",\"ObjectName\":\"folderName\",\"Length\":0,\"LastChanged\":\"2022-11-14T09:34:37.747\",\"ServerId\":0,\"ArrayNumber\":0,\"IsDirectory\":true,\"UserId\":\"6fc9081f-43ce-475a-8215-ba2ebfd36d91\",\"ContentType\":\"\",\"DateCreated\":\"2022-11-14T09:34:37.747\",\"StorageZoneId\":1000009,\"Checksum\":null,\"ReplicatedZones\":null}";
            var expected = new StorageObject
            {
              Checksum = null,
              ContentType = "",
              Guid = Guid.Parse("338d2080-bbbf-407f-b756-048c2eb0838f"),
              StorageZoneName = "testStorageZone",
              Path = "/testStorageZone/",
              ObjectName = "folderName",
              Length = 0,
              LastChanged = DateTime.Parse("2022-11-14T09:34:37.747"),
              ServerId = 0,
              ArrayNumber = 0,
              IsDirectory = true,
              UserId = Guid.Parse("6fc9081f-43ce-475a-8215-ba2ebfd36d91"),
              DateCreated = DateTime.Parse("2022-11-14T09:34:37.747"),
              StorageZoneId = 1000009,
              ReplicatedZones = null
            };

            // Act
            var result = Serializer.Deserialize<StorageObject>(input);

            // Assert
            Assert.Equivalent(expected, result);
        }
    }
}