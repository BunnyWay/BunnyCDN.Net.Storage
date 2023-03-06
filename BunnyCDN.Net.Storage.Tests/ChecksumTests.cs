using Xunit;
using System.IO;
using System.Text;

namespace BunnyCDN.Net.Storage.Tests
{
    public class ChecksumTests
    {
        /*
            Hashes generated using: echo -n "<<Data>>" | shasum -a 256 | awk '{print toupper($1)}'
            Shasum version: 6.02
        */
        [Theory]
        [InlineData("Test",
            "532EAABD9574880DBF76B9B8CC00832C20A6EC113D682299550D7A6E0F345E25")]
        [InlineData("szdhjfkgdsjzhfgjsdfjzshfgjkskdjfzdjkhgj", 
            "48593515AEF42567E7FF4BF352D984A41F86D030CBF439A07D1A5C4FDD4F4179")]
        [InlineData("It'sTh3Way...TheBunnyW4y", 
            "48CF44BB1031DDF5F2FB2531AB1F10CAA1C49DC3EE6764D8180CD26A50C6DF1E")]
        public void Generate_ShouldGenerateCorrectHash(string input, string expectedOutput)
        {
            // Arrange
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));

            // Act
            var result = Checksum.Generate(stream);

            // Assert
            Assert.Equal(expectedOutput, result);
        }
    }
}