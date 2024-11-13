using Microsoft.AspNetCore.Mvc;
using OrderServiceApp.Services;
namespace OrderServiceApp.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
  private readonly ILogger<OrderController> _logger;
  private readonly IOrderService _orderService;

  public OrderController(ILogger<OrderController> logger, IOrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }

    // Create a GET endpoint that just calls the OrderService's PlaceOrder method
    [HttpGet(Name = "PlaceOrder")]
    public async Task<ActionResult> PlaceOrder()
    {
      try
      {
        var order = await _orderService.PlaceOrder(Request);
        return Ok(order);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Failed to place order");
        return StatusCode(500);
      }
    }
}