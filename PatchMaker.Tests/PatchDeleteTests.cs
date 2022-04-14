using Microsoft.VisualStudio.TestTools.UnitTesting;
using PatchMaker.Sitecore;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PatchMaker.Tests
{

    [TestClass]
    public class PatchDeleteTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchDelete_Constructor_NullPath_Throws()
        {
            var sut = new PatchDelete(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchDelete_Constructor_EmptyPath_Throws()
        {
            var sut = new PatchDelete(string.Empty);
        }

        [TestMethod]
        public void PatchDelete_Constructor_ValidPath_Works()
        {
            var sut = new PatchDelete("/sites/site");

            Assert.AreEqual("/sites/site", sut.XPathForElement);
        }

        [TestMethod]
        public void PatchDelete_Constructor_WithRBC_Works()
        {
            var sut = new PatchDelete("/sites/site", new ConfigRule[] { new ConfigRule("role", "require", "Standalone") });

            Assert.AreEqual(1, sut.RoleBasedRules.Length);
            Assert.AreEqual("require", sut.RoleBasedRules[0].Name);
            Assert.AreEqual("Standalone", sut.RoleBasedRules[0].Value);
        }

        [TestMethod]
        public void PatchDelete_PatchGenerator_Accepts_Delete()
        {
            var de = new PatchDelete("/sitecore/sites/site[@name='a']");
            var xml = XDocument.Parse("<sitecore><sites><site name=\"a\"/><site name=\"b\"/></sites></sitecore>");

            var sut = new PatchGenerator(xml);

            var result = sut.GeneratePatchFile(new BasePatch[] { de });

            var patch = result
                .Element("configuration")
                .Element("sitecore")
                .Element("sites")
                .Element("site")
                .Element(Namespaces.Patch + "delete");

            Assert.IsNotNull(patch);
        }

        [TestMethod]
        public void PatchDelete_PatchGenerator_Accepts_Delete_WithRBC()
        {
            var de = new PatchDelete("/sitecore/sites/site[@name='a']", new ConfigRule[] { new ConfigRule("https://x/role/", "require","Standalone") });
            var xml = XDocument.Parse("<sitecore><sites><site name=\"a\"/><site name=\"b\"/></sites></sitecore>");

            var sut = new PatchGenerator(xml);

            var result = sut.GeneratePatchFile(new BasePatch[] { de });

            var patch = result
                .Element("configuration")
                .Element("sitecore")
                .Element("sites")
                .Element("site")
                .Element(Namespaces.Patch + "delete");

            Assert.IsNotNull(patch);

            XNamespace ns = "https://x/role/";
            var p = patch.Attribute(ns + "require");
            
            Assert.IsNotNull(p);
            Assert.AreEqual("Standalone", p.Value);
        }

        [TestMethod]
        public void PatchDelete_PatchGenerator_Accepts_MultipleDeletes()
        {
            var sourceXml = XDocument.Parse("<sitecore><sites><site name=\"a\"/><site name=\"b\"/></sites></sitecore>");

            var deletes = new BasePatch[] {
                new PatchDelete("/sitecore/sites/site[@name='a']"),
                new PatchDelete("/sitecore/sites/site[@name='b']")
            };

            var sut = new PatchGenerator(sourceXml);

            var result = sut.GeneratePatchFile(deletes);

            var roots = result.XPathSelectElements("/configuration/sitecore/sites");
            Assert.AreEqual(1, roots.Count());

            var patches = result.XPathSelectElements("/configuration/sitecore/sites/site");

            Assert.IsNotNull(patches);
            Assert.AreEqual(2, patches.Count());

            var first = patches.ElementAt(0).Attribute("name");
            Assert.AreEqual("a", first.Value);

            var second = patches.ElementAt(1).Attribute("name");
            Assert.AreEqual("b", second.Value);
        }

        [TestMethod]
        public void PatchDelete_GivesCorrectResult_ComparedToSitecore()
        {
            var sourceXmlText = "<sitecore><sites><site name=\"a\"/><site name=\"b\"/></sites></sitecore>";
            var sourceXml = XDocument.Parse(sourceXmlText);

            var deletes = new BasePatch[] {
                new PatchDelete("/sitecore/sites/site[@name='a']"),
                new PatchDelete("/sitecore/sites/site[@name='b']")
            };

            var sut = new PatchGenerator(sourceXml);

            var patchXmlText = sut.GeneratePatchFile(deletes).ToString();

            var resultXmlText = SitecorePatcher.ApplyWithoutRoles(sourceXmlText, patchXmlText);

            Assert.AreEqual("<sitecore><sites></sites></sitecore>", resultXmlText);
        }

        [TestMethod]
        [ExpectedException(typeof(XPathException))]
        public void PatchDelete_InvalidXPath_Throws()
        {
            var sourceXmlText = "<sitecore><sites><site name=\"a\"/></sites></sitecore>";
            var sourceXml = XDocument.Parse(sourceXmlText);

            var deletes = new BasePatch[] {
                new PatchDelete("/sitecore/sites/site[@name='z'"),
            };

            var sut = new PatchGenerator(sourceXml);

            var result = sut.GeneratePatchFile(deletes);
        }

        [TestMethod]
        public void InserElement_XPathMatchingNothing_AddsComment()
        {
            var sourceXmlText = "<sitecore><sites><site name=\"a\"/></sites></sitecore>";
            var sourceXml = XDocument.Parse(sourceXmlText);

            var deletes = new BasePatch[] {
                new PatchDelete("/sitecore/sites/sitae[@name='z']"),
            };

            var sut = new PatchGenerator(sourceXml);

            var result = sut.GeneratePatchFile(deletes);

            Assert.IsTrue(result.ToString().Contains("<!--ERROR:"));
        }
    }

}