using System.Xml.Linq;

namespace PatchMaker
{
    public static class XNamespaceExtensions
    {
        public static bool IsIgnorable(this XNamespace ns)
        {
            if(ns == "http://www.w3.org/2000/xmlns/" 
                || ns == Namespaces.Set
                || ns == Namespaces.Patch
                || ns == Namespaces.Security
                || ns == Namespaces.Role)
            {
                return true;
            }

            return false;
        }
        public static bool IsIgnorable(this XAttribute attr)
        {
            if(attr.Value == Namespaces.PatchUri
                || attr.Value == Namespaces.SetUri
                || attr.Value == Namespaces.RoleUri
                || attr.Value == Namespaces.SecurityUri)
            {
                return true;
            }

            return attr.Name.Namespace.IsIgnorable();
        }
    }

}
