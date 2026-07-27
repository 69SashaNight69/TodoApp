namespace TodoApp.Core.Entities;

public class Category
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public Guid UserId { get; set; }

    public User? User { get; set; }

    public ICollection<TaskItem> Tasks { get; set; } = [];
}
