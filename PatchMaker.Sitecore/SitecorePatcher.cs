using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Visualization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;

namespace PatchMaker.Sitecore
{

	public static class SitecorePatcher
    {
        public static string Apply(string sourceXml, string patchXml, string patchFileName = null)
        {
            var sourceDoc = new XmlDocument();
            sourceDoc.LoadXml(sourceXml);

            var patcher = new global::Sitecore.Configuration.ConfigPatcher(sourceDoc.DocumentElement);

            using (var xml = new StringReader(patchXml))
            {
                try
                {
                    patcher.ApplyPatch(xml, patchFileName);
                }
                catch (Exception ex)
                {
                    return $"<error><!-- {ex.Message} --></error>";
                }
            }

            return patcher.Document.OuterXml;
        }

        //
        // V9 / role approach worked out from: https://github.com/benmcevoy/ConfigViewer/blob/master/ConfigView/Program.cs
        //
        public static string Apply(string sourceXml, string patchXml, string patchFileName, Dictionary<string, string> roles)
		{
			var c = new CustomConfigurator();

			c.SourceXml = sourceXml;
			c.PatchXml = patchXml;
			c.PatchFileName = patchFileName;
			var xml = c.Run("Sitecore|Modules|Environment|Custom", roles);	

			return xml.OuterXml;
		}
	}

}