using ASP_NET_07._TaskFlow_Introduction.Data;
using ASP_NET_07._TaskFlow_Introduction.Models;
using ASP_NET_07._TaskFlow_Introduction.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASP_NET_07._TaskFlow_Introduction.Services;

public class TaskItemService : ITaskItemService
{
    private readonly TaskFlowDbContext _context;

    public TaskItemService(TaskFlowDbContext context)
    {
        _context = context;
    }

    public async Task<TaskItem> CreateAsync(TaskItem taskItem)
    {
        var isProjectExists = await _context
                                        .Projects
                                        .AnyAsync(p=> p.Id == taskItem.ProjectId);

        if (!isProjectExists)
            throw new ArgumentException($"Project with ID {taskItem.ProjectId} not found");

        taskItem.CreatedAt = DateTime.UtcNow;
        taskItem.UpdatedAt = null;
        taskItem.Status = Models.TaskStatus.ToDo;

        _context.TaskItems.Add(taskItem);
        await _context.SaveChangesAsync();

        await _context
                    .Entry(taskItem)
                    .Reference(t => t.Project)
                    .LoadAsync();

        return taskItem;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _context.TaskItems.FindAsync(id);

        if (task is null) return false;

        _context.TaskItems.Remove(task);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await _context
                        .TaskItems
                        .Include(t=> t.Project)
                        .ToListAsync();
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context
            .TaskItems
            .Include(t => t.Project)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetByProjectIdAsync(int projectId)
    {
       return await _context
            .TaskItems
            .Include(t=>t.Project)
            .Where(t=> t.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<TaskItem?> UpdateAsync(int id, TaskItem taskItem)
    {
        var task = await _context
                                .TaskItems
                                .Include(t=>t.Project)
                                .FirstOrDefaultAsync(t=> t.Id == id);

        if (task is null) return null;

        task.Title = taskItem.Title;
        task.Description = taskItem.Description;
        task.Status = taskItem.Status;
        taskItem.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return task;
    }
}
