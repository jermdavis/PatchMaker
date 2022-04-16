using System.Xml.Linq;

namespace PatchMaker
{

    public class PatchAttribute : BaseAttributeChange
    {
        public PatchAttribute(string xPathForElement, string attributeName, string attributeValue, ConfigRule[] roleBasedRules = null)
            : base(xPathForElement, attributeName, attributeValue, roleBasedRules)
        {
        }

        protected override XElement ApplyPatch(XElement currentPatchNode)
        {
            var child = new XElement(Namespaces.Patch + "attribute",
                    new XAttribute("name", AttributeName),
                    new XAttribute("value", AttributeValue));

            // append patch:attribute element
            currentPatchNode.Add(child);

            return child;
        }

        public override string ToString()
        {
            var rules = RoleBasedRules == null ? string.Empty : $" [Rules: {RoleBasedRules.Length}]";
            return $"ATTR: (PATCH) {XPathForElement} {AttributeName}{rules}";
        }
    }

}