using Xunit.Abstractions;

namespace BunnyCDN.Net.Storage.Tests
{
    public partial class BunnyCDNStorageTests
    {
        private readonly ITestOutputHelper _output;

        public BunnyCDNStorageTests(ITestOutputHelper output)
        {
            this._output = output;
        }
    }
}