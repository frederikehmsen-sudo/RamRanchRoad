namespace Service.DTOs;

public class OrderResponse
{
    public int Id { get; set; }
    public int ListingId { get; set; }
    public string ProductTitle { get; set; } = "";
    public int Quantity { get; set; }
    public decimal PricePaid { get; set; }
    public DateTime CreatedAt { get; set; }
}