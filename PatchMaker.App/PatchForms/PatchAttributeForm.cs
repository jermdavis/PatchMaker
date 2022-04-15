using System;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PatchMaker.App.PatchForms
{

    public partial class PatchAttributeForm : Form, IAddPatchForm
    {
        private readonly TreeNode _treeNode;
        private ConfigRule[] _rules = null;

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

            UpdateButton();
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
            _rules = patch.RoleBasedRules;

            UpdateButton();
        }

        private void ConfigureDefaultDropdown(TreeNode node)
        {
            var element = node.Tag as XElement;

            foreach (var attr in element.Attributes())
            {
                if (attr.Name.Namespace.IsIgnorable(ignoreRoleAndSecurity: true))
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
            if (!isValid)
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

            if ((AttributePatchTypes)patchTypeCombo.SelectedItem == AttributePatchTypes.Patch)
            {
                patch = new PatchAttribute(elementXPathTextBox.Text, nameTextBox.Text, valueTextBox.Text, _rules);
            }
            else
            {
                patch = new SetAttribute(elementXPathTextBox.Text, nameTextBox.Text, valueTextBox.Text, _rules);
            }

            Patch = new PatchItem(patch, _treeNode);
        }

        private void ApplyDefaultBtn_Click(object sender, EventArgs e)
        {
            if (defaultComboBox.SelectedItem != null)
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