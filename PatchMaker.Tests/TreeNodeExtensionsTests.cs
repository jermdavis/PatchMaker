using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatchMaker.App;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PatchMaker.Tests
{

    [TestClass]
    public class TreeNodeExtensionsTests
    {
        [TestMethod]
        public void TreeNodeExtensions_GeneratePatchFromElementWithXpath_DoesNotFail()
        {
            var xml = XDocument.Parse(@"<root><e name=""1"" query=""/test[@a='b']""/></root>");
            var tree = new TreeView();

            PatchProcessManager.MapTreeView(xml.Root, tree, null);

            var node = tree.Nodes[0].Nodes[0];

            var xpath = TreeNodeExtensions.DefaultXPath(node);

            var elements = xml.Root.XPathSelectElements(xpath);

            // Avoid creating xpath attribute checks where a clause containing
            // single quotes is itself wrapped in single quotes
            Assert.IsNotNull(elements);
            Assert.AreEqual(1, elements.Count());
        }

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