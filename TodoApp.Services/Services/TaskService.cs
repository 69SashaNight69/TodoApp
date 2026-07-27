using TodoApp.Core.Common;
using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;
using TodoApp.Services.DTOs.Task;
using TodoApp.Services.Exceptions;
using TodoApp.Services.Interfaces;
using TodoApp.Services.Mapping;

namespace TodoApp.Services.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TaskService(
            ITaskRepository taskRepository,
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TaskDto> GetByIdAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default)
        {
            var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken);

            if (task == null)
            {
                throw new NotFoundException("Завдання не знайдено.");
            }

            if (task.UserId != userId)
            {
                throw new ForbiddenException("У вас немає доступу до цього завдання.");
            }

            return task.ToDto();
        }

        public async Task<PagedTasksDto> GetTasksAsync(Guid userId, TaskQueryParameters query, CancellationToken cancellationToken = default)
        {
            var tasks = await _taskRepository.GetTasksAsync(userId, query, cancellationToken);
            var totalCount = await _taskRepository.CountAsync(userId, query, cancellationToken);

            return new PagedTasksDto
            {
                Items = tasks.Select(t => t.ToDto()).ToList(),
                TotalCount = totalCount
            };
        }

        public async Task<TaskDto> CreateTaskAsync(Guid userId, CreateTaskDto dto, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new BusinessValidationException("Назва завдання не може бути порожньою.");
            }

            Category? category = null;
            if (dto.CategoryId.HasValue)
            {
                category = await _categoryRepository.GetByIdAsync(dto.CategoryId.Value, cancellationToken);

                if (category == null || category.UserId != userId)
                {
                    throw new BusinessValidationException("Вказану категорію не знайдено або доступ заборонено.");
                }
            }

            var task = new TaskItem
            {
                Title = dto.Title.Trim(),
                Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim(),
                DueDate = dto.DueDate,
                CategoryId = dto.CategoryId,
                Category = category,
                UserId = userId,
                IsCompleted = false
            };

            await _taskRepository.AddAsync(task, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return task.ToDto();
        }

        public async Task<TaskDto> UpdateTaskAsync(Guid userId, Guid taskId, UpdateTaskDto dto, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Title))
            {
                throw new BusinessValidationException("Назва завдання не може бути порожньою.");
            }

            var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken);
            if (task == null)
            {
                throw new NotFoundException("Завдання не знайдено.");
            }

            if (task.UserId != userId)
            {
                throw new ForbiddenException("У вас немає прав для редагування цього завдання.");
            }

            Category? category = null;
            if (dto.CategoryId.HasValue)
            {
                category = await _categoryRepository.GetByIdAsync(dto.CategoryId.Value, cancellationToken);
                if (category == null || category.UserId != userId)
                {
                    throw new BusinessValidationException("Вказану категорію не знайдено або доступ заборонено.");
                }
            }

            task.Title = dto.Title.Trim();
            task.Description = string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description.Trim();
            task.IsCompleted = dto.IsCompleted;
            task.DueDate = dto.DueDate;
            task.CategoryId = dto.CategoryId;
            task.Category = category;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return task.ToDto();
        }

        public async Task DeleteTaskAsync(Guid userId, Guid taskId, CancellationToken cancellationToken = default)
        {
            var task = await _taskRepository.GetByIdAsync(taskId, cancellationToken);
            if (task == null)
            {
                throw new NotFoundException("Завдання не знайдено.");
            }

            if (task.UserId != userId)
            {
                throw new ForbiddenException("У вас немає прав для видалення цього завдання.");
            }

            _taskRepository.Remove(task);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}