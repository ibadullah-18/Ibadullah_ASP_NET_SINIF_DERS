namespace ASP_NET_16._TaskFlow_Resource_Based_Authorization.Storage;

public interface IFileStorage
{
    Task<StoredFileInfo> UploadAsync(
        Stream stream,
        string originalFileName,
        string contentType,
        string folderKey,
        CancellationToken cancellation = default);
    Task<Stream> OpenReadAsync(
        string storageKey,
        CancellationToken cancellation = default);
    Task DeleteAsync(
        string storageKey,
        CancellationToken cancellation = default);

}
