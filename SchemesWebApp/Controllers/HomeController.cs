﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Schemes.Manager;
using Schemes.Repository;
using Schemes.ViewModels;
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
                List<SchemeDetails> list = new CustomerManager(_dbContext).GetSchemesList(name);
                return View(list);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult GetLoanDetails(string Loantype,string bankName)
        {
            try
            {
                LoanDetails details = new CustomerManager(_dbContext).GetLoanDetails(Loantype,bankName);
                return View(details);
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
    }
}
