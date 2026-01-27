using ASP_NET_09._TaskFlow_AutoMapper.Models;

namespace ASP_NET_09._TaskFlow_AutoMapper.DTOs.Task_Items_DTOs;

public class UpdateTaskItemRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Models.TaskStatus Status { get; set; }
}
