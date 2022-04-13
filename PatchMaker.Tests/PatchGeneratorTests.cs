using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml.Linq;

namespace PatchMaker.Tests
{

    [TestClass]
    public class PatchGeneratorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchGenerator_Constructor_ThrowsOnNullDocument()
        {
            var sut = new PatchGenerator(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void PatchGenerator_Constructor_ThrowsIfDocumentEmpty()
        {
            var xml = new XDocument();
            var sut = new PatchGenerator(xml);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PatchGenerator_GeneratePatchFile_NullPatches_Throws()
        {
            var source = XDocument.Parse("<sites/>");

            var sut = new PatchGenerator(source);

            sut.GeneratePatchFile(null);
        }

        [TestMethod]
        public void PatchGenerator_GeneratePatchFile_EmptyPatches_GivesADocument()
        {
            var source = XDocument.Parse("<sites/>");

            var sut = new PatchGenerator(source);

            XDocument result = sut.GeneratePatchFile(new BasePatch[] { });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void PatchGenerator_GeneratePatchFile_NoPatches_GivesEmptyOutput()
        {
            var source = XDocument.Parse("<sites/>");

            var sut = new PatchGenerator(source);

            var result = sut.GeneratePatchFile(new BasePatch[] { });

            Assert.AreEqual("configuration", result.Root.Name);
            Assert.AreEqual(string.Empty, result.Root.Value);
        }

        [TestMethod]
        public void PatchGenerator_GeneratePatchFile_AddsPatchingNamespacesToRoot()
        {
            var source = XDocument.Parse("<sites/>");

            var sut = new PatchGenerator(source);

            var result = sut.GeneratePatchFile(new BasePatch[] { });

            var patchNS = result.Root.Attribute(XNamespace.Xmlns + "patch");
            Assert.IsNotNull(patchNS);
            Assert.AreEqual("http://www.sitecore.net/xmlconfig/", patchNS.Value);

            var setNS = result.Root.Attribute(XNamespace.Xmlns + "set");
            Assert.IsNotNull(setNS);
            Assert.AreEqual("http://www.sitecore.net/xmlconfig/set/", setNS.Value);
        }
    }

}