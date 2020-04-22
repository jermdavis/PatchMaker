using System;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PatchMaker.App.PatchForms
{

    public partial class PatchInsteadForm : Form, IAddPatchForm
    {
        private TreeNode _treeNode;

        public PatchItem Patch { get; private set; }

        protected PatchInsteadForm()
        {
            InitializeComponent();
            this.ConfigureDialog();

            xPathForParent.PerformValidation = s => XPathExpression.Compile(s);
            xPathForReplacement.PerformValidation = s => XPathExpression.Compile(s);
        }

        public PatchInsteadForm(TreeNode node) : this()
        {
            _treeNode = node ?? throw new ArgumentNullException(nameof(node));

            var rootNode = treeView.BuildTreeView(node);

            var xPath = rootNode.DefaultXPath();
            var idx = xPath.LastPathSegmentIndex();
            var parent = xPath.FirstPathSegment(idx);
            var order = xPath.RemainingPathSegments(idx);

            xPathForParent.Text = parent;
            xPathForReplacement.Text = order;
            newElementTextBox.Text = "<xml/>";
        }

        public PatchInsteadForm(PatchItem patchItem) : this()
        {
            _treeNode = patchItem.RelatedTreeNode;
            treeView.BuildTreeView(_treeNode);

            var patch = patchItem.Patch as PatchInstead;

            xPathForParent.Text = patch.XPathForParent;
            xPathForReplacement.Text = patch.XPathForReplacement;
            newElementTextBox.Text = patch.Replacement.ToString();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            var xml = XElement.Parse(newElementTextBox.Text);
            var patchInstead = new PatchInstead(xPathForParent.Text, xPathForReplacement.Text, xml);
            Patch = new PatchItem(patchInstead, _treeNode);
        }

        private void PatchInsteadForm_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HelpSpawner.SpawnLocalFile("replace");
        }
    }

}