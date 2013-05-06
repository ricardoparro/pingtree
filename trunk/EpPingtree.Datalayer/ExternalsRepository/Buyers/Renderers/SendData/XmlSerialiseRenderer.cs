using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using EpPingtree.Datalayer.ExternalsRepository.Buyers.Xml;
using EpPingtree.Datalayer.Interfaces.Communication;

namespace EpPingtree.Datalayer.ExternalsRepository.Buyers.Renderers.SendData
{
    public class XmlSerialiseRenderer : BaseSendDataRenderer
    {
        #region Properties

        public bool IncludeWhitespace { get; set; }
        public string UrlToSendTo { get; set; }

        protected Dictionary<string, string> RequestHeaders;

        #endregion

        #region Constructor

        private readonly IWebRequestRepository _webRequestRepo;

        public XmlSerialiseRenderer(IWebRequestRepository webRequestRepo)
        {
            _webRequestRepo = webRequestRepo;

            RequestHeaders = new Dictionary<string, string>();
            RequestHeaders.Add("content-type", @"text/xml;");
        }

        #endregion

        #region BaseSendDataRenderer overrides

        public override string PrepareRequest(object[] requestNode)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter writer = new XmlTextWriter(sw);

            foreach (IXmlRender item in requestNode)
            {
                item.WriteTo(writer, IncludeWhitespace);
            }

            //Return the string that is returned
            return sw.ToString();
        }

        public override string SendRequest(string requestString)
        {
            //_webRequestRepo.Timeout = MsAllowedToDecide;

            string responseXML = _webRequestRepo.PostRequest(UrlToSendTo, requestString, RequestHeaders);

            return responseXML;
        }

        #endregion
    }
}
