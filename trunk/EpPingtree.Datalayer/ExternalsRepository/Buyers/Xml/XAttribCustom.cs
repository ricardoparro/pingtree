using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace EpPingtree.Datalayer.ExternalsRepository.Buyers.Xml
{
    public class XAttributeCustom : XAttribute, IXmlRender
    {
        private string _name;
        private string _value;

        public XAttributeCustom(string name, string value)
            : base("NotUsed", "NotUsed")
        {
            _name = name;
            _value = value;
        }
        public void WriteTo(XmlWriter writer, bool includeWhitespace)
        {
            writer.WriteAttributeString(_name, _value);
        }
    }
}
