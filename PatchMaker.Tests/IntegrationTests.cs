using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatchMaker.Sitecore;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.XPath;

namespace PatchMaker.Tests
{

    [TestClass]
    public class IntegrationTests
    {
        [TestMethod]
        public void IntegrationTest_SourceXmlWithConfigNode_GivesRightAnswer_WithRoles()
        {
            var xml = System.IO.File.ReadAllText(@"..\..\ExampleXml\Sitecore.ContentSearch.Solr.DefaultIndexConfiguration.config");
            var sitecoreConfig = XDocument.Parse(xml);

            var newElement = new XElement("xml");

            var patches = new BasePatch[] {
                new PatchInsert(
                    "/configuration/sitecore/contentSearch/indexConfigurations/defaultSolrIndexConfiguration[@type='Sitecore.ContentSearch.SolrProvider.SolrIndexConfiguration, Sitecore.ContentSearch.SolrProvider']", 
                    ElementInsertPosition.Before, 
                    "initializeOnAdd", 
                    newElement)
            };

            var sut = new PatchGenerator(sitecoreConfig);

            var patchData = sut.GeneratePatchFile(patches);

            var roles = new Dictionary<string, string>();
            var newXml = SitecorePatcher.ApplyWithRoles(xml, patchData.ToString(), "testpatch.config", roles);

            Assert.IsFalse(newXml.StartsWith("<!--ERROR"));
            Assert.IsFalse(newXml.StartsWith("<error>"));
            Assert.IsTrue(newXml.Contains("<fieldMap"));
            Assert.IsTrue(newXml.Contains("<xml"));
        }

        [TestMethod]
        public void IntegrationTest_SourceXmlWithConfigNode_GivesRightAnswer_WithoutRoles()
        {
            var xml = System.IO.File.ReadAllText(@"..\..\ExampleXml\Sitecore.ContentSearch.Solr.DefaultIndexConfiguration.config");
            var sitecoreConfig = XDocument.Parse(xml);

            var newElement = new XElement("xml");

            var patches = new BasePatch[] {
                new PatchInsert(
                    "/configuration/sitecore/contentSearch/indexConfigurations/defaultSolrIndexConfiguration[@type='Sitecore.ContentSearch.SolrProvider.SolrIndexConfiguration, Sitecore.ContentSearch.SolrProvider']",
                    ElementInsertPosition.Before,
                    "initializeOnAdd",
                    newElement)
            };

            var sut = new PatchGenerator(sitecoreConfig);

            var patchData = sut.GeneratePatchFile(patches);

            var newXml = SitecorePatcher.ApplyWithoutRoles(xml, patchData.ToString(), "testpatch.config");

            Assert.IsFalse(newXml.StartsWith("<!--ERROR"));
            Assert.IsFalse(newXml.StartsWith("<error>"));
            Assert.IsTrue(newXml.Contains("<fieldMap"));
            Assert.IsTrue(newXml.Contains("<xml"));
        }

        [TestMethod]
        public void IntegrationTest_MultiPatchOnSitecoreConfig_GivesRightAnswers()
        {
            var xml = System.IO.File.ReadAllText(@"..\..\ExampleXml\Sitecore.config");

            var sitecoreConfig = XDocument.Parse(xml);

            var newSite = new XElement("site",
                new XAttribute("inherits", "site1"),
                new XAttribute("name", "new"),
                new XAttribute("hostName", "www.test.com")
            );

            var newDataFolder = new XElement("sc.variable",
                new XAttribute("name", "dataFolder"),
                new XAttribute("value", "/sitedata")
            );

             var patches = new BasePatch[] {
                new PatchInsert("/sitecore/sites", ElementInsertPosition.After, "site[@name='website']", newSite),
                 new SetAttribute("/sitecore/mediaLibrary/mediaPrefixes/prefix[@value='~/media']", "value", "~/art"),
                new PatchInstead("/sitecore", "sc.variable[@name='dataFolder']", newDataFolder),
                new PatchDelete("/sitecore/tracking/untrackedPages/add[@path='/sitecore/default.aspx']")
            };

            var sut = new PatchGenerator(sitecoreConfig);

            var patchData = sut.GeneratePatchFile(patches);

            var newXml = SitecorePatcher.ApplyWithoutRoles(xml, patchData.ToString(), "testpatch.config");


            var newXDoc = XDocument.Parse(newXml);

            var patchedSite = newXDoc.XPathSelectElement("/sitecore/sites/site[@name='new']");
            Assert.IsNotNull(patchedSite);
            Assert.AreEqual("site1", patchedSite.Attribute("inherits").Value);

            var patchedMediaPrefix = newXDoc.XPathSelectElement("/sitecore/mediaLibrary/mediaPrefixes/prefix");
            Assert.IsNotNull(patchedMediaPrefix);
            Assert.AreEqual("~/art", patchedMediaPrefix.Attribute("value").Value);

            var patchedVariable = newXDoc.XPathSelectElement("/sitecore/sc.variable[@name='dataFolder']");
            Assert.IsNotNull(patchedVariable);
            Assert.AreEqual("/sitedata", patchedVariable.Attribute("value").Value);

            var patchedPages = newXDoc.XPathSelectElements("/sitecore/tracking/untrackedPages/add");
            foreach(var patchedPage in patchedPages)
            {
                Assert.AreNotEqual("/sitecore/default.aspx']", patchedPage.Attribute("path").Value);
            }
        }

        [TestMethod]
        public void IntegrationTest_RoleNamespace_IsHandledCorrectly()
        {
            var xml = System.IO.File.ReadAllText(@"..\..\ExampleXml\role-namespace.config");

            var sitecoreConfig = XDocument.Parse(xml);

            var patches = new BasePatch[] {
                new SetAttribute("/sitecore/sc.variable[@name='mediaFolder' and @role:require='Standalone']", "value", "~/StandAloneData"),
            };

            var sut = new PatchGenerator(sitecoreConfig);

            var patchData = sut.GeneratePatchFile(patches);

            var patchElement = patchData
                .Element("configuration")
                .Element("sitecore")
                .Element("sc.variable");

            Assert.IsNotNull(patchElement);
            Assert.AreEqual(4, patchElement.Attributes().Count());
            Assert.AreEqual("mediaFolder", patchElement.Attribute("name").Value);

            var roles = new Dictionary<string, string>() { {"role", "Standalone" }};

            var newXml = SitecorePatcher.ApplyWithRoles(xml, patchData.ToString(), "testpatch.config", roles);

            Assert.IsFalse(string.IsNullOrWhiteSpace(newXml));

            var newXDoc = XDocument.Parse(newXml);

            var newElement = newXDoc.XPathSelectElement("/sitecore/sc.variable[@name='mediaFolder']");

            Assert.IsNotNull(newElement);
            Assert.AreEqual(3, newElement.Attributes().Count());
            Assert.AreEqual("~/StandAloneData", newElement.Attribute("value").Value);
        }
    }

}