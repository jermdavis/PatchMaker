using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            PerformValidation = s => {
                var xml = XDocument.Parse(s);
            };
        }

        private void inject(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            this.Paste($" {menu.Tag} ");
        }

        public void ConfigureInsertionContextMenu(XElement root)
        {
            var m = new ContextMenuStrip();

            // add operations to copy any namespaces declared on the root element
            foreach(var nsAttr in root.Attributes())
            {
                if(nsAttr.Name.NamespaceName == Namespaces.XmlnsUri)
                {
                    m.Items.Add(makeMenu(nsAttr.Name.LocalName, nsAttr.Value));
                }
            }

            // But check to ensure that role / security are always there because they're important
            addIfMissing(m, root, "role", Namespaces.Role);
            addIfMissing(m, root, "security", Namespaces.Security);

            this.ContextMenuStrip = m;
        }

        private void addIfMissing(ContextMenuStrip m, XElement root, string name, XNamespace ns)
        {
            var existed = root.Attributes().Where(a => a.Value == ns.NamespaceName).Any();
            if (!existed)
            {
                m.Items.Add(makeMenu(name, ns));
            }
        }

        private ToolStripMenuItem makeMenu(string name, string ns)
        {
            var menu = new ToolStripMenuItem();

            menu.Text = $"Add '{name}' namespace";
            menu.Tag = $"xmlns:{name}='{ns}'";
            menu.Click += inject;

            return menu;
        }

        private ToolStripMenuItem makeMenu(string name, XNamespace ns)
        {
            return makeMenu(name, ns.NamespaceName);
        }
    }

}   