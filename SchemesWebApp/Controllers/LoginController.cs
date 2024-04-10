using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schemes.Manager;
using Schemes.Repository;
using Schemes.ViewModels;
using static Schemes.ViewModels.Enums;

namespace SchemesWebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly SchemesContext _dbContext;
        public LoginController(LoginManager loginManager, SchemesContext DbContext)
        {
            _dbContext = DbContext;


        }
        public IActionResult Login()
        {
            ViewBag.CustomerId = 0;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string EmailId, string password)
        {
            if (ModelState.IsValid)
            {
                int CustomerId = new LoginManager(_dbContext).GetAdminLogin(EmailId, password);
                if (CustomerId != 0)
                {
                    ViewBag.CustomerId = CustomerId;
                    return RedirectToAction("HomePage", "Home");
                }
                else
                {
                    ViewBag.LoginFailed = "Invalid Credentials";
                    return View();
                }
            }
            return BadRequest();
        }

        public IActionResult CustomerRegistration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CustomerRegistration(RegistrationViewModel registrationViewModel)
        {
            new LoginManager(_dbContext).CustomerRegistration(registrationViewModel);
            return RedirectToAction("Login", "Login");

        }

        [HttpPost]
        public IActionResult SendRegistrationOTP(string emailId)
        {
            if (ModelState.IsValid)
            {
                var OTPType = OTPtypeEnum. RegistrationThroughEmail;
                new LoginManager(_dbContext).SendOTP(emailId, OTPType);
                return Ok();
            }
            return BadRequest();

        }
        [HttpPost]
        public IActionResult SendSMSOTP(string phoneNumber)
        {
            if (ModelState.IsValid)
            {
                var OTPType = OTPtypeEnum.RegistrationThroughPhone;
                new LoginManager(_dbContext).SendOTPToPhoneNumber(phoneNumber, OTPType);
                return Ok();
            }
            return BadRequest();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(RegistrationViewModel registrationViewModel)
        {

            new LoginManager(_dbContext).UpdatePassword(registrationViewModel);
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public IActionResult ForgotPasswordOTP(string emailId)
        {

            var OTP = OTPtypeEnum.ForgotYourPassword;
            ///checkMail
            var res = new LoginManager(_dbContext).CheckEmailForPasswordChange(emailId);
            if (res != false)
            {
                new LoginManager(_dbContext).SendOTP(emailId, OTP);
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }
        [HttpPost]
        public IActionResult VerifyOTP(string emailId, string OTP)
        {
            var IsOTPVerified = new LoginManager(_dbContext).VerifyOTP(emailId, OTP);
            return Json(IsOTPVerified);
        }
        [HttpPost]
        public IActionResult VerifySMSOTP(string phoneNumber, string OTP)
        {
            var IsOTPVerified = new LoginManager(_dbContext).VerifyOTP(phoneNumber, OTP);
            return Json(IsOTPVerified);
        }
        [HttpPost]
        public IActionResult UpdateNewPassword(RegistrationViewModel registrationViewModel)
        {
            new LoginManager(_dbContext).UpdatePassword(registrationViewModel);
            return View("Updated");///
        }

        public ActionResult LogOut()
        {
            try
            {
                //createSession.SetSession(new SessionUser(), "SessionUser");
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public IActionResult SkipLogin()
        {
            ViewBag.CustomerId = 0;
            return RedirectToAction("HomePage","Home");
        }
    }
}
