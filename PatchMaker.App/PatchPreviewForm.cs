using PatchMaker.Sitecore;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PatchMaker.App
{
    public partial class PatchPreviewForm : Form
    {
        public PatchPreviewForm(string sourceXml, string patchXml)
        {
            InitializeComponent();
            this.ConfigureDialog();

            var result = SitecorePatcher.Apply(sourceXml, patchXml, "preview.patch.config");

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
    }
}