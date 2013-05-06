using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using EpPingtree.Datalayer.Interfaces;
using EpPingtree.Datalayer.Interfaces.Buyers;
using EpPingtree.Datalayer.Interfaces.Communication;
using EpPingtree.Datalayer.Interfaces.Xml;
using EpPingtree.Model;
using EpPingtree.Tests.TestData;
using Moq;
using NUnit.Framework;

namespace EpPingtree.Tests.UnitTests.DataLayer.Buyers
{
    public class FakeBuyerTest : UnitTestBase<IFakeBuyer>
    {
        private string _xmlSent;

        [SetUp]
        public void InjectMocks()
        {
            MockContainer.AddNeverMockedType<IXMLSerialisation>();

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EpPingtree.Tests.TestData.Buyers.FAKE.Response.FakeResponse.xml");
            StreamReader reader = new StreamReader(stream);

            string successXML = reader.ReadToEnd();

            Mock<IWebRequestRepository> webRequestRepository = new Mock<IWebRequestRepository>();
            webRequestRepository.Setup(a => a.PostRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(successXML)
                .Callback<string, string, Dictionary<string, string>>((url, request, headers) => _xmlSent = request);

            InjectMock(webRequestRepository);

        }

        [Test]
        public void TestSampleFromAPI()
        {
            Lead lead = ModelPopulateFactory.GetLead();

            using (BeginScope())
            {
                (ClassToTest as IBuyerRepository<Lead>).SellLead(lead, "whateverintegrationLink");
            }

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("EpPingtree.Tests.TestData.Buyers.FAKE.Request.FakeRequest.xml");
            StreamReader reader = new StreamReader(stream);

            string expectedXML = reader.ReadToEnd();
            expectedXML = expectedXML.Replace("  ", "");
            expectedXML = expectedXML.Replace(Environment.NewLine, "");
            
            _xmlSent = _xmlSent.Replace("  ", "");
            _xmlSent = _xmlSent.Replace(Environment.NewLine, "");
            Assert.AreEqual(expectedXML, _xmlSent);

        }
    }
}
