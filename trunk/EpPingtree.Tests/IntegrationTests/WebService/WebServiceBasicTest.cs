using System;
using System.Collections.Generic;
using System.Linq;
using EpPingtree.Datalayer.Interfaces;
using EpPingtree.Model;
using EpPingtree.Model.Enums;
using EpPingtree.Services.NewLead;
using EpPingtree.Tests.PingtreeWebService;
using EpPingtree.Tests.TestData;
using Moq;
using NUnit.Framework;
using LeadRequest = EpPingtree.Model.Apply.Request.LeadRequest;
using SellLeadResponse = EpPingtree.Model.Apply.Response.SellLeadResponse;

namespace EpPingtree.Tests.UnitTests.WebService
{
    [TestFixture]
    public class WebServiceBasicTest : BaseTestWithTestClass<ApplyLeadService>
    {
        private List<Seller> _sellersToDelete;
        private List<Buyer> _buyersToDelete;
        private List<Billing> _billingsToDelete;
        private int _sellerID;
        private int _buyerID;

        #region TestsSetup
            [SetUp]
        public void Setup()
        {
            using (BeginScope())
            {
                //Add a fake seller
                EprospectsDataContext context = Resolve<EprospectsDataContext>();

                Seller seller = new Seller();
                seller.SellerName = "FakeSeller";
                seller.Active = true;
                seller.Country = "UK";

                context.Sellers.InsertOnSubmit(seller);

                //Add to sellers list to delete on the tear down
                _sellersToDelete = new List<Seller>();

                _sellersToDelete.Add(seller);

                context.SubmitChanges();

                _sellerID = seller.SellerId;
            }


        }

   
        
        [TearDown]
        public void AfterTests()
        {
            using (BeginScope())
            {
                EprospectsDataContext context = Resolve<EprospectsDataContext>();

                int[] buyersInt = _buyersToDelete.Select(a => a.BuyerId).ToArray();

                List<LeadBought> leadBoughts = (from leadBought in context.LeadBoughts 
                                                where buyersInt.Contains(leadBought.BuyerId)
                                                select leadBought).ToList();

                context.LeadBoughts.DeleteAllOnSubmit(leadBoughts);


                foreach (Billing billing in _billingsToDelete)
                {
                    Billing b = (from a in context.Billings
                                 where a.BillingId == billing.BillingId 
                                 select a).FirstOrDefault();

                    context.Billings.DeleteOnSubmit(b);
                }

                foreach (Seller seller in _sellersToDelete)
                {
                    Seller firstOrDefault = (from seller1 in context.Sellers
                                             where seller1.SellerId == seller.SellerId
                                             select seller1).FirstOrDefault();
                    context.Sellers.DeleteOnSubmit(firstOrDefault);
                }

                foreach(Buyer buyer in _buyersToDelete)
                {
                    Buyer buy = (from b in context.Buyers
                                 where b.BuyerId == buyer.BuyerId
                                 select b).FirstOrDefault();
                    context.Buyers.DeleteOnSubmit(buy);
                }

                int[] sellerIds = _sellersToDelete.Select(a => a.SellerId).ToArray();

                List<Lead> leads = (from lead in context.Leads
                              where lead.EmailAddress == "fakeapplicant@gmail.com" && sellerIds.Contains(lead.SellerId)
                              select lead).ToList();
                context.Leads.DeleteAllOnSubmit(leads);

                context.SubmitChanges();

                _buyersToDelete = new List<Buyer>();
                _sellersToDelete = new List<Seller>();
                _billingsToDelete = new List<Billing>();
            }
        }
#endregion

