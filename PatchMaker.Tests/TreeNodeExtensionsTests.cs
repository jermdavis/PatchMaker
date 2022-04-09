using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatchMaker.App;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PatchMaker.Tests
{
    
    [TestClass]
    public class TreeNodeExtensionsTests
    {
        [TestMethod]
        public void TreeNodeExtensions_XpathForNamespacedNode_DoesNotIncludeEmptyIndexer()
        {
            var xml = XDocument.Load(@"..\..\ExampleXml\Sitecore.ContentSearch.Solr.DefaultIndexConfiguration.config");
            var tree = new TreeView();

            PatchProcessManager.MapTreeView(xml.Root, tree, null);

            var node = tree.Nodes[0];

            var xpath = TreeNodeExtensions.DefaultXPath(node);

            // the xpath generation should never create
            // "/root[]/node" when there are only namespaces to process
            // that aren't relevant to the final expression
            Assert.IsFalse(xpath.Contains("[]"));
        }
    }

}