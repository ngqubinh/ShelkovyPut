using Domain.Models.Management;

namespace Application.ViewModels.User
{
    public class MyOrderVM
    {
        public int OrderId { get; set; }
        public DateTime CreatedDate { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public IEnumerable<OrderDetailVM>? OrderDetails { get; set; }
    }
}
