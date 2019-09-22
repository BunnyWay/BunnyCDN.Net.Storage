# BunnyCDN.Net.Storage
The official .NET library used for interacting with the BunnyCDN Storage API.

### How to use:

The storage library is very simple to use. First we need to create the basic BunnyCDNStorage object with the authentication details
```c#
var bunnyCDNStorage = new BunnyCDNStorage("storagezonename", "MyAccessKey");
```
- [Upload](#uploading-an-object)
- [List Objects](#listing-objects)
- [Delete](#deleting-an-object)

### Uploading an object:
Uploading supports either loading from a stream or reading directly from a local file path. If the path to the object does not exist yet, it will be automatically created.

**Uploading from a stream**
```c#
await bunnyCDNStorage.UploadAsync(stream, "/storagezonename/helloworld.txt");
```

**Uploading a local file**
```c#
await bunnyCDNStorage.UploadAsync("local/file/path/helloworld.txt", "/storagezonename/helloworld.txt");
```

### Listing objects:
Deleting supports both files and directories. If the target object is a directory, the directory content will also be deleted.
```c#
await bunnyCDNStorage.GetStorageObjectsAsync("/storagezonename/");
```

### Deleting an object:
Deleting supports both files and directories. If the target object is a directory, the directory content will also be deleted.
```c#
await bunnyCDNStorage.DeleteObjectAsync("/storagezonename/helloworld.txt");
```
