using Microsoft.EntityFrameworkCore;
using Schemes.ViewModels;
using Schemes.Models;
using System.Net.Mail;
using System.Net;
using static Schemes.ViewModels.Enums;

namespace Schemes.Repository
{
    public class LoginRepository
    {
        private readonly SchemesContext _dbContext;
        public LoginRepository(SchemesContext DbContext)
        {
            _dbContext = DbContext;
        }
        public int GetCustomerLogin(string email, string password)
        {
            try
            {
                int CustomerID = 0;
                var CustomerLogin = _dbContext.CustomerLogin.Where(e => e.EmailId == email &&e.Password==password).FirstOrDefault();
                if (CustomerLogin != null)
                {
                    
                            return CustomerLogin.CustomerId;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string AddOTP(string EmailId,OTPtypeEnum OTPType, string PhoneNumber="")
        {
            Models.OTPDetails oTPDetails = new Models.OTPDetails();
            string VerificationCode = "";


            try
            {
                string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
                VerificationCode = GenerateRandomOTP(6, saAllowedCharacters);
                if (!string.IsNullOrEmpty(EmailId))
                {
                    oTPDetails.SentDetails = EmailId;
                    oTPDetails.OTPType = (int)OTPtypeEnum.RegistrationThroughEmail;

                }
                if (!string.IsNullOrEmpty(PhoneNumber))
                {
                    oTPDetails.SentDetails = PhoneNumber;
                    oTPDetails.OTPType = (int)OTPtypeEnum.RegistrationThroughPhone;

                }
                oTPDetails.InsertedBy = "Customer";
                oTPDetails.InsertedDate = DateTime.Now;
                oTPDetails.IsVerified = false;
               
               bool result= SendEmailToUser(EmailId, VerificationCode);
                if (result)
                {
                    oTPDetails.OtpStatus = (int)OtpStatus.Sent;
                }
                else
                {
                    oTPDetails.OtpStatus = (int)OtpStatus.Failed;
                }
                _dbContext.OTPDetails.Add(oTPDetails);
                _dbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                throw;
            }
            return VerificationCode;
        }

        public void AddCustomerRegistrationDetails(RegistrationViewModel registrationViewModel)
        {
            Customer Customer = new Customer();
            Customer.EmailId = registrationViewModel.EmailId;
            Customer.FirstName = registrationViewModel.FirstName;
            Customer.PhoneNo = registrationViewModel.PhoneNumber;
            Customer.DateOfBirth = registrationViewModel.DateOfBirth;
            Customer.LastName = registrationViewModel.LastName;
            Customer.InsertedDate = DateTime.Now;
            Customer.InsertedBy = registrationViewModel.FirstName;
            _dbContext.Customers.Add(Customer);
            _dbContext.SaveChanges();
        }
        public void AddCustomerRegistrationLoginDetails(RegistrationViewModel registrationViewModel)
        {
            CustomerLogin CustomerLogin = new CustomerLogin();
            var CustomerId = _dbContext.Customers.Where(e => e.EmailId == registrationViewModel.EmailId).Select(e => e.CustomerId).FirstOrDefault();
            
            CustomerLogin.CustomerId = CustomerId;
            CustomerLogin.PhoneNumber = registrationViewModel.PhoneNumber;
            CustomerLogin.EmailId = registrationViewModel.EmailId;
            CustomerLogin.Password = registrationViewModel.Password;
            CustomerLogin.LoginStatus = true;
            CustomerLogin.InsertedDate = DateTime.Now;
            CustomerLogin.InsertedBy = registrationViewModel.FirstName;
            _dbContext.CustomerLogin.Add(CustomerLogin);
            _dbContext.SaveChanges();
        }

        public bool VerifyOTP(string emailID, string OTP)
        {
            bool isOTPVerified = false;
            try
            {
                var identityVerificationDetails = _dbContext.OTPDetails.Where(s => s.SentDetails.Equals(emailID)).OrderByDescending(e => e.InsertedDate).First();
                if (identityVerificationDetails != null)
                {
                    if (!string.IsNullOrEmpty(OTP))
                    {

                            identityVerificationDetails.IsVerified = true;
                            identityVerificationDetails.OtpStatus = (int)OtpStatus.Success;
                            identityVerificationDetails.UpdatedBy = emailID;
                            identityVerificationDetails.UpdatedDate = DateTime.Now;
                            _dbContext.SaveChanges();
                            isOTPVerified = true;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return isOTPVerified;

        }

        private string GenerateRandomOTP(int iOTPLength, string[] saAllowedCharacters)
        {
            string sOTP = String.Empty;
            string sTempChars = String.Empty;
            Random rand = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {
                int p = rand.Next(0, saAllowedCharacters.Length);
                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
                sOTP += sTempChars;
            }
            return sOTP;
        }

        public bool CheckEmailForPasswordChange(string emailid)
        {
            var CustomerInfo = _dbContext.CustomerLogin.Where(s => s.EmailId == emailid).FirstOrDefault();
            if (CustomerInfo != null)
            {
                return true;
            }
            return false;
        }

        public void UpdatePassword(RegistrationViewModel registrationViewModel)
        {
            var CustomerDetails = _dbContext.CustomerLogin.Where(e => e.EmailId == registrationViewModel.EmailId).FirstOrDefault();
            
            CustomerDetails.Password = registrationViewModel.Password;
            _dbContext.SaveChanges();

        }
        public bool SendEmailToUser(string emailid,string otp)
        {
            string smtpServer = "smtp.gmail.com";
            string smtpUsername = "tejaswinireddyy0701@gmail.com";
            string smtpPassword = "ufze suca vblq arhh";

            string body = $"Your OTP code is: {otp}";
            MailMessage mail = new MailMessage(smtpUsername, emailid, otp, body);

            SmtpClient smtpClient = new SmtpClient(smtpServer)
            {
                Port = 587,
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true,
            };

            try
            {
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }
    }
}
