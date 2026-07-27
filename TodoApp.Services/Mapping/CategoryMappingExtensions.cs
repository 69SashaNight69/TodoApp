using System;
using TodoApp.Core.Entities;
using TodoApp.Services.DTOs;

namespace TodoApp.Services.Mapping;
public static class CategoryMappingExtensions
{
    public static CategoryDto ToDto(this Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };
    }
}
