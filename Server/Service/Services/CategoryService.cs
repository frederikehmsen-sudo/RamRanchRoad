using System.ComponentModel.DataAnnotations;
using Infrastructure;
using Infrastructure.Entities;
using LinqToDB;
using Service.DTOs;

namespace Service.Services;

public class CategoryService(RamRanchDatabase db)
{
    public List<CategoryResponse> GetCategories()
    {
        return db.Categories
            .OrderBy(c => c.Name)
            .ToList()
            .Select(c => new CategoryResponse(c))
            .ToList();
    }

    public CategoryResponse CreateCategory(CreateCategoryRequest request)
    {
        var name = request.Name?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(name))
            throw new ValidationException("Category name is required");
        if (name.Length > 50)
            throw new ValidationException("Category name cannot be longer than 50 characters");

        var lower = name.ToLower();
        if (db.Categories.Any(c => c.Name.ToLower() == lower))
            throw new ValidationException($"A category named '{name}' already exists");

        var category = new Category { Name = name };
        category.Id = db.InsertWithInt32Identity(category);

        return new CategoryResponse(category);
    }

    public void DeleteCategory(int categoryId)
    {
        var category = db.Categories.FirstOrDefault(c => c.Id == categoryId) ??
                       throw new ValidationException("That category doesn't exist");

        if (db.Listings.Any(l => l.CategoryId == categoryId))
            throw new ValidationException(
                $"Cannot delete '{category.Name}' because it still has listings");

        db.Delete(category);
    }
}