using System;
using System.Linq;
using System.Xml.Linq;

namespace PatchMaker
{

    public class PatchDelete : BasePatch
    {
        public string XPathForElement { get; }

        public PatchDelete(string xPathForElement)
        {
            if(string.IsNullOrWhiteSpace(xPathForElement))
            {
                throw new ArgumentNullException(nameof(xPathForElement));
            }
            XPathForElement = xPathForElement;
        }

        public override void ApplyPatchElement(XDocument sourceXml, XDocument patchXml)
        {
            // select element
            var targetElement = sourceXml.SafeXPathSelectElement(XPathForElement);

            if (targetElement == null)
            {
                patchXml.AddFirst(new XComment($"ERROR: Patch '{this.GetType().Name}' targeting '{XPathForElement}' found zero elements in source XML, and cannot be applied."));
                return;
            }

            // copy element with attributes
            var newTargetElement = new XElement(targetElement.Name);
            foreach(var attr in targetElement.Attributes())
            {
                // don't copy anything from the patch namespace!
                if(attr.Name.Namespace.IsIgnorable())
                {
                    continue;
                }
                newTargetElement.Add(attr);
            }
            var currentPatchNode = base.CopyAncestors(targetElement, patchXml.Root);
            currentPatchNode.Add(newTargetElement);

            // add child element for delete
            var deleteElement = new XElement(Namespaces.Patch + "delete");
            newTargetElement.Add(deleteElement);
        }

        public override string ToString()
        {
            return $"DELETE: {XPathForElement}";
        }
    }

}