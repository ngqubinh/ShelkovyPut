﻿using Application.DTOs.Request.Management;
using Application.Interfaces.Management;
using Application.ViewModels;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ShelkovyPut_Main.Controllers.Management
{
    public class CartController : Controller
    {
        private readonly ShelkobyPutDbContext _context;
        private readonly ICartService _cart;

        public CartController(ShelkobyPutDbContext context, ICartService cart)
        {
            _context = context;
            _cart = cart;
        }       

        public async Task<ActionResult> AddItem(int productId, int qty = 1, int redirect = 0)
        {
            var cartCount = await _cart.AddItem(productId, qty);
            if (redirect == 0)
            {
                return Ok(cartCount);
            }
            return RedirectToAction(nameof(GetUserCart));
        }

        public async Task<ActionResult> RemoveItem(int productId)
        {
            var cartCount = await _cart.RemoveItem(productId);
            return RedirectToAction(nameof(GetUserCart));
        }

        [HttpGet]
        [Route("Cart/UserCart")]
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cart.GetUserCart();
            return View(cart);
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItems = await _cart.GetCartItemCount();
            return Ok(cartItems);
        }

        public IActionResult Checkout()
        {           
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Checkout(CheckoutRequest model)
        {
            if (!ModelState.IsValid)
            {               
                return View(model);
            }            

            bool isCheckedOut = await _cart.DoCheckout(model);
            if (!isCheckedOut)
            {
                return RedirectToAction(nameof(OrderFailure));
            }

            return RedirectToAction(nameof(OrderSuccess));
        }
        public IActionResult OrderSuccess()
        {
            return View();
        }

        public IActionResult OrderFailure()
        {
            return View();
        }
    }
}
