using System.Xml.Linq;

namespace PatchMaker
{

    public class PatchAttribute : BaseAttributeChange
    {
        public PatchAttribute(string xPathForElement, string attributeName, string attributeValue)
            : base(xPathForElement, attributeName, attributeValue)
        {
        }

        protected override void ApplyPatch(XElement currentPatchNode)
        {
            // append patch:attribute element
            currentPatchNode.Add(
                new XElement(Namespaces.Patch + "attribute",
                    new XAttribute("name", AttributeName),
                    new XAttribute("value", AttributeValue)));
        }

        public override string ToString()
        {
            return $"ATTR: (PATCH) {XPathForElement} {AttributeName}";
        }
    }

}