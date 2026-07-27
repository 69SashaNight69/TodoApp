using TodoApp.Core.Common;
using TodoApp.Core.Entities;

namespace TodoApp.Core.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TaskItem>> GetTasksAsync(Guid userId, TaskQueryParameters query, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Guid userId, TaskQueryParameters query, CancellationToken cancellationToken = default);
        Task<TaskItem> AddAsync(TaskItem task, CancellationToken cancellationToken = default);
        void Remove(TaskItem task);
    }
}