using System.ComponentModel.DataAnnotations;
using Infrastructure;
using Infrastructure.Entities;
using LinqToDB;
using Service.DTOs;
using Service.Services;
using Xunit;

namespace Service.UnitTests;

public class OrderServiceTest : IClassFixture<TestDatabaseFixture>
{
    private const int BuyerId = 1;
    private const int VendorId = 2;

    private readonly TestDatabaseFixture _fixture;

    public OrderServiceTest(TestDatabaseFixture fixture)
    {
        _fixture = fixture;
    }
    
    private static int AddListing(RamRanchDatabase db, decimal price = 10m, int stock = 10, int vendorId = VendorId)
        => db.InsertWithInt32Identity(new Listing
        {
            Title = "Fake Coke, 1g",
            Description = "Definitely not baking soda",
            Price = price,
            Stock = stock,
            CategoryId = 1,
            VendorId = vendorId
        });
    

    [Fact]
    public void PlaceOrder_ValidRequest_ReturnsResponseWithTotalPrice()
    {
        using var db = _fixture.CreateConnection();
        var listingId = AddListing(db, price: 20m, stock: 50);
        var service = new OrderService(db);
        
        var result = service.PlaceOrder(new CreateOrderRequest { ListingId = listingId, Quantity = 3 }, BuyerId);
        
        Assert.Equal(listingId, result.ListingId);
        Assert.Equal("Fake Coke, 1g", result.ProductTitle);
        Assert.Equal(3, result.Quantity);
        Assert.Equal(60m, result.PricePaid);
    }

    [Fact]
    public void PlaceOrder_ValidRequest_DecreasesStock()
    {
        using var db = _fixture.CreateConnection();
        var listingId = AddListing(db, stock: 10);
        var service = new OrderService(db);

        service.PlaceOrder(new CreateOrderRequest { ListingId = listingId, Quantity = 4 }, BuyerId);

        var updated = db.Listings.First(l => l.ListingId == listingId);
        Assert.Equal(6, updated.Stock);
    }

    [Fact]
    public void PlaceOrder_ValidRequest_SavesOrderWithBuyerAndVendor()
    {
        using var db = _fixture.CreateConnection();
        var listingId = AddListing(db);
        var service = new OrderService(db);

        var result = service.PlaceOrder(new CreateOrderRequest { ListingId = listingId, Quantity = 2 }, BuyerId);

        var saved = db.Orders.Single();
        Assert.Equal(result.Id, saved.Id);
        Assert.Equal(BuyerId, saved.BuyerId);
        Assert.Equal(VendorId, saved.VendorId);
        Assert.Equal(listingId, saved.ListingId);
    }

    [Fact]
    public void PlaceOrder_QuantityEqualsStock_Succeeds()
    {
        using var db = _fixture.CreateConnection();
        var listingId = AddListing(db, stock: 5);
        var service = new OrderService(db);

        service.PlaceOrder(new CreateOrderRequest { ListingId = listingId, Quantity = 5 }, BuyerId);

        Assert.Equal(0, db.Listings.First(l => l.ListingId == listingId).Stock);
    }


    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void PlaceOrder_QuantityBelowOne_Throws(int quantity)
    {
        using var db = _fixture.CreateConnection();
        var listingId = AddListing(db);
        var service = new OrderService(db);

        Assert.Throws<ValidationException>(() =>
            service.PlaceOrder(new CreateOrderRequest { ListingId = listingId, Quantity = quantity }, BuyerId));
    }

    [Fact]
    public void PlaceOrder_UnknownListing_Throws()
    {
        using var db = _fixture.CreateConnection();
        var service = new OrderService(db);

        Assert.Throws<ValidationException>(() =>
            service.PlaceOrder(new CreateOrderRequest { ListingId = 999, Quantity = 1 }, BuyerId));
    }

    [Fact]
    public void PlaceOrder_BuyingOwnListing_Throws()
    {
        using var db = _fixture.CreateConnection();
        var listingId = AddListing(db, vendorId: BuyerId);
        var service = new OrderService(db);

        Assert.Throws<ValidationException>(() =>
            service.PlaceOrder(new CreateOrderRequest { ListingId = listingId, Quantity = 1 }, BuyerId));
    }

    [Fact]
    public void PlaceOrder_QuantityAboveStock_ThrowsAndChangesNothing()
    {
        using var db = _fixture.CreateConnection();
        var listingId = AddListing(db, stock: 3);
        var service = new OrderService(db);

        Assert.Throws<ValidationException>(() =>
            service.PlaceOrder(new CreateOrderRequest { ListingId = listingId, Quantity = 4 }, BuyerId));

        Assert.Equal(3, db.Listings.First(l => l.ListingId == listingId).Stock);
        Assert.Empty(db.Orders);
    }
}