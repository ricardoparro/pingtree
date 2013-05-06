using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EpPingtree.Datalayer.Interfaces.Xml
{
    public interface IXMLSerialisation
    {
        string GenerateXml<T>(T node);

        T DeserialiseXML<T>(string xml);
        T DeserialiseXML<T>(string responseXML, string parentNodeName, string parentNodeNamespace);
    
    }
}
