using ASP_NET_07._TaskFlow_Introduction.Models;
using ASP_NET_07._TaskFlow_Introduction.Services;
using ASP_NET_07._TaskFlow_Introduction.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP_NET_07._TaskFlow_Introduction.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaskItemsController : ControllerBase
{
    private readonly ITaskItemService _taskItemService;

    public TaskItemsController(ITaskItemService taskItemService)
    {
        _taskItemService = taskItemService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
    {
        var tasks = await _taskItemService.GetAllAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> GetById(int id)
    {
        var task = await _taskItemService.GetByIdAsync(id);
        if (task is null)
            return NotFound($"Tas kItem with ID {id} not found");
        return Ok(task);
    }

    [HttpGet("project/{projectId}")]
    public async Task<ActionResult<TaskItem>> GetByProjectId(int projectId)
    {
        return Ok();
    }
    [HttpPost]
    public async Task<ActionResult<TaskItem>> Create([FromBody] TaskItem taskItem)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var task = await _taskItemService.CreateAsync(taskItem);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }
        catch (ArgumentException ex)
        {

            return BadRequest(ex.Message);
        }

    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskItem>> Update(int id, [FromBody] TaskItem taskItem)
    {
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<TaskItem>> Delete(int id)
    {    
        return NoContent();
    }
}
