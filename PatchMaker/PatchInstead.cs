using System;
using System.Xml.Linq;

namespace PatchMaker
{
    public class PatchInstead : BasePatch
    {
        public string XPathForParent { get; }
        public string XPathForReplacement { get; }
        public XElement Replacement { get; }

        public PatchInstead(string xPathForParent, string xPathForReplacement, XElement replacement, ConfigRule[] roleBasedRules = null) : base(roleBasedRules)
        {
            if (string.IsNullOrWhiteSpace(xPathForParent))
            {
                throw new ArgumentNullException(nameof(xPathForParent));
            }
            XPathForParent = xPathForParent;

            if (string.IsNullOrWhiteSpace(xPathForReplacement))
            {
                throw new ArgumentNullException(nameof(xPathForReplacement));
            }
            XPathForReplacement = xPathForReplacement;

            Replacement = replacement ?? throw new ArgumentNullException(nameof(replacement));
        }

        public override void ApplyPatchElement(XDocument sourceXml, XDocument patchXml)
        {
            // select element
            var xns = sourceXml.MakeNamespaceManager();
            var targetElement = sourceXml.SafeXPathSelectElement(XPathForParent, xns);

            if (targetElement == null)
            {
                patchXml.AddFirst(new XComment($"ERROR: Patch '{this.GetType().Name}' targeting '{XPathForParent}' found zero elements in source XML, and cannot be applied."));
                return;
            }

            // append patch:instead instruction
            var newElement = new XElement(Replacement);
            newElement.Add(new XAttribute(Namespaces.Patch + "instead", XPathForReplacement));

            // add new xml
            var currentPatchNode = base.CopyAncestorsAndSelf(targetElement, patchXml.Root);
            RemoveCoreNamespaces(patchXml, newElement);
            ApplyRuleBasedConfig(patchXml, newElement);
            currentPatchNode.Add(newElement);
        }

        public override string ToString()
        {
            var rules = RoleBasedRules == null ? string.Empty : $" [Rules: {RoleBasedRules.Length}]";
            return $"INSTEAD: {XPathForParent} {XPathForReplacement}{rules}";
        }
    }

}
