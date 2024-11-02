using Domain.Models.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Management
{
    public class Shipping
    {
        [Key]
        public int Id { get; set; }
        public bool IsAccepted { get; set; }   
        public DateTime AcceptedDate { get; set; }

        // Relationship
        //public int AddressId { get; set; }
        //[ForeignKey(nameof(AddressId))]
        //public Address? Address { get; set; }
        public string? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order? Order { get; set; }
    }
}
