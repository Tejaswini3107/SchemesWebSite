using System.ComponentModel.DataAnnotations;
using static Schemes.ViewModels.Enums;

namespace Schemes.ViewModels
{
    public class CustomerLoginVM
    {
        [Required(ErrorMessage = "Emailid is required")]
        [RegularExpression(@"^([a-zA-Z0-9]+[a-zA-Z0-9\.]*[a-zA-Z0-9]+)@(gmail)\.(com)$", ErrorMessage = "EmailId must contain @gmail domain")]
        public string? EmailId { get; set; }
        [Required(ErrorMessage = "PhoneNumber is required")]
        [RegularExpression(@"(^[6 - 9]\d{9}$)", ErrorMessage = "PhoneNumber is required")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }

    public class CustomerVM
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
    public class  AdminLoginVM
    {
        [Required(ErrorMessage = "Emailid is required")]
        [RegularExpression(@"^([a-zA-Z0-9]+[a-zA-Z0-9\.]*[a-zA-Z0-9]+)@(gmail)\.(com)$", ErrorMessage = "EmailId must contain @gmail domain")]
        public string? EmailId { get; set; }
        [Required(ErrorMessage = "PhoneNumber is required")]
        [RegularExpression(@"(^[6 - 9]\d{9}$)", ErrorMessage = "PhoneNumber is required")]
        public string? PhoneNumber { get; set; }

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

    public class CustomerDetails
    {
        public int CustomerId { get; set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string PhoneNo { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public CustomerStatus CustomerStatus { get; set; }
    }

    public class SchemeDetails
    {
        public int? SchemesDetailID { get; set; }
        public string? NameOftheScheme { get; set; }
        public string? Description { get; set; }
        public string? EligibilityCriteria { get; set; }
        public string? Benefits { get; set; }
        public string? Area { get; set; }
        public string? DocumentsRequired { get; set; }
        public string? ApplyAndLink { get; set; }
        public bool? IsActive { get; set; }
        public string? AvailableFor { get; set; }

    }
    public class RegistrationViewModel
    {
        [Required]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Your First Name must be Characters")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = "Your Last Name must be Characters")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9]+[a-zA-Z0-9\.]*[a-zA-Z0-9]+)@(gmail)\.(com)$", ErrorMessage = "EmailId must contain @gmail domain")]
        public string EmailId { get; set; }
        [Required]
        [RegularExpression(@"(?=.*\d)(?=.*[A-Za-z]).{5,}", ErrorMessage = "Your password must be at least 5 characters long and contain at least 1 letter and 1 number")]
        public string Password { get; set; }

        [Required]
        [RegularExpression(@"(^[6 - 9]\d{9}$)", ErrorMessage = "Invalid mobile number")]
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }


    }
}
