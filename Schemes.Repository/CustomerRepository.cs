using Schemes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
