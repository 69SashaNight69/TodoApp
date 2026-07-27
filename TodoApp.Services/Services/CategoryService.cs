using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Core.Entities;
using TodoApp.Core.Interfaces;
using TodoApp.Services.DTOs;
using TodoApp.Services.Exceptions;
using TodoApp.Services.Interfaces;
using TodoApp.Services.Mapping;

namespace TodoApp.Services.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var categories = await _categoryRepository.GetByUserIdAsync(userId, cancellationToken);
            return categories.Select(c => c.ToDto()).ToList();
        }

        public async Task<CategoryDto> CreateCategoryAsync(Guid userId, CreateCategoryDto dto, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new BusinessValidationException("Назва категорії не може бути порожньою.");
            }

            var trimmedName = dto.Name.Trim();

            if (await _categoryRepository.ExistsAsync(userId, trimmedName, cancellationToken))
            {
                throw new BusinessValidationException($"Категорія з назвою '{trimmedName}' вже існує.");
            }

            var category = new Category
            {
                Name = trimmedName,
                UserId = userId
            };

            await _categoryRepository.AddAsync(category, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return category.ToDto();
        }

        public async Task DeleteCategoryAsync(Guid userId, Guid categoryId, CancellationToken cancellationToken = default)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);

            if (category == null)
            {
                throw new NotFoundException($"Категорію з ID {categoryId} не знайдено.");
            }

            if (category.UserId != userId)
            {
                throw new ForbiddenException("У вас немає прав для видалення цієї категорії.");
            }

            _categoryRepository.Remove(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}