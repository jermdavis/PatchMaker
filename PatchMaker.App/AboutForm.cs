using System.Diagnostics;
using System.Windows.Forms;

namespace PatchMaker.App
{

    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            HelpSpawner.SpawnUrl("https://twitter.com/jermdavis");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            HelpSpawner.SpawnUrl("https://github.com/jermdavis/PatchMaker");
        }
    }

}
