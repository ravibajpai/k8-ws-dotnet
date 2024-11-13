using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using InventoryServiceApp.Models;

namespace InventoryServiceApp.Services
{
    public interface IInventoryService
    {
        bool UseIngredients(Dictionary<string, int> ingredients);
        Dictionary<string, Ingredient> GetStock();

    }
}