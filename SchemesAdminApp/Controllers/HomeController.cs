using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schemes.Manager;
using Schemes.Repository;
using Schemes.ViewModels;
using SchemesAdminApp.Models;
using System.Diagnostics;

namespace SchemesAdminApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SchemesContext _dbContext;

        public HomeController(SchemesContext DbContext, ILogger<HomeController> logger)
        {
            _logger = logger;
            _dbContext = DbContext;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult GetSchemesList()
        {
            try
            {
                var list = new CustomerManager(_dbContext).GetAllSchemesList();
                return View(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult GetCustomersList()
        {
            try
            {
                var list = new CustomerManager(_dbContext).GetCustomersList();
                return View(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public IActionResult AddNewScheme()
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
        [HttpPost]
        public IActionResult AddNewScheme(SchemeDetails schemeDetails)
        {
            try
            {
                var list = new CustomerManager(_dbContext).AddNewScheme(schemeDetails);
                return RedirectToAction("GetSchemesList");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public IActionResult UpdateScheme(int id)
        {
            try
            {
                var personalInfo = new CustomerManager(_dbContext).GetSchemesDetails(id);
                return View(personalInfo);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public IActionResult UpdateScheme(SchemeDetails schemeDetails)
        {
            try
            {

                new CustomerManager(_dbContext).UpdateScheme(schemeDetails);
                return RedirectToAction("GetSchemesList");
                //return RedirectToAction("GetPersonalInfo", new { Id = UpdatedId });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        
        [HttpGet]
        public IActionResult DeleteScheme(int id)
        {
            try
            {
                
                new CustomerManager(_dbContext).DeleteScheme(id);
                return RedirectToAction("GetSchemesList");
                //return RedirectToAction("GetPersonalInfo", new { Id = UpdatedId });
            }
            catch (Exception ex)
            {
                return null ;
            }
        }
        public IActionResult GetBanksLoanList()
        {
            try
            {
                var list = new CustomerManager(_dbContext).GetAllBanksList();
                return View(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public IActionResult AddBanksLoanDetails()
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
        [HttpPost]
        public IActionResult AddBanksLoanDetails(LoanDetails LoanDetails)
        {
            try
            {
                var list = new CustomerManager(_dbContext).AddBanksLoanDetails(LoanDetails);
                return RedirectToAction("GetBanksLoanList");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public IActionResult UpdateBanksLoanDetails(int id)
        {
            try
            {
                var details = new CustomerManager(_dbContext).GetLoanDetailsByID(id);
                return View(details);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpPost]
        public IActionResult UpdateBanksLoanDetails(LoanDetails LoanDetails)
        {
            try
            {

                new CustomerManager(_dbContext).UpdateLoanDetails(LoanDetails);
                return RedirectToAction("GetBanksLoanList");
                //return RedirectToAction("GetPersonalInfo", new { Id = UpdatedId });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        public IActionResult CreateNewAdmin()
        {
            try
            {

               
                return View();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        [HttpPost]
        public IActionResult CreateNewAdmin(AdminRegistrationVM adminRegistrationVM )
        {
            try
            {

                new CustomerManager(_dbContext).CreateNewAdmin(adminRegistrationVM);
                return RedirectToAction("Index");
                //return RedirectToAction("GetPersonalInfo", new { Id = UpdatedId });
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
