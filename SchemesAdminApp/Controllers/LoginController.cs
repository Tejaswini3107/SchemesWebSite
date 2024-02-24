using Microsoft.AspNetCore.Mvc;

namespace SchemesAdminApp.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
