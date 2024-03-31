using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schemes.Models
{
    public class LoanInterestDetails :Base
    {
        [Key]
        public int LoanInterestDetailID {  get; set; }
        public string BankName {  get; set; }
        public string LoanType {  get; set; }
        public string MinLoanAmount {  get; set; }
        public string MaxloanAmount {  get; set; }
        public string MinInterestRate {  get; set; }
        public string MaxInterestRate {  get; set; }
    }
}
