using System;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PatchMaker.App.PatchForms
{

    public partial class PatchInsteadForm : Form, IAddPatchForm
    {
        private readonly TreeNode _treeNode;
        private ConfigRule[] _rules = null;

        private XDocument RootXml => (_treeNode.Tag as XElement).Document;

        public PatchItem Patch { get; private set; }

        protected PatchInsteadForm()
        {
            InitializeComponent();
            this.ConfigureDialog();

            xPathForParent.PerformValidation = s => XPathExpression.Compile(s, RootXml.MakeNamespaceManager());
            xPathForReplacement.PerformValidation = s => XPathExpression.Compile(s, RootXml.MakeNamespaceManager());
        }

        public PatchInsteadForm(TreeNode node) : this()
        {
            _treeNode = node ?? throw new ArgumentNullException(nameof(node));

            newElementTextBox.ConfigureInsertionContextMenu(RootXml.Root);
            var rootNode = treeView.BuildTreeView(node);

            var xPath = rootNode.DefaultXPath();
            var idx = xPath.LastPathSegmentIndex();
            var parent = xPath.FirstPathSegment(idx);
            var order = xPath.RemainingPathSegments(idx);

            xPathForParent.Text = parent;
            xPathForReplacement.Text = order;
            newElementTextBox.Text = "<xml/>";

            UpdateButton();
        }

        public PatchInsteadForm(PatchItem patchItem) : this()
        {
            _treeNode = patchItem.RelatedTreeNode;

            newElementTextBox.ConfigureInsertionContextMenu(RootXml.Root);
            treeView.BuildTreeView(_treeNode);

            var patch = patchItem.Patch as PatchInstead;

            xPathForParent.Text = patch.XPathForParent;
            xPathForReplacement.Text = patch.XPathForReplacement;
            newElementTextBox.Text = patch.Replacement.ToString();
            _rules = patch.RoleBasedRules;

            UpdateButton();
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            var xml = XElement.Parse(newElementTextBox.Text);
            var patchInstead = new PatchInstead(xPathForParent.Text, xPathForReplacement.Text, xml, _rules);
            Patch = new PatchItem(patchInstead, _treeNode);
        }

        private void PatchInsteadForm_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HelpSpawner.SpawnLocalFile("replace");
        }

        private void UpdateButton()
        {
            var len = _rules == null ? 0 : _rules.Length;
            RBC_Btn.Text = $"Rules ({len})";
        }

        private void RBC_Btn_Click(object sender, EventArgs e)
        {
            // populate rule dialog
            // launch rule dialog

            // docs need updating for the rules dialog, and all the diagrams of patch dialogs, to add new button
            // This should be v1.6.0

            // assign dialog data to rule data
            _rules = new ConfigRule[] {
                new ConfigRule("http://www.sitecore.net/xmlconfig/localenv/", "require", "qa"),
                new ConfigRule("http://www.sitecore.net/xmlconfig/role/", "require", "ContentManagement")
            };

            UpdateButton();
        }
    }

}