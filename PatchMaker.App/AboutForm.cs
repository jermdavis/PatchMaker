using System;
using System.Diagnostics;
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
