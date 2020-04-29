using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;

namespace PatchMaker
{

    public class PatchGenerator
    {
        private XDocument _sourceXml;

        public PatchGenerator(XDocument sourceXml)
        {
            _sourceXml = sourceXml ?? throw new ArgumentNullException(nameof(sourceXml));

            if(_sourceXml.Root == null)
            {
                throw new ArgumentException("The source XML needs to have at least one element to be patched.", nameof(sourceXml));
            }
        }

        public XDocument GeneratePatchFile(IEnumerable<BasePatch> patches)
        {
            if(patches == null)
            {
                throw new ArgumentNullException(nameof(patches));
            }

            var patchDocument = generateBasePatchData();

            foreach(var patch in patches)
            {
                patch.ApplyPatchElement(_sourceXml, patchDocument);
            }

            return patchDocument;
        }

        private XDocument generateBasePatchData()
        {
            XDocument basePatchDoc = new XDocument();

            // copy root of source to output
            var newRoot = new XElement("configuration");
            basePatchDoc.Add(newRoot);

            // add patching namespaces
            basePatchDoc.Root.Add(new XAttribute(XNamespace.Xmlns + "patch", Namespaces.PatchUri));
            basePatchDoc.Root.Add(new XAttribute(XNamespace.Xmlns + "set", Namespaces.SetUri));
            
            foreach(var nsAttr in _sourceXml.Root.Attributes())
            {
                if(nsAttr.Value == Namespaces.PatchUri || nsAttr.Value == Namespaces.SetUri)
                {
                    continue;
                }
                if(nsAttr.Name.NamespaceName == Namespaces.XmlnsUri)
                {
                    basePatchDoc.Root.Add(nsAttr);
                }
            }

            return basePatchDoc;
        }
    }

}
