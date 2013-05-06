using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace EpPingtree.Datalayer.ExternalsRepository.Buyers.Xml
{
    public class XElementWithAttrib : XElement, IXmlRender
    {
        public string ElementName { get; private set; }
        public XAttributeCustom[] AttributesCustom { get; private set; }

        public XElementWithAttrib(string elementName, object content)
            : this(elementName, new[] {content})
        {
        }

        public XElementWithAttrib(string elementName, object[] content)
            : this(elementName, null, content)
        {
        }

        public XElementWithAttrib(string elementName, XAttributeCustom attribute, object content)
            : this(elementName, new[] {attribute}, new[] {content})
        {
        }

        public XElementWithAttrib(string elementName, XAttributeCustom[] attribute, object content)
            : this(elementName, attribute, new[] {content})
        {
        }

        public XElementWithAttrib(string elementName, XAttributeCustom[] attributes, object[] content)
            : base("MyNode", content)
        {
            ElementName = elementName;
            AttributesCustom = attributes;
        }

        public void WriteTo(XmlWriter writer, bool includeWhitespace)
        {
            if (includeWhitespace)
                writer.WriteWhitespace(Environment.NewLine);

            writer.WriteStartElement(ElementName);

            if (AttributesCustom != null && AttributesCustom.Length > 0)
            {
                foreach (XAttributeCustom attribute in AttributesCustom)
                    attribute.WriteTo(writer, includeWhitespace); //Call custom render
            }

            bool hadChildNodes = false;

            if (HasElements)
            {
                foreach (XElementWithAttrib child in Nodes())
                {
                    child.WriteTo(writer, includeWhitespace);
                }

                hadChildNodes = true;
            }
            else
                //This element has just a text value
                writer.WriteValue(Value);

            if (hadChildNodes)
                //Write a new line before the End element
                writer.WriteWhitespace(Environment.NewLine);

            writer.WriteFullEndElement();
        }



    }
}
