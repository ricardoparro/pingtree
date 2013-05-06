using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Model;
using EpPingtree.Model.Enums;
using EpPingtree.Services.NewLead;
using EpPingtree.Tests.PingtreeWebService;
using NUnit.Framework;

namespace EpPingtree.Tests.IntegrationTests.WebService
{
    [TestFixture]
    public class WebServiceAcceptanceRejectionTests : BaseTestWithTestClass<ApplyLeadService>
    {
        private List<Billing> _billingsToDelete;
        private List<Seller> _sellersToDelete;
        private List<Buyer> _buyersToDelete;


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

                //Add a fake buyer

                Buyer buyer = new Buyer();
                buyer.Alias = "FAKE";
                buyer.Active = true;
                buyer.Country = "UK";
                buyer.EmailAddress = "fake@wonderland.com";
                buyer.FixedAmount = 20;
                buyer.Name = "FAKE1";
                buyer.Username = "fake";
                buyer.Password = "default";
                buyer.IntegrationUrl = "fakesuccess.com";
                buyer.RefKey = "FAKE";
                context.Buyers.InsertOnSubmit(buyer);

                Buyer buyer2 = new Buyer();
                buyer2.Alias = "FAKE";
                buyer2.Active = true;
                buyer2.Country = "UK";
                buyer2.EmailAddress = "fake@wonderland.com";
                buyer2.FixedAmount = 33;
                buyer2.Name = "FAKE2";
                buyer2.Username = "fake";
                buyer2.Password = "default";
                buyer2.IntegrationUrl = "fakerejected.com";
                buyer2.RefKey = "FAKE";
                context.Buyers.InsertOnSubmit(buyer2);

                Buyer buyer3 = new Buyer();
                buyer3.Alias = "FAKE";
                buyer3.Active = true;
                buyer3.Country = "UK";
                buyer3.EmailAddress = "fake@wonderland.com";
                buyer3.FixedAmount = 32.1;
                buyer3.Name = "FAKE3";
                buyer3.Username = "fake";
                buyer3.Password = "default";
                buyer3.IntegrationUrl = "fakerejected.com";
                buyer3.RefKey = "FAKE";
                context.Buyers.InsertOnSubmit(buyer3);


                //Add to buyers to delete on the tear down

                _buyersToDelete = new List<Buyer>();
                _buyersToDelete.Add(buyer);
                _buyersToDelete.Add(buyer2);
                _buyersToDelete.Add(buyer3);



                context.SubmitChanges();

            }

            using (BeginScope())
            {
                EprospectsDataContext context = Resolve<EprospectsDataContext>();

                Seller seller = (from sel in context.Sellers
                                 where sel.SellerName == "FakeSeller"
                                 select sel).FirstOrDefault();

                Buyer buyer1 = (from buy in context.Buyers
                                where buy.Name == "FAKE1"
                                select buy).FirstOrDefault();


                Billing billing = new Billing();
                billing.BuyerId = buyer1.BuyerId;
                billing.SellerId = seller.SellerId;
                billing.BillingTypeId = (int)ValueEnums.BillingTypes.FixedPrice;
                billing.Value = 2;

                context.Billings.InsertOnSubmit(billing);

                _billingsToDelete = new List<Billing>();
                _billingsToDelete.Add(billing);






                Buyer buyer2 = (from buy in context.Buyers
                                where buy.Name == "FAKE2"
                                select buy).FirstOrDefault();


                Billing billing2 = new Billing();
                billing2.BuyerId = buyer2.BuyerId;
                billing2.SellerId = seller.SellerId;
                billing2.BillingTypeId = (int)ValueEnums.BillingTypes.FixedPrice;
                billing2.Value = 2;
                context.Billings.InsertOnSubmit(billing2);
                _billingsToDelete.Add(billing2);



                Buyer buyer3 = (from buy in context.Buyers
                                where buy.Name == "FAKE3"
                                select buy).FirstOrDefault();


                Billing billing3 = new Billing();
                billing3.BuyerId = buyer3.BuyerId;
                billing3.SellerId = seller.SellerId;
                billing3.BillingTypeId = (int)ValueEnums.BillingTypes.FixedPrice;
                billing3.Value = 2;

                context.Billings.InsertOnSubmit(billing3);
                _billingsToDelete.Add(billing3);





                context.SubmitChanges();
            }

        }

        [TearDown]
        public void DeleteTestData()
        {
            using (BeginScope())
            {
                EprospectsDataContext context = Resolve<EprospectsDataContext>();

                List<LeadBought> leadBoughts = (from leadBought in context.LeadBoughts select leadBought).ToList();

                context.LeadBoughts.DeleteAllOnSubmit(leadBoughts);

                List<LeadRejected> leadRejecteds = (from leadRejected in context.LeadRejecteds
                                                    select leadRejected).ToList();

                context.LeadRejecteds.DeleteAllOnSubmit(leadRejecteds);

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

                foreach (Buyer buyer in _buyersToDelete)
                {
                    Buyer buy = (from b in context.Buyers
                                 where b.BuyerId == buyer.BuyerId
                                 select b).FirstOrDefault();
                    context.Buyers.DeleteOnSubmit(buy);
                }


                List<Lead> leads = (from lead in context.Leads
                                    where lead.EmailAddress == "fakeapplicant@gmail.com"
                                    select lead).ToList();
                context.Leads.DeleteAllOnSubmit(leads);

                context.SubmitChanges();

            }

        }

        [Test]
        public void TestOneAccept2Rejects()
        {
            using (BeginScope())
            {
                //Arrange
                ServiceSoapClient client = new ServiceSoapClient();

                //Act

                PingtreeWebService.LeadRequest lead = GetLeadRequest();
                lead.BankAccountNumber = "99876545";
                lead.WorkPhone = "02938276356";

                PingtreeWebService.SellLeadResponse sellLeadResponse = client.SubmitLead(lead);

                EprospectsDataContext context = Resolve<EprospectsDataContext>();

                List<LeadRejected> leadRejecteds = (from leadRejected in context.LeadRejecteds
                                                    select leadRejected).OrderBy(a => a.LeadRejectedId).ToList();


                Assert.AreEqual(2, leadRejecteds.Count);

                int buyer1Id = leadRejecteds[0].BuyerId;

                Buyer buyer1ToCompare = _buyersToDelete.Where(a => a.BuyerId == buyer1Id).FirstOrDefault();
                Assert.AreEqual("FAKE2", buyer1ToCompare.Name);

                int buyer2Id = leadRejecteds[1].BuyerId;

                Buyer buyer2ToCompare = _buyersToDelete.Where(a => a.BuyerId == buyer2Id).FirstOrDefault();
                Assert.AreEqual("FAKE3", buyer2ToCompare.Name);

              

                LeadBought orderSuccess = (from leadBought in context.LeadBoughts
                                           select leadBought).FirstOrDefault();

                Assert.IsNotNull(orderSuccess);

                Buyer buyerSuccess = (from buyer in context.Buyers
                                        where buyer.BuyerId == orderSuccess.BuyerId
                                        select buyer).FirstOrDefault();

                Assert.IsNotNull(buyerSuccess);

                Assert.AreEqual("FAKE1", buyerSuccess.Name);

                Assert.IsNull(sellLeadResponse.ErrorMessage);

                Assert.AreEqual(ESellLeadResponse.Accepted, sellLeadResponse.Result);
            }
        }


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

    }
}
