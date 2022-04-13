using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PatchMaker.App.PatchForms
{
    public partial class PatchNewChildForm : Form, IAddPatchForm
    {
        private readonly TreeNode _treeNode;

        private XDocument RootXml => (_treeNode.Tag as XElement).Document;

        public PatchItem Patch { get; private set; }

        public PatchNewChildForm()
        {
            InitializeComponent();
            this.ConfigureDialog();

            xPathForParent.PerformValidation = s => XPathExpression.Compile(s, RootXml.MakeNamespaceManager());
        }

        public PatchNewChildForm(TreeNode node) : this()
        {
            _treeNode = node ?? throw new ArgumentNullException(nameof(node));

            newElementTextBox.ConfigureInsertionContextMenu(RootXml.Root);
            var rootNode = treeView.BuildTreeView(node);

            var xPath = rootNode.DefaultXPath();

            xPathForParent.Text = xPath;
            newElementTextBox.Text = "<xml/>";
        }

        public PatchNewChildForm(PatchItem patchItem) : this()
        {
            _treeNode = patchItem.RelatedTreeNode;

            newElementTextBox.ConfigureInsertionContextMenu(RootXml.Root);
            treeView.BuildTreeView(_treeNode);

            var patch = patchItem.Patch as PatchInstead;

            xPathForParent.Text = patch.XPathForParent;
            newElementTextBox.Text = patch.Replacement.ToString();
        }

        private void OkBtn_Click(object sender, EventArgs e)
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
