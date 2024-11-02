using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Management
{
    public class Demo
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
