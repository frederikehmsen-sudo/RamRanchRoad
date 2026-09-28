using LinqToDB.Mapping;

namespace Infrastructure.Entities;

public class Order
{
    [PrimaryKey] public int Id { get; set; }
    public int BuyerId { get; set; }
    public int ListingId { get; set; }
    public string ProductTitle { get; set; } = "";
    public int Quantity { get; set; }
    public decimal PricePaid { get; set; } 
    public DateTime CreatedAt { get; set; }
}