using Application.Interfaces.Management;
using Application.ViewModels.Order;
using Domain.Constants;
using Domain.Models.Auth;
using Domain.Models.Management;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShelkovyPut_Main.Controllers.Admin
{
    [Authorize(Roles = StaticUserRole.ADMIN)]
    public class AdminController : Controller
    {
        private readonly IOrderService _order;
        private readonly UserManager<Domain.Models.Auth.User> _userManager;
        private readonly ShelkobyPutDbContext _context;
        public AdminController(IOrderService order, UserManager<Domain.Models.Auth.User> userManager, ShelkobyPutDbContext context)
        {
            _order = order;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> AllOrders()
        {
            var orders = await _order.UserOrders(true);
            return View(orders);
        }

        public async Task<IActionResult> TogglePaymentStatus(int orderId)
        {
            try
            {
                await _order.TogglePaymentStatus(orderId);
            }
            catch (Exception ex)
            {
                // log exception here
            }
            return RedirectToAction(nameof(AllOrders));
        }

        public async Task<IActionResult> UpdateOrderStatus(int orderId)
        {
            var order = await _order.GetOrderById(orderId);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with id:{orderId} does not found.");
            }
            var orderStatusList = (await _order.GetOrderStatuses()).Select(orderStatus =>
            {
                return new SelectListItem { Value = orderStatus.Id.ToString(), Text = orderStatus.StatusName, Selected = order.OrderStatusId == orderStatus.Id };
            });
            var data = new UpdateOrderStatusVM
            {
                OrderId = orderId,
                OrderStatusId = order.OrderStatusId,
                OrderStatusList = orderStatusList
            };
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(UpdateOrderStatusVM data)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    data.OrderStatusList = (await _order.GetOrderStatuses()).Select(orderStatus =>
                    {
                        return new SelectListItem { Value = orderStatus.Id.ToString(), Text = orderStatus.StatusName, Selected = orderStatus.Id == data.OrderStatusId };
                    });

                    return View(data);
                }
                await _order.ChangeOrderStatus(data);
                TempData["msg"] = "Updated successfully";
            }
            catch (Exception ex)
            {
                // catch exception here
                TempData["msg"] = "Something went wrong";
            }
            return RedirectToAction(nameof(UpdateOrderStatus), new { orderId = data.OrderId });
        }

        [HttpGet]
        public async Task<IActionResult> AssignOrders()
        {
            var shippers = await _userManager.GetUsersInRoleAsync(StaticUserRole.STAFF);
            var orders = await _order.GetUnshippedOrdersAsync();
            orders.ForEach(o => o.AvailableShippers = shippers.ToList());
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> AssignOrders(List<UnshippedOrderVM> model)
        {
            await _order.AssignShipperToOrdersAsync(model);
            return RedirectToAction(nameof(AssignOrders));
        }


        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
