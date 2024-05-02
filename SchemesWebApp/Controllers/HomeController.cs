using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Schemes.Manager;
using Schemes.Repository;
using Schemes.ViewModels;
using SchemesWebApp.Models;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Net.Http;
using System.Text;

namespace SchemesWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SchemesContext _dbContext;
        private readonly IConfiguration _configuration;
        public HomeController(LoginManager loginManager, SchemesContext DbContext,ILogger<HomeController> logger,IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
            _dbContext = DbContext;
        }

        public IActionResult Index()
        {
            return View();
        }
       
        
        public IActionResult HomePage(int customerID=0,string selectedlang="en")
        {
            try
            {
                ViewBag.CustomerID = ""+customerID;
                ViewBag.Selectedlang = selectedlang;
                ViewBag.CustomerName = "";
                if (customerID > 0)
                {
                    CustomerDetails ? details = new CustomerManager(_dbContext).GetCustomersList().Where(s => s.CustomerId == customerID).FirstOrDefault();
                    if (details != null)
                    {
                        ViewBag.CustomerName = details.FirstName + details.LastName;
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult GetSchemesList(string name,string langCode="en")
        {
            try
            {
                ViewBag.Selectedlang = langCode;

                List<SchemeDetails> list = new CustomerManager(_dbContext).GetSchemesListByLangCode(name,langCode);
                ViewBag.AvailableFor = name;
                return View(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult CustomerProfileDetails(int customerID)
        {
            try
            {
                CustomerDetails? details = new CustomerManager(_dbContext).GetCustomersList().Where(s=>s.CustomerId==customerID).FirstOrDefault();

                return PartialView(details);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult GetLoanDetails(string bankName,string langCode="en")
        {
            try
            {
                LoanDetails details = new CustomerManager(_dbContext).GetLoanDetails(bankName);
                details.LangCode = langCode;
                return Ok(details);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult GetLoantypes(string bankName)
        {
            try
            {
                List<string> list = new CustomerManager(_dbContext).GetLoantypes(bankName);
                return View(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public IActionResult InactivateCustomer(int customerID)
        {
            try
            {
                bool result = new CustomerManager(_dbContext).UpdateCustomerStatus(customerID);

                if(result)
                {
                    return RedirectToAction("Login","Login");
                }
                return RedirectToAction("HomePage","Home");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult Chatbot(string message="")
        {
            string responseMsg ="Hello";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpContent content = new StringContent("{\"message\": \"" + message + "\"}", Encoding.UTF8, "application/json");

                try
                {

                    HttpResponseMessage response = httpClient.PostAsync("https://vrwknc12-5000.inc1.devtunnels.ms/bot", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = response.Content.ReadAsStringAsync().Result;
                        var resp = JsonConvert.DeserializeObject<ChatBotResponse>(responseContent);
                        if (resp != null)
                        {
                            responseMsg = resp.Message;
                        }

                    }
                    else
                    {
                        Console.WriteLine($"Failed to make the request. Status code: {response.StatusCode}");
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }
            }
            return Ok(responseMsg);

        }


    }
    public class ChatBotResponse
    {
        public string Message { get; set; }
    }
}
