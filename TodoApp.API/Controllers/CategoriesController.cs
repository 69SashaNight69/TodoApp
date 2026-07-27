using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Services.DTOs;
using TodoApp.Services.Interfaces;

namespace TodoApp.API.Controllers;

[Authorize]
public class CategoriesController : BaseApiController
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var result = await _categoryService.GetCategoriesAsync(CurrentUserId, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryDto dto, CancellationToken cancellationToken)
    {
        var result = await _categoryService.CreateCategoryAsync(CurrentUserId, dto, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken cancellationToken)
    {
        await _categoryService.DeleteCategoryAsync(CurrentUserId, id, cancellationToken);
        return NoContent();
    }
}
