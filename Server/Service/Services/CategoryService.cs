using Infrastructure;
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
}