using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schemes.Models
{
    public class AdminLogin: Base
    {
        [Key]
        public int AdminLoginId { get; set; }
        public int AdminId { get; set; }
        public string? EmailId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public bool? LoginStatus { get; set; }
    }
}
