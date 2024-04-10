using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schemes.Manager;
using Schemes.Repository;
using Schemes.ViewModels;

namespace SchemesAdminApp.Controllers
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
        public IActionResult Login(string userName, string password)
        {
            if (ModelState.IsValid)
            {
                int CustomerId = new LoginManager(_dbContext).GetAdminLogin(userName, password);
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
    }
}
