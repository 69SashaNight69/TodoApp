namespace TodoApp.Services.DTOs.Task
{
    public class CreateTaskDto
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public DateTime? DueDate { get; set; }

        public Guid? CategoryId { get; set; }
    }
}