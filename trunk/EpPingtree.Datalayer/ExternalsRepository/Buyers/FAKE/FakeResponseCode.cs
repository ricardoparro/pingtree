using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace EpPingtree.Datalayer.ExternalsRepository.Buyers.FAKE
{
    public enum FakeResponseCode
    {
        [XmlEnum("0")]
        Failed = 0,

        [XmlEnum("1")]
        Accepted = 1,

        [XmlEnum("-1")]
        XmlError = 2
    }
}