        [Test]
        public void TestWebserviceLeadValidationFails()
        {
            using (BeginScope())
            {
                EprospectsDataContext context = new EprospectsDataContext();

                //Add a fake buyer

                Buyer buyer = new Buyer();
                buyer.Alias = "FAKE";
                buyer.Active = true;
                buyer.Country = "UK";
                buyer.EmailAddress = "fake@wonderland.com";
                buyer.FixedAmount = 122;
                buyer.Name = "FAKE";
                buyer.Username = "fake";
                buyer.Password = "default";
                buyer.IntegrationUrl = "fakerejected.com";
                buyer.RefKey = "FAKE";
                context.Buyers.InsertOnSubmit(buyer);

                //Add to buyers to delete on the tear down

                _buyersToDelete = new List<Buyer>();
                _buyersToDelete.Add(buyer);

                context.SubmitChanges();
                _buyerID = buyer.BuyerId;
            }
            using (BeginScope())
            {

                EprospectsDataContext context = Resolve<EprospectsDataContext>();
               
                Billing billing = new Billing();
                billing.BuyerId =_buyerID;
                billing.SellerId = _sellerID;
                billing.BillingTypeId = (int) ValueEnums.BillingTypes.FixedPrice;
                billing.Value = 2;
                context.Billings.InsertOnSubmit(billing);
                _billingsToDelete = new List<Billing>();
                _billingsToDelete.Add(billing);

                context.SubmitChanges();
            }
            //Arrange
                ServiceSoapClient client = new ServiceSoapClient();

                //Act

                PingtreeWebService.LeadRequest lead = GetLeadRequest();


                PingtreeWebService.SellLeadResponse sellLeadResponse = client.SubmitLead(lead);

                //Assert
                Assert.IsNotNull(sellLeadResponse);
                Assert.AreEqual(PingtreeWebService.ESellLeadResponse.Invalid, sellLeadResponse.Result);
                Assert.AreEqual(sellLeadResponse.ErrorMessage.ErrorReasons.Count(), 2);
                //verify that the fields are workphoneNumber and bankaccount number
                Assert.AreEqual(sellLeadResponse.ErrorMessage.ErrorReasons[0].Field, "WorkPhone");
                Assert.AreEqual(sellLeadResponse.ErrorMessage.ErrorReasons[1].Field, "BankAccountNumber");
            }
        


        [Test]
        public void TestWebserviceLeadValidationSuccessAcceptedResponse()
        {
            using (BeginScope())
            {
                EprospectsDataContext context = Resolve<EprospectsDataContext>();
                //Add a fake buyer

                Buyer buyer = new Buyer();
                buyer.Alias = "FAKE";
                buyer.Active = true;
                buyer.Country = "UK";
                buyer.EmailAddress = "fake@wonderland.com";
                buyer.FixedAmount = 122;
                buyer.Name = "FAKE";
                buyer.Username = "fake";
                buyer.Password = "default";
                buyer.IntegrationUrl = "fakesuccess.com";
                buyer.RefKey = "FAKE";
                context.Buyers.InsertOnSubmit(buyer);

                //Add to buyers to delete on the tear down

                _buyersToDelete = new List<Buyer>();
                _buyersToDelete.Add(buyer);

                context.SubmitChanges();

                _buyerID = buyer.BuyerId;
            }
            using (BeginScope())
            {
                EprospectsDataContext context = new EprospectsDataContext();

                Billing billing = new Billing();
                billing.BuyerId = _buyerID;
                billing.SellerId = _sellerID;
                billing.BillingTypeId = (int) ValueEnums.BillingTypes.FixedPrice;
                billing.Value = 2;
                context.Billings.InsertOnSubmit(billing);
                _billingsToDelete = new List<Billing>();
                _billingsToDelete.Add(billing);

                context.SubmitChanges();
            }

            using (BeginScope())
            {
                //Arrange
                ServiceSoapClient client = new ServiceSoapClient();

                //Act

                PingtreeWebService.LeadRequest lead = GetLeadRequest();
                lead.BankAccountNumber = "99876545";
                lead.WorkPhone = "02938276356";

                PingtreeWebService.SellLeadResponse sellLeadResponse = client.SubmitLead(lead);

                List<LeadBought> leadBoughts = null;


                using (BeginScope())
                {
                    EprospectsDataContext context = Resolve<EprospectsDataContext>();

                    leadBoughts = (from leadBought in context.LeadBoughts
                                   where leadBought.BuyerId == _buyerID
                                        select leadBought).ToList();    
                }

                Assert.IsNotNull(leadBoughts);
                Assert.AreEqual(1, leadBoughts.Count);
                Assert.AreEqual(sellLeadResponse.Result, ESellLeadResponse.Accepted);
                
            }
        }

                #region helpers

        private PingtreeWebService.LeadRequest GetLeadRequest()
        {
            PingtreeWebService.LeadRequest lead = new PingtreeWebService.LeadRequest();
            lead.AddressLine1 = "12 Bellefields road";
            lead.AddressLine2 = "flat 2";
            lead.ApplicationDate = DateTime.Now;
            lead.BankAccountNumber = "0978897";
            lead.BankSortcode = "234423";
            lead.Country = "UK";
            lead.County = "London";
            lead.DebitCardType = "Visa";
            lead.DirectDeposit = true;
            lead.Dob = DateTime.Now.AddYears(-35);
            lead.EmailAddress = "fakeapplicant@gmail.com";
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
#endregion
    }
}
