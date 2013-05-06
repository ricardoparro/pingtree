using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EpPingtree.Model.ViewModel;
using EpPingtree.Web.PingtreeWebservice;

namespace EpPingtree.Web.Controllers
{
    public class MainFormController : Controller
    {
        //
        // GET: /MainForm/
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(IframeFormModel model)
        {

            LeadRequest lead = new LeadRequest();
            lead.AddressLine1 = model.Address1;
            lead.AddressLine2 = "";//TODO: not using the autocomplete for address. empty for now - when integrating fill this field
            lead.ApplicationDate = DateTime.Now;
            lead.BankAccountNumber = model.BankAccountNumber;
            lead.BankSortcode = model.BankSortCode;
            lead.Country = "UK";
            lead.County = "";//TODO: Same as Address 1 . If there is no county send it as empty
            lead.DebitCardType = GetCardType(model.DebitCardType);
            lead.DirectDeposit = model.PayByDD;
            lead.Dob = new DateTime(model.DobYear, model.DobMonth, model.DobDay);
            lead.EmailAddress = model.Email;
            lead.EmployersName = model.EmployerName;
            lead.EmploymentStatus = GetEmploymentType(model.EmploymentType);
            lead.Forename = model.FirstName;
            lead.HomePhone = model.HomePhone;
            lead.HomeStatus = GetHomeStatus(model.HomeOwner);
            lead.IncomeFrequency = GetIncomeFrequency(model.SalaryFrequency);
            lead.IpAddress = Request.UserHostAddress;
            lead.LoanAmount = (int) model.LoanAmount;
            lead.MobilePhone = model.MobilePhone;
            lead.MonthlyIncome = (int) model.NetMonthlyPay;
            lead.MonthsAtAddress = model.MonthAtAddress + (12 * model.YearAtAddress);
            lead.MonthsAtEmployer = model.MonthAtEmployer + (12*model.YearAtEmployer);
            lead.PaybackDate = model.NextPayday;
           // lead.NexPaybackDate = model.NextNextPayday; //TODO: ADD TO pingtree database and to the models
            lead.Postcode = model.PostCode;
            lead.Source = "GetMoneyNow";
            lead.SubSource = "Affilinet";
            lead.Surname = model.LastName;
            lead.Title = model.Title;
            lead.Town = model.Town;
            lead.WorkPhone = model.WorkPhone;
            lead.SellerName = "GetCashNow";
            

            ServiceSoapClient client = new ServiceSoapClient();

        


            SellLeadResponse sellLeadResponse = client.SubmitLead(lead);


            return View();
        }

        private string GetCardType(string debitCardType){
        

            switch (debitCardType)
            {
                case "NDC":
                    return "NoDebitCard";
                case "SM":
                    return "SwitchMaestro";
                case "SOLO":
                    return "Solo";
                case "VD":
                    return "VisaDelta";
                case "VDEBIT":
                    return "VisaDebit";
                case "VE":
                    return "VisaElectron";
                default:
                    return "NoDebitCard";
            }
        }

        private string GetEmploymentType(string employmentType)
        {
            

            switch (employmentType)
            {
                case "1":
                    return "FullTime";
                case "2":
                    return "PartTime";
                case "3":
                    return "SelfEmployed";
                default:
                    return "Other";

            }
            
        }

        private string GetHomeStatus(string homeOwner)
        {
            switch (homeOwner)
            {
                case "CT":
                    return "CouncilTenant";
                case "HO":
                    return "Homeowner";
                case "LWP":
                    return "LivingWithParents";
                case "PT":
                    return "PrivateTenant";
                case "OTHER":
                    return "Other";
                default:
                    return "Other";
            }
        }

        private string GetIncomeFrequency(string salaryFrequency)
        {
            switch (salaryFrequency)
            {
                case "BW":
                    return "Biweekly";
                case "FW":
                    return "Fourweekly";
                case "LFM":
                    return "LastFridayOfMonth";
                case "LMM":
                    return "LastMondayOfMonth";
                case "LTM":
                    return "LastTuesdayOfMonth";
                case "LWM":
                    return "LastWednesdayOfMonth";
                case "LTHM":
                    return "Last Thursday of Month";
                case "SD":
                    return "SpecificDate";
                case "SDM":
                    return "SpecificDayOfMonth";
                case "TM":
                    return "TwiceMonthly";
                case "W":
                    return "Weekly";
                default:
                    return "";
            }
        }
    }
}
