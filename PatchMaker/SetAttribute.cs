using System.Xml.Linq;

namespace PatchMaker
{

    public class SetAttribute : BaseAttributeChange
    {
        public SetAttribute(string xPathForElement, string attributeName, string attributeValue, ConfigRule[] roleBasedRules = null)
            : base(xPathForElement, attributeName, attributeValue, roleBasedRules)
        {
        }

        protected override XElement ApplyPatch(XElement currentPatchNode)
        {
            var child = new XAttribute(Namespaces.Set + AttributeName, AttributeValue);
            
            // append set:attribute
            currentPatchNode.Add(child);

            return currentPatchNode;
        }

        public override string ToString()
        {
            var rules = RoleBasedRules == null ? string.Empty : $" [Rules: {RoleBasedRules.Length}]";
            return $"ATTR: (SET) {XPathForElement} {AttributeName}{rules}";
        }
    }

}