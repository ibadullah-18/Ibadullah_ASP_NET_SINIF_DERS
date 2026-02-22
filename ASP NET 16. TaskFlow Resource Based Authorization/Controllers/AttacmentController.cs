using ASP_NET_16._TaskFlow_Resource_Based_Authorization.Common;
using ASP_NET_16._TaskFlow_Resource_Based_Authorization.DTOs;
using ASP_NET_16._TaskFlow_Resource_Based_Authorization.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASP_NET_16._TaskFlow_Resource_Based_Authorization.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize(Policy = "UserOrAbove")]
public class AttacmentController : ControllerBase
{
    private readonly IAttachmentService _attachmentService;
    private readonly IAuthorizationService _authorizationService;
    private readonly ITaskItemService _taskItemService;
    private readonly IProjectService _projectService;

    private string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    public AttacmentController(IAttachmentService attachmentService, IAuthorizationService authorizationService, ITaskItemService taskItemService, IProjectService projectService)
    {
        _attachmentService = attachmentService;
        _authorizationService = authorizationService;
        _taskItemService = taskItemService;
        _projectService = projectService;
    }

    [HttpPost("~/api/tasks/{taskId}/attacments")]
    public async Task<ActionResult<ApiResponse<AttachmentResponseDto>>> Upload(int taskId, IFormFile file,
        CancellationToken cancellationToken)
    {
        var task = await _taskItemService.GetTaskEntityAsync(taskId);
        if (task == null)
            return NotFound();
        var project = await _projectService.GetProjectEntityAsync(task.ProjectId);
        if (project == null)
            return NotFound();
        var authResult = await _authorizationService.AuthorizeAsync(User, project, "ProjectMemberOrHigher");
        if (!authResult.Succeeded)
            return Forbid();


        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        AttachmentResponseDto attachment;
        await using var stream = file.OpenReadStream();
        attachment = await _attachmentService.UploadAsync(taskId, stream, file.FileName, file.ContentType, file.Length, UserId!, cancellationToken);
        if (attachment == null)
            return NotFound();

        return Ok(ApiResponse<AttachmentResponseDto>.SuccessResponse(attachment, "File uploaded"));
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> Download(int id, CancellationToken cancellationToken)
    {
        var attachmentInfo = await _attachmentService.GetAttachmentInfoAsync(id, cancellationToken);
        if (attachmentInfo == null)
            return NotFound();
        var project = await _projectService.GetProjectEntityAsync(attachmentInfo.ProjectId);
        if (project == null)
            return NotFound();
        var authResult = await _authorizationService.AuthorizeAsync(User, project, "ProjectMemberOrHigher");
        if (!authResult.Succeeded)
            return Forbid();
        var result = await _attachmentService.GetDownloadAsync(id, cancellationToken);
        if (result == null)
            return NotFound();

        return File(result.Value.stream, result.Value.contentType, result.Value.fileName);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var attachmentInfo = await _attachmentService.GetAttachmentInfoAsync(id, cancellationToken);
        if (attachmentInfo == null)
            return NotFound();
        var project = await _projectService.GetProjectEntityAsync(attachmentInfo.ProjectId);
        if (project == null)
            return NotFound();
        var authResult = await _authorizationService.AuthorizeAsync(User, project, "ProjectOwnerOrAdmin");
        if (!authResult.Succeeded)
            return Forbid();
        var success = await _attachmentService.DeleteAsync(id, cancellationToken);
        if (!success)
            return NotFound();
        return NoContent();
    }
}