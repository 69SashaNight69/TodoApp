using TodoApp.Core.Entities;

namespace TodoApp.Core.Interfaces;
public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Category>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<Category> AddAsync(Category category, CancellationToken cancellationToken = default);

    void Remove(Category category);
}

