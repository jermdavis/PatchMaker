using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Xml.Linq;

namespace PatchMaker.Tests
{

    [TestClass]
    public class PatchNewChildTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchNewChild_Constructor_NullPath_Throws()
        {
            var newElement = new XElement("test");

            var sut = new PatchNewChild(null, newElement);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchNewChild_Constructor_EmptyPath_Throws()
        {
            var newElement = new XElement("test");

            var sut = new PatchNewChild(string.Empty, newElement);
        }

        [TestMethod]
        public void PatchNewChild_Constructor_ValidPath_Works()
        {
            var newElement = new XElement("test");

            var sut = new PatchNewChild("/sites/site", newElement);

            Assert.AreEqual("/sites/site", sut.XPathForParent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchNewChild_Constructor_NullPatchXml_Throws()
        {
            var sut = new PatchNewChild("/sites/site", null);
        }

        [TestMethod]
        public void PatchNewChild_Constructor_ValidPatchXml_Works()
        {
            var newElement = new XElement("test");
            var sut = new PatchNewChild("/sites/site", newElement);

            Assert.AreEqual("test", sut.ChildXml.Name);
        }

        [TestMethod]
        public void PatchNewChild_PatchGenerator_Accepts_Insert()
        {
            var ins = new PatchNewChild("/sitecore/sites", new XElement("site", new XAttribute("name", "c")));
            var xml = XDocument.Parse("<sitecore><sites/></sitecore>");

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
        }
    }
}