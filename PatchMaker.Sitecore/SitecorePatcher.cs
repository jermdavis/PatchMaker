using System;
using System.Collections.Generic;
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
                catch(Exception ex)
                {
                    return $"<error><!-- {ex.Message} --></error>";
                }
            }

            return patcher.Document.OuterXml;
        }
    }

}