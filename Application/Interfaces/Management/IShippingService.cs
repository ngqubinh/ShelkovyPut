using Domain.Models.Management;

namespace Application.Interfaces.Management
{
    public interface IShippingService
    {
        Task<IEnumerable<Shipping>> GetAllShippings();
        Task<IEnumerable<Shipping>> GenerateShippingsFromOrders();
        Task<IEnumerable<Order>> OrdersForShipping();
    }
}
