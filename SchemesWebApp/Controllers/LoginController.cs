using Microsoft.AspNetCore.Mvc;

namespace SchemesWebApp.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string EmailId, string password)
        {
            if (ModelState.IsValid)
            {
                int employeeId = _accountService.GetEmployeeLogin(EmailId, password);
                if (employeeId != 0)
                {
                    HttpContext.Session.SetString("EmpID", employeeId.ToString());
                    SessionUser user = new SessionUser();
                    user.Id = employeeId;
                    createSession.SetSession(user, "SessionUser");
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
        public IActionResult EmployeeRegistration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EmployeeRegistration(RegistrationViewModel registrationViewModel)
        {
            _accountService.EmployeeRegistration(registrationViewModel);
            return RedirectToAction("Login", "Account");

        }

        [HttpPost]
        public IActionResult SendRegistrationOTP(string emailId)
        {
            if (ModelState.IsValid)
            {
                var OTPType = OTPtypeEnum.Registration;
                _accountService.SendOTP(emailId, OTPType);
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

            _accountService.UpdatePassword(registrationViewModel);
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult ForgotPasswordOTP(string emailId)
        {

            var OTP = OTPtypeEnum.ForgotYourPassword;
            ///checkMail
            var res = _accountService.CheckEmailForPasswordChange(emailId);
            if (res != false)
            {
                _accountService.SendOTP(emailId, OTP);
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
            var IsOTPVerified = _accountService.VerifyOTP(emailId, OTP);
            return Json(IsOTPVerified);
        }
        [HttpPost]
        public IActionResult UpdateNewPassword(RegistrationViewModel registrationViewModel)
        {
            _accountService.UpdatePassword(registrationViewModel);
            return View("Updated");///
        }

        public ActionResult LogOut()
        {
            try
            {
                createSession.SetSession(new SessionUser(), "SessionUser");
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
