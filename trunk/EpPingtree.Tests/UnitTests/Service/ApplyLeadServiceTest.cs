using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using EpPingtree.Datalayer.Interfaces;
using EpPingtree.Datalayer.Interfaces.Buyers;
using EpPingtree.Datalayer.Interfaces.Communication;
using EpPingtree.Datalayer.Interfaces.Files;
using EpPingtree.Datalayer.Interfaces.Xml;
using EpPingtree.Model;
using EpPingtree.Model.Apply.Response;
using EpPingtree.Model.Enums;
using EpPingtree.Model.ServiceModel;
using EpPingtree.Services.NewLead;
using EpPingtree.Tests.TestData;
using Moq;
using NUnit.Framework;

namespace EpPingtree.Tests.UnitTests
{
    [TestFixture]
    public class ApplyLeadServiceTest : XmlBaseTest<ApplyLeadService>
    {
        private Lead request;
        private string _xmlSent;

        [SetUp]
        public void Setup()
        {
            MockContainer.AddNeverMockedType<IXMLSerialisation>();

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EpPingtree.Tests.TestData.Buyers.FAKE.Response.FakeResponse.xml");
            StreamReader reader = new StreamReader(stream);

            string successXML = reader.ReadToEnd();

            Mock<IWebRequestRepository> webRequestRepository = new Mock<IWebRequestRepository>();
            webRequestRepository.Setup(a => a.PostRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(successXML)
                .Callback<string, string, Dictionary<string, string>>((url, request, headers) => _xmlSent = request);

            InjectMock(webRequestRepository);

            //Mock File repository 
            Mock<IFileRepository> fileRepo = new Mock<IFileRepository>();
            fileRepo.Setup(a => a.CombineParts(It.IsAny<string>(), It.IsAny<string>())).Returns("C:");
            fileRepo.Setup(a => a.SaveFileContents(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            InjectMock(fileRepo);

            //Mock Lead Repository
            Mock<ILeadRepository> leadRepository = new Mock<ILeadRepository>();
            leadRepository.Setup(a => a.InsertLead(It.IsAny<Lead>()));
            InjectMock(leadRepository);

            //Mock Seller Repository
            Seller seller =new Seller();
            seller.SellerName = "Default";
            seller.Active = true;
            Mock<ISellerRepository> sellerRepository = new Mock<ISellerRepository>();
            sellerRepository.Setup(a => a.GetSellerByName(It.IsAny<string>())).Returns(seller);

            //Mock LeadBought Repository
            Mock<ILeadBoughtRepository> leadBougthRepository = new Mock<ILeadBoughtRepository>();
            leadBougthRepository.Setup(a => a.InsertLeadBought(It.IsAny<LeadBought>()));
            InjectMock(leadBougthRepository);

            //Mock LeadRejected Repository
            Mock<ILeadRejectedRepository> leadRejectedRepository = new Mock<ILeadRejectedRepository>();
            leadRejectedRepository.Setup(a => a.InsertLeadRejected(It.IsAny<LeadRejected>()));
            InjectMock(leadRejectedRepository);
         
        }

        //Test the Rejected answer for when there are no active buyers
        [Test]
        public void TestGetBuyersForLeadRejectedResult()
        {
            //Arrange
            request = ModelPopulateFactory.GetLead();

            SellLeadResponse resp = null;

            //begin a new scope
            using (BeginScope())
            {
                List<BuyerBilling> buyers = new List<BuyerBilling>();

                Mock<IBuyerConfigRepository> buyerConfigRepo = new Mock<IBuyerConfigRepository>();
                buyerConfigRepo.Setup(a => a.GetAllBuyersBillingByCountry(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>())).Returns(buyers);
                InjectMock(buyerConfigRepo);

                //Act
                resp = ClassToTest.SendLeadToBuyers(request.Country, request);

                
            }

            //Assert
            Assert.IsNotNull(resp);
            Assert.AreEqual(resp.Result, BuyerEnum.ESellLeadResponse.Rejected);
            Assert.AreEqual(resp.RedirectUrl, null);
            Assert.AreEqual(resp.ErrorMessage.ErrorReasons.Count, 1);
            Assert.AreEqual(resp.ErrorMessage.ErrorReasons[0].Reason, "There are no active Buyers");

            
        }

        //Arrange, Act, Assert
        //This test will cover the main functionality of the pingtree.
        //First test will test the retrieval of an accepted response by a fake buyer.
        [Test]
        public void TestGetBuyersForLeadAcceptedResult()
        {
            //Arrange
            request = ModelPopulateFactory.GetLead();

           
            //mock to return a list with the fake buyer
            List<BuyerBilling> buyers = ModelPopulateFactory.GetBuyerBillings();
            Mock<IBuyerConfigRepository> buyerConfigRepo = new Mock<IBuyerConfigRepository>();
            buyerConfigRepo.Setup(a => a.GetAllBuyersBillingByCountry(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<int>())).Returns(buyers);

            InjectMock(buyerConfigRepo);
           

            //Act
            SellLeadResponse response = ClassToTest.SendLeadToBuyers(request.Country, request);

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.Result, BuyerEnum.ESellLeadResponse.Accepted);
            Assert.AreEqual(response.RedirectUrl, "http://whatever.com/ApplyLead?lala=TEST");
            Assert.IsNull(response.ErrorMessage);
        }

       


    }
}
