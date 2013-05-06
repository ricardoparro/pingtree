using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Model;
using EpPingtree.Model.Apply.Request;
using EpPingtree.Model.ServiceModel;

namespace EpPingtree.Tests.TestData
{
    public class ModelPopulateFactory
    {
        public static List<Buyer> GetBuyers()
        {
            List<Buyer> buyers = new List<Buyer>();

            Buyer buyer1 = new Buyer();
            buyer1.Active = true;
            buyer1.Alias = "FAKE";
            buyer1.RefKey = "FAKE";
            buyer1.BuyerId = 1;
            buyer1.Country = "UK";
            buyer1.EmailAddress = "buyerFake@whatever.com";
            buyer1.FixedAmount = 12;
            buyer1.IntegrationUrl = "Http://whatever.com/ApplyLead?lala";

            //Buyer buyer2 = new Buyer();
            //buyer2.Active = true;
            //buyer2.Alias = "CreditoMais";
            //buyer2.BuyerId = 2;
            //buyer2.Country = "UK";
            //buyer2.EmailAddress = "buyer2Mem@whatever.com";
            //buyer2.FixedAmount = 11;
            //buyer2.IntegrationUrl = "Http://whatever.com/ApplyLead?lala";

            //Buyer buyer3 = new Buyer();
            //buyer3.Active = true;
            //buyer3.Alias = "PdbUk";
            //buyer3.BuyerId = 3;
            //buyer3.Country = "UK";
            //buyer3.EmailAddress = "buyerpdbuk@whatever.com";
            //buyer3.FixedAmount = 15;
            //buyer3.IntegrationUrl = "Http://whatever.com/pdbuk/ApplyLead?lala";

            //Buyer buyer4 = new Buyer();
            //buyer4.Active = true;
            //buyer4.Alias = "LeadGen";
            //buyer4.BuyerId = 4;
            //buyer4.Country = "UK";
            //buyer4.EmailAddress = "buyerLeadGen@whatever.com";
            //buyer4.FixedAmount = 9;
            //buyer4.IntegrationUrl = "Http://whatever.com/ApplyLead?lala";

            //Buyer buyer5 = new Buyer();
            //buyer5.Active = true;
            //buyer5.Alias = "PingTree";
            //buyer5.BuyerId = 5;
            //buyer5.Country = "UK";
            //buyer5.EmailAddress = "buyerPingTree@whatever.com";
            //buyer5.FixedAmount = 50;
            //buyer5.IntegrationUrl = "Http://whatever.com/ApplyLead?lala";

            //Buyer buyer6 = new Buyer();
            //buyer6.Active = true;
            //buyer6.Alias = "WageDay";
            //buyer6.BuyerId = 6;
            //buyer6.Country = "UK";
            //buyer6.EmailAddress = "buyerWageDay@whatever.com";
            //buyer6.FixedAmount = 25;
            //buyer6.IntegrationUrl = "Http://whatever.com/WageDay/ApplyLead?lala";

            buyers.Add(buyer1);
            //buyers.Add(buyer2);
            //buyers.Add(buyer3);
            //buyers.Add(buyer4);
            //buyers.Add(buyer5);
            //buyers.Add(buyer6);

            return buyers.OrderByDescending(a => a.FixedAmount).ToList();
        }

        public static LeadRequest GetLeadRequest()
        {
            LeadRequest lead = new LeadRequest();
            lead.AddressLine1 = "12 Bellefields road";
            lead.AddressLine2 = "flat 2";
            lead.ApplicationDate = DateTime.Now;
            lead.BankAccountNumber = "0978897";
            lead.BankSortcode = "234423";
            lead.Country = "UK";
            lead.County = "London";
            lead.DebitCardType = "Visa";
            lead.DirectDeposit = true;
            lead.Dob = DateTime.Now.AddYears(-35).Date;
            lead.EmailAddress = "applicant@gmail.com";
            lead.EmployersName = "Sony";
            lead.EmploymentStatus = "Full Time";
            lead.Forename = "Joe";
            lead.Surname = "Blogs";
            lead.Gender = "Male";
            lead.HomePhone = "02766676767";
            lead.MobilePhone = "07533383678";
            lead.HomeStatus = "Owner";
            lead.IncomeFrequency = "Monthly";
            lead.IpAddress = "10.123.123.2222";
            lead.LoanAmount = 250;
            lead.MonthlyIncome = 1000;
            lead.MonthsAtAddress = 12;
            lead.MonthsAtEmployer = 11;
            lead.PaybackDate = DateTime.Now.AddMonths(1);
            lead.Postcode = "NW1 0SU";
            lead.Source = "myWebsiteNumber2";
            lead.SubSource = "emailcampainWeek2";
            lead.Title = "Mr";
            lead.Town = "London";
            lead.WorkPhone = "02020292929292";
            lead.SellerName = "FakeSeller";
            return lead;
        }

        public static Lead GetLead()
        {
            Lead lead = new Lead();
            lead.AddressLine1 = "12 Bellefields road";
            lead.AddressLine2 = "flat 2";
            lead.ApplicationDate = DateTime.Now;
            lead.BankAccountNumber = "0978897";
            lead.BankSortcode = "234423";
            lead.Country = "UK";
            lead.County = "London";
            lead.DebitCardType = "Visa";
            lead.DirectDeposit = true;
            lead.Dob = new DateTime(1977, 01 ,30);
            lead.EmailAddress = "applicant@gmail.com";
            lead.EmployersName = "Sony";
            lead.EmploymentStatus = "Full Time";
            lead.Forename = "Joe";
            lead.Surname = "Blogs";
            lead.Gender = "Male";
            lead.HomePhone = "02766676767";
            lead.MobilePhone = "07533383678";
            lead.HomeStatus = "Owner";
            lead.IncomeFrequency = "Monthly";
            lead.IpAddress = "10.123.123.2222";
            lead.LeadId = 1;
            lead.LoanAmount = 250;
            lead.MonthlyIncome = 1000;
            lead.MonthsAtAddress = 12;
            lead.MonthsAtEmployer = 11;
            lead.PaybackDate = new DateTime(2012,12,31);
            lead.Postcode = "NW1 0SU";
            lead.Source = "myWebsiteNumber2";
            lead.SubSource = "emailcampainWeek2";
            lead.Title = "Mr";
            lead.Town = "London";
            lead.WorkPhone = "02020292929292";

            return lead;
        }

        public static List<BuyerBilling> GetBuyerBillings()
        {
            List<BuyerBilling> buyerBillings = new List<BuyerBilling>();
            

            Billing billing = new Billing();
            billing.BuyerId = 1;
            billing.BillingTypeId = 1;
            billing.BillingId = 1;
            billing.SellerId = 1;
            billing.Value = (decimal) 0.50;

             Buyer buyer1 = new Buyer();
            buyer1.Active = true;
            buyer1.Alias = "FAKE";
            buyer1.RefKey = "FAKE";
            buyer1.BuyerId = 1;
            buyer1.Country = "UK";
            buyer1.EmailAddress = "buyerFake@whatever.com";
            buyer1.FixedAmount = 12;
            buyer1.IntegrationUrl = "Http://whatever.com/ApplyLead?lala";
            
            List<Billing> billings = new List<Billing>();
            billings.Add(billing);

            BuyerBilling buyerBilling = new BuyerBilling();
            buyerBilling.Billing = billing;
            buyerBilling.Buyer = buyer1;

            buyerBillings.Add(buyerBilling);

            return buyerBillings;
        }
    }
}
