using System.Xml.Linq;

namespace PatchMaker
{

    public static class XNamespaceExtensions
    {
        public static bool IsIgnorable(this XNamespace ns, bool ignoreRoleAndSecurity = false)
        {
            if (ignoreRoleAndSecurity)
            {
                if (ns == Namespaces.Security || ns == Namespaces.Role)
                {
                    return true;
                }
            }

            if (ns == "http://www.w3.org/2000/xmlns/" || ns == Namespaces.Set || ns == Namespaces.Patch)
            {
                return true;
            }

            return false;
        }
        public static bool IsIgnorable(this XAttribute attr, bool ignoreRoleAndSecurity = false)
        {
            if (ignoreRoleAndSecurity)
            {
                if (attr.Value == Namespaces.RoleUri || attr.Value == Namespaces.SecurityUri)
                {
                    return true;
                }
            }

            if (attr.Value == Namespaces.PatchUri || attr.Value == Namespaces.SetUri)
            {
                return true;
            }

            return attr.Name.Namespace.IsIgnorable(ignoreRoleAndSecurity);
        }
    }

}