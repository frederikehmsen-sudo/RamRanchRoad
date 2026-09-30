using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Services;

namespace Api.Controllers;

public class ListingController(ListingService service) : ControllerBase
{
    private const int CurrentUserId = 1; // "ordinaryUser" from the seeder, until real login exists

    [HttpGet(nameof(GetListings))]
    public List<ListingResponse> GetListings()
    {
        return service.GetListings();
    }

    [HttpPost(nameof(CreateListing))]
    public ListingResponse CreateListing(CreateListingRequest request)
    {
        return service.CreateListing(request, CurrentUserId);
    }

    [HttpPut(nameof(UpdateListing))]
    public ListingResponse UpdateListing(UpdateListingRequest request)
    {
        return service.UpdateListing(request, CurrentUserId);
    }


    [HttpDelete(nameof(DeleteListings))]
    public void DeleteListings(int listingId)
    {
        service.DeleteListing(listingId, CurrentUserId);
    }
}