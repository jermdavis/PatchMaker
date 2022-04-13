using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Xml;

namespace PatchMaker.Sitecore
{

    // Inspired by https://github.com/benmcevoy/ConfigViewer
    public class CustomRuleBasedConfigReader : global::Sitecore.Configuration.RuleBasedConfigReader
    {
        public string SourceXml { get; set; }
        public string PatchXml { get; set; }
        public string PatchFileName { get; set; }

        public CustomRuleBasedConfigReader(IEnumerable<string> includeFiles, NameValueCollection ruleCollection)
            : base(includeFiles, ruleCollection)
        {
        }

        private XmlNode GetCustomConfigNode()
        {
            var xd = new XmlDocument();
            xd.LoadXml(SourceXml);

            return xd.DocumentElement;
        }

        public new XmlDocument DoGetConfiguration()
        {
            var configNode = GetCustomConfigNode();

            // replace this - would this ever error any more?
            global::Sitecore.Diagnostics.Assert.IsNotNull((object)configNode, "Could not read Sitecore configuration.");

            var xmlDocument = new XmlDocument();

            // don't need to give it disk files to patch in
            // the patch is coming from in-memory below
            //this.IncludeFiles = new List<string>() { "patch.config" };

            xmlDocument.AppendChild(xmlDocument.CreateElement("sitecore"));
            var cp = GetConfigPatcher(xmlDocument.DocumentElement);
            cp.ApplyPatch(configNode);

            // we don't have all the other config files on disk,
            // so don't want to have this bit run
            //ExpandIncludeFiles(xmlDocument.DocumentElement, new Hashtable());
            //LoadIncludeFiles(xmlDocument.DocumentElement);

            try
            {
                using (var xml = new StringReader(PatchXml))
                {
                    cp.ApplyPatch(xml, PatchFileName);
                }
            }
            catch (Exception ex)
            {
                throw new RenderException($"Error processing patch file: {ex.Message}", ex);
            }

            ReplaceGlobalVariables(xmlDocument.DocumentElement);
            ReplaceEnvironmentVariables(xmlDocument.DocumentElement);

            return xmlDocument;
        }
    }

}