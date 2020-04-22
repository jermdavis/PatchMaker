using PatchMaker.Sitecore;
using System.Data;
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

            patchResultEdit.Text = xml.ToString();
            patchResultEdit.Select(0, 0);
        }
    }
}
