﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PatchMaker.App
{
    public partial class PatchGenerationForm : Form
    {
        private readonly IEnumerable<BasePatch> _patches;
        private readonly XDocument _sourceXml;
        private readonly XDocument _patch;
        private readonly string _sourceFileName;

        public Dictionary<string, string> RoleConfig { get; set; } = new Dictionary<string, string>();

        public PatchGenerationForm(ListBox patchListView, XDocument sourceXml, string sourceFileName)
        {
            InitializeComponent();
            this.ConfigureDialog();

            if (!File.Exists("Sitecore.Kernel.dll"))
            {
                previewBtn.Enabled = false;
            }

            if (patchListView == null)
            {
                throw new ArgumentNullException(nameof(patchListView));
            }

            _sourceXml = sourceXml ?? throw new ArgumentNullException(nameof(sourceXml));

            _sourceFileName = sourceFileName;

            _patches = ExtractPatches(patchListView);

            var generator = new PatchGenerator(_sourceXml);
            _patch = generator.GeneratePatchFile(_patches);
            patchXmlEdit.Text = _patch.ToString();

            // Generating a preview for a patch with errors can crash the Sitecore patch engine
            // So we prevent that to avoid the confusing UI generated
            if (patchXmlEdit.Text.Contains("<!--ERROR:"))
            {
                previewBtn.Enabled = false;
            }

            patchXmlEdit.Select(0, 0);
        }

        private IEnumerable<BasePatch> ExtractPatches(ListBox listView)
        {
            var patches = new List<BasePatch>();

            foreach (PatchItem patchListItem in listView.Items)
            {
                if (patchListItem != null)
                {
                    patches.Add(patchListItem.Patch);
                }
            }

            return patches;
        }

        private void PreviewBtn_Click(object sender, EventArgs e)
        {
            var preview = new PatchPreviewForm(_sourceXml.ToString(), patchXmlEdit.Text, RoleConfig);
            preview.ShowDialog(this);
        }

        private string MakeNewFilename(string fileName)
        {
            return $"{Path.GetFileNameWithoutExtension(fileName)}-patch";
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog()
            {
                Filter = "Sitecore Config Patch (*.config)|*.config",
                OverwritePrompt = true,
                FileName = MakeNewFilename(_sourceFileName)
            };
            var dr = sfd.ShowDialog(this);

            if (dr == DialogResult.OK)
            {
                _patch.Save(sfd.FileName);
            }
        }

        private void PatchGenerationForm_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HelpSpawner.SpawnLocalFile("generate");
        }

        private void RoleBtn_Click(object sender, EventArgs e)
        {
            var rc = new RoleConfigForm();
            rc.Initialise(this.RoleConfig);

            var dr = rc.ShowDialog(this);

            if (dr == DialogResult.OK)
            {
                this.RoleConfig = rc.RoleConfig;
            }
        }
    }
}
