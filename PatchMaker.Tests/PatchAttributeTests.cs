using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatchMaker.Sitecore;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PatchMaker.Tests
{
    [TestClass]
    public class PatchAttributeTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchAttribute_Constructor_NullPath_Throws()
        {
            var sut = new PatchAttribute(null, "test", "a");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchAttribute_Constructor_EmptyPath_Throws()
        {
            var sut = new PatchAttribute(string.Empty, "test", "a");
        }

        [TestMethod]
        public void PatchAttribute_Constructor_ValidPath_Works()
        {
            var sut = new PatchAttribute("/sites/site", "test", "a");

            Assert.AreEqual("/sites/site", sut.XPathForElement);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchAttribute_Constructor_NullAttrName_Throws()
        {
            var sut = new PatchAttribute("/site", null, "a");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchAttribute_Constructor_EmptyAttrName_Throws()
        {
            var sut = new PatchAttribute("/site", string.Empty, "a");
        }

        [TestMethod]
        public void PatchAttribute_Constructor_ValidAttrName_Works()
        {
            var sut = new PatchAttribute("/sites/site", "test", "a");

            Assert.AreEqual("test", sut.AttributeName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchAttribute_Constructor_NullAttrValue_Throws()
        {
            var sut = new PatchAttribute("/site", "a", null);
        }

        [TestMethod]
        public void PatchAttribute_Constructor_ValidAttrValue_Works()
        {
            var sut = new PatchAttribute("/sites/site", "a", "test");

            Assert.AreEqual("test", sut.AttributeValue);
        }

        [TestMethod]
        public void PatchAttribute_PatchGenerator_Accepts_PatchAttribute()
        {
            var pa = new PatchAttribute("/sitecore/sites/site[@name='a']", "cheese", "1");
            var xml = XDocument.Parse("<sitecore><sites><site name=\"a\"/></sites></sitecore>");

            var sut = new PatchGenerator(xml);

            var result = sut.GeneratePatchFile(new BasePatch[] { pa });

            var el = result
                .Element("configuration")
                .Element("sitecore")
                .Element("sites")
                .Element("site");

            Assert.IsNotNull(el);

            var patch = el.Element(Namespaces.Patch + "attribute");
            Assert.IsNotNull(patch);
            Assert.AreEqual("cheese", patch.Attribute("name").Value);
            Assert.AreEqual("1", patch.Attribute("value").Value);
        }


        [TestMethod]
        public void PatchAttribute_PatchGenerator_Accepts_MultiplePatchAttribute_OnSameElement()
        {
            var patches = new BasePatch[] {
                new PatchAttribute("/sitecore/sites/site[@name='a']", "cheese", "1"),
                new PatchAttribute("/sitecore/sites/site[@name='a']", "biscuits", "2")
            };
            var xml = XDocument.Parse("<sitecore><sites><site name=\"a\"/></sites></sitecore>");

            var sut = new PatchGenerator(xml);

            var result = sut.GeneratePatchFile(patches);

            var el = result
                .Element("configuration")
                .Element("sitecore")
                .Element("sites")
                .Element("site");

            Assert.IsNotNull(el);

            var patch1 = el.Elements().ElementAt(0);
            Assert.IsNotNull(patch1);
            Assert.AreEqual("cheese", patch1.Attribute("name").Value);
            Assert.AreEqual("1", patch1.Attribute("value").Value);

            var patch2 = el.Elements().ElementAt(1);
            Assert.IsNotNull(patch2);
            Assert.AreEqual("biscuits", patch2.Attribute("name").Value);
            Assert.AreEqual("2", patch2.Attribute("value").Value);
        }

        [TestMethod]
        public void SetAttribute_PatchGenerator_Accepts_MultipleSetAttribute_OnDifferentElements()
        {
            var patches = new BasePatch[] {
                new PatchAttribute("/sitecore/sites/site[@name='a']", "cheese", "1"),
                new PatchAttribute("/sitecore/sites/site[@name='b']", "cheese", "2")
            };
            var xml = XDocument.Parse("<sitecore><sites><site name=\"a\"/><site name=\"b\"/></sites></sitecore>");

            var sut = new PatchGenerator(xml);

            var result = sut.GeneratePatchFile(patches);

            var els = result
                .Element("configuration")
                .Element("sitecore")
                .Element("sites")
                .Elements("site");

            Assert.IsNotNull(els);
            Assert.AreEqual(2, els.Count());

            var el1 = els.ElementAt(0);
            var patch1 = el1.Elements().First();
            Assert.IsNotNull(patch1);
            Assert.AreEqual("cheese", patch1.Attribute("name").Value);
            Assert.AreEqual("1", patch1.Attribute("value").Value);

            var el2 = els.ElementAt(1);
            var patch2 = el2.Elements().First();
            Assert.IsNotNull(patch2);
            Assert.AreEqual("cheese", patch2.Attribute("name").Value);
            Assert.AreEqual("2", patch2.Attribute("value").Value);
        }

        [TestMethod]
        public void SetAttribute_GivesCorrectResult_ComparedToSitecore()
        {
            var sourceXmlText = "<sitecore><sites><site name=\"a\"/><site name=\"b\"/></sites></sitecore>";
            var sourceXml = XDocument.Parse(sourceXmlText);

            var patches = new BasePatch[] {
                new PatchAttribute("/sitecore/sites/site[@name='a']", "fish", "trout"),
                new PatchAttribute("/sitecore/sites/site[@name='b']", "insect", "spider")
            };

            var sut = new PatchGenerator(sourceXml);

            var patchXmlText = sut.GeneratePatchFile(patches).ToString();

            var resultXmlText = SitecorePatcher.ApplyWithoutRoles(sourceXmlText, patchXmlText);

            Assert.AreEqual("<sitecore><sites><site name=\"a\" fish=\"trout\" /><site name=\"b\" insect=\"spider\" /></sites></sitecore>", resultXmlText);
        }

        [TestMethod]
        [ExpectedException(typeof(XPathException))]
        public void PatchAttribute_InvalidXPath_Throws()
        {
            var patches = new BasePatch[] {
                new PatchAttribute("/sitecore/sites/site[@name='b'", "cheese", "1"),
            };
            var xml = XDocument.Parse("<sitecore><sites><site name=\"a\"/></sites></sitecore>");

            var sut = new PatchGenerator(xml);

            var result = sut.GeneratePatchFile(patches);
        }

        [TestMethod]
        public void PatchAttribute_XPathMatchingNothing_AddsComment()
        {
            var patches = new BasePatch[] {
                new PatchAttribute("/sitecore/sites/site[@name='b']", "cheese", "1"),
            };
            var xml = XDocument.Parse("<sitecore><sites><site name=\"a\"/></sites></sitecore>");

            var sut = new PatchGenerator(xml);

            var result = sut.GeneratePatchFile(patches);

            Assert.IsTrue(result.ToString().Contains("<!--ERROR:"));
        }
    }

}