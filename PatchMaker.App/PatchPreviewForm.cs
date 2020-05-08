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
        public PatchPreviewForm(string sourceXml, string patchXml, Dictionary<string,string> roles)
        {
            InitializeComponent();
            this.ConfigureDialog();

            renderRoles(rolesLabel, roles);

            var result = SitecorePatcher.ApplyWithRoles(sourceXml, patchXml, "preview.patch.config", roles);

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

        private void roleWarningLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            HelpSpawner.SpawnLocalFile("preview");
        }

        private void nextBtn_Click(object sender, System.EventArgs e)
        {
            int pos = richTextBox.SelectionStart + richTextBox.SelectionLength;
            int hit = richTextBox.Find("patch:source=\"preview.patch.config\"", pos, RichTextBoxFinds.WholeWord);
            if (hit == -1 && e != null)
            {
                // If no hit, go back to the start - but don't let it loop indefinitely
                richTextBox.Select(0, 0);
                nextBtn_Click(sender, null);
            }
            else
            {
                int idx = richTextBox.GetFirstCharIndexOfCurrentLine();
                int pos2 = richTextBox.SelectionStart + richTextBox.SelectionLength;
                richTextBox.SelectionStart = idx;
                richTextBox.SelectionLength = pos2 - idx + 1;
                richTextBox.ScrollToCaret();
            }
        }
    }
}