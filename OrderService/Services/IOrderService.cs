using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace OrderServiceApp.Services
{
    public interface IOrderService
    {
        Task<IActionResult> PlaceOrder(HttpRequest request);
    }
}