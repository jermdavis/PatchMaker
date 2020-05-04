using PatchMaker.Sitecore;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PatchMaker.App
{
    public partial class PatchPreviewForm : Form
    {
        public string Role { get; set; } = "Standalone";

        public PatchPreviewForm(string sourceXml, string patchXml, Dictionary<string,string> roles)
        {
            InitializeComponent();
            this.ConfigureDialog();

            roleWarningLabel.Visible = checkIfRoleWarningRequired(patchXml);

            renderRoles(rolesLabel, roles);

            var result = SitecorePatcher.Apply(sourceXml, patchXml, "preview.patch.config", roles);

            var xml = XDocument.Parse(result);

            richTextBox.Text = xml.ToString();

            foreach(var line in richTextBox.Lines)
            {
                if(line.Contains("preview.patch.config"))
                {
                    int idx = richTextBox.Find(line);
                    richTextBox.Select(idx, line.Length);
                    richTextBox.SelectionColor = Color.Red;
                }
            }

            richTextBox.Select(0, 0);
        }

        private void renderRoles(Label rolesLabel, Dictionary<string,string> roles)
        {
            rolesLabel.Text = string.Empty; 
            foreach (var role in roles)
            {
                if(rolesLabel.Text.Length > 0)
                {
                    rolesLabel.Text += ", ";
                }
                rolesLabel.Text += role.Key + "=" + role.Value;
            }
        }

        private bool checkIfRoleWarningRequired(string patchXml)
        {
            return patchXml.Contains(Namespaces.RoleUri);
        }

        private void roleWarningLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            HelpSpawner.SpawnLocalFile("preview");
        }
    }
}