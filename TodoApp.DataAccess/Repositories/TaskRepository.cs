using Microsoft.EntityFrameworkCore;
using TodoApp.Core.Common;
using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;

namespace TodoApp.DataAccess.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<TaskItem> AddAsync(TaskItem task, CancellationToken cancellationToken = default)
    {
        await _context.TaskItems.AddAsync(task, cancellationToken);
        return task;
    }

    public async Task<int> CountAsync(Guid userId, TaskQueryParameters query, CancellationToken cancellationToken = default)
    {
        var dbQuery = _context.TaskItems
                      .AsNoTracking()
                      .Where(t => t.UserId == userId);

        if (!string.IsNullOrEmpty(query.SearchTerm))
        {
            var search = query.SearchTerm.Trim().ToLower();
            dbQuery = dbQuery.Where(t => t.Title.ToLower().Contains(search) ||
                                    (t.Description != null && t.Description.ToLower().Contains(search)));
        }

        if (query.CategoryId.HasValue)
        {
            dbQuery = dbQuery.Where(t => t.CategoryId == query.CategoryId.Value);
        }

        return await dbQuery.CountAsync(cancellationToken);
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.TaskItems
                    .Include(t => t.Category)
                    .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<TaskItem>> GetTasksAsync(Guid userId, TaskQueryParameters query, CancellationToken cancellationToken = default)
    {
        var dbQuery = _context.TaskItems
                     .Include(t => t.Category)
                     .AsNoTracking()
                     .Where(t => t.UserId == userId);

        if(!string.IsNullOrEmpty(query.SearchTerm))
        {
            var search = query.SearchTerm.Trim().ToLower();
            dbQuery = dbQuery.Where(t => t.Title.ToLower().Contains(search) || 
                                              (t.Description != null && t.Description.ToLower().Contains(search)));
        }

        if (query.CategoryId.HasValue)
        {
            dbQuery = dbQuery.Where(t => t.CategoryId ==  query.CategoryId.Value);
        }

        return await dbQuery
                .OrderByDescending(t => t.CreatedAt)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);
    }

    public void Remove(TaskItem task)
    {
        _context.TaskItems.Remove(task);
    }
}

