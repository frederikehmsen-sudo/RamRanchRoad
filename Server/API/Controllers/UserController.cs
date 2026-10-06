using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Services;

namespace Api.Controllers;

[ApiController]
public class UserController(ListingService listingService, UserService userService) : ControllerBase
{
    private const int CurrentUserId = 1; // "ordinaryUser" until real login exists

    [HttpGet(nameof(GetMyListings))]
    public List<ListingResponse> GetMyListings()
        => listingService.GetByVendor(CurrentUserId);
    
    [HttpGet(nameof(GetMe))]
    public UserResponse GetMe()
        => userService.GetById(CurrentUserId);
}