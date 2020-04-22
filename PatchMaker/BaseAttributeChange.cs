﻿using System;
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
            var targetElement = sourceXml.SafeXPathSelectElement(XPathForElement);

            if (targetElement == null)
            {
                throw new PatchException(nameof(PatchAttribute), XPathForElement, nameof(XPathForElement));
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
                if (currentPatchNode.Attribute(attr.Name) == null)
                {
                    currentPatchNode.Add(attr);
                }
            }

            applyPatch(currentPatchNode);
        }
    }

}
