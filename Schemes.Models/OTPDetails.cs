using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schemes.Models
{
    public class OTPDetails: Base
    {
        [Key]
        public int OTPDetailsId { get; set; }
        public string SentDetails {  get; set; }
        public string OTP {  get; set; }
        public int OTPType { get; set;}
        public int OtpStatus { get; set;}
        public bool IsVerified { get; set;}
    }
}
