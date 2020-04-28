using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PatchMaker
{
    public abstract class BasePatch
    {
        public abstract void ApplyPatchElement(XDocument sourceXml, XDocument patchXml);

        private bool requiresAttributeCopy(XElement node)
        {
            if(node != null && node.Parent != null)
            {
                var nodeName = node.Name;

                var siblings = node.Parent.Elements(nodeName);

                return siblings.Count() > 1;
            }

            return false;
        }

        private XElement matchElement(XElement node, XElement ancestor, bool copyAttrs)
        {
            if(copyAttrs)
            {
                var nodes = node.Elements(ancestor.Name);

                if(nodes.Count() == 0)
                {
                    return null;
                }

                foreach(var possibleNode in nodes)
                {
                    foreach(var attr in possibleNode.Attributes())
                    {
                        var ancestorAttr = ancestor.Attribute(attr.Name);
                        if(ancestorAttr == null || ancestorAttr.Value != attr.Value)
                        {
                            return null;
                        }
                    }

                    return possibleNode;
                }

                return null;
            }
            else
            {
                return node.Element(ancestor.Name);
            }
        }

        private XElement performCopy(XElement root, IEnumerable<XElement> ancestors)
        {
            var currentPatchNode = root;

            foreach (var ancestor in ancestors)
            {
                if (currentPatchNode.Name != ancestor.Name)
                {
                    bool copyAttrs = requiresAttributeCopy(ancestor);
                    var child = matchElement(currentPatchNode, ancestor, copyAttrs);

                    if (child == null)
                    {
                        var newSourceNode = new XElement(ancestor.Name);

                        if (copyAttrs)
                        {
                            foreach (var attr in ancestor.Attributes())
                            {
                                // don't copy patch:source attributes!
                                if(attr.Name.Namespace.IsIgnorable())
                                {
                                    continue;
                                }
                                newSourceNode.Add(new XAttribute(attr));
                            }
                        }
                        currentPatchNode.Add(newSourceNode);
                        currentPatchNode = newSourceNode;
                    }
                    else
                    {
                        currentPatchNode = currentPatchNode.Element(ancestor.Name);
                    }
                }
            }

            return currentPatchNode;
        }

        protected XElement CopyAncestors(XElement targetElement, XElement root, bool copyAttrs = false)
        {
            var ancestors = targetElement.Ancestors().Reverse();
            return performCopy(root, ancestors);
        }

        protected XElement CopyAncestorsAndSelf(XElement targetElement, XElement root, bool copyAttrs = false)
        {
            var ancestors = targetElement.AncestorsAndSelf().Reverse();
            return performCopy(root, ancestors);
        }
    }

}