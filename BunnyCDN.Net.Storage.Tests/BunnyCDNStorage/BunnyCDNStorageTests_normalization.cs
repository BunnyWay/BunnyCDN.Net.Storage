using Xunit;


namespace BunnyCDN.Net.Storage.Tests
{
    public partial class BunnyCDNStorageTests
    {
        [Fact]
        public void NormalizePath_valid()
        {
            var bunnyStorage = new BunnyCDNStorage("testingZone", string.Empty);
            var correctPath = "testingZone/validPath/testFile.txt";

            var result = bunnyStorage.NormalizePath(correctPath);
            Assert.Equal(correctPath, result);
        }

        [Fact]
        public void NormalizePath_stripPrefixingSlashes()
        {
            var bunnyStorage = new BunnyCDNStorage("testingZone", string.Empty);
            var correctPath = "///testingZone/validPath/testFile.txt";

            var result = bunnyStorage.NormalizePath(correctPath);
            Assert.Equal("testingZone/validPath/testFile.txt", result);
        }

        [Fact]
        public void NormalizePath_stripWhitespace()
        {
            var bunnyStorage = new BunnyCDNStorage("testingZone", string.Empty);
            var correctPath = "      testingZone/validPath/testFile.txt       ";

            var result = bunnyStorage.NormalizePath(correctPath);
            Assert.Equal("testingZone/validPath/testFile.txt", result);
        }

        [Theory]
        [InlineData("/directory/file.txt")]
        [InlineData("file.txt")]
        [InlineData("/invalidZone/directory/file.txt")]
        [InlineData("invalidZone/directory/file.txt")]
        public void NormalizePath_invalidZone(string path)
        {
            var bunnyStorage = new BunnyCDNStorage("testingZone", string.Empty);

            var exception = Assert.Throws<BunnyCDNStorageException>(() => bunnyStorage.NormalizePath(path));
            Assert.Equal("Path validation failed. File path must begin with /testingZone/.", exception.Message);
        }

        [Fact]
        public void NormalizePath_fixWindows()
        {
            var bunnyStorage = new BunnyCDNStorage("testingZone", string.Empty);
            string path = "testingZone\\windows\\path\\shouldbeRectified";

            var result = bunnyStorage.NormalizePath(path);

            Assert.Equal(path.Replace("\\", "/"), result);
        }

        [Fact]
        public void NormalizePath_fixExtraSlashes()
        {
            var bunnyStorage = new BunnyCDNStorage("testingZone", string.Empty);

            var result = bunnyStorage.NormalizePath("testingZone//directory///path/file.txt");
            Assert.Equal("testingZone/directory/path/file.txt", result);
        }

        [Theory]
        [InlineData("testingZone/directory/file/", "testingZone/directory/file/")]
        [InlineData("testingZone/directory/file//", "testingZone/directory/file/")]
        [InlineData("testingZone/directory/file/////", "testingZone/directory/file/")]
        [InlineData("testingZone/directory/file", "testingZone/directory/file/")]
        public void NormalizePath_path_directoryValidation_autoFix(string path, string expected)
        {
            var bunnyStorage = new BunnyCDNStorage("testingZone", string.Empty);

            var result = bunnyStorage.NormalizePath(path, true);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("testingZone/directory/file/")]
        [InlineData("testingZone/directory/file//")]
        [InlineData("testingZone/directory/file/////")]
        public void NormalizePath_path_directoryValidation_failFile(string path)
        {
            var bunnyStorage = new BunnyCDNStorage("testingZone", string.Empty);

            var exception = Assert.Throws<BunnyCDNStorageException>(() => bunnyStorage.NormalizePath(path, false));
            Assert.Equal("The requested path is invalid, cannot be directory.", exception.Message);
        }
    }
}