using System.Xml.Linq;

namespace PatchMaker
{

    public class SetAttribute : BaseAttributeChange
    {
        public SetAttribute(string xPathForElement, string attributeName, string attributeValue)
            : base(xPathForElement, attributeName, attributeValue)
        {
        }

        protected override void ApplyPatch(XElement currentPatchNode)
        {
            // append set:attribute
            currentPatchNode.Add(new XAttribute(Namespaces.Set + AttributeName, AttributeValue));
        }

        public override string ToString()
        {
            return $"ATTR: (SET) {XPathForElement} {AttributeName}";
        }
    }

}