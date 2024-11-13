using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using OrderServiceApp.Models;

namespace OrderServiceApp.Services
{
    public class OrderService : IOrderService
    {
        private readonly Dictionary<string, Dictionary<string, int>> _coffees;
        private readonly string _inventoryUrl;
        private static readonly object _lock = new object();
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderService(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _coffees = new Dictionary<string, Dictionary<string, int>>
            {
                { "cappuccino", new Dictionary<string, int> { { "espressoShot", 1 }, { "milk", 200 }, { "milkFoam", 50 } } },
                { "americano", new Dictionary<string, int> { { "espressoShot", 1 }, { "hotWater", 150 } } }
            };
            _inventoryUrl = configuration["INVENTORY_URL"];
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> PlaceOrder(HttpRequest request)
        {
            string coffeeType = request.Query.ContainsKey("coffeeType") ? request.Query["coffeeType"].ToString() : "cappuccino";
            string quantityStr = request.Query.ContainsKey("quantity") ? request.Query["quantity"].ToString() : "1";

            if (!int.TryParse(quantityStr, out int quantity))
            {
                return new BadRequestObjectResult("Invalid quantity");
            }

            var ingredients = GetIngredients(coffeeType, quantity);
            bool available = await CheckInventory(ingredients);

            var order = new Order
            
            {
                CoffeeType = coffeeType,
                Quantity = quantity,
                Status = available ? "Confirmed" : "Out of Stock"
            };

            return new JsonResult(order);
        }

        private Dictionary<string, int> GetIngredients(string coffeeType, int quantity)
        {
            lock (_lock)
            {
                if (!_coffees.ContainsKey(coffeeType))
                {
                    throw new ArgumentException("Unknown coffee type");
                }

                var ingredientsOrg = _coffees[coffeeType];
                var ingredients = new Dictionary<string, int>();

                foreach (var (key, value) in ingredientsOrg)
                {
                    ingredients[key] = value * quantity;
                }

                return ingredients;
            }
        }

        private async Task<bool> CheckInventory(Dictionary<string, int> ingredients)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var content = new StringContent(JsonSerializer.Serialize(ingredients), Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{_inventoryUrl}/inventory/used", content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                bool available = JsonSerializer.Deserialize<bool>(responseBody);
                
                return available;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to check inventory: {ex.Message}");
                return false;
            }
        }
    }
}
