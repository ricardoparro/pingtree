using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using EpPingtree.Datalayer.Interfaces.Xml;

namespace EpPingtree.Datalayer.Repository.XML
{
    public class XMLSerialisation : BaseRepository, IXMLSerialisation
    {

        #region Generate XML

        public string GenerateXml<T>(T node)
        {
            return GenerateXml(node, a => new XmlTextWriter(a));
        }

        private string GenerateXml<T>(T node, Func<StreamWriter, XmlTextWriter> getXmlTextWriter)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces xmlnsEmpty = new XmlSerializerNamespaces();
            xmlnsEmpty.Add("", ""); //Add so doesn't add any namespace declarations

            MemoryStream stream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(stream);

            XmlTextWriter xmlTextWriter = getXmlTextWriter(streamWriter);

            serializer.Serialize(xmlTextWriter, node, xmlnsEmpty);
            stream.Seek(0, SeekOrigin.Begin);

            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        #endregion

        #region Deserialise

        public T DeserialiseXML<T>(string xml)
        {
            return DeserialiseXML<T>(xml, string.Empty, string.Empty);
        }

        public T DeserialiseXML<T>(string responseXML, string parentNodeName, string parentNodeNamespace)
        {
            XmlSerializerFactory factory = new XmlSerializerFactory();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;
            settings.IgnoreWhitespace = true;

            StringReader streamReader = new StringReader(responseXML);
            XmlReader reader = XmlReader.Create(streamReader, settings);

            if (!string.IsNullOrEmpty(parentNodeName))
            {
                //Want to only deserialise a part of the response
                bool readToDescendant = reader.ReadToFollowing(parentNodeName, parentNodeNamespace);

                if (!readToDescendant)
                    throw new Exception("Didn't find " + parentNodeName);
            }

            XmlSerializer deserializer = factory.CreateSerializer(typeof(T));
            return (T)deserializer.Deserialize(reader);
        }

        #endregion
    }
}
