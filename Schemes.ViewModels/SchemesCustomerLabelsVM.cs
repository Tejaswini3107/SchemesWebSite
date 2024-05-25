using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schemes.ViewModels
{
    public class SchemesCustomerLabelsVM
    {
        public string SideBarLabel1 {  get; set; }
        public string SideBarLabel2 { get; set; }
        public string SideBarLabel3 { get; set; }
        public string SideBarLabel4 { get; set; }
        public string SideBarLabel5 { get; set; }
        public string EmiTitle { get; set; }
        public string BanksTitle { get; set; }
        public string Bank1 { get; set; }
        public string Bank2 { get; set; }
        public string Bank3 { get; set; }
        public string LoanAmountLBL { get; set; }
        public string InterestRateLBL { get; set; }
        public string loanTenureLBL { get; set; }
        public string calculateEMIBtnTxt { get; set; }
        public string ResultTxt { get; set; }
        public string loanEMITxt { get; set; }
        public string TIPTxt { get; set; }
        public string TPTxt { get; set; }
        public List<string>? strings { get; set; }
        public List<Schemes.ViewModels.SchemeDetails>? SchemeDetailList { get; set; }
    }
}
