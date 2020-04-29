using System;
using System.Linq;
using System.Xml.Linq;

namespace PatchMaker
{
    public abstract class BaseAttributeChange : BasePatch
    {
        public string XPathForElement { get; }
        public string AttributeName { get; }
        public string AttributeValue { get; }

        public BaseAttributeChange(string xPathForElement, string attributeName, string attributeValue)
        {
            if (string.IsNullOrWhiteSpace(xPathForElement))
            {
                throw new ArgumentNullException(nameof(xPathForElement));
            }
            XPathForElement = xPathForElement;

            if (string.IsNullOrWhiteSpace(attributeName))
            {
                throw new ArgumentNullException(nameof(attributeName));
            }
            AttributeName = attributeName;

            if (attributeValue == null)
            {
                throw new ArgumentNullException(nameof(attributeValue));
            }
            AttributeValue = attributeValue;
        }

        protected abstract void applyPatch(XElement currentPatchNode);

        public override void ApplyPatchElement(XDocument sourceXml, XDocument patchXml)
        {
            // select element
            var xns = sourceXml.MakeNamespaceManager();
            var targetElement = sourceXml.SafeXPathSelectElement(XPathForElement, xns);

            if (targetElement == null)
            {
                patchXml.AddFirst(new XComment($"ERROR: Patch '{this.GetType().Name}' targeting '{XPathForElement}' found zero elements in source XML, and cannot be applied."));
                return;
            }

            // copy ancestors-and-self
            var currentPatchNode = patchXml.Root;
            var ancestors = targetElement.AncestorsAndSelf().Reverse();
            foreach (var ancestor in ancestors)
            {
                if (!currentPatchNode.ShallowEquals(ancestor))
                {
                    var child = currentPatchNode.Element(ancestor.Name);

                    if (child == null || !child.ShallowEquals(ancestor))
                    {
                        var newSourceNode = new XElement(ancestor.Name);
                        currentPatchNode.Add(newSourceNode);
                        currentPatchNode = newSourceNode;
                    }
                    else
                    {
                        currentPatchNode = currentPatchNode.Element(ancestor.Name);
                    }
                }
            }

            // if nesc, copy other attributes from source doc!
            foreach (var attr in targetElement.Attributes())
            {
                // don't copy existing patch stuff
                if(attr.Name.Namespace.IsIgnorable())
                {
                    continue;
                }

                if (currentPatchNode.Attribute(attr.Name) == null)
                {
                    currentPatchNode.Add(attr);
                }
            }

            applyPatch(currentPatchNode);
        }
    }

}
