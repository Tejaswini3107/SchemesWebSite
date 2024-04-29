using Microsoft.EntityFrameworkCore;
using Schemes.Models;
using Schemes.Repository.Migrations;
using Schemes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Schemes.ViewModels.Enums;

namespace Schemes.Repository
{
    public class CustomerRepository
    {
        private readonly SchemesContext _dbContext;
        public CustomerRepository(SchemesContext DbContext)
        {
            _dbContext = DbContext;
        }
        public List<SchemeDetails> GetSchemesList(string name)
        {
            List<SchemeDetails> schemeDetailsList =new List<SchemeDetails>();
             var details   = _dbContext.SchemesDetails.Where(s=>s.AvaliableFor.Contains(name) &&s.IsActive==true).ToList();
            foreach (var scheme in details)
            {
                SchemeDetails schemeDetails=new SchemeDetails();
                schemeDetails.SchemesDetailID = scheme.SchemesDetailID;
                schemeDetails.Area = scheme.Area;
                schemeDetails.ApplyAndLink = scheme.ApplyAndLink;
                schemeDetails.NameOftheScheme = scheme.NameOftheScheme;
                schemeDetails.Benefits = scheme.Benefits;
                schemeDetails.EligibilityCriteria = scheme.EligibilityCriteria;
                schemeDetails.Description = scheme.Description;
                schemeDetails.DocumentsRequired = scheme.DocumentsRequired;
                schemeDetails.AvailableFor = scheme.AvaliableFor;

                schemeDetailsList.Add(schemeDetails);
            }
            return schemeDetailsList;
        }
        public List<SchemeDetails>? GetSchemesListByLangCode(string name, string langCode)
        {
            List<SchemeDetails>? schemeDetailsList = new List<SchemeDetails>();
            var schemeDetailsFromDB = _dbContext.SchemesDetails.Where(s => s.AvaliableFor.Contains(name)&&s.IsActive == true).ToList();
            if (schemeDetailsFromDB != null)
            {
                var details = _dbContext.MultilingualSchemesData.Where(s => s.AvaliableFor.Contains(name) && s.LangCode == langCode && s.IsActive == true).ToList();
                if (details != null)
                {
                    foreach (var scheme in details)
                    {
                        SchemeDetails schemeDetails = new SchemeDetails();
                        schemeDetails.SchemesDetailID = scheme.SchemesDetailID;
                        schemeDetails.Area = scheme.Area;
                        schemeDetails.ApplyAndLink = scheme.ApplyAndLink;
                        schemeDetails.NameOftheScheme = scheme.NameOftheScheme;
                        schemeDetails.Benefits = scheme.Benefits;
                        schemeDetails.EligibilityCriteria = scheme.EligibilityCriteria;
                        schemeDetails.Description = scheme.Description;
                        schemeDetails.DocumentsRequired = scheme.DocumentsRequired;

                        schemeDetailsList.Add(schemeDetails);
                    }
                }
                else
                {
                    foreach (var scheme in schemeDetailsFromDB)
                    {
                        SchemeDetails schemeDetails = new SchemeDetails();
                        schemeDetails.SchemesDetailID = scheme.SchemesDetailID;
                        schemeDetails.Area = scheme.Area;
                        schemeDetails.ApplyAndLink = scheme.ApplyAndLink;
                        schemeDetails.NameOftheScheme = scheme.NameOftheScheme;
                        schemeDetails.Benefits = scheme.Benefits;
                        schemeDetails.EligibilityCriteria = scheme.EligibilityCriteria;
                        schemeDetails.Description = scheme.Description;
                        schemeDetails.DocumentsRequired = scheme.DocumentsRequired;

                        schemeDetailsList.Add(schemeDetails);
                    }
                }
            }
            return schemeDetailsList;
        }
        public List<SchemeDetails> GetAllSchemesList()
        {
            List<SchemeDetails> schemeDetailsList = new List<SchemeDetails>();
            var details = _dbContext.SchemesDetails.Where(s=>s.IsActive==true).ToList();
            foreach (var scheme in details)
            {
                SchemeDetails schemeDetails = new SchemeDetails();
                schemeDetails.SchemesDetailID = scheme.SchemesDetailID;
                schemeDetails.Area = scheme.Area;
                schemeDetails.ApplyAndLink = scheme.ApplyAndLink;
                schemeDetails.NameOftheScheme = scheme.NameOftheScheme;
                schemeDetails.Benefits = scheme.Benefits;
                schemeDetails.EligibilityCriteria = scheme.EligibilityCriteria;
                schemeDetails.Description = scheme.Description;
                schemeDetails.DocumentsRequired = scheme.DocumentsRequired;
                schemeDetails.IsActive = scheme.IsActive;
                schemeDetails.AvailableFor = scheme.AvaliableFor;


                schemeDetailsList.Add(schemeDetails);
            }
            return schemeDetailsList;
        }
        public int AddNewScheme(SchemeDetails scheme)
        {
            int schemeID = 0;
            Models.SchemesDetails schemeDetails = new SchemesDetails();
                schemeDetails.Area = scheme.Area;
                schemeDetails.ApplyAndLink = scheme.ApplyAndLink;
                schemeDetails.NameOftheScheme = scheme.NameOftheScheme;
                schemeDetails.Benefits = scheme.Benefits;
                schemeDetails.EligibilityCriteria = scheme.EligibilityCriteria;
                schemeDetails.Description = scheme.Description;
                schemeDetails.DocumentsRequired = scheme.DocumentsRequired;
                schemeDetails.AvaliableFor = scheme.AvailableFor;
                schemeDetails.IsActive = true;
            schemeDetails.InsertedDate = DateTime.Now;
            schemeDetails.InsertedBy = "Admin";
               var details= _dbContext.SchemesDetails.Add(schemeDetails).Entity;
                _dbContext.SaveChanges();

            List<SMSDetails> sMSDetails = new List<SMSDetails>();
            var customersList = GetCustomersList();
            var text = $"Fineasee:Dear Customer, New Scheme {scheme.NameOftheScheme} is available for: {scheme.AvailableFor} on our website";
            foreach (var customer in customersList)
            {
                new LoginRepository(_dbContext).SendEmailToUser(customer.EmailId, text, false);
                sMSDetails.Add(new SMSDetails()
                {
                    phone_number = customer.PhoneNo,
                    text_message = text
                });
            }
            new LoginRepository(_dbContext).SendOTPAsync(sMSDetails);
            MultilingualSchemesDataVM schemeData = new MultilingualSchemesDataVM();
            schemeData.Area = scheme.Area;
            schemeData.ApplyAndLink = scheme.ApplyAndLink;
            schemeData.NameOftheScheme = scheme.NameOftheScheme;
            schemeData.Benefits = scheme.Benefits;
            schemeData.EligibilityCriteria = scheme.EligibilityCriteria;
            schemeData.Description = scheme.Description;
            schemeData.DocumentsRequired = scheme.DocumentsRequired;
            schemeData.AvaliableFor = scheme.AvailableFor;
            AddNewSchemeByLangCode(schemeData, details.SchemesDetailID, "en");
            schemeID = details.SchemesDetailID;
            return schemeID;
        }
        public bool AddNewSchemeByLangCode(MultilingualSchemesDataVM scheme,int schemeID,string LangCode)
        {
            Models.MultilingualSchemesData schemeDetails = new MultilingualSchemesData();
            schemeDetails.Area = scheme.Area;
            schemeDetails.ApplyAndLink = scheme.ApplyAndLink;
            schemeDetails.NameOftheScheme = scheme.NameOftheScheme;
            schemeDetails.Benefits = scheme.Benefits;
            schemeDetails.EligibilityCriteria = scheme.EligibilityCriteria;
            schemeDetails.Description = scheme.Description;
            schemeDetails.DocumentsRequired = scheme.DocumentsRequired;
            schemeDetails.AvaliableFor = scheme.AvaliableFor;
            schemeDetails.LangCode = LangCode;
            schemeDetails.SchemesDetailID = schemeID;
            schemeDetails.IsActive = true;
            schemeDetails.InsertedDate = DateTime.Now;
            schemeDetails.InsertedBy = "Admin";
            _dbContext.MultilingualSchemesData.Add(schemeDetails);
            _dbContext.SaveChanges();
            return true;
        }
        public bool UpdateScheme(SchemeDetails scheme)
        {

            var details = _dbContext.SchemesDetails.Where(s => s.SchemesDetailID == scheme.SchemesDetailID).FirstOrDefault();
            if (details != null)
            {
                details.Area = scheme.Area;
                if(scheme.ApplyAndLink !=null ) {
                  details.ApplyAndLink=  ReplaceUrlsWithLinks(scheme.ApplyAndLink);
                }
                details.NameOftheScheme = scheme.NameOftheScheme;
                details.Benefits = scheme.Benefits;
                details.EligibilityCriteria = scheme.EligibilityCriteria;
                details.Description = scheme.Description;
                details.DocumentsRequired = scheme.DocumentsRequired;
                details.AvaliableFor = scheme.AvailableFor;
                details.UpdatedDate = DateTime.Now;
                details.UpdatedBy = "Admin";
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
        public bool UpdateSchemeStatus(int schemeID)
        {

            var details = _dbContext.SchemesDetails.Where(s => s.SchemesDetailID == schemeID).FirstOrDefault();
            if (details != null)
            {
                if ((bool)details.IsActive)
                {
                    details.IsActive = false;
                }
                else
                {
                    details.IsActive = true;
                }
                _dbContext.SaveChanges();

                return true;
            }
            return false;
        }
        public bool UpdateCustomerStatus(int customerID)
        {

            var details = _dbContext.Customers.Where(s => s.CustomerId == customerID).FirstOrDefault();
            if (details != null)
            {
                
                    details.CustomerStatus = (int)CustomerStatus.InActive;
                _dbContext.SaveChanges();

                return true;
            }
            return false;
        }

        public List<CustomerDetails> GetCustomersList()
        {
            List<CustomerDetails> customersList = new List<CustomerDetails>();
            customersList = _dbContext.Customers.Select(s=> new CustomerDetails()
            {
                CustomerId=s.CustomerId,
                DateOfBirth=s.DateOfBirth,
                EmailId=s.EmailId,
                FirstName=s.FirstName,
                PhoneNo=s.PhoneNo,
                LastName=s.LastName,
                CustomerStatus=(CustomerStatus)s.CustomerStatus
            }).ToList();
            
            return customersList;
        }
        public SchemeDetails GetSchemesDetails(int schemeiD)
        {
            SchemeDetails schemeDetails = new SchemeDetails();
            var scheme = _dbContext.SchemesDetails.Where(s => s.SchemesDetailID== schemeiD).FirstOrDefault();
            if (scheme != null)
            {
                schemeDetails.SchemesDetailID = scheme.SchemesDetailID;
                schemeDetails.Area = scheme.Area;
                schemeDetails.ApplyAndLink = scheme.ApplyAndLink;
                schemeDetails.NameOftheScheme = scheme.NameOftheScheme;
                schemeDetails.Benefits = scheme.Benefits;
                schemeDetails.EligibilityCriteria = scheme.EligibilityCriteria;
                schemeDetails.Description = scheme.Description;
                schemeDetails.DocumentsRequired = scheme.DocumentsRequired;
                schemeDetails.AvailableFor = scheme.AvaliableFor;
            }
            
            return schemeDetails;
        }
        public LoanDetails GetLoanDetails(string bankName)
        {
            LoanDetails loanDetails = new LoanDetails();
            var loanInterestDetails = _dbContext.LoanInterestDetails.Where(s => s.BankName.Contains(bankName)).FirstOrDefault();
            if (loanInterestDetails != null)
            {
                loanDetails.LoanType = loanInterestDetails.LoanType;
                loanDetails.BankName =loanInterestDetails.BankName;
                loanDetails.MaxloanAmount = loanInterestDetails.MaxloanAmount;
                loanDetails.MinLoanAmount = loanInterestDetails.MinLoanAmount;
                loanDetails.MaxInterestRate = loanInterestDetails.MaxInterestRate;
                loanDetails.MinInterestRate = loanInterestDetails.MinInterestRate;

            }

            return loanDetails;
        }
        public List<LoanDetails> GetLoanDetailsList()
        {
            List<LoanDetails> list = new List<LoanDetails>();
            var loanInterestDetailsList = _dbContext.LoanInterestDetails.ToList();
            if (loanInterestDetailsList.Count >0)
            {
                foreach (var loanInterestDetails in loanInterestDetailsList)
                {
                    LoanDetails loanDetails = new LoanDetails();
                    loanDetails.LoanType = loanInterestDetails.LoanType;
                    loanDetails.LoanInterestDetailID = loanInterestDetails.LoanInterestDetailID;
                    loanDetails.BankName = loanInterestDetails.BankName;
                    loanDetails.MaxloanAmount = loanInterestDetails.MaxloanAmount;
                    loanDetails.MinLoanAmount = loanInterestDetails.MinLoanAmount;
                    loanDetails.MaxInterestRate = loanInterestDetails.MaxInterestRate;
                    loanDetails.MinInterestRate = loanInterestDetails.MinInterestRate;
                    list.Add(loanDetails);
                }

            }

            return list;
        }
        public bool AddLoanDetails(LoanDetails loanInterestDetails)
        {
            LoanInterestDetails loanDetails = new LoanInterestDetails();
            if (loanInterestDetails != null)
            {
                loanDetails.LoanType = loanInterestDetails.LoanType;
                loanDetails.BankName = loanInterestDetails.BankName;
                loanDetails.MaxloanAmount = loanInterestDetails.MaxloanAmount;
                loanDetails.MinLoanAmount = loanInterestDetails.MinLoanAmount;
                loanDetails.MaxInterestRate = loanInterestDetails.MaxInterestRate;
                loanDetails.MinInterestRate = loanInterestDetails.MinInterestRate;
                loanDetails.InsertedBy = "Admin";
                loanDetails.InsertedDate = DateTime.Now;
                _dbContext.LoanInterestDetails.Add(loanDetails);
                _dbContext.SaveChanges();
                List<SMSDetails> sMSDetails = new List<SMSDetails>();
                var customersList = GetCustomersList();
                var text = $"Fineasee:Dear Customer,{loanDetails.BankName} bank is providing Loans For {loanDetails.LoanType} at interest rates starting at {loanDetails.MinInterestRate} and is available on our website";
                foreach (var customer in customersList)
                {
                    new LoginRepository(_dbContext).SendEmailToUser(customer.EmailId, text, false);
                    sMSDetails.Add(new SMSDetails()
                    {
                        phone_number = customer.PhoneNo,
                        text_message = text
                    });
                }
                new LoginRepository(_dbContext).SendOTPAsync(sMSDetails);
                return true;
            }

            return false;
        }
        public bool UpdateLoanDetails(LoanDetails loanInterestDetails)
        {

            var loanDetails = _dbContext.LoanInterestDetails.Where(s => s.LoanInterestDetailID == loanInterestDetails.LoanInterestDetailID).FirstOrDefault();
            if (loanDetails != null)
            {
                loanDetails.LoanType = loanInterestDetails.LoanType;
                loanDetails.BankName = loanInterestDetails.BankName;
                loanDetails.MaxloanAmount = loanInterestDetails.MaxloanAmount;
                loanDetails.MinLoanAmount = loanInterestDetails.MinLoanAmount;
                loanDetails.MaxInterestRate = loanInterestDetails.MaxInterestRate;
                loanDetails.MinInterestRate = loanInterestDetails.MinInterestRate;
                loanDetails.UpdatedDate = DateTime.Now;
                loanDetails.UpdatedBy = "Admin";
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
        public List<string> GetLoantypes(string bankName)
        {
            var loanTypesList = _dbContext.LoanInterestDetails.Where(s => s.BankName.Contains(bankName)).Select(e=>e.LoanType).ToList();
            return loanTypesList;
        }

        public static string ReplaceUrlsWithLinks(string text)
        {
            string pattern = @"(?<=(^|\s))(https?://\S+)";
            Regex regex = new Regex(pattern);

            string replacedText = regex.Replace(text, match =>
            {
                string url = match.Value;
                return $"<a class=\"btn btn-link\" href=\"{url}\">{url}</a>";
            });

            return replacedText;
        }
        public bool CreateNewAdmin(AdminRegistrationVM adminRegistrationVM)
        {
            AdminLogin adminLogin = new AdminLogin();
            Admin admin = new Admin();
            if (adminRegistrationVM != null)
            {
                adminLogin.UserName = adminRegistrationVM.UserName;
                adminLogin.Password = adminRegistrationVM.Password;
                admin.EmailId= adminRegistrationVM.EmailId;
                admin.PhoneNo = adminRegistrationVM.PhoneNo;
                admin.FirstName = adminRegistrationVM.FirstName;
                admin.LastName = adminRegistrationVM.LastName;
                admin.InsertedBy = "Admin";
                admin.InsertedDate = DateTime.Now;
                _dbContext.Admin.Add(admin);
                _dbContext.SaveChanges();
                adminLogin.AdminId = admin.AdminId;
                adminLogin.InsertedBy = "Admin";
                adminLogin.InsertedDate = DateTime.Now;
                _dbContext.AdminLogin.Add(adminLogin);
                _dbContext.SaveChanges();
                return true;
            }

            return false;
        }
    }
}
