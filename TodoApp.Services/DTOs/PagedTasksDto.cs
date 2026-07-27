namespace TodoApp.Services.DTOs.Task
{
    public class PagedTasksDto
    {
        public IReadOnlyList<TaskDto> Items { get; set; } = [];

        public int TotalCount { get; set; }
    }
}