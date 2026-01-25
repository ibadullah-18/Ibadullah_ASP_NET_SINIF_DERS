using ASP_NET_07._TaskFlow_Introduction.Models;
using ASP_NET_07._TaskFlow_Introduction.Services.Interfaces;
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
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetByProjectId(int projectId)
    {
        var tasks = await _taskItemService.GetByProjectIdAsync(projectId);
        return Ok(tasks);
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
        if(!ModelState.IsValid) 
            return BadRequest(ModelState);
        var task = await _taskItemService.UpdateAsync(id, taskItem);

        if (task is null)
            return NotFound($"Task Item with ID {id} not found");

        return Ok(task);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var isDeleted = await _taskItemService.DeleteAsync(id);

        if(!isDeleted) 
            return NotFound($"Task Item with ID {id} not found");

        return NoContent();
    }
}
