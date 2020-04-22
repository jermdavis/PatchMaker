using System.Xml.Linq;
using System.Xml.XPath;

namespace PatchMaker
{
    public static class XDocumentExtensions
    {
        public static XElement SafeXPathSelectElement(this XDocument doc, string xPath)
        {
            if (xPath == "/")
            {
                return doc.Root;
            }
            else
            {
                return doc.XPathSelectElement(xPath);
            }
        }
    }

}
