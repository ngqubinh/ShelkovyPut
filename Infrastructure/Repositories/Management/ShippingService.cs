using Application.Interfaces.Management;
using Domain.Constants;
using Domain.Models.Management;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Management
{
    public class ShippingService : IShippingService
    {
        private readonly ShelkobyPutDbContext _context;

        public ShippingService(ShelkobyPutDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Shipping>> GetAllShippings()
        {
            return await _context.Shippings.Include(s => s.Order).Include(s => s.User).ToListAsync();            
        }

        public async Task<IEnumerable<Shipping>> GenerateShippingsFromOrders()
        {
            var orders = await _context.Orders.ToListAsync();
            var users = await _context.Users.ToListAsync();

            var shippings = orders.Select(order => new Shipping
            {
                OrderId = order.Id,
                Order = order,
                UserId = order.UserId, // Initial unassigned staff
                User = users.FirstOrDefault(u => u.Id == order.UserId),
                IsAccepted = false,
                AcceptedDate = DateTime.MinValue,
            }).ToList();

            // Save or Update the Shipping data
            foreach (var shipping in shippings)
            {
                var existingShipping = _context.Shippings.FirstOrDefault(s => s.OrderId == shipping.OrderId);
                if (existingShipping == null)
                {
                    _context.Shippings.Add(shipping);
                }
            }

            await _context.SaveChangesAsync();
            return shippings;
        }

        public async Task<IEnumerable<Order>> OrdersForShipping()
        {
            var orders = await _context.Orders
                                    .Include(o => o.OrderStatus)
                                    .Include(o => o.Addresses)
                                    .Where(o => o.OrderStatus.StatusName ==  StaticOrderStatus.ConfirmedOrder
                                        || o.OrderStatus.StatusName ==  StaticOrderStatus.PreparingOrder 
                                        || o.OrderStatus.StatusName == StaticOrderStatus.Pending 
                                        || o.OrderStatus.StatusName == StaticOrderStatus.Done)
                                    .ToListAsync();
            return orders;
        }
    }    
}
