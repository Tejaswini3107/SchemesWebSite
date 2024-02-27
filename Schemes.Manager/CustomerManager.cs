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
    }
}
