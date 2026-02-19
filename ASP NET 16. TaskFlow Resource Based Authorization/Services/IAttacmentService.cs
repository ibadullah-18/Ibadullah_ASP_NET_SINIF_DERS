using ASP_NET_16._TaskFlow_Resource_Based_Authorization.DTOs;

namespace ASP_NET_16._TaskFlow_Resource_Based_Authorization.Services;

public interface IAttacmentService
{
    Task<AttachmentResponseDto?> UploadAsync(int taskId, Stream fileStream,string originalFileName,string contentType,long length,string userId,CancellationToken cancellationToken = default);

    Task<(Stream stream,string fileName, string contentType)?> GetDownloadAsync(int attachmentId, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int attachmentId, CancellationToken cancellationToken = default);

    Task<TaskAttachmentInfo?> GetTaskAttachmentInfoAsync(int attachmentId, CancellationToken cancellationToken = default);
}

public class TaskAttachmentInfo 
{ 
    public int Id { get; set; }
    public int TaskId { get; set; }
    public int ProjectId { get; set; }
    public string StoredFileName { get; set; } = string.Empty;
    public string StoregeKey { get; set; } = string.Empty;
    public string UploadedUserId { get; set; } = string.Empty;
}