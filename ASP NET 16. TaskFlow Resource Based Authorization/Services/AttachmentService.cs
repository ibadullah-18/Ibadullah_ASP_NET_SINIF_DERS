using ASP_NET_16._TaskFlow_Resource_Based_Authorization.Data;
using ASP_NET_16._TaskFlow_Resource_Based_Authorization.DTOs;
using ASP_NET_16._TaskFlow_Resource_Based_Authorization.Models;
using ASP_NET_16._TaskFlow_Resource_Based_Authorization.Storage;
using Microsoft.EntityFrameworkCore;

namespace ASP_NET_16._TaskFlow_Resource_Based_Authorization.Services;

public class AttachmentService : IAttacmentService
{
    public const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB
    public static readonly string[] AllowedExtensions =  { ".jpg", ".jpeg", ".png", ".pdf", ".txt", ".zip" };
    public static readonly string[] AllowedContextType =  { "image/jpeg", "image/png", "application/pdf", "text/plain", "application/zip","application/x-zip-compressed" };
    
    private readonly TaskFlowDbContext _context;
    private readonly IFileStorage _Storage;

    public AttachmentService(TaskFlowDbContext context, IFileStorage storage)
    {
        _context = context;
        _Storage = storage;
    }


    public async Task<AttachmentResponseDto?> UploadAsync(int taskId, Stream fileStream, string originalFileName, string contentType, long length, string userId, CancellationToken cancellationToken = default)
    {
        if (length > MaxFileSizeBytes)
        {
            throw new ArgumentException($"File size exceeds the maximum allowed size of {MaxFileSizeBytes} bytes.");

        }

        var ext = Path.GetExtension(originalFileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(ext) || !AllowedExtensions.Contains(ext))
        {
            throw new ArgumentException($"File type {ext} is not allowed. Allowed types are: {string.Join(", ", AllowedExtensions)}");
        }
        if (!AllowedContextType.Contains(contentType, StringComparer.OrdinalIgnoreCase))
        {
            throw new ArgumentException($"Content type {contentType} is not allowed. Allowed types are: {string.Join(", ", AllowedContextType)}");
        }

        var task = await _context.TaskItems.FindAsync( [ taskId ], cancellationToken);

        if (task is null) return null;

        var folderKey = $"task/{taskId}";
        var info = await _Storage.UploadAsync(fileStream, originalFileName, contentType, folderKey, cancellationToken);
        var attachment = new TaskAttachment
        {
            TaskItemId = taskId,
            OriginalFileName = originalFileName,
            StoredFileName = info.StoredFileName,
            ContentType = contentType,
            Size = info.Size,
            UploadedUserId = userId,
            UploadedAt = DateTimeOffset.UtcNow
        };

        _context.TaskAttachments.Add(attachment);
        await _context.SaveChangesAsync(cancellationToken);
        return new AttachmentResponseDto
        {
            Id = attachment.Id,
            TaskItemId = attachment.TaskItemId,
            OriginalFileName = attachment.OriginalFileName,
            ContentType = attachment.ContentType,
            Size = attachment.Size,
            UploadedAt = attachment.UploadedAt
        };
    }

    public async Task<(Stream stream, string fileName, string contentType)?> GetDownloadAsync(int attachmentId, CancellationToken cancellationToken = default)
    {
        var att =await _context.TaskAttachments.FirstOrDefaultAsync(a => a.Id == attachmentId, cancellationToken);
        if (att is null) return null;

        var key = $"tasks/{att.TaskItemId}/{att.StoredFileName}";

        var stream = await _Storage.OpenReadAsync(key, cancellationToken);
        return (stream, att.OriginalFileName, att.ContentType);
    }
    
    public Task<TaskAttachmentInfo> GetTaskAttachmentInfoAsync(int attachmentId, CancellationToken cancellationToken = default)
    {
        var att = _context.TaskAttachments.Include(a => a.TaskItem).FirstOrDefaultAsync(a => a.Id == attachmentId);

        if (att is null) return Task.FromResult<TaskAttachmentInfo>(null);

        return Task.FromResult(new TaskAttachmentInfo
        {
            Id = att.Id,
            TaskId = att.TaskItemId,
            ProjectId = att.TaskItem.ProjectId,
            StoredFileName = att.StoredFileName,
            StoregeKey = $"tasks/{att.TaskItemId}/{att.StoredFileName}",
            UploadedUserId = att.UploadedUserId
        });
    }
    
    public Task<bool> DeleteAsync(int attachmentId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }



}
