using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace PatchMaker
{
    public abstract class BasePatch
    {
        public ConfigRule[] RoleBasedRules { get; }

        public BasePatch(ConfigRule[] roleBasedRules = null)
        {
            RoleBasedRules = roleBasedRules;
        }

        public abstract void ApplyPatchElement(XDocument sourceXml, XDocument patchXml);

        private bool RequiresAttributeCopy(XElement node)
        {
            if (node != null && node.Parent != null)
            {
                var nodeName = node.Name;

                var siblings = node.Parent.Elements(nodeName);

                return siblings.Count() > 1;
            }

            return false;
        }

        private XElement MatchElement(XElement node, XElement ancestor, bool copyAttrs)
        {
            if (copyAttrs)
            {
                var nodes = node.Elements(ancestor.Name);

                if (nodes.Count() == 0)
                {
                    return null;
                }

                foreach (var possibleNode in nodes)
                {
                    foreach (var attr in possibleNode.Attributes())
                    {
                        var ancestorAttr = ancestor.Attribute(attr.Name);
                        if (ancestorAttr == null || ancestorAttr.Value != attr.Value)
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

        private XElement PerformCopy(XElement root, IEnumerable<XElement> ancestors)
        {
            var currentPatchNode = root;

            foreach (var ancestor in ancestors)
            {
                if (currentPatchNode.Name != ancestor.Name)
                {
                    bool copyAttrs = RequiresAttributeCopy(ancestor);
                    var child = MatchElement(currentPatchNode, ancestor, copyAttrs);

                    if (child == null)
                    {
                        var newSourceNode = new XElement(ancestor.Name);

                        if (copyAttrs)
                        {
                            foreach (var attr in ancestor.Attributes())
                            {
                                // don't copy patch:source attributes!
                                if (attr.Name.Namespace.IsIgnorable())
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

        protected XElement CopyAncestors(XElement targetElement, XElement root)
        {
            var ancestors = targetElement.Ancestors().Reverse();
            return PerformCopy(root, ancestors);
        }

        protected XElement CopyAncestorsAndSelf(XElement targetElement, XElement root)
        {
            var ancestors = targetElement.AncestorsAndSelf().Reverse();
            return PerformCopy(root, ancestors);
        }

        protected void RemoveCoreNamespaces(XDocument patchXml, XElement newNode)
        {
            var rootNode = patchXml.Root;
            foreach (var attr in newNode.Attributes())
            {
                if (attr.Name.Namespace.NamespaceName == Namespaces.XmlnsUri)
                {
                    // when a namespace declaration exists at the patch root, just remove it from this node
                    // otherwise copy it to the patch root and then remove it from here.
                    var match = rootNode.Attributes().Where(a => a.Value == attr.Value).Any();
                    if (!match)
                    {
                        patchXml.Root.Add(attr);
                    }
                    attr.Remove();
                }
            }
        }
    }

}