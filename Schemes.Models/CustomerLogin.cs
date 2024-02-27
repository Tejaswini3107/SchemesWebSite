using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schemes.Models
{
    public class CustomerLogin: Base
    {
        [Key]
        public int CustomerLoginId { get; set; }
        public int CustomerId { get; set; }    
        public string? EmailId { get; set; } 
        public string? PhoneNumber { get; set; } 
        public string? Password { get; set; }
        public bool? LoginStatus { get; set; }
    }
}
