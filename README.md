# BunnyCDN.Net.Storage
The official .NET library used for interacting with the BunnyCDN Storage API.

### How to use:

The storage library is very simple to use. First we need to create the basic BunnyCDNStorage object with the authentication details
```c#
var bunnyCDNStorage = new BunnyCDNStorage("storagezonename", "MyAccessKey");
```
- [Upload](#uploading-objects)
- [List](#listing-objects)
- [Download](#downloading-objects)
- [Delete](#deleting-objects)

<br/>

## Uploading objects:
Uploading supports either loading from a stream or reading directly from a local file path. If the path to the object does not exist yet, it will be automatically created.

**Uploading from a stream**
```c#
await bunnyCDNStorage.UploadAsync(stream, "/storagezonename/helloworld.txt");
```

**Uploading a local file**
```c#
await bunnyCDNStorage.UploadAsync("local/file/path/helloworld.txt", "/storagezonename/helloworld.txt");
```

<br/>

## Listing objects:
Get a list of of all objects on the given path.
```c#
var objects = await bunnyCDNStorage.GetStorageObjectsAsync("/storagezonename/");
```

The StorageObject contains the following properties:
- **Guid** - The unique GUID of the file
- **UserId** - The ID of the BunnyCDN user that holds the file
- **DateCreated** - The date when the file was created
- **LastChanged** - The date when the file was last modified
- **StorageZoneName** - The name of the storage zone to which the file is linked
- **Path** - The path to the object
- **ObjectName** - The name of the object
- **Length** - The total of the object in bytes
- **IsDirectory** - True if the object is a directory, otherwise false.
- **ServerId** - The ID of the storage server that the file resides on
- **StorageZoneId** - The ID of the storage zone that the object is linked to
- **FullPath** - The full path to the file


<br/>

## Downloading objects:
Downloading supports either loading into a stream or saving directly to a local file.

**Download as a stream**
```c#
await bunnyCDNStorage.DownloadObjectAsStreamAsync("/storagezonename/helloworld.txt");
```

**Download as a file**
```c#
await bunnyCDNStorage.DownloadObjectAsync("/storagezonename/helloworld.txt", "local/file/path/helloworld.txt");
```

<br/>

## Deleting objects:
Deleting supports both files and directories. If the target object is a directory, the directory content will also be deleted.
```c#
await bunnyCDNStorage.DeleteObjectAsync("/storagezonename/helloworld.txt");
```
