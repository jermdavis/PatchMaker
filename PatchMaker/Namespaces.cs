using System.Xml.Linq;

namespace PatchMaker
{
    public static class Namespaces
    {
        public static readonly string PatchUri = "http://www.sitecore.net/xmlconfig/";
        public static readonly string SetUri = "http://www.sitecore.net/xmlconfig/set/";

        public static readonly XNamespace Patch = PatchUri;
        public static readonly XNamespace Set = SetUri;
    }

}