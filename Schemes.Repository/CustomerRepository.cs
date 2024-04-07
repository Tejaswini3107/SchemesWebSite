using Schemes.Models;
using Schemes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

                schemeDetailsList.Add(schemeDetails);
            }
            return schemeDetailsList;
        }
        public List<SchemeDetails>? GetSchemesListByLangCode(string name, string langCode)
        {
            List<SchemeDetails>? schemeDetailsList = new List<SchemeDetails>();
            var schemeDetailsFromDB = _dbContext.MultilingualSchemesData.Where(s => s.AvaliableFor.Contains(name)&&s.IsActive == true).ToList();
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
            var details = _dbContext.SchemesDetails.ToList();
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
        public bool AddNewScheme(SchemeDetails scheme)
        {
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
            foreach (var customer in customersList)
            {
                sMSDetails.Add(new SMSDetails()
                {
                    phone_number = customer.PhoneNo,
                    text_message = $"New Scheme {scheme.NameOftheScheme} is available for: {scheme.AvailableFor} on our website"
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
            return true;
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
            return true;
        }
        public bool UpdateScheme(SchemeDetails scheme)
        {

            var details = _dbContext.SchemesDetails.Where(s => s.SchemesDetailID == scheme.SchemesDetailID).FirstOrDefault();
            if (details != null)
            {
                details.Area = scheme.Area;
                details.ApplyAndLink = scheme.ApplyAndLink;
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
            }
            
            return schemeDetails;
        }
        public LoanDetails GetLoanDetails(string Loantype, string bankName)
        {
            LoanDetails loanDetails = new LoanDetails();
            var loanInterestDetails = _dbContext.LoanInterestDetails.Where(s => s.BankName.Contains(bankName) &&s.LoanType.Contains(Loantype)).FirstOrDefault();
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
        public List<string> GetLoantypes(string bankName)
        {
            var loanTypesList = _dbContext.LoanInterestDetails.Where(s => s.BankName.Contains(bankName)).Select(e=>e.LoanType).ToList();
            return loanTypesList;
        }
    }
}
