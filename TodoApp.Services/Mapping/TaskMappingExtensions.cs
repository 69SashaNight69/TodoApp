using TodoApp.Core.Entities;
using TodoApp.Services.DTOs.Task;

namespace TodoApp.Services.Mapping;

public static class TaskMappingExtensions
{
    public static TaskDto ToDto(this TaskItem task)
    {
        ArgumentNullException.ThrowIfNull(task);

        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            Category = task.Category?.ToDto()
        };
    }
}