using InventoryServiceApp.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace InventoryServiceApp.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly Dictionary<string, Ingredient> _stock;
        private readonly object _lock = new object();

        public InventoryService(IConfiguration configuration)
        {
            _stock = new Dictionary<string, Ingredient>
            {
                { "espressoShot", new Ingredient { Name = "Espresso Shot", Quantity = configuration.GetValue<int>("ESPRESSO_SHOT_QUANTITY", 10) } },
                { "milk", new Ingredient { Name = "Milk", Quantity = configuration.GetValue<int>("MILK_QUANTITY", 1000) } },
                { "milkFoam", new Ingredient { Name = "Milk Foam", Quantity = configuration.GetValue<int>("MILK_FOAM_QUANTITY", 500) } },
                { "hotWater", new Ingredient { Name = "Hot Water", Quantity = configuration.GetValue<int>("HOT_WATER_QUANTITY", 99999999) } }
            };
        }

        public Dictionary<string, Ingredient> GetStock()
        {
            lock (_lock)
            {
                return _stock.ToDictionary(entry => entry.Key, entry => new Ingredient { Name = entry.Value.Name, Quantity = entry.Value.Quantity });
            }
        }

        public bool UseIngredients(Dictionary<string, int> ingredients)
        {
            lock (_lock)
            {
                foreach (var ingredient in ingredients)
                {
                    if (!_stock.ContainsKey(ingredient.Key) || _stock[ingredient.Key].Quantity < ingredient.Value)
                    {
                        return false;
                    }
                }

                foreach (var ingredient in ingredients)
                {
                    _stock[ingredient.Key].Quantity -= ingredient.Value;
                }

                return true;
            }
        }
    }
}