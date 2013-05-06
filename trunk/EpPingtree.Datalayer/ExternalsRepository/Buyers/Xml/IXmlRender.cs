using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace EpPingtree.Datalayer.ExternalsRepository.Buyers.Xml
{
    public interface IXmlRender
    {
        void WriteTo(XmlWriter writer, bool includeWhitespace);
    }
}
