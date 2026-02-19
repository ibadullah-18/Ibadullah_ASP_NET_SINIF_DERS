
namespace ASP_NET_16._TaskFlow_Resource_Based_Authorization.Storage;

public class LocalDiskStorage : IFileStorage
{
    private readonly string _basePath;
    private readonly ILogger<LocalDiskStorage> _logger;
    public LocalDiskStorage(IWebHostEnvironment env , ILogger<LocalDiskStorage> logger)
    {
        _basePath = Path.Combine(env.ContentRootPath,"Storage");
        _logger = logger;
    }

    public async Task<StoredFileInfo> UploadAsync(Stream stream, string originalFileName, string contentType, string folderKey, CancellationToken cancellation = default)
    {
        var ext = Path.GetExtension(originalFileName);

        if (string.IsNullOrEmpty(ext))
        {
            ext = ".bin";
        }

        var storageName = $"{Guid.NewGuid():N}{ext}";

        var relativePath = Path.Combine(folderKey, storageName);

        var fullPath = Path.Combine(_basePath, relativePath);

        var dir = Path.GetDirectoryName(fullPath);
        Directory.CreateDirectory(dir!);

        await using (var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
        {
            await stream.CopyToAsync(fs, cancellation);
        }

        var size = new FileInfo(fullPath).Length;

        _logger.LogInformation("File {OriginalFileName} is stored at {FullPath} with size {Size} bytes", originalFileName, fullPath, size);

        return new StoredFileInfo
        {
            StorageKey = relativePath.Replace('\\','/'),
            StoredFileName = storageName,
            Size = size
        };
    }
    public Task<Stream> OpenReadAsync(string storageKey, CancellationToken cancellation = default)
    {
        var fullPath = Path.Combine(_basePath, storageKey.Replace('/',Path.DirectorySeparatorChar));

        if (File.Exists(fullPath))
        {
            throw new FileNotFoundException("File not found", storageKey);
        }
        var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Task.FromResult<Stream>(stream);
    }
    public Task DeleteAsync(string storageKey, CancellationToken cancellation = default)
    {
        var fullPath = Path.Combine(_basePath, storageKey.Replace('/', Path.DirectorySeparatorChar));

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
            _logger.LogInformation(" deleted  file{Path}", fullPath);
        }
        return Task.CompletedTask;
    }


}
