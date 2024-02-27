using System.ComponentModel.DataAnnotations;

namespace Schemes.Models
{
    public class Customer: Base
    {
        [Key]
        public int CustomerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailId { get; set; }
        public string? PhoneNo { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int CustomerStatus {  get; set; }

    }
}
