using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;

namespace TodoApp.DataAccess.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _context.Categories.AddAsync(category, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid userId, string name, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
        .AnyAsync(c => c.UserId == userId && c.Name == name, cancellationToken);
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
                     .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Category>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Categories
                     .AsNoTracking()
                     .Where(c => c.UserId == userId)
                     .ToListAsync(cancellationToken);
    }

    public void Remove(Category category)
    {
        _context.Categories.Remove(category);
    }
}
