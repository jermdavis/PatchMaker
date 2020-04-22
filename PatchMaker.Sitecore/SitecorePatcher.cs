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
                patcher.ApplyPatch(xml, patchFileName);
            }

            return patcher.Document.OuterXml;
        }
    }

}