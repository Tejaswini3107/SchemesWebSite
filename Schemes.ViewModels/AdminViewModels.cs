using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schemes.ViewModels
{
    public class AdminLoginVM
    {
        [Required(ErrorMessage = "UserName is required")]
        public string? EmailId { get; set; }
            
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
    public class AdminVM
    {
        public int AdminId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }
        public List<CustomerDetails> customerDetails { get; set; }
    }
}
