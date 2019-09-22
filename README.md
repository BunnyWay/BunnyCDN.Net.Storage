# BunnyCDN.Net.Storage
The official .NET library used for interacting with the BunnyCDN Storage API.

### How to use:

The storage library is very simple to use. First we need to create the basic BunnyCDNStorage object with the authentication details
```c#
var bunnyCDNStorage = new BunnyCDNStorage("storagezonename", "MyAccessKey");
```

### Uploading a file:
Uploading supports either loading from a stream or reading directly from a local file path.

**Uploading from a stream**
```c#
await bunnyCDNStorage.UploadAsync(stream, "/storagezonename/helloworld.txt");
```

**Uploading a local file**
```c#
await bunnyCDNStorage.UploadAsync("local/file/path/helloworld.txt", "/storagezonename/helloworld.txt");
```
