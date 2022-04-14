using System.Xml.Linq;

namespace PatchMaker
{
    
    public class ConfigRule
    {
        public XNamespace Namespace { get; set; }
        public string Name { get; set; }
        public string Value { get; set; } 

        public ConfigRule(XNamespace ns, string name, string value)
        {
            Namespace = ns;
            Name = name;
            Value = value;
        }
    }

}