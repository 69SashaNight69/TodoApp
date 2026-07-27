using TodoApp.Core.Common;
using TodoApp.Services.DTOs.Task;

namespace TodoApp.Services.Interfaces
{
    public interface ITaskService
    {
        Task<TaskDto> GetByIdAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default);

        Task<PagedTasksDto> GetTasksAsync(Guid userId, TaskQueryParameters query, CancellationToken cancellationToken = default);

        Task<TaskDto> CreateTaskAsync(Guid userId, CreateTaskDto dto, CancellationToken cancellationToken = default);

        Task<TaskDto> UpdateTaskAsync(Guid userId, Guid taskId, UpdateTaskDto dto, CancellationToken cancellationToken = default);

        Task DeleteTaskAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default);
    }
}