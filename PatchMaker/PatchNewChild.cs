using System;
using System.Xml.Linq;

namespace PatchMaker
{
    public class PatchNewChild : BasePatch
    {
        public string XPathForParent { get; }
        public XElement ChildXml { get; }

        public PatchNewChild(string xPathForParent, XElement childXml)
        {
            if (string.IsNullOrWhiteSpace(xPathForParent))
            {
                throw new ArgumentNullException(nameof(xPathForParent));
            }
            XPathForParent = xPathForParent;

            if (childXml == null)
            {
                throw new ArgumentNullException(nameof(childXml));
            }
            ChildXml = childXml;
        }

        public override void ApplyPatchElement(XDocument sourceXml, XDocument patchXml)
        {
            // select element
            var targetElement = sourceXml.SafeXPathSelectElement(XPathForParent);

            if (targetElement == null)
            {
                throw new PatchException(nameof(PatchInsert), XPathForParent, nameof(XPathForParent));
            }

            var newNode = new XElement(ChildXml);

            // 
            var currentPatchNode = base.CopyAncestorsAndSelf(targetElement, patchXml.Root, copyAttrs: true);
            currentPatchNode.Add(newNode);
        }

        public override string ToString()
        {
            return $"CHILD: {XPathForParent}";
        }
    }

}