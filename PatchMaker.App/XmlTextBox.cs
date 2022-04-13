using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PatchMaker.App
{

    public class XmlTextBox : RequiredFieldTextBox
    {
        public XmlTextBox()
        {
            this.Multiline = true;
            this.AcceptsReturn = true;
            PerformValidation = s =>
            {
                var xml = XDocument.Parse(s);
            };
        }

        private void Inject(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            this.Paste($" {menu.Tag} ");
        }

        public void ConfigureInsertionContextMenu(XElement root)
        {
            var m = new ContextMenuStrip();

            // add operations to copy any namespaces declared on the root element
            foreach (var nsAttr in root.Attributes())
            {
                if (nsAttr.Name.NamespaceName == Namespaces.XmlnsUri)
                {
                    m.Items.Add(MakeMenu(nsAttr.Name.LocalName, nsAttr.Value));
                }
            }

            // But check to ensure that role / security are always there because they're important
            AddIfMissing(m, root, "role", Namespaces.Role);
            AddIfMissing(m, root, "security", Namespaces.Security);

            this.ContextMenuStrip = m;
        }

        private void AddIfMissing(ContextMenuStrip m, XElement root, string name, XNamespace ns)
        {
            var existed = root.Attributes().Where(a => a.Value == ns.NamespaceName).Any();
            if (!existed)
            {
                m.Items.Add(MakeMenu(name, ns));
            }
        }

        private ToolStripMenuItem MakeMenu(string name, string ns)
        {
            var menu = new ToolStripMenuItem
            {
                Text = $"Add '{name}' namespace",
                Tag = $"xmlns:{name}='{ns}'"
            };
            menu.Click += Inject;

            return menu;
        }

        private ToolStripMenuItem MakeMenu(string name, XNamespace ns)
        {
            return MakeMenu(name, ns.NamespaceName);
        }
    }

}