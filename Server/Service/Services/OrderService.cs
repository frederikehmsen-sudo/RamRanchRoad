using System.ComponentModel.DataAnnotations;
using Infrastructure;
using Infrastructure.Entities;
using LinqToDB;
using Service.DTOs;

namespace Service.Services;

public class OrderService(RamRanchDatabase db)
{
    public OrderResponse PlaceOrder(CreateOrderRequest request, int buyerId)
    {
        if (request.Quantity < 1)
            throw new ValidationException("Quantity must be at least 1");

        var listing = db.Listings.FirstOrDefault(l => l.ListingId == request.ListingId)
                      ?? throw new ValidationException("That listing doesn't exist");

        if (listing.VendorId == buyerId)
            throw new ValidationException("You cannot buy your own listing");
        if (request.Quantity > listing.Stock)
            throw new ValidationException("Not enough stock");

        using var tx = db.BeginTransaction();

        var updated = db.Listings
            .Where(l => l.ListingId == listing.ListingId && l.Stock >= request.Quantity)
            .Set(l => l.Stock, l => l.Stock - request.Quantity)
            .Update();
        if (updated == 0)
            throw new ValidationException("Not enough stock");

        var order = new Order
        {
            BuyerId = buyerId,
            VendorId = listing.VendorId,
            ListingId = listing.ListingId,
            ProductTitle = listing.Title,
            Quantity = request.Quantity,
            PricePaid = listing.Price * request.Quantity,
            CreatedAt = DateTime.UtcNow
        };
        order.Id = db.InsertWithInt32Identity(order);
        
        tx.Commit();

        return new OrderResponse()
        {
            Id = order.Id, ListingId = order.ListingId, ProductTitle = order.ProductTitle, Quantity = order.Quantity,
            PricePaid = order.PricePaid, CreatedAt = order.CreatedAt
        };  
    }
}