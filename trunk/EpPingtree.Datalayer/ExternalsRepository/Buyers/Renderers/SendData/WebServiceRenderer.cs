using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Datalayer.ExternalsRepository.Buyers.Xml;
using EpPingtree.Datalayer.Interfaces.Communication;

namespace EpPingtree.Datalayer.ExternalsRepository.Buyers.Renderers.SendData
{
    public class WebServiceRenderer : XmlSerialiseRenderer
    {
        public WebServiceRenderer(IWebRequestRepository webRequestRepo)
            : base(webRequestRepo)
        {
            ShouldAddSoapEnvelope = true;
        }

        /// <summary>
        /// Set the actual operation we are calling on the Web Service
        /// </summary>
        public string WebServiceMethodName { get; set; }

        /// <summary>
        /// The namespace that is next to the Web Service method name on the Request XML
        /// </summary>
        public string WebServiceMethodNameNamespace { get; set; }

        /// <summary>
        /// Whether to put the soap envelope around the main xml
        /// </summary>
        public bool ShouldAddSoapEnvelope { get; set; }


        public override string PrepareRequest(object[] requestNodes)
        {
            IncludeWhitespace = false;

            object[] webServiceReqNodes = requestNodes;

            if (ShouldAddSoapEnvelope)
            {
                //Add the soap action request header
                string soapAction = WebServiceMethodNameNamespace;

                if (!soapAction.EndsWith("/"))
                    soapAction = soapAction + "/";

                soapAction = soapAction + WebServiceMethodName;
                RequestHeaders.Add("SOAPAction", "\"" + soapAction + "\"");

                //Create the soap nodes to put around the actual payload
                XAttributeCustom[] soapAttributes = new[] { 
                    new XAttributeCustom("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                    new XAttributeCustom("xmlns:xsd", "http://www.w3.org/2001/XMLSchema"),
                    new XAttributeCustom("xmlns:soap", "http://schemas.xmlsoap.org/soap/envelope/") 
                };

                webServiceReqNodes = new object[] 
                                    {
                                            new XmlDeclarationCustom("1.0", "utf-8"),
                                            new XElementWithAttrib("soap:Envelope", soapAttributes,
                                                                    //Child node
                                                                    new XElementWithAttrib("soap:Body", 
                                                                        new XElementWithAttrib(WebServiceMethodName, 
                                                                        new XAttributeCustom("xmlns", WebServiceMethodNameNamespace), requestNodes)
                                                                ) //End soap body
                                                ) //Soap envelope
                                        };
            }

            return base.PrepareRequest(webServiceReqNodes);
        }
    }
}
