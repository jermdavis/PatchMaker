using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatchMaker.Sitecore;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PatchMaker.Tests
{

    [TestClass]
    public class PatchInsertTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchInsert_Constructor_NullPath_Throws()
        {
            var newElement = new XElement("test");

            var sut = new PatchInsert(null, ElementInsertPosition.Before, "//*[]", newElement);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchInsert_Constructor_EmptyPath_Throws()
        {
            var newElement = new XElement("test");

            var sut = new PatchInsert(string.Empty, ElementInsertPosition.Before, "//*[]", newElement);
        }

        [TestMethod]
        public void PatchInsert_Constructor_ValidPath_Works()
        {
            var newElement = new XElement("test");

            var sut = new PatchInsert("/sites/site", ElementInsertPosition.Before, "//*[]", newElement);

            Assert.AreEqual("/sites/site", sut.XPathForParent);
        }

        [TestMethod]
        public void PatchInsert_Constructor_ValidInsertPosition_Works()
        {
            var newElement = new XElement("test");
            var sut = new PatchInsert("/sites/site", ElementInsertPosition.Before, "//*[]", newElement);

            Assert.AreEqual(ElementInsertPosition.Before, sut.Position);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchInsert_Constructor_NullPatchXml_Throws()
        {
            var sut = new PatchInsert("/sites/site", ElementInsertPosition.Before, "//*[]", null);
        }

        [TestMethod]
        public void PatchInsert_Constructor_ValidPatchXml_Works()
        {
            var newElement = new XElement("test");
            var sut = new PatchInsert("/sites/site", ElementInsertPosition.Before, "//*[]", newElement);

            Assert.AreEqual("test", sut.NewElement.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchInsert_Constructor_NullOrder_Throws()
        {
            var newElement = new XElement("test");

            var sut = new PatchInsert("/sites/site", ElementInsertPosition.Before, null, newElement);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchInsert_Constructor_EmptyOrder_Throws()
        {
            var newElement = new XElement("test");

            var sut = new PatchInsert("/sites/site", ElementInsertPosition.Before, string.Empty, newElement);
        }

        [TestMethod]
        public void PatchInsert_Constructor_ValidOrder_Works()
        {
            var newElement = new XElement("test");

            var sut = new PatchInsert("/sites/site", ElementInsertPosition.Before, "//*[]", newElement);

            Assert.AreEqual("//*[]", sut.XPathForOrder);
        }

        [TestMethod]
        public void PatchInsert_PatchGenerator_Accepts_Insert()
        {
            var ins = new PatchInsert("/sitecore/sites", ElementInsertPosition.After, "*[@name='a']", new XElement("site", new XAttribute("name", "c")));
            var xml = XDocument.Parse("<sitecore><sites><site name=\"a\"/><site name=\"b\"/></sites></sitecore>");

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

            Assert.IsNotNull(newSite.Attribute(Namespaces.Patch + "after"));
        }

        [TestMethod]
        public void PatchInsert_PatchGenerator_Accepts_MultipleInserts()
        {
            var sourceXml = XDocument.Parse("<sitecore><sites><site name=\"a\"/><site name=\"b\"/></sites></sitecore>");

            var inserts = new BasePatch[] {
                new PatchInsert("/sitecore/sites", ElementInsertPosition.Before, "site[@name='b']", new XElement("site", new XAttribute("name", "before"))),
                new PatchInsert("/sitecore/sites", ElementInsertPosition.After, "site[@name='b']", new XElement("site", new XAttribute("name", "after")))
            };

            var sut = new PatchGenerator(sourceXml);

            var result = sut.GeneratePatchFile(inserts);

            var roots = result.XPathSelectElements("/configuration/sitecore/sites");
            Assert.AreEqual(1, roots.Count());

            var patches = result.XPathSelectElements("/configuration/sitecore/sites/site");

            Assert.IsNotNull(patches);
            Assert.AreEqual(2, patches.Count());

            var first = patches.ElementAt(0).Attribute("name");
            Assert.AreEqual("before", first.Value);

            var second = patches.ElementAt(1).Attribute("name");
            Assert.AreEqual("after", second.Value);
        }

        [TestMethod]
        public void InserElement_GivesCorrectResult_ComparedToSitecore()
        {
            var sourceXmlText = "<sitecore><sites><site name=\"a\"/><site name=\"b\"/></sites></sitecore>";
            var sourceXml = XDocument.Parse(sourceXmlText);

            var inserts = new BasePatch[] {
                new PatchInsert("/sitecore/sites", ElementInsertPosition.Before, "site[@name='b']", new XElement("site", new XAttribute("name", "before"))),
                new PatchInsert("/sitecore/sites", ElementInsertPosition.After, "site[@name='b']", new XElement("site", new XAttribute("name", "after")))
            };

            var sut = new PatchGenerator(sourceXml);

            var patchXmlText = sut.GeneratePatchFile(inserts).ToString();

            var resultXmlText = SitecorePatcher.Apply(sourceXmlText, patchXmlText);

            Assert.AreEqual("<sitecore><sites><site name=\"a\" /><site name=\"before\" /><site name=\"b\" /><site name=\"after\" /></sites></sitecore>", resultXmlText);
        }

        [TestMethod]
        [ExpectedException(typeof(PatchException))]
        public void InserElement_InvalidXPath_Throws()
        {
            var sourceXmlText = "<sitecore><sites><site name=\"a\"/><site name=\"b\"/></sites></sitecore>";
            var sourceXml = XDocument.Parse(sourceXmlText);

            var inserts = new BasePatch[] {
                new PatchInsert("/sitecore/xsites", ElementInsertPosition.Before, "site[@name='b']", new XElement("site", new XAttribute("name", "before")))
            };

            var sut = new PatchGenerator(sourceXml);

            var result = sut.GeneratePatchFile(inserts).ToString();
        }
    }

}