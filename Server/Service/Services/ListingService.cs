using System.ComponentModel.DataAnnotations;
using Infrastructure;
using Infrastructure.Entities;
using LinqToDB;
using Service.DTOs;

namespace Service.Services;

public class ListingService(RamRanchDatabase db)
{
    
    public List<ListingResponse> GetListings(int? categoryId = null)
    {
        IQueryable<Listing> query = db.Listings;

        if (categoryId.HasValue)
            query = query.Where(l => l.CategoryId == categoryId.Value);
        
        return query
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
    
    public ListingResponse UpdateListing(UpdateListingRequest request, int userId)
    {
        var listing = db.Listings.FirstOrDefault(l => l.ListingId == request.ListingIdForLookup) ??
                      throw new ValidationException("That listing doesn't exist");
        if (listing.VendorId != userId)
            throw new ValidationException("You can only change your own listings");

        if (string.IsNullOrWhiteSpace(request.Title))
            throw new ValidationException("Title is required");
        if (request.Price <= 0)
            throw new ValidationException("Price must be greater than 0");
        if (request.Stock < 0)
            throw new ValidationException("Stock cannot be negative");

        var category = db.Categories.FirstOrDefault(c => c.Id == request.CategoryId) ??
                       throw new ValidationException("That category doesn't exist");

        listing.Title = request.Title.Trim();
        listing.Description = request.Description;
        listing.Price = request.Price;
        listing.Stock = request.Stock;
        listing.CategoryId = category.Id;
        db.Update(listing);

        var vendor = db.Users.First(u => u.Id == userId);
        return new ListingResponse(listing)
        {
            VendorName = vendor.Username,
            CategoryName = category.Name
        };
    }
    
    public void DeleteListing(int listingId, int userId)
    {
        var listing = db.Listings.FirstOrDefault(l => l.ListingId == listingId) ??
                      throw new ValidationException("That listing doesn't exist");
        if (listing.VendorId != userId)
            throw new ValidationException("You can only change your own listings");

        db.Delete(listing);
    }
}