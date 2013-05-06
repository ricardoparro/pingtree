using System.Collections.Generic;
using System.Linq;
using EpPingtree.Datalayer.Interfaces;
using EpPingtree.Model;
using EpPingtree.Services.NewLead;
using EpPingtree.Tests.PingtreeWebService;
using Moq;
using NUnit.Framework;

namespace EpPingtree.Tests.UnitTests.WebService
{
    [TestFixture]
    public class WebServiceTest : BaseTestWithTestClass<ApplyLeadService>
    {
        private List<Seller> _sellersToDelete;


        [TestFixtureSetUp]
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
                context.SubmitChanges();

                //Add to sellers list do delete on the tear down
                _sellersToDelete = new List<Seller>();

                _sellersToDelete.Add(seller);
            }

        }

        [TearDown]
        public void AfterTests()
        {
            using (BeginScope())
            {
                EprospectsDataContext context = Resolve<EprospectsDataContext>();

                foreach (Seller seller in _sellersToDelete)
                {
                    Seller firstOrDefault = (from seller1 in context.Sellers
                                             where seller1.SellerId == seller.SellerId
                                             select seller1).FirstOrDefault();
                    context.Sellers.DeleteOnSubmit(firstOrDefault);
                    context.SubmitChanges();
                }
            }
        }
        [Test]
        public void TestWebserviceLeadValidationFails()
        {
            using (BeginScope())
            {
                //Arrange
                ServiceSoapClient client = new ServiceSoapClient();

                //Act
                LeadRequest lead = new LeadRequest();
                lead.SellerName = "FakeSeller";
                SellLeadResponse sellLeadResponse = client.SubmitLead(lead);

                //Assert
                Assert.IsNotNull(sellLeadResponse);
                Assert.AreEqual(ESellLeadResponse.Invalid, sellLeadResponse.Result);
            }
        }
    }
}
