using System;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PatchMaker.App.PatchForms
{

    public partial class PatchDeleteForm : Form, IAddPatchForm
    {
        private readonly TreeNode _treeNode;
        
        private XDocument RootXml => (_treeNode.Tag as XElement).Document;

        public PatchItem Patch { get; private set; }

        protected PatchDeleteForm()
        {
            InitializeComponent();
            this.ConfigureDialog();

            xPathTextBox.PerformValidation = s => XPathExpression.Compile(s, RootXml.MakeNamespaceManager());
        }

        public PatchDeleteForm(TreeNode node) : this()
        {
            _treeNode = node ?? throw new ArgumentNullException(nameof(node));

            var rootNode = treeView.BuildTreeView(node);
            xPathTextBox.Text = rootNode.DefaultXPath();
        }

        public PatchDeleteForm(PatchItem patchItem) : this()
        {
            _treeNode = patchItem.RelatedTreeNode;

            treeView.BuildTreeView(_treeNode);

            var delete = patchItem.Patch as PatchDelete;
            xPathTextBox.Text = delete.XPathForElement;
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            var patchDelete = new PatchDelete(xPathTextBox.Text);
            Patch = new PatchItem(patchDelete, _treeNode);
        }

        private void PatchDeleteForm_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HelpSpawner.SpawnLocalFile("delete");
        }
    }

}
