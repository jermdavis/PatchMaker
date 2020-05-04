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

		/// <summary>
		/// Initializes a new instance of the <see cref="T:global::Sitecore.Configuration.RuleBasedConfigReader" /> class.
		/// </summary>
		/// <param name="includeFiles">The include files.</param>
		/// <param name="ruleCollection">The rule collection.</param>
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

			using (var xml = new StringReader(PatchXml))
			{
				cp.ApplyPatch(xml, PatchFileName);
			}
			
			ReplaceGlobalVariables(xmlDocument.DocumentElement);
			ReplaceEnvironmentVariables(xmlDocument.DocumentElement);

			return xmlDocument;
		}
	}

}