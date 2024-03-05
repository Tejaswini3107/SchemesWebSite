using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Schemes.Manager;
using Schemes.Repository;
using SchemesWebApp.Models;
using System.Diagnostics;

namespace SchemesWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SchemesContext _dbContext;

        public HomeController(LoginManager loginManager, SchemesContext DbContext,ILogger<HomeController> logger)
        {
            _logger = logger;
            _dbContext = DbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
       
        
        public IActionResult HomePage()
        {
            try
            {
               
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult GetSchemesList(string name)
        {
            try
            {
                var list = new CustomerManager(_dbContext).GetSchemesList(name);
                return View("_GetSchemesList.cshtml",list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
