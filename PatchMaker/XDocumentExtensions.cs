using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PatchMaker
{
    public static class XDocumentExtensions
    {
        public static XmlNamespaceManager MakeNamespaceManager(this XDocument doc)
        {
            var xns = new XmlNamespaceManager(new NameTable());

            foreach (var attr in doc.Root.Attributes())
            {
                if (attr.Name.Namespace.NamespaceName == Namespaces.XmlnsUri)
                {
                    xns.AddNamespace(attr.Name.LocalName, attr.Value);
                }
            }

            return xns;
        }

        public static XElement SafeXPathSelectElement(this XDocument doc, string xPath, XmlNamespaceManager xns)
        {
            if (xPath == "/")
            {
                return doc.Root;
            }
            else
            {
                return doc.XPathSelectElement(xPath, xns);
            }
        }
    }

}
