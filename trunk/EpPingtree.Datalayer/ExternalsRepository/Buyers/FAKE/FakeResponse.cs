using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using EpPingtree.Datalayer.ExternalsRepository.Buyers.FAKE;

namespace EpPingtree.Datalayer.ExternalsRepository.Buyers.FAKE
{
    [XmlRoot("RESULT")]
    public class FakeResponse
    {
        [XmlElement("CODE")]
        public FakeResponseCode Code { get; set; }

        [XmlElement("MESSAGE")]
        public string Message { get; set; }

        [XmlElement("URL")]
        public string Url { get; set; }
    }
}
