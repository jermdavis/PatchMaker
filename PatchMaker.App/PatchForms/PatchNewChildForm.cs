using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PatchMaker.App.PatchForms
{
    public partial class PatchNewChildForm : Form, IAddPatchForm
    {
        private TreeNode _treeNode;

        private XDocument _rootXml => (_treeNode.Tag as XElement).Document;

        public PatchItem Patch { get; private set; }

        public PatchNewChildForm()
        {
            InitializeComponent();
            this.ConfigureDialog();

            xPathForParent.PerformValidation = s => XPathExpression.Compile(s, _rootXml.MakeNamespaceManager());
        }

        public PatchNewChildForm(TreeNode node) : this()
        {
            _treeNode = node ?? throw new ArgumentNullException(nameof(node));

            newElementTextBox.ConfigureInsertionContextMenu(_rootXml.Root);
            var rootNode = treeView.BuildTreeView(node);

            var xPath = rootNode.DefaultXPath();

            xPathForParent.Text = xPath;
            newElementTextBox.Text = "<xml/>";
        }

        public PatchNewChildForm(PatchItem patchItem) : this()
        {
            _treeNode = patchItem.RelatedTreeNode;

            newElementTextBox.ConfigureInsertionContextMenu(_rootXml.Root);
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

        private void PatchNewChildForm_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            HelpSpawner.SpawnLocalFile("newchild");
        }
    }
}
