using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatchMaker.Sitecore;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PatchMaker.Tests
{

    [TestClass]
    public class IntegrationTests
    {
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

            var newXml = SitecorePatcher.Apply(xml, patchData.ToString(), "testpatch.config");


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
            var xml = System.IO.File.ReadAllText(@"..\..\ExampleXml\v92-Sitecore.config");

            var sitecoreConfig = XDocument.Parse(xml);

            var patches = new BasePatch[] {
                new SetAttribute("/sitecore/sc.variable[@name='dataFolder' and @role:require='Standalone']", "value", "~/StandAloneData"),
            };

            var sut = new PatchGenerator(sitecoreConfig);

            var patchData = sut.GeneratePatchFile(patches);

            var newXml = SitecorePatcher.Apply(xml, patchData.ToString(), "testpatch.config");

            Assert.IsFalse(string.IsNullOrWhiteSpace(newXml));
        }
    }

}