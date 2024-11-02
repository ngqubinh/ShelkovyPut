using Application.Interfaces.Management;
using Application.ViewModels.User;
using Infrastructure.Data;
using Infrastructure.Repositories.Management;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ShelkovyPut_Main.Controllers.User
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<Domain.Models.Auth.User> _userManager;
        private readonly ShelkobyPutDbContext _context;
        private readonly IOrderService _order;
        public UserController(UserManager<Domain.Models.Auth.User> userManager, ShelkobyPutDbContext context, IOrderService order)
        {
            _userManager = userManager;
            _context = context;
            _order = order;
        }

        [HttpGet]
        [Route("User/Dashboard")]
        public async Task<IActionResult> Dashboard(string filter)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            var orders = await _order.GetOrdersByFilter(userId, filter);

            var userDashboardVM = new UserDashboardVM
            {
                UserId = user.Id,
                Email = user.Email,
                Orders = orders
            };

            return View(userDashboardVM);

        }

        [HttpGet]
        [Route("User/Profile/{id}")]
        public async Task<IActionResult> Profile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("404 Profile page");
            }

            var viewModel = new ProfileVM()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.FullName,
                CreatedDate = user.CreatedDate,
            };

            return View(viewModel);
        }

        //[HttpGet]
        //[Route("User/MyOrders")]
        //public async Task<IActionResult> UserOrders()
        //{
        //    var userId = _userManager.GetUserId(User);
        //    var orders = await _context.Orders.Where(o => o.UserId == userId)
        //            .Include(o => o.OrderDetails).ThenInclude(o => o!.Product)
        //            .Include(o => o.OrderStatus).ToListAsync();

        //    var orderVM = orders.Select(o => new MyOrderVM()
        //    {
        //        OrderId = o.Id,
        //        CreatedDate = o.CreatedDate,
        //        OrderStatus = o.OrderStatus,
        //        OrderDetails = o.OrderDetails.Select(i => new OrderDetailVM()
        //        {
        //            ProductName = i.Product?.ProductName,
        //            Quantity = i.Quantity,
        //            TotalPrice = i.Product.ProductPrice * i.Quantity  
        //        }).ToList()
        //    }).ToList();

        //    return View(orderVM);
        //}

        [HttpGet]
        [Route("User/MyOrders")]
        public async Task<IActionResult> UserOrders(string filter)
        {
            var userId = _userManager.GetUserId(User);
            var orders = await _order.GetOrdersByFilter(userId, filter);
            return View(orders);
        }


        [HttpGet]
        [Route("User/Profile/Edit/{id}")]
        public async Task<IActionResult> EditProfile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("404 Profile page");
            }

            var viewModel = new ProfileVM()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.FullName,
                CreatedDate = user.CreatedDate,
            };

            return View(viewModel);
        }

        [HttpGet]
        [Route("User/Orders/Details/{id}")]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var orderDetails = await _order.GetOrderDetails(id);
            if (orderDetails == null)
            {
                return NotFound("No data");
            }
            return View(orderDetails);
        }
    }
} 
