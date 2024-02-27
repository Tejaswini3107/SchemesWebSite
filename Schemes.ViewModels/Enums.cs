using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schemes.ViewModels
{
    public class Enums
    {
        public enum OTPtypeEnum : byte
        {
            RegistrationThroughEmail = 0,
            RegistrationThroughPhone = 1,
            ForgotYourPassword = 2,
        }
        public enum OtpStatus : byte
        {
            Sent=0,
            Failed=1,
            Success=2
        }
        public enum CustomerStatus : byte
        {
            Active=10,
            InActive=20
        }
    }
}
