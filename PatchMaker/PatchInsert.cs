﻿using System;
using System.Xml.Linq;

namespace PatchMaker
{

    public class PatchInsert : BasePatch
    {
        public string XPathForParent { get; }
        public string XPathForOrder { get; }
        public ElementInsertPosition Position { get; }
        public XElement NewElement { get; }

        public PatchInsert(string xPathForParent, ElementInsertPosition position, string xPathForOrder, XElement newElement, ConfigRule[] roleBasedRules = null) : base(roleBasedRules)
        {
            if (string.IsNullOrWhiteSpace(xPathForParent))
            {
                throw new ArgumentNullException(nameof(xPathForParent));
            }
            XPathForParent = xPathForParent;

            Position = position;

            if (string.IsNullOrWhiteSpace(xPathForOrder))
            {
                throw new ArgumentNullException(nameof(xPathForOrder));
            }
            XPathForOrder = xPathForOrder;

            NewElement = newElement ?? throw new ArgumentNullException(nameof(newElement));
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

            // Add patch:{Position}="{XPathForOrder}" attribute
            var newNode = new XElement(NewElement);
            newNode.Add(new XAttribute(Namespaces.Patch + Position.ToString().ToLower(), XPathForOrder));

            RemoveCoreNamespaces(patchXml, newNode);

            ApplyRuleBasedConfig(patchXml, newNode);

            // append PatchXml element as child
            var currentPatchNode = base.CopyAncestorsAndSelf(targetElement, patchXml.Root);
            currentPatchNode.Add(newNode);
        }

        public override string ToString()
        {
            var rules = RoleBasedRules == null ? string.Empty : $" [Rules: {RoleBasedRules.Length}]";
            return $"INSERT: {Position} {XPathForOrder}{rules}";
        }
    }

}
