using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpPingtree.Datalayer.ExternalsRepository.Buyers.Xml
{
    public class XmlDeclarationCustom : IXmlRender
    {
        private string _version;
        private string _encoding;

        public XmlDeclarationCustom(string version, string encoding)
        {
            _version = version;
            _encoding = encoding;
        }

        public void WriteTo(System.Xml.XmlWriter writer, bool includeWhitespace)
        {
            writer.WriteRaw("<?xml version=\"");
            writer.WriteRaw(_version);
            writer.WriteRaw("\" encoding=\"");
            writer.WriteRaw(_encoding);
            writer.WriteRaw("\" ?>");
        }
    }
}
