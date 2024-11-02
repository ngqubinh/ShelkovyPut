using Application.Interfaces.Management;
using Microsoft.AspNetCore.Mvc;

namespace ShelkovyPut_Main.Controllers.Management
{
    public class ShippingController : Controller
    {
        private readonly IShippingService _shipping;

        public ShippingController(IShippingService shipping)
        {
            _shipping = shipping;
        }

        public async Task<IActionResult> Shipping()
        {
            var shippings = await _shipping.OrdersForShipping();

            return View(shippings);
        }


    }
}
