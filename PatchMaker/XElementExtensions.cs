using System.Xml.Linq;

namespace PatchMaker
{

    public static class XElementExtensions
    {
        public static bool ShallowEquals(this XElement a, XElement b)
        {
            if(a.Name != b.Name)
            {
                return false;
            }

            foreach(var attrA in a.Attributes())
            {
                if(attrA.Name.Namespace == "http://www.w3.org/2000/xmlns/" || attrA.Name.Namespace == Namespaces.Set || attrA.Name.Namespace == Namespaces.Patch)
                {
                    continue;
                }

                var attrB = b.Attribute(attrA.Name);

                if(attrB == null)
                {
                    return false;
                }

                if(attrA.Value != attrB.Value)
                {
                    return false;
                }
            }

            return true;
        }

        public static string FirstPathSegment(this string xPath, int dividerIndex)
        {
            var fragment = xPath.Substring(0, dividerIndex);

            if(string.IsNullOrWhiteSpace(fragment))
            {
                return "/";
            }
            else
            {
                return fragment;
            }
        }

        public static string RemainingPathSegments(this string xPath, int dividerIndex)
        {
            return xPath.Substring(dividerIndex + 1);
        }

        public static int LastPathSegmentIndex(this string xPath)
        {
            if (string.IsNullOrWhiteSpace(xPath))
            {
                return 0;
            }

            var idx = xPath.Length - 1;
            var inClause = false;

            while (idx >= 0)
            {
                var ch = xPath[idx];
                if (ch == ']')
                {
                    inClause = true;
                }
                if (ch == '[')
                {
                    inClause = false;
                }

                if (!inClause && ch == '/')
                {
                    return idx;
                }

                idx -= 1;
            }

            return 0;
        }
    }

}
