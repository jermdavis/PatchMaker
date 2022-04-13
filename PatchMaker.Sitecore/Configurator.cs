using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace PatchMaker.Sitecore
{

	// Inspired by https://github.com/benmcevoy/ConfigViewer
	public class CustomConfigurator
	{
		public string SourceXml { get; set; }
		public string PatchXml { get; set; }
		public string PatchFileName { get; set; }

		public XmlDocument Run(string layers, Dictionary<string, string> roles)
		{
			var nameValueCollection = new System.Collections.Specialized.NameValueCollection();

			foreach (var role in roles)
			{
				var suffix = global::Sitecore.Configuration.RuleBasedConfigReader.RuleDefineSuffix;
				nameValueCollection.Add($"{role.Key}:{suffix}", role.Value);
			}

			return (nameValueCollection.Keys.Count == 0 && layers == string.Empty)
				? global::Sitecore.Configuration.Factory.GetConfiguration()
				: GetRuleBasedConfiguration(nameValueCollection, layers ?? "");
		}

		/// <summary>
		/// Gets the configuration for specific configuration.
		/// </summary>
		/// <param name="ruleCollection">The rules collection.</param>
		/// <param name="layers">The layers.</param>
		/// <returns>
		/// Xml document containing the entire Sitecore configuration for specific configuration.
		/// </returns>
		protected virtual XmlDocument GetRuleBasedConfiguration(System.Collections.Specialized.NameValueCollection ruleCollection, string layers)
		{
			// Don't actually want to do layer processing here
			// We're only doing in-memory patching of two files...
			var includeFiles = new List<string>(); //GetIncludeFiles(layers2);

            var ruleBasedConfigReader = new CustomRuleBasedConfigReader(includeFiles, ruleCollection)
            {
                SourceXml = SourceXml,
                PatchXml = PatchXml,
                PatchFileName = PatchFileName
            };

            return ruleBasedConfigReader.DoGetConfiguration();
		}

		/// <summary>
		/// Returns a collection of include files for specific layers.
		/// </summary>
		/// <param name="layers">The layers array.</param>
		/// <returns>The list of include files.</returns>
		protected IEnumerable<string> GetIncludeFiles(string[] layers)
		{
			var layeredConfiguration = GetLayeredConfiguration();
			var source = layeredConfiguration.ConfigurationLayerProviders.SelectMany(x => x.GetLayers());

			if (layers.Length == 0)
			{
				return source.SelectMany(x => x.GetConfigurationFiles()).Distinct(StringComparer.OrdinalIgnoreCase);
			}

			return (from x in source
					where layers.Contains(x.Name)
					select x).SelectMany(x => x.GetConfigurationFiles()).Distinct(StringComparer.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Gets the layered configuration.
		/// </summary>
		/// <returns>An instance of the <see cref="T:global::Sitecore.Configuration.LayeredConfigurationFiles" /> class.</returns>
		protected global::Sitecore.Configuration.LayeredConfigurationFiles GetLayeredConfiguration()
		{
			return new global::Sitecore.Configuration.LayeredConfigurationFiles();
		}
	}

}