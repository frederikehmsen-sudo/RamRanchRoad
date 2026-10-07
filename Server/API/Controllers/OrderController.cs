using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Services;

namespace Api.Controllers;

[ApiController]

public class OrderController(OrderService service, ICurrentUser currentUser) : ControllerBase
{
    [HttpPost(nameof(PlaceOrder))]
    public OrderResponse PlaceOrder(CreateOrderRequest request)
        => service.PlaceOrder(request, currentUser.Id);
}