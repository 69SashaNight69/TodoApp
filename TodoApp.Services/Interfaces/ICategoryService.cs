using TodoApp.Services.DTOs;

namespace TodoApp.Services.Interfaces;
public interface ICategoryService
{
    Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<CategoryDto> CreateCategoryAsync(Guid userId, CreateCategoryDto dto, CancellationToken cancellationToken = default);
    Task DeleteCategoryAsync(Guid userId, Guid categoryId, CancellationToken cancellationToken = default);
}
