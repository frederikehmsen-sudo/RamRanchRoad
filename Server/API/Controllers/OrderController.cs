using Microsoft.AspNetCore.Mvc;
using Service.DTOs;
using Service.Services;

namespace Api.Controllers;

public class OrderController(OrderService service) : ControllerBase
{
    private const int CurrentUserId = 1;

    [HttpPost(nameof(PlaceOrder))]
    public OrderResponse PlaceOrder(CreateOrderRequest request)
        => service.PlaceOrder(request, CurrentUserId);
}