using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PatchMaker.App.PatchForms
{

    public partial class PatchAttributeForm : Form, IAddPatchForm
    {
        private readonly TreeNode _treeNode;

        private XDocument RootXml => (_treeNode.Tag as XElement).Document;

        public PatchItem Patch { get; private set; }

        protected PatchAttributeForm()
        {
            InitializeComponent();
            this.ConfigureDialog();

            this.nameTextBox.PerformValidation = s => new XAttribute(nameTextBox.Text, "");
            this.elementXPathTextBox.PerformValidation = s => XPathExpression.Compile(s, RootXml.MakeNamespaceManager());

            patchTypeCombo.Items.Add(AttributePatchTypes.Patch);
            patchTypeCombo.Items.Add(AttributePatchTypes.Set);
            patchTypeCombo.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public PatchAttributeForm(TreeNode node, AttributePatchTypes patchType) : this()
        {
            _treeNode = node ?? throw new ArgumentNullException(nameof(node));

            ConfigureDefaultDropdown(_treeNode);

            var rootNode = treeView.BuildTreeView(node);
            var xPath = rootNode.DefaultXPath();

            elementXPathTextBox.Text = xPath;
            nameTextBox.Text = string.Empty;
            valueTextBox.Text = string.Empty;
            patchTypeCombo.SelectedItem = patchType;
        }

        public PatchAttributeForm(PatchItem patchItem) : this()
        {
            _treeNode = patchItem.RelatedTreeNode;

            ConfigureDefaultDropdown(_treeNode);

            treeView.BuildTreeView(_treeNode);

            var patch = patchItem.Patch as BaseAttributeChange;

            patchTypeCombo.SelectedItem = AttributePatchTypes.Patch;
            elementXPathTextBox.Text = patch.XPathForElement;
            nameTextBox.Text = patch.AttributeName;
            valueTextBox.Text = patch.AttributeValue;
        }

        private void ConfigureDefaultDropdown(TreeNode node)
        {
            var element = node.Tag as XElement;

            foreach (var attr in element.Attributes())
            {
                if(attr.Name.Namespace.IsIgnorable(ignoreRoleAndSecurity:true))
                {
                    continue;
                }
                defaultComboBox.Items.Add(attr);
            }

            defaultComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            bool isValid = ValidateChildren();
            if(!isValid)
            {
                // Prevent closing the dialog if we've not got valid fields.
                // necessary because the name attribute name field starts empty - and hence doesn't get validated by default
                this.DialogResult = DialogResult.None;
                return;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }

            BasePatch patch;

            if( (AttributePatchTypes)patchTypeCombo.SelectedItem == AttributePatchTypes.Patch)
            {
                patch = new PatchAttribute(elementXPathTextBox.Text, nameTextBox.Text, valueTextBox.Text);
            }
            else
            {
                patch = new SetAttribute(elementXPathTextBox.Text, nameTextBox.Text, valueTextBox.Text);
            }

            Patch = new PatchItem(patch, _treeNode);
        }

        private void ApplyDefaultBtn_Click(object sender, EventArgs e)
        {
            if(defaultComboBox.SelectedItem != null)
            {
                var attr = defaultComboBox.SelectedItem as XAttribute;
                nameTextBox.Text = attr.Name.ToString();
                valueTextBox.Text = attr.Value;
            }
        }

        private void PatchAttributeForm_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            HelpSpawner.SpawnLocalFile("attributes");
        }
    }

}