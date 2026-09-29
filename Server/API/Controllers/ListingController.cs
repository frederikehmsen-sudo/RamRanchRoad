using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace Api.Controllers;

public class ListingController(ListingService service) : ControllerBase
{
    [HttpGet(nameof(GetListings))]
    public List<Listing> GetListings()
    {
        return service.GetListings();
    }
}