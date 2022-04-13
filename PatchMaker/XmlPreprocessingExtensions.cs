using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace PatchMaker
{

    public static class XmlPreprocessingExtensions
    {
        private static readonly Regex ns = new Regex(@"xmlns:(?<name>\w*)="".*?""", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool RequiresSimplifying(string xml)
        {
            var config = XDocument.Parse(xml);
            if (config != null && config.Root.Name.LocalName == "configuration")
            {
                return true;
            }

            if (xml.Contains("patch:") && !xml.Contains("patch:source"))
            {
                return true;
            }

            return false;
        }

        public static XDocument LoadAndSimplify(string xml)
        {
            var matches = ns.Matches(xml);
            foreach (Match match in matches)
            {
                var name = match.Groups["name"].Value;
                if (!string.IsNullOrEmpty(name))
                {
                    xml = Regex.Replace(xml, $@"{name}:.*?="".*?""", "");
                }
            }

            var config = XDocument.Parse(xml);

            var cfg = config.Root;
            var sc = cfg.Element("sitecore");

            if (cfg != null && sc != null)
            {
                cfg.Remove();
                config.Add(sc);
            }

            return config;
        }
    }

}