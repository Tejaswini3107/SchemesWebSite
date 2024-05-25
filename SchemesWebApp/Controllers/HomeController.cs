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

        public IActionResult Index(string LangCode="en")
        {
            SchemesCustomerLabelsVM schemesCustomerLabelsVM=new SchemesCustomerLabelsVM();

            if (LangCode == "en")
            {
                schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                {
                    SideBarLabel1 = "Home",
                    SideBarLabel2 = "Government Schemes",
                    SideBarLabel3 = "Farmers",
                    SideBarLabel4 = "Students",
                    SideBarLabel5 = "Backward Caste",
                    EmiTitle = "EMI Calculator",
                    BanksTitle = "Banks",
                    Bank1 = "SBI",
                    Bank2 = "UNION",
                    Bank3 = "KARNATAKA",
                    LoanAmountLBL = "Loan Amount (₹)",
                    InterestRateLBL = "Interest Rate (%)",
                    loanTenureLBL = "Loan Tenure (years)",
                    calculateEMIBtnTxt = "Calculate EMI",
                    ResultTxt = "Results",
                    loanEMITxt = "Loan EMI: ₹",
                    TIPTxt = "Total Interest Payable: ₹ ",
                    TPTxt = "Total Payment (Principal + Interest): ₹"
                };
            }
            else if (LangCode == "hi")
            {
                schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                {
                    SideBarLabel1 = "मुख्य पृष्ठ",
                    SideBarLabel2 = "सरकारी योजनाएं",
                    SideBarLabel3 = "किसान",
                    SideBarLabel4 = "छात्र",
                    SideBarLabel5 = "पिछड़ा वर्ग",
                    EmiTitle = "ईएमआई कैलकुलेटर",
                    BanksTitle = "बैंक",
                    Bank1 = "एसबीआई",
                    Bank2 = "यूनियन",
                    Bank3 = "कर्नाटका",
                    LoanAmountLBL = "ऋण राशि (₹)",
                    InterestRateLBL = "ब्याज दर (%)",
                    loanTenureLBL = "ऋण अवधि (वर्ष)",
                    calculateEMIBtnTxt = "ईएमआई की गणना करें",
                    ResultTxt = "परिणाम",
                    loanEMITxt = "ऋण ईएमआई: ₹",
                    TIPTxt = "कुल ब्याज देय: ₹ ",
                    TPTxt = "कुल भुगतान (प्रधान + ब्याज): ₹"
                };
            }
            else if (LangCode == "te")
            {
                schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                {
                    SideBarLabel1 = "హోం",
                    SideBarLabel2 = "ప్రభుత్వ పథకాలు",
                    SideBarLabel3 = "రైతులు",
                    SideBarLabel4 = "విద్యార్థులు",
                    SideBarLabel5 = "వెనుకబడిన వర్గం",
                    EmiTitle = "EMI క్యాలిక్యులేటర్",
                    BanksTitle = "బ్యాంకులు",
                    Bank1 = "SBI",
                    Bank2 = "యూనియన్",
                    Bank3 = "కర్ణాటక",
                    LoanAmountLBL = "పరమాణు మొత్తం (₹)",
                    InterestRateLBL = "వడ్డీ రేటు (%)",
                    loanTenureLBL = "పరమాణు కాలం (సంవత్సరాలు)",
                    calculateEMIBtnTxt = "EMI లెక్కించండి",
                    ResultTxt = "ఫలితాలు",
                    loanEMITxt = "పరమాణు EMI: ₹",
                    TIPTxt = "మొత్తం వడ్డీ చెల్లించాలి: ₹ ",
                    TPTxt = "మొత్తం చెల్లింపు (మూల + వడ్డీ): ₹"
                };
            }
            else if (LangCode == "ur")
            {
                schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                {
                    SideBarLabel1 = "گھر",
                    SideBarLabel2 = "حکومتی اسکیمیں",
                    SideBarLabel3 = "کسان",
                    SideBarLabel4 = "طلباء",
                    SideBarLabel5 = "پسماندہ طبقہ",
                    EmiTitle = "ای ایم آئی کیلکولیٹر",
                    BanksTitle = "بینک",
                    Bank1 = "ایس بی آئی",
                    Bank2 = "یونین",
                    Bank3 = "کرناٹکا",
                    LoanAmountLBL = "قرض کی رقم (₹)",
                    InterestRateLBL = "سود کی شرح (%)",
                    loanTenureLBL = "قرض کی مدت (سال)",
                    calculateEMIBtnTxt = "ای ایم آئی حساب کریں",
                    ResultTxt = "نتائج",
                    loanEMITxt = "قرض ای ایم آئی: ₹",
                    TIPTxt = "کل سود کی ادائیگی: ₹ ",
                    TPTxt = "کل ادائیگی (اصل + سود): ₹"
                };
            }
            else if (LangCode == "kn")
            {
                schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                {
                    SideBarLabel1 = "ಮನೆ",
                    SideBarLabel2 = "ಸರ್ಕಾರಿ ಯೋಜನೆಗಳು",
                    SideBarLabel3 = "ರೈತರು",
                    SideBarLabel4 = "ವಿದ್ಯಾರ್ಥಿಗಳು",
                    SideBarLabel5 = "ಹಿಂದಿನ ವರ್ಗ",
                    EmiTitle = "EMI ಕ್ಯಾಲ್ಕ್ಯುಲೆಟರ್",
                    BanksTitle = "ಬ್ಯಾಂಕುಗಳು",
                    Bank1 = "SBI",
                    Bank2 = "ಯೂನಿಯನ್",
                    Bank3 = "ಕರ್ನಾಟಕ",
                    LoanAmountLBL = "ಕಡಿತ ಮೊತ್ತ (₹)",
                    InterestRateLBL = "ಬಡ್ಡಿದರ (%)",
                    loanTenureLBL = "ಕಡಿತ ಅವಧಿ (ವರ್ಷ)",
                    calculateEMIBtnTxt = "EMI ಲೆಕ್ಕಹಾಕಿ",
                    ResultTxt = "ಫಲಿತಾಂಶಗಳು",
                    loanEMITxt = "ಕಡಿತ EMI: ₹",
                    TIPTxt = "ಒಟ್ಟು ಬಡ್ಡಿ ಪಾವತಿ: ₹ ",
                    TPTxt = "ಒಟ್ಟು ಪಾವತಿ (ಮೂಲ + ಬಡ್ಡಿ): ₹"
                };
            }
            else if (LangCode == "ta")
            {
                schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                {
                    SideBarLabel1 = "முகப்பு",
                    SideBarLabel2 = "அரசுத் திட்டங்கள்",
                    SideBarLabel3 = "விவசாயிகள்",
                    SideBarLabel4 = "மாணவர்கள்",
                    SideBarLabel5 = "பின்னோக்கினர்",
                    EmiTitle = "EMI கணிப்பான்",
                    BanksTitle = "வங்கிகள்",
                    Bank1 = "SBI",
                    Bank2 = "யூனியன்",
                    Bank3 = "கர்நாடகா",
                    LoanAmountLBL = "கடன் தொகை (₹)",
                    InterestRateLBL = "வட்டி விகிதம் (%)",
                    loanTenureLBL = "கடன் காலம் (ஆண்டுகள்)",
                    calculateEMIBtnTxt = "EMI கணக்கிடு",
                    ResultTxt = "முடிவுகள்",
                    loanEMITxt = "கடன் EMI: ₹",
                    TIPTxt = "மொத்த வட்டி செலுத்தவேண்டியது: ₹ ",
                    TPTxt = "மொத்த செலுத்தல் (மூலதனம் + வட்டி): ₹"
                };
            }
            else if (LangCode == "ml")
            {
                schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                {
                    SideBarLabel1 = "ഹോം",
                    SideBarLabel2 = "സർക്കാർ പദ്ധതികൾ",
                    SideBarLabel3 = "കർഷകർ",
                    SideBarLabel4 = "വിദ്യാർത്ഥികൾ",
                    SideBarLabel5 = "പശ്ചാത്തമർ",
                    EmiTitle = "EMI കാൽക്കുലേറ്റർ",
                    BanksTitle = "ബാങ്കുകൾ",
                    Bank1 = "SBI",
                    Bank2 = "യൂണിയൻ",
                    Bank3 = "കർണാടക",
                    LoanAmountLBL = "ലോൺ തുക (₹)",
                    InterestRateLBL = "പലിശ നിരക്ക് (%)",
                    loanTenureLBL = "ലോൺ കാലയളവ് (വർഷങ്ങൾ)",
                    calculateEMIBtnTxt = "EMI കണക്കുകൂട്ടുക",
                    ResultTxt = "ഫലങ്ങൾ",
                    loanEMITxt = "ലോൺ EMI: ₹",
                    TIPTxt = "മൊത്തം പലിശ അടയ്ക്കേണ്ടത്: ₹ ",
                    TPTxt = "മൊത്തം പണമടച്ചുക (അടിസ്ഥാന + പലിശ): ₹"
                };
            }

            return View(schemesCustomerLabelsVM);
        }
       
        
        public IActionResult HomePage(int customerID=0,string LangCode = "en")
        {
            SchemesCustomerLabelsVM schemesCustomerLabelsVM = new SchemesCustomerLabelsVM();

            try
            {
                if (LangCode == "en")
                {
                    schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                    {
                        SideBarLabel1 = "Home",
                        SideBarLabel2 = "Government Schemes",
                        SideBarLabel3 = "Farmers",
                        SideBarLabel4 = "Students",
                        SideBarLabel5 = "Backward Caste",
                        EmiTitle = "EMI Calculator",
                        BanksTitle = "Banks",
                        Bank1 = "SBI",
                        Bank2 = "UNION",
                        Bank3 = "KARNATAKA",
                        LoanAmountLBL = "Loan Amount (₹)",
                        InterestRateLBL = "Interest Rate (%)",
                        loanTenureLBL = "Loan Tenure (years)",
                        calculateEMIBtnTxt = "Calculate EMI",
                        ResultTxt = "Results",
                        loanEMITxt = "Loan EMI: ₹",
                        TIPTxt = "Total Interest Payable: ₹ ",
                        TPTxt = "Total Payment (Principal + Interest): ₹"
                    };
                }
                else if (LangCode == "hi")
                {
                    schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                    {
                        SideBarLabel1 = "मुख्य पृष्ठ",
                        SideBarLabel2 = "सरकारी योजनाएं",
                        SideBarLabel3 = "किसान",
                        SideBarLabel4 = "छात्र",
                        SideBarLabel5 = "पिछड़ा वर्ग",
                        EmiTitle = "ईएमआई कैलकुलेटर",
                        BanksTitle = "बैंक",
                        Bank1 = "एसबीआई",
                        Bank2 = "यूनियन",
                        Bank3 = "कर्नाटका",
                        LoanAmountLBL = "ऋण राशि (₹)",
                        InterestRateLBL = "ब्याज दर (%)",
                        loanTenureLBL = "ऋण अवधि (वर्ष)",
                        calculateEMIBtnTxt = "ईएमआई की गणना करें",
                        ResultTxt = "परिणाम",
                        loanEMITxt = "ऋण ईएमआई: ₹",
                        TIPTxt = "कुल ब्याज देय: ₹ ",
                        TPTxt = "कुल भुगतान (प्रधान + ब्याज): ₹"
                    };
                }
                else if (LangCode == "te")
                {
                    schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                    {
                        SideBarLabel1 = "హోం",
                        SideBarLabel2 = "ప్రభుత్వ పథకాలు",
                        SideBarLabel3 = "రైతులు",
                        SideBarLabel4 = "విద్యార్థులు",
                        SideBarLabel5 = "వెనుకబడిన వర్గం",
                        EmiTitle = "EMI క్యాలిక్యులేటర్",
                        BanksTitle = "బ్యాంకులు",
                        Bank1 = "SBI",
                        Bank2 = "యూనియన్",
                        Bank3 = "కర్ణాటక",
                        LoanAmountLBL = "పరమాణు మొత్తం (₹)",
                        InterestRateLBL = "వడ్డీ రేటు (%)",
                        loanTenureLBL = "పరమాణు కాలం (సంవత్సరాలు)",
                        calculateEMIBtnTxt = "EMI లెక్కించండి",
                        ResultTxt = "ఫలితాలు",
                        loanEMITxt = "పరమాణు EMI: ₹",
                        TIPTxt = "మొత్తం వడ్డీ చెల్లించాలి: ₹ ",
                        TPTxt = "మొత్తం చెల్లింపు (మూల + వడ్డీ): ₹"
                    };
                }
                else if (LangCode == "ur")
                {
                    schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                    {
                        SideBarLabel1 = "گھر",
                        SideBarLabel2 = "حکومتی اسکیمیں",
                        SideBarLabel3 = "کسان",
                        SideBarLabel4 = "طلباء",
                        SideBarLabel5 = "پسماندہ طبقہ",
                        EmiTitle = "ای ایم آئی کیلکولیٹر",
                        BanksTitle = "بینک",
                        Bank1 = "ایس بی آئی",
                        Bank2 = "یونین",
                        Bank3 = "کرناٹکا",
                        LoanAmountLBL = "قرض کی رقم (₹)",
                        InterestRateLBL = "سود کی شرح (%)",
                        loanTenureLBL = "قرض کی مدت (سال)",
                        calculateEMIBtnTxt = "ای ایم آئی حساب کریں",
                        ResultTxt = "نتائج",
                        loanEMITxt = "قرض ای ایم آئی: ₹",
                        TIPTxt = "کل سود کی ادائیگی: ₹ ",
                        TPTxt = "کل ادائیگی (اصل + سود): ₹"
                    };
                }
                else if (LangCode == "kn")
                {
                    schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                    {
                        SideBarLabel1 = "ಮನೆ",
                        SideBarLabel2 = "ಸರ್ಕಾರಿ ಯೋಜನೆಗಳು",
                        SideBarLabel3 = "ರೈತರು",
                        SideBarLabel4 = "ವಿದ್ಯಾರ್ಥಿಗಳು",
                        SideBarLabel5 = "ಹಿಂದಿನ ವರ್ಗ",
                        EmiTitle = "EMI ಕ್ಯಾಲ್ಕ್ಯುಲೆಟರ್",
                        BanksTitle = "ಬ್ಯಾಂಕುಗಳು",
                        Bank1 = "SBI",
                        Bank2 = "ಯೂನಿಯನ್",
                        Bank3 = "ಕರ್ನಾಟಕ",
                        LoanAmountLBL = "ಕಡಿತ ಮೊತ್ತ (₹)",
                        InterestRateLBL = "ಬಡ್ಡಿದರ (%)",
                        loanTenureLBL = "ಕಡಿತ ಅವಧಿ (ವರ್ಷ)",
                        calculateEMIBtnTxt = "EMI ಲೆಕ್ಕಹಾಕಿ",
                        ResultTxt = "ಫಲಿತಾಂಶಗಳು",
                        loanEMITxt = "ಕಡಿತ EMI: ₹",
                        TIPTxt = "ಒಟ್ಟು ಬಡ್ಡಿ ಪಾವತಿ: ₹ ",
                        TPTxt = "ಒಟ್ಟು ಪಾವತಿ (ಮೂಲ + ಬಡ್ಡಿ): ₹"
                    };
                }
                else if (LangCode == "ta")
                {
                    schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                    {
                        SideBarLabel1 = "முகப்பு",
                        SideBarLabel2 = "அரசுத் திட்டங்கள்",
                        SideBarLabel3 = "விவசாயிகள்",
                        SideBarLabel4 = "மாணவர்கள்",
                        SideBarLabel5 = "பின்னோக்கினர்",
                        EmiTitle = "EMI கணிப்பான்",
                        BanksTitle = "வங்கிகள்",
                        Bank1 = "SBI",
                        Bank2 = "யூனியன்",
                        Bank3 = "கர்நாடகா",
                        LoanAmountLBL = "கடன் தொகை (₹)",
                        InterestRateLBL = "வட்டி விகிதம் (%)",
                        loanTenureLBL = "கடன் காலம் (ஆண்டுகள்)",
                        calculateEMIBtnTxt = "EMI கணக்கிடு",
                        ResultTxt = "முடிவுகள்",
                        loanEMITxt = "கடன் EMI: ₹",
                        TIPTxt = "மொத்த வட்டி செலுத்தவேண்டியது: ₹ ",
                        TPTxt = "மொத்த செலுத்தல் (மூலதனம் + வட்டி): ₹"
                    };
                }
                else if (LangCode == "ml")
                {
                    schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                    {
                        SideBarLabel1 = "ഹോം",
                        SideBarLabel2 = "സർക്കാർ പദ്ധതികൾ",
                        SideBarLabel3 = "കർഷകർ",
                        SideBarLabel4 = "വിദ്യാർത്ഥികൾ",
                        SideBarLabel5 = "പശ്ചാത്തമർ",
                        EmiTitle = "EMI കാൽക്കുലേറ്റർ",
                        BanksTitle = "ബാങ്കുകൾ",
                        Bank1 = "SBI",
                        Bank2 = "യൂണിയൻ",
                        Bank3 = "കർണാടക",
                        LoanAmountLBL = "ലോൺ തുക (₹)",
                        InterestRateLBL = "പലിശ നിരക്ക് (%)",
                        loanTenureLBL = "ലോൺ കാലയളവ് (വർഷങ്ങൾ)",
                        calculateEMIBtnTxt = "EMI കണക്കുകൂട്ടുക",
                        ResultTxt = "ഫലങ്ങൾ",
                        loanEMITxt = "ലോൺ EMI: ₹",
                        TIPTxt = "മൊത്തം പലിശ അടയ്ക്കേണ്ടത്: ₹ ",
                        TPTxt = "മൊത്തം പണമടച്ചുക (അടിസ്ഥാന + പലിശ): ₹"
                    };
                }
                ViewBag.CustomerID = ""+customerID;
                ViewBag.Selectedlang = LangCode;
                ViewBag.CustomerName = "";
                if (customerID > 0)
                {
                    CustomerDetails ? details = new CustomerManager(_dbContext).GetCustomersList().Where(s => s.CustomerId == customerID).FirstOrDefault();
                    if (details != null)
                    {
                        ViewBag.CustomerName = details.FirstName + details.LastName;
                    }
                }
                return View(schemesCustomerLabelsVM);
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
        public IActionResult GetLoanDetails(string bankName,string loanType = "")
        {
            try
            {
                LoanDetails details = new CustomerManager(_dbContext).GetLoanDetails(bankName, loanType);
                return Ok(details);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult GetLoantypes(string bankName,string LangCode="en")
        {
            try
            {
                SchemesCustomerLabelsVM schemesCustomerLabelsVM = new SchemesCustomerLabelsVM();

                if (LangCode == "en")
                {
                    schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                    {
                        SideBarLabel1 = "Home",
                        SideBarLabel2 = "Government Schemes",
                        SideBarLabel3 = "Farmers",
                        SideBarLabel4 = "Students",
                        SideBarLabel5 = "Backward Caste",
                        EmiTitle = "EMI Calculator",
                        BanksTitle = "Banks",
                        Bank1 = "SBI",
                        Bank2 = "UNION",
                        Bank3 = "KARNATAKA",
                        LoanAmountLBL = "Loan Amount (₹)",
                        InterestRateLBL = "Interest Rate (%)",
                        loanTenureLBL = "Loan Tenure (years)",
                        calculateEMIBtnTxt = "Calculate EMI",
                        ResultTxt = "Results",
                        loanEMITxt = "Loan EMI: ₹",
                        TIPTxt = "Total Interest Payable: ₹ ",
                        TPTxt = "Total Payment (Principal + Interest): ₹"
                    };
                }
                else if (LangCode == "hi")
                {
                    schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                    {
                        SideBarLabel1 = "मुख्य पृष्ठ",
                        SideBarLabel2 = "सरकारी योजनाएं",
                        SideBarLabel3 = "किसान",
                        SideBarLabel4 = "छात्र",
                        SideBarLabel5 = "पिछड़ा वर्ग",
                        EmiTitle = "ईएमआई कैलकुलेटर",
                        BanksTitle = "बैंक",
                        Bank1 = "एसबीआई",
                        Bank2 = "यूनियन",
                        Bank3 = "कर्नाटका",
                        LoanAmountLBL = "ऋण राशि (₹)",
                        InterestRateLBL = "ब्याज दर (%)",
                        loanTenureLBL = "ऋण अवधि (वर्ष)",
                        calculateEMIBtnTxt = "ईएमआई की गणना करें",
                        ResultTxt = "परिणाम",
                        loanEMITxt = "ऋण ईएमआई: ₹",
                        TIPTxt = "कुल ब्याज देय: ₹ ",
                        TPTxt = "कुल भुगतान (प्रधान + ब्याज): ₹"
                    };
                }
                else if (LangCode == "te")
                {
                    schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                    {
                        SideBarLabel1 = "హోం",
                        SideBarLabel2 = "ప్రభుత్వ పథకాలు",
                        SideBarLabel3 = "రైతులు",
                        SideBarLabel4 = "విద్యార్థులు",
                        SideBarLabel5 = "వెనుకబడిన వర్గం",
                        EmiTitle = "EMI క్యాలిక్యులేటర్",
                        BanksTitle = "బ్యాంకులు",
                        Bank1 = "SBI",
                        Bank2 = "యూనియన్",
                        Bank3 = "కర్ణాటక",
                        LoanAmountLBL = "పరమాణు మొత్తం (₹)",
                        InterestRateLBL = "వడ్డీ రేటు (%)",
                        loanTenureLBL = "పరమాణు కాలం (సంవత్సరాలు)",
                        calculateEMIBtnTxt = "EMI లెక్కించండి",
                        ResultTxt = "ఫలితాలు",
                        loanEMITxt = "పరమాణు EMI: ₹",
                        TIPTxt = "మొత్తం వడ్డీ చెల్లించాలి: ₹ ",
                        TPTxt = "మొత్తం చెల్లింపు (మూల + వడ్డీ): ₹"
                    };
                }
                else if (LangCode == "ur")
                {
                    schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                    {
                        SideBarLabel1 = "گھر",
                        SideBarLabel2 = "حکومتی اسکیمیں",
                        SideBarLabel3 = "کسان",
                        SideBarLabel4 = "طلباء",
                        SideBarLabel5 = "پسماندہ طبقہ",
                        EmiTitle = "ای ایم آئی کیلکولیٹر",
                        BanksTitle = "بینک",
                        Bank1 = "ایس بی آئی",
                        Bank2 = "یونین",
                        Bank3 = "کرناٹکا",
                        LoanAmountLBL = "قرض کی رقم (₹)",
                        InterestRateLBL = "سود کی شرح (%)",
                        loanTenureLBL = "قرض کی مدت (سال)",
                        calculateEMIBtnTxt = "ای ایم آئی حساب کریں",
                        ResultTxt = "نتائج",
                        loanEMITxt = "قرض ای ایم آئی: ₹",
                        TIPTxt = "کل سود کی ادائیگی: ₹ ",
                        TPTxt = "کل ادائیگی (اصل + سود): ₹"
                    };
                }
                else if (LangCode == "kn")
                {
                    schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                    {
                        SideBarLabel1 = "ಮನೆ",
                        SideBarLabel2 = "ಸರ್ಕಾರಿ ಯೋಜನೆಗಳು",
                        SideBarLabel3 = "ರೈತರು",
                        SideBarLabel4 = "ವಿದ್ಯಾರ್ಥಿಗಳು",
                        SideBarLabel5 = "ಹಿಂದಿನ ವರ್ಗ",
                        EmiTitle = "EMI ಕ್ಯಾಲ್ಕ್ಯುಲೆಟರ್",
                        BanksTitle = "ಬ್ಯಾಂಕುಗಳು",
                        Bank1 = "SBI",
                        Bank2 = "ಯೂನಿಯನ್",
                        Bank3 = "ಕರ್ನಾಟಕ",
                        LoanAmountLBL = "ಕಡಿತ ಮೊತ್ತ (₹)",
                        InterestRateLBL = "ಬಡ್ಡಿದರ (%)",
                        loanTenureLBL = "ಕಡಿತ ಅವಧಿ (ವರ್ಷ)",
                        calculateEMIBtnTxt = "EMI ಲೆಕ್ಕಹಾಕಿ",
                        ResultTxt = "ಫಲಿತಾಂಶಗಳು",
                        loanEMITxt = "ಕಡಿತ EMI: ₹",
                        TIPTxt = "ಒಟ್ಟು ಬಡ್ಡಿ ಪಾವತಿ: ₹ ",
                        TPTxt = "ಒಟ್ಟು ಪಾವತಿ (ಮೂಲ + ಬಡ್ಡಿ): ₹"
                    };
                }
                else if (LangCode == "ta")
                {
                    schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                    {
                        SideBarLabel1 = "முகப்பு",
                        SideBarLabel2 = "அரசுத் திட்டங்கள்",
                        SideBarLabel3 = "விவசாயிகள்",
                        SideBarLabel4 = "மாணவர்கள்",
                        SideBarLabel5 = "பின்னோக்கினர்",
                        EmiTitle = "EMI கணிப்பான்",
                        BanksTitle = "வங்கிகள்",
                        Bank1 = "SBI",
                        Bank2 = "யூனியன்",
                        Bank3 = "கர்நாடகா",
                        LoanAmountLBL = "கடன் தொகை (₹)",
                        InterestRateLBL = "வட்டி விகிதம் (%)",
                        loanTenureLBL = "கடன் காலம் (ஆண்டுகள்)",
                        calculateEMIBtnTxt = "EMI கணக்கிடு",
                        ResultTxt = "முடிவுகள்",
                        loanEMITxt = "கடன் EMI: ₹",
                        TIPTxt = "மொத்த வட்டி செலுத்தவேண்டியது: ₹ ",
                        TPTxt = "மொத்த செலுத்தல் (மூலதனம் + வட்டி): ₹"
                    };
                }
                else if (LangCode == "ml")
                {
                    schemesCustomerLabelsVM = new SchemesCustomerLabelsVM
                    {
                        SideBarLabel1 = "ഹോം",
                        SideBarLabel2 = "സർക്കാർ പദ്ധതികൾ",
                        SideBarLabel3 = "കർഷകർ",
                        SideBarLabel4 = "വിദ്യാർത്ഥികൾ",
                        SideBarLabel5 = "പശ്ചാത്തമർ",
                        EmiTitle = "EMI കാൽക്കുലേറ്റർ",
                        BanksTitle = "ബാങ്കുകൾ",
                        Bank1 = "SBI",
                        Bank2 = "യൂണിയൻ",
                        Bank3 = "കർണാടക",
                        LoanAmountLBL = "ലോൺ തുക (₹)",
                        InterestRateLBL = "പലിശ നിരക്ക് (%)",
                        loanTenureLBL = "ലോൺ കാലയളവ് (വർഷങ്ങൾ)",
                        calculateEMIBtnTxt = "EMI കണക്കുകൂട്ടുക",
                        ResultTxt = "ഫലങ്ങൾ",
                        loanEMITxt = "ലോൺ EMI: ₹",
                        TIPTxt = "മൊത്തം പലിശ അടയ്ക്കേണ്ടത്: ₹ ",
                        TPTxt = "മൊത്തം പണമടച്ചുക (അടിസ്ഥാന + പലിശ): ₹"
                    };
                }
                List<string> list = new CustomerManager(_dbContext).GetLoantypes(bankName);
                ViewBag.BankName = bankName;
                ViewBag.LangCode = LangCode;
                schemesCustomerLabelsVM.strings = new List<string>();
                schemesCustomerLabelsVM.strings = list;
                return View(schemesCustomerLabelsVM);
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
