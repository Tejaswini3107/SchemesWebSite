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
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public bool? LoginStatus { get; set; }
    }
}
