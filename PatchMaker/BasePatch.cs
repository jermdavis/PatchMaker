﻿using System.Linq;
using System.Xml.Linq;

namespace PatchMaker
{
    public abstract class BasePatch
    {
        public abstract void ApplyPatchElement(XDocument sourceXml, XDocument patchXml);

        protected XElement CopyAncestors(XElement targetElement, XElement root, bool copyAttrs = false)
        {
            var currentPatchNode = root;
            var ancestors = targetElement.Ancestors().Reverse();
            foreach (var ancestor in ancestors)
            {
                if (currentPatchNode.Name != ancestor.Name)
                {
                    var child = currentPatchNode.Element(ancestor.Name);

                    if (child == null)
                    {
                        var newSourceNode = new XElement(ancestor.Name);
                        if (copyAttrs)
                        {
                            foreach (var attr in ancestor.Attributes())
                            {
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

        protected XElement CopyAncestorsAndSelf(XElement targetElement, XElement root, bool copyAttrs = false)
        {
            var currentPatchNode = root;
            var ancestors = targetElement.AncestorsAndSelf().Reverse();
            foreach (var ancestor in ancestors)
            {
                if (currentPatchNode.Name != ancestor.Name)
                {
                    var child = currentPatchNode.Element(ancestor.Name);

                    if (child == null)
                    {
                        var newSourceNode = new XElement(ancestor.Name);
                        if (copyAttrs)
                        {
                            foreach(var attr in ancestor.Attributes())
                            {
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
    }

}