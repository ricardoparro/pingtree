using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using EpPingtree.Datalayer.Interfaces;
using EpPingtree.Datalayer.Interfaces.Communication;
using EpPingtree.Datalayer.Interfaces.Xml;
using Moq;

namespace EpPingtree.Tests
{
    public class XmlBaseTest<TClassToTest> : BaseTestWithTestClass<TClassToTest>
    {
        protected void SetupXMLResponse(string resourceName)
        {
            MockContainer.AddNeverMockedType<IXMLSerialisation>();
            MockContainer.AddNeverMockedType<IConfigRepository>();

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
            StreamReader reader = new StreamReader(stream);

            string successXML = reader.ReadToEnd();

            Mock<IWebRequestRepository> webRequestRepository = new Mock<IWebRequestRepository>();
            webRequestRepository.Setup(a => a.PostRequest(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>())).Returns(successXML);
            InjectMock(webRequestRepository);
        }
    }
}
