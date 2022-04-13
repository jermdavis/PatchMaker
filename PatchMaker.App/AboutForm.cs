using System.Reflection;
using System.Windows.Forms;

namespace PatchMaker.App
{

    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            var version = Assembly.GetExecutingAssembly().GetName().Version;
            versionLabel.Text = $"v{version.Major}.{version.Minor}";
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            HelpSpawner.SpawnUrl("https://twitter.com/jermdavis");
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            HelpSpawner.SpawnUrl("https://github.com/jermdavis/PatchMaker");
        }
    }

}
