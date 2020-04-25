﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PatchMaker.App.PatchForms
{
    public partial class PatchNewChildForm : Form, IAddPatchForm
    {
        private TreeNode _treeNode;

        public PatchItem Patch { get; private set; }

        public PatchNewChildForm()
        {
            InitializeComponent();
            this.ConfigureDialog();
        }

        public PatchNewChildForm(TreeNode node) : this()
        {
            _treeNode = node ?? throw new ArgumentNullException(nameof(node));

            var rootNode = treeView.BuildTreeView(node);

            var xPath = rootNode.DefaultXPath();

            xPathForParent.Text = xPath;
            newElementTextBox.Text = "<xml/>";
        }

        public PatchNewChildForm(PatchItem patchItem) : this()
        {
            _treeNode = patchItem.RelatedTreeNode;
            treeView.BuildTreeView(_treeNode);

            var patch = patchItem.Patch as PatchInstead;

            xPathForParent.Text = patch.XPathForParent;
            newElementTextBox.Text = patch.Replacement.ToString();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            var xml = XElement.Parse(newElementTextBox.Text);
            var patchChild = new PatchNewChild(xPathForParent.Text, xml);
            Patch = new PatchItem(patchChild, _treeNode);
        }

        private void PatchInsteadForm_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HelpSpawner.SpawnLocalFile("newchild");
        }
    }
}
