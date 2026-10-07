using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Services;

namespace Api.Controllers;

[ApiController]
public class UserController(ListingService listingService, UserService userService, ICurrentUser currentUser) : ControllerBase
{

    [HttpGet(nameof(GetMyListings))]
    public List<ListingResponse> GetMyListings()
        => listingService.GetByVendor(currentUser.Id);
    
    [HttpGet(nameof(GetMe))]
    public UserResponse GetMe()
        => userService.GetById(currentUser.Id);
}