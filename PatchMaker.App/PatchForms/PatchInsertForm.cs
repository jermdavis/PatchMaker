using System;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PatchMaker.App.PatchForms
{

    public partial class PatchInsertForm : Form, IAddPatchForm
    {
        private TreeNode _treeNode;

        private XDocument _rootXml => (_treeNode.Tag as XElement).Document;

        public PatchItem Patch { get; private set; }

        protected PatchInsertForm()
        {
            InitializeComponent();
            this.ConfigureDialog();

            parentXPathTextBox.PerformValidation = s => XPathExpression.Compile(s, _rootXml.MakeNamespaceManager());
            orderXPathTextBox.PerformValidation = s => XPathExpression.Compile(s, _rootXml.MakeNamespaceManager());

            positionComboBox.Items.Add(ElementInsertPosition.Before);
            positionComboBox.Items.Add(ElementInsertPosition.After);
            positionComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public PatchInsertForm(TreeNode node) : this()
        {
            _treeNode = node ?? throw new ArgumentNullException(nameof(node));

            var rootNode = treeView.BuildTreeView(node);

            var xPath = rootNode.DefaultXPath();
            var idx = xPath.LastPathSegmentIndex();
            var parent = xPath.FirstPathSegment(idx);
            var order = xPath.RemainingPathSegments(idx);

            parentXPathTextBox.Text = parent;
            orderXPathTextBox.Text = order;
            positionComboBox.SelectedIndex = 0;
            newElementTextBox.Text = "<xml/>";
        }

        public PatchInsertForm(PatchItem patchItem) : this()
        {
            _treeNode = patchItem.RelatedTreeNode;
            treeView.BuildTreeView(_treeNode);

            var patch = patchItem.Patch as PatchInsert;

            parentXPathTextBox.Text = patch.XPathForParent;
            orderXPathTextBox.Text = patch.XPathForOrder;
            positionComboBox.SelectedItem = patch.Position;
            newElementTextBox.Text = patch.NewElement.ToString();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            var xml = XElement.Parse(newElementTextBox.Text);

            var patchInsert = new PatchInsert(
                parentXPathTextBox.Text,
                (ElementInsertPosition)positionComboBox.SelectedItem,
                orderXPathTextBox.Text,
                xml
            );

            Patch = new PatchItem(patchInsert, _treeNode);
        }

        private void PatchInsertForm_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HelpSpawner.SpawnLocalFile("insert");
        }
    }
}
