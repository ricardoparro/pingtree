using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Datalayer.ExternalsRepository.Buyers.Renderers.ReceiveData;
using EpPingtree.Datalayer.ExternalsRepository.Buyers.Renderers.SendData;
using EpPingtree.Datalayer.Interfaces;
using EpPingtree.Datalayer.Interfaces.Buyers;
using EpPingtree.Datalayer.Interfaces.Files;
using EpPingtree.Model;
using EpPingtree.Model.Apply.Response;

namespace EpPingtree.Datalayer.ExternalsRepository.Buyers.WageDayAdvance
{
    public class WageDayAdvanceBuyer : BaseBuyer<WebServiceRenderer, XmlDeserialiseRenderer, WageDayAdvanceResponse>, IWageDayAdvanceBuyer    
   {
        public WageDayAdvanceBuyer(WebServiceRenderer sendRenderer, XmlDeserialiseRenderer receiveRenderer, IFileRepository fileRepository, IConfigRepository configRepository)
            : base(sendRenderer, receiveRenderer, fileRepository, configRepository)
        {
        }

        protected override void PrepareSendRenderer(string integrationUrl)
        {
            SendRenderer.UrlToSendTo = integrationUrl;
            SendRenderer.WebServiceMethodName = "";
            SendRenderer.WebServiceMethodNameNamespace = "https://www.wagedayadvance.co.uk/";
        }

        protected override object[] GetRequestNodes(Lead request)
        {
            throw new NotImplementedException();
        }

        protected override SellLeadResponse ConvertBuyerResponse(WageDayAdvanceResponse buyerModel)
        {
            throw new NotImplementedException();
        }
   }
}
