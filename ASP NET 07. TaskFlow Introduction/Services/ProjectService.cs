using ASP_NET_07._TaskFlow_Introduction.Data;
using ASP_NET_07._TaskFlow_Introduction.Models;
using ASP_NET_07._TaskFlow_Introduction.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASP_NET_07._TaskFlow_Introduction.Services;

public class ProjectService : IProjectService
{

    private readonly TaskFlowDbContext _context;

    public ProjectService(TaskFlowDbContext context)
    {
        _context = context;
    }

    public async Task<Project> CreateAsync(Project project)
    {
        project.CreatedAt = DateTime.UtcNow;
        project.UpdatedAt = null!;

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        await _context
                    .Entry(project)
                    .Collection(p => p.Tasks)
                    .LoadAsync();
        return project;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await _context.Projects.FindAsync(id);

        if (project is null) return false;

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        return await _context
                            .Projects
                            .Include(p=>p.Tasks)
                            .ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        return await _context
                            .Projects
                            .Include(p => p.Tasks)
                            .FirstOrDefaultAsync(p=> p.Id == id);
    }

    public async Task<Project?> UpdateAsync(int id, Project project)
    {
       var updatedProject = await _context
                                    .Projects
                                    .Include(p=>p.Tasks)
                                    .FirstOrDefaultAsync(p=> p.Id == id);

        if (project is null) return null;

        updatedProject!.Name = project.Name;
        updatedProject.Description = project.Description;
        project.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return project;
    }
}
