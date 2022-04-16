using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatchMaker.Sitecore;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PatchMaker.Tests
{

    [TestClass]
    public class SetAttributeTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetAttribute_Constructor_NullPath_Throws()
        {
            var sut = new SetAttribute(null, "test", "a");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetAttribute_Constructor_EmptyPath_Throws()
        {
            var sut = new SetAttribute(string.Empty, "test", "a");
        }

        [TestMethod]
        public void SetAttribute_Constructor_ValidPath_Works()
        {
            var sut = new SetAttribute("/sites/site", "test", "a");

            Assert.AreEqual("/sites/site", sut.XPathForElement);
        }

        [TestMethod]
        public void SetAttribute_Constructor_WithRBC_Works()
        {
            var sut = new SetAttribute("/sites/site", "test", "a", new ConfigRule[] { new ConfigRule("role", "require", "Standalone") });

            Assert.AreEqual(1, sut.RoleBasedRules.Length);
            Assert.AreEqual("require", sut.RoleBasedRules[0].Name);
            Assert.AreEqual("Standalone", sut.RoleBasedRules[0].Value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetAttribute_Constructor_NullAttrName_Throws()
        {
            var sut = new SetAttribute("/site", null, "a");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetAttribute_Constructor_EmptyAttrName_Throws()
        {
            var sut = new SetAttribute("/site", string.Empty, "a");
        }

        [TestMethod]
        public void SetAttribute_Constructor_ValidAttrName_Works()
        {
            var sut = new SetAttribute("/sites/site", "test", "a");

            Assert.AreEqual("test", sut.AttributeName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetAttribute_Constructor_NullAttrValue_Throws()
        {
            var sut = new SetAttribute("/site", "a", null);
        }

        [TestMethod]
        public void SetAttribute_Constructor_ValidAttrValue_Works()
        {
            var sut = new SetAttribute("/sites/site", "a", "test");

            Assert.AreEqual("test", sut.AttributeValue);
        }

        [TestMethod]
        public void SetAttribute_PatchGenerator_Accepts_SetAttribute()
        {
            var sa = new SetAttribute("/sitecore/sites/site[@name='a']", "cheese", "1");
            var xml = XDocument.Parse("<sitecore><sites><site name=\"a\"/></sites></sitecore>");

            var sut = new PatchGenerator(xml);

            var result = sut.GeneratePatchFile(new BasePatch[] { sa });

            var patch = result
                .Element("configuration")
                .Element("sitecore")
                .Element("sites")
                .Element("site");

            Assert.IsNotNull(patch);

            var attr = patch.Attribute(Namespaces.Set + "cheese");
            Assert.IsNotNull(attr);
            Assert.AreEqual("1", attr.Value);
        }

        [TestMethod]
        public void SetAttribute_PatchGenerator_Accepts_SetAttribute_WithRBC()
        {
            var sa = new SetAttribute("/sitecore/sites/site[@name='a']", "cheese", "1", new ConfigRule[] { new ConfigRule("https://x/role/", "require", "Standalone") });
            var xml = XDocument.Parse("<sitecore><sites><site name=\"a\"/></sites></sitecore>");

            var sut = new PatchGenerator(xml);

            var result = sut.GeneratePatchFile(new BasePatch[] { sa });

            var patch = result
                .Element("configuration")
                .Element("sitecore")
                .Element("sites")
                .Element("site");

            Assert.IsNotNull(patch);

            var attr = patch.Attribute(Namespaces.Set + "cheese");
            Assert.IsNotNull(attr);
            Assert.AreEqual("1", attr.Value);

            XNamespace ns = "https://x/role/";
            var p = patch.Attribute(ns + "require");

            Assert.IsNotNull(p);
            Assert.AreEqual("Standalone", p.Value);
        }

        [TestMethod]
        public void SetAttribute_PatchGenerator_Accepts_MultipleSetAttribute_OnSameElement()
        {
            var sets = new BasePatch[] {
                new SetAttribute("/sitecore/sites/site[@name='a']", "cheese", "1"),
                new SetAttribute("/sitecore/sites/site[@name='a']", "biscuits", "2")
            };
            var xml = XDocument.Parse("<sitecore><sites><site name=\"a\"/></sites></sitecore>");

            var sut = new PatchGenerator(xml);

            var result = sut.GeneratePatchFile(sets);

            var patches = result
                .Element("configuration")
                .Element("sitecore")
                .Element("sites")
                .Elements("site");

            Assert.IsNotNull(patches);
            Assert.AreEqual(1, patches.Count());

            var patch = patches.First();

            var attr1 = patch.Attribute(Namespaces.Set + "cheese");
            Assert.IsNotNull(attr1);
            Assert.AreEqual("1", attr1.Value);

            var attr2 = patch.Attribute(Namespaces.Set + "biscuits");
            Assert.IsNotNull(attr2);
            Assert.AreEqual("2", attr2.Value);

            var originalAttr = patch.Attribute("name");
            Assert.IsNotNull(originalAttr);
            Assert.AreEqual("a", originalAttr.Value);
        }

        [TestMethod]
        public void SetAttribute_PatchGenerator_Accepts_MultipleSetAttribute_OnDifferentElements()
        {
            var sets = new BasePatch[] {
                new SetAttribute("/sitecore/sites/site[@name='a']", "cheese", "1"),
                new SetAttribute("/sitecore/sites/site[@name='b']", "cheese", "2")
            };
            var xml = XDocument.Parse("<sitecore><sites><site name=\"a\"/><site name=\"b\"/></sites></sitecore>");

            var sut = new PatchGenerator(xml);

            var result = sut.GeneratePatchFile(sets);

            var patches = result
                .Element("configuration")
                .Element("sitecore")
                .Element("sites")
                .Elements("site");

            Assert.IsNotNull(patches);
            Assert.AreEqual(2, patches.Count());

            var patch1 = patches.ElementAt(0);
            var attr1 = patch1.Attribute(Namespaces.Set + "cheese");
            Assert.IsNotNull(attr1);
            Assert.AreEqual("1", attr1.Value);

            var patch2 = patches.ElementAt(1);
            var attr2 = patch2.Attribute(Namespaces.Set + "cheese");
            Assert.IsNotNull(attr2);
            Assert.AreEqual("2", attr2.Value);
        }

        [TestMethod]
        public void SetAttribute_GivesCorrectResult_ComparedToSitecore()
        {
            var sourceXmlText = "<sitecore><sites><site name=\"a\"/><site name=\"b\"/></sites></sitecore>";
            var sourceXml = XDocument.Parse(sourceXmlText);

            var sets = new BasePatch[] {
                new SetAttribute("/sitecore/sites/site[@name='a']", "fish", "trout"),
                new SetAttribute("/sitecore/sites/site[@name='b']", "insect", "spider")
            };

            var sut = new PatchGenerator(sourceXml);

            var patchXmlText = sut.GeneratePatchFile(sets).ToString();

            var resultXmlText = SitecorePatcher.ApplyWithoutRoles(sourceXmlText, patchXmlText);

            Assert.AreEqual("<sitecore><sites><site name=\"a\" fish=\"trout\" /><site name=\"b\" insect=\"spider\" /></sites></sitecore>", resultXmlText);
        }

        [TestMethod]
        [ExpectedException(typeof(XPathException))]
        public void SetAttribute_InvalidXPath_Throws()
        {
            var patches = new BasePatch[] {
                new SetAttribute("/sitecore/sites/site[@name='b'", "cheese", "1"),
            };
            var xml = XDocument.Parse("<sitecore><sites><site name=\"a\"/></sites></sitecore>");

            var sut = new PatchGenerator(xml);

            var result = sut.GeneratePatchFile(patches);
        }

        [TestMethod]
        public void SetAttribute_XPathMatchingNothing_AddsComment()
        {
            var patches = new BasePatch[] {
                new SetAttribute("/sitecore/sites/site[@name='b']", "cheese", "1"),
            };
            var xml = XDocument.Parse("<sitecore><sites><site name=\"a\"/></sites></sitecore>");

            var sut = new PatchGenerator(xml);

            var result = sut.GeneratePatchFile(patches);

            Assert.IsTrue(result.ToString().Contains("<!--ERROR:"));
        }
    }

}