using System.ComponentModel.DataAnnotations;
using Infrastructure;
using Infrastructure.Entities;
using LinqToDB;
using Service.DTOs;

namespace Service.Services;

public class ListingService(RamRanchDatabase db)
{
    
    public List<ListingResponse> GetListings()
    {
        return db.Listings
            .LoadWith(l => l.Vendor)
            .LoadWith(l => l.Category)
            .ToList()
            .Select(l => new ListingResponse(l)
            {
                VendorName = l.Vendor?.Username ?? "",
                CategoryName = l.Category?.Name ?? ""
            })
            .ToList();
    }

    public ListingResponse CreateListing(CreateListingRequest request, int vendorId)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ValidationException("Title is required");
        if (request.Price <= 0)
            throw new ValidationException("Price must be greater than 0");
        if (request.Stock < 0)
            throw new ValidationException("Stock cannot be negative");

        var category = db.Categories.FirstOrDefault(c => c.Id == request.CategoryId) ??
                       throw new ValidationException("That category doesn't exist");
        var vendor = db.Users.FirstOrDefault(u => u.Id == vendorId) ??
                     throw new ValidationException("That vendor doesn't exist");

        var listing = new Listing
        {
            Title = request.Title.Trim(),
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            CategoryId = category.Id,
            VendorId = vendor.Id
        };
        listing.ListingId = db.InsertWithInt32Identity(listing);

        return new ListingResponse(listing)
        {
            VendorName = vendor.Username,
            CategoryName = category.Name
        };
    }
}