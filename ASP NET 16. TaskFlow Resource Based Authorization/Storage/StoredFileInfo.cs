namespace ASP_NET_16._TaskFlow_Resource_Based_Authorization.Storage;

public class StoredFileInfo
{
    public string StorageKey { get; set; } = string.Empty;
    public string StoredFileName { get; set; } = string.Empty;
    public long Size { get; set; }
}
