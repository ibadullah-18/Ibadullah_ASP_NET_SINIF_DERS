namespace ASP_NET_09._TaskFlow_AutoMapper.DTOs.Task_Items_DTOs;

public class CreateTaskItemRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int ProjectId { get; set; }
}
