﻿using System;
using System.Xml.Linq;

namespace PatchMaker
{
    public class PatchNewChild : BasePatch
    {
        public string XPathForParent { get; }
        public XElement ChildXml { get; }

        public PatchNewChild(string xPathForParent, XElement childXml, ConfigRule[] roleBasedRules = null) : base(roleBasedRules)
        {
            if (string.IsNullOrWhiteSpace(xPathForParent))
            {
                throw new ArgumentNullException(nameof(xPathForParent));
            }
            XPathForParent = xPathForParent;

            ChildXml = childXml ?? throw new ArgumentNullException(nameof(childXml));
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

            var newNode = new XElement(ChildXml);
            ApplyRuleBasedConfig(patchXml, newNode);

            var currentPatchNode = base.CopyAncestorsAndSelf(targetElement, patchXml.Root);
            currentPatchNode.Add(newNode);
        }

        public override string ToString()
        {
            var rules = RoleBasedRules == null ? string.Empty : $" [Rules: {RoleBasedRules.Length}]";
            return $"CHILD: {XPathForParent}{rules}";
        }
    }

}