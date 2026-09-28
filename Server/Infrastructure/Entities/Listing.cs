using LinqToDB.Mapping;

namespace Infrastructure.Entities;

public class Listing
{
    [PrimaryKey] public string ListingId { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public int VendorId { get; set; }
    
    [Association(ThisKey = nameof(VendorId), OtherKey = nameof(User.Id))]
    public User Vendor { get; set; } = null!;
    
    [Association(ThisKey = nameof(CategoryId), OtherKey = nameof(Category.Id))]
    public Category Category { get; set; } = null!;
}