using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Datalayer.Interfaces;
using EpPingtree.Model;
using EpPingtree.Model.Apply.Request;
using EpPingtree.Model.Apply.Response;
using EpPingtree.Model.Enums;
using EpPingtree.Services.NewLead;
using Moq;
using NUnit.Framework;

namespace EpPingtree.Tests.IntegrationTests
{
    [TestFixture]
    public class ApplyLeadServiceTest : BaseTestWithTestClass<ApplyLeadService>
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
                Mock<ILeadRepository> leadRepo = new Mock<ILeadRepository>();
                leadRepo.Setup(a => a.InsertLead(It.IsAny<Lead>()));
                InjectMock(leadRepo);
                //Act
                LeadRequest lead = new LeadRequest();
                lead.SellerName = "FakeSeller";
                SellLeadResponse sellLeadResponse = ClassToTest.ApplyLead(lead);

                //Assert
                Assert.IsNotNull(sellLeadResponse);
                Assert.AreEqual(BuyerEnum.ESellLeadResponse.Invalid, sellLeadResponse.Result);
            }
        }
    }
}
