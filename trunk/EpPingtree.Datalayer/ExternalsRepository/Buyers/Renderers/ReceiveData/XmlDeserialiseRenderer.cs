using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EpPingtree.Datalayer.Interfaces.Xml;

namespace EpPingtree.Datalayer.ExternalsRepository.Buyers.Renderers.ReceiveData
{
    public class XmlDeserialiseRenderer : BaseReceiveDataRenderer
    {
        private readonly IXMLSerialisation _xmlSerialisation;

        public string ReadToChildNodeFirst { get; set; }
        public string ReadToChildNamespaceFirst { get; set; }

        public XmlDeserialiseRenderer(IXMLSerialisation xmlSerialisation)
        {
            _xmlSerialisation = xmlSerialisation;
        }

        public override TBuyerModel ConvertBuyerResponse<TBuyerModel>(string response)
        {
            return _xmlSerialisation.DeserialiseXML<TBuyerModel>(response, ReadToChildNodeFirst, ReadToChildNamespaceFirst);
        }
    }
}
