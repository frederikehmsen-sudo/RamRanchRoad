using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Services;

namespace Api.Controllers;

[ApiController]
public class CategoryController(CategoryService service) : ControllerBase
{
    [HttpGet(nameof(GetCategories))]
    public List<CategoryResponse> GetCategories() => service.GetCategories();

    [HttpPost(nameof(CreateCategory))]
    public CategoryResponse CreateCategory(CreateCategoryRequest request)
        => service.CreateCategory(request);

    [HttpDelete(nameof(DeleteCategory))]
    public void DeleteCategory(int categoryId)
        => service.DeleteCategory(categoryId);
}