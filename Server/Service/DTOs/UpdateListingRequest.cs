namespace Service.DTOs;

public class UpdateListingRequest
{
    public int ListingIdForLookup { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
}