using Microsoft.AspNetCore.Mvc;
using InventoryServiceApp.Services;
using InventoryServiceApp.Models;
namespace InventoryServiceApp.Controllers;

[ApiController]
[Route("[controller]")]
public class InventoryController : ControllerBase
{
  private readonly ILogger<InventoryController> _logger;
  private readonly IInventoryService _inventoryService;

  public InventoryController(ILogger<InventoryController> logger, IInventoryService inventoryService)
    {
        _logger = logger;
        _inventoryService = inventoryService;
    }

    
    [HttpGet("stock")]
    public ActionResult<Dictionary<string, Ingredient>> GetStock()
    {
        return Ok(_inventoryService.GetStock());
    }

    [HttpPost("used")]
    public ActionResult<bool> UseIngredients([FromBody] Dictionary<string, int> ingredients)
    {
        return Ok(_inventoryService.UseIngredients(ingredients));
    }
}