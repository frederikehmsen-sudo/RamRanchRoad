using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Services;

namespace Api.Controllers;

public class CategoryController(CategoryService service) : ControllerBase
{
    [HttpGet(nameof(GetCategories))]
    public List<CategoryResponse> GetCategories()
    {
        return service.GetCategories();
    }
}