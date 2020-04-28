using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatchMaker.Sitecore;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PatchMaker.Tests
{
    [TestClass]
    public class PatchInsteadTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchInstead_Constructor_NullPath_Throws()
        {
            var newElement = new XElement("test");

            var sut = new PatchInstead(null, "*[]", newElement);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchInstead_Constructor_EmptyPath_Throws()
        {
            var newElement = new XElement("test");

            var sut = new PatchInstead(string.Empty, "*[]", newElement);
        }

        [TestMethod]
        public void PatchInstead_Constructor_ValidPath_Works()
        {
            var newElement = new XElement("test");

            var sut = new PatchInstead("/sites", "*[]", newElement);

            Assert.AreEqual("/sites", sut.XPathForParent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchInstead_Constructor_NullXPath_Throws()
        {
            var newElement = new XElement("test");

            var sut = new PatchInstead("/sites", null, newElement);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchInstead_Constructor_EmptyXPath_Throws()
        {
            var newElement = new XElement("test");

            var sut = new PatchInstead("/sites", string.Empty, newElement);
        }

        [TestMethod]
        public void PatchInstead_Constructor_ValidXPath_Works()
        {
            var newElement = new XElement("test");

            var sut = new PatchInstead("/sites", "*[]", newElement);

            Assert.AreEqual("*[]", sut.XPathForReplacement);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchInstead_Constructor_NullPatchXml_Throws()
        {
            var sut = new PatchInstead("/sites", "*[]", null);
        }

        [TestMethod]
        public void PatchInstead_Constructor_ValidPatchXml_Works()
        {
            var newElement = new XElement("test");
            var sut = new PatchInstead("/sites", "*[]", newElement);

            Assert.AreEqual("test", sut.Replacement.Name);
        }

        [TestMethod]
        public void InsertElement_PatchGenerator_Accepts_Insert()
        {
            var ins = new PatchInstead("/sitecore/sites", "site[@name='a']", new XElement("site", new XAttribute("name", "c")));
            var xml = XDocument.Parse("<sitecore><sites><site name=\"a\"/></sites></sitecore>");

            var sut = new PatchGenerator(xml);

            var result = sut.GeneratePatchFile(new BasePatch[] { ins });

            var sites = result
                .Element("configuration")
                .Element("sitecore")
                .Element("sites")
                .Elements("site");

            Assert.AreEqual(1, sites.Count());

            var newSite = result
                .Element("configuration")
                .Element("sitecore")
                .Element("sites")
                .Element("site");

            Assert.IsNotNull(newSite);
            Assert.AreEqual("c", newSite.Attribute("name").Value);

            Assert.IsNotNull(newSite.Attribute(Namespaces.Patch + "instead"));
        }

        [TestMethod]
        public void InsertElement_PatchGenerator_Accepts_MultipleInserts()
        {
            var sourceXml = XDocument.Parse("<sitecore><sites><site name=\"a\"/><site name=\"b\"/></sites></sitecore>");

            var replaces = new BasePatch[] {
                new PatchInstead("/sitecore/sites", "site[@name='a']", new XElement("site", new XAttribute("name", "1"))),
                new PatchInstead("/sitecore/sites", "site[@name='b']", new XElement("site", new XAttribute("name", "2")))
            };

            var sut = new PatchGenerator(sourceXml);

            var result = sut.GeneratePatchFile(replaces);

            var roots = result.XPathSelectElements("/configuration/sitecore/sites");
            Assert.AreEqual(1, roots.Count());

            var patches = result.XPathSelectElements("/configuration/sitecore/sites/site");

            Assert.IsNotNull(patches);
            Assert.AreEqual(2, patches.Count());

            var first = patches.ElementAt(0).Attribute("name");
            Assert.AreEqual("1", first.Value);

            var second = patches.ElementAt(1).Attribute("name");
            Assert.AreEqual("2", second.Value);
        }

        [TestMethod]
        public void InserElement_GivesCorrectResult_ComparedToSitecore()
        {
            var sourceXmlText = "<sitecore><sites><site name=\"a\"/><site name=\"b\"/></sites></sitecore>";
            var sourceXml = XDocument.Parse(sourceXmlText);

            var replaces = new BasePatch[] {
                new PatchInstead("/sitecore/sites", "site[@name='b']", new XElement("site", new XAttribute("name", "2"))),
                new PatchInstead("/sitecore/sites", "site[@name='a']", new XElement("site", new XAttribute("name", "1")))
            };

            var sut = new PatchGenerator(sourceXml);

            var patchXmlText = sut.GeneratePatchFile(replaces).ToString();

            var resultXmlText = SitecorePatcher.Apply(sourceXmlText, patchXmlText);

            Assert.AreEqual("<sitecore><sites><site name=\"1\" /><site name=\"2\" /></sites></sitecore>", resultXmlText);
        }

        [TestMethod]
        [ExpectedException(typeof(XPathException))]
        public void InserElement_InvalidXPath_Throws()
        {
            var sourceXmlText = "<sitecore><sites><site name=\"a\"/></sites></sitecore>";
            var sourceXml = XDocument.Parse(sourceXmlText);

            var replaces = new BasePatch[] {
                new PatchInstead("/sitecore/zsites[", "site[@name='b']", new XElement("site", new XAttribute("name", "2")))
            };

            var sut = new PatchGenerator(sourceXml);

            var result = sut.GeneratePatchFile(replaces).ToString();
        }

        [TestMethod]
        public void InserElement_XPathMatchingNothing_AddsComment()
        {
            var sourceXmlText = "<sitecore><sites><site name=\"a\"/></sites></sitecore>";
            var sourceXml = XDocument.Parse(sourceXmlText);

            var replaces = new BasePatch[] {
                new PatchInstead("/sitecore/zsites", "site[@name='b']", new XElement("site", new XAttribute("name", "2")))
            };

            var sut = new PatchGenerator(sourceXml);

            var result = sut.GeneratePatchFile(replaces).ToString();

            Assert.IsTrue(result.ToString().Contains("<!--ERROR:"));
        }
    }

}