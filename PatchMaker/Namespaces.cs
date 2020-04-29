using System.Xml.Linq;

namespace PatchMaker
{
    public static class Namespaces
    {
        public static readonly string XmlnsUri = "http://www.w3.org/2000/xmlns/";

        public static readonly string PatchUri = "http://www.sitecore.net/xmlconfig/";
        public static readonly string SetUri = "http://www.sitecore.net/xmlconfig/set/";
        public static readonly string RoleUri = "http://www.sitecore.net/xmlconfig/role/";
        public static readonly string SecurityUri = "http://www.sitecore.net/xmlconfig/security/";

        public static readonly XNamespace Patch = PatchUri;
        public static readonly XNamespace Set = SetUri;
        public static readonly XNamespace Role = RoleUri;
        public static readonly XNamespace Security = SecurityUri;
    }

}