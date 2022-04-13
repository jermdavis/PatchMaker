using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PatchMaker.App
{

    public static class TreeNodeExtensions
    {
        private static string fetchFirstAttribute(TreeNode node, Func<XAttribute, string> func)
        {
            var element = node.Tag as XElement;

            if (element == null)
            {
                return string.Empty;
            }

            if (!element.Attributes().Any())
            {
                return string.Empty;
            }

            var attr = element.Attributes().First();

            if (attr == null)
            {
                return string.Empty;
            }

            return func(attr);
        }

        public static string FirstAttributeName(this TreeNode node)
        {
            return fetchFirstAttribute(node, a => a.Name.ToString());
        }

        public static string FirstAttributeValue(this TreeNode node)
        {
            return fetchFirstAttribute(node, a => a.Value);
        }

        private static string computeAppropriateName(XAttribute attr, XElement parentElement)
        {
            var name = attr.Name.LocalName;

            if(!string.IsNullOrWhiteSpace(attr.Name.NamespaceName))
            {
                var root = parentElement.Document.Root;

                foreach(var nsAttr in root.Attributes())
                {
                    if(nsAttr.Value == attr.Name.NamespaceName)
                    {
                        return $"{nsAttr.Name.LocalName}:{name}";
                    }    
                }
            }

            return name;
        }

        public static string DefaultXPath(this TreeNode node)
        {
            string xPath = string.Empty;
            var currentNode = node;

            while (currentNode != null)
            {
                if (currentNode.Tag != null)
                {
                    var xElement = currentNode.Tag as XElement;
                    xPath += "/" + xElement.Name.LocalName;

                    if (xElement.Attributes().Any())
                    {
                        var query = string.Empty;
                        foreach (var xAttr in xElement.Attributes())
                        {
                            if(xAttr.IsIgnorable())
                            {
                                // we never want to query on the patch namespace, if it's in the source file?
                                continue;
                            }

                            var attrVal = xAttr.Value;
                            var quoteChar = '\'';

                            // if the attribute value we're testing for contains a single quote char,
                            // make sure the surrounding xpath clause does not use single quotes itself
                            if (attrVal.Contains("'"))
                            {
                                quoteChar = '"';
                            }

                            var clause = $"@{computeAppropriateName(xAttr, xElement)}={quoteChar}{attrVal}{quoteChar}";
                            if (query.Length > 0)
                            {
                                query += " and ";
                            }
                            query += clause;
                        }

                        if (query.Length > 0)
                        {
                            xPath += $"[{query}]";
                        }
                    }
                }

                currentNode = currentNode.FirstNode;
            }

            return xPath;
        }

        public static void ExpandAncestors(this TreeNode node)
        {
            while(node != null)
            {
                node.Expand();
                node = node.Parent;
            }
        }

        public static void HighlightNodesRecursive(this TreeNode node, string textToFind)
        {
            if(node == null)
            {
                return;
            }

            if (node.Text.IndexOf(textToFind, StringComparison.OrdinalIgnoreCase) >= 0 && !string.IsNullOrWhiteSpace(textToFind))
            {
                if (node is HighlightableTreeNode)
                {
                    (node as HighlightableTreeNode).SetHighlight();
                    node.ExpandAncestors();
                }
            }
            else
            {
                if (node is HighlightableTreeNode)
                {
                    (node as HighlightableTreeNode).RemoveHighlight();
                }
            }

            foreach(TreeNode child in node.Nodes)
            {
                HighlightNodesRecursive(child, textToFind);
            }
        }

        public static void MoveDown(this ListBox list)
        {
            var idx = list.SelectedIndex + 1;

            var itm = list.Items[idx];
            list.Items.RemoveAt(idx);

            list.Items.Insert(list.SelectedIndex, itm);
        }

        public static void MoveUp(this ListBox list)
        {
            var idx = list.SelectedIndex - 1;

            var itm = list.Items[list.SelectedIndex];
            list.Items.RemoveAt(list.SelectedIndex);

            list.Items.Insert(idx, itm);
        }
    }

}