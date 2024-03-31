using Schemes.Repository;
using Schemes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Schemes.ViewModels.Enums;

namespace Schemes.Manager
{
    public class LoginManager
    {

        private readonly SchemesContext _dbContext;
        public LoginManager(SchemesContext DbContext)
        {
            _dbContext = DbContext;
        }
        public int GetCustomerLogin(string email, string password)
        {
            CustomerLoginVM CustomerLoginViewModel = new CustomerLoginVM();
            var empID = new LoginRepository(_dbContext).GetCustomerLogin(email, password);

            return empID;
        }

        public void CustomerRegistration(RegistrationViewModel registrationViewModel)
        {
            try
            {
                new LoginRepository(_dbContext).AddCustomerRegistrationDetails(registrationViewModel);
                new LoginRepository(_dbContext).AddCustomerRegistrationLoginDetails(registrationViewModel);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public void SendOTP(string? EmailId, OTPtypeEnum OTPType)
        {
            try
            {
                new LoginRepository(_dbContext).AddOTP(EmailId, OTPType);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public void SendOTPToPhoneNumber(string phoneNumber, OTPtypeEnum OTPType)
        {
            try
            {
                new LoginRepository(_dbContext).AddOTP(null, OTPType, phoneNumber);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool VerifyOTP(string emailOrPhNumber, string OTP)
        {
            bool isOtpVerified;
            try
            {
                isOtpVerified = new LoginRepository(_dbContext).VerifyOTP(emailOrPhNumber, OTP);
            }
            catch (Exception)
            {
                throw;
            }
            return isOtpVerified;
        }

        public void ForgotPassword()
        {

            try
            {

            }
            catch (Exception)
            {
                throw;
            }

        }

        public bool CheckEmailForPasswordChange(string emailid)
        {
            var checkResult = new LoginRepository(_dbContext).CheckEmailForPasswordChange(emailid);
            return checkResult;

        }
        public void UpdatePassword(RegistrationViewModel registrationViewModel)
        {
            new LoginRepository(_dbContext).UpdatePassword(registrationViewModel);
        }




    }
}

