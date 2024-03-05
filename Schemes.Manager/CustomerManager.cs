using Schemes.Repository;
using Schemes.ViewModels;
using System.Collections.Generic;

namespace Schemes.Manager
{
    public class CustomerManager
    {
        private readonly SchemesContext _dbContext;
        public CustomerManager(SchemesContext DbContext)
        {
            _dbContext = DbContext;
        }
        public List<SchemeDetails> GetSchemesList(string name)
        {
            List<SchemeDetails> schemeDetails = new CustomerRepository(_dbContext).GetSchemesList(name);
            return schemeDetails;
        }
        public List<SchemeDetails> GetAllSchemesList()
        {
            List<SchemeDetails> schemeDetails = new CustomerRepository(_dbContext).GetAllSchemesList();
            return schemeDetails;
        }
        public bool AddNewScheme(SchemeDetails schemeDetails)
        {
            var result = new CustomerRepository(_dbContext).AddNewScheme(schemeDetails);
            return result;
        }
        public List<CustomerDetails> GetCustomersList()
        {
            var list = new CustomerRepository(_dbContext).GetCustomersList();
            return list;
        } 
        public SchemeDetails GetSchemesDetails(int schemeiD)
        {
            var scheme = new CustomerRepository(_dbContext).GetSchemesDetails(schemeiD);
            return scheme;
        }
        public bool UpdateScheme(SchemeDetails schemeDetails)
        {
            var result = new CustomerRepository(_dbContext).UpdateScheme(schemeDetails);
            return result;
        }
    }
}
