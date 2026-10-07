using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Services;

namespace Api.Controllers;

[ApiController]
public class ListingController(ListingService service, ICurrentUser currentUser) : ControllerBase
{

    [HttpGet(nameof(GetListings))]
    public List<ListingResponse> GetListings([FromQuery] int? categoryId)
    {
        return service.GetListings(categoryId);
    }

    [HttpPost(nameof(CreateListing))]
    public ListingResponse CreateListing(CreateListingRequest request)
    {
        return service.CreateListing(request, currentUser.Id);
    }

    [HttpPut(nameof(UpdateListing))]
    public ListingResponse UpdateListing(UpdateListingRequest request)
    {
        return service.UpdateListing(request, currentUser.Id);
    }


    [HttpDelete(nameof(DeleteListings))]
    public void DeleteListings(int listingId)
    {
        service.DeleteListing(listingId, currentUser.Id);
    }
}