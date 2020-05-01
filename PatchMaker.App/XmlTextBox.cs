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

            foreach(var nsAttr in root.Attributes())
            {
                if(nsAttr.Name.NamespaceName == Namespaces.XmlnsUri)
                {
                    m.Items.Add(makeMenu(nsAttr.Name.LocalName, nsAttr.Value));
                }
            }

            this.ContextMenuStrip = m;
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