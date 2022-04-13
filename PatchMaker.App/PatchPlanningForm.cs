using PatchMaker.App.PatchForms;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace PatchMaker.App
{
    public partial class PatchPlanningForm : Form
    {
        private readonly string _titleTemplate = "PatchMaker: [{0}]";
        private readonly PatchProcessManager _manager = new PatchProcessManager();

        public PatchPlanningForm()
        {
            InitializeComponent();

            // Make sure right click selects like left click in both listings
            sourceTreeView.NodeMouseClick += (sender, args) => sourceTreeView.SelectedNode = args.Node;
            patchListBox.MouseDown += (sender, e) => patchListBox.SelectedIndex = patchListBox.IndexFromPoint(e.X, e.Y);

            _manager.ConfigureControls(sourceTreeView, patchListBox, treeMenu);

            if (!File.Exists("Sitecore.Kernel.dll"))
            {
                statusStrip.Items.Add("Sitecore.Kernel.dll not found - preview disabled.");
            }
            else
            {
                statusStrip.Items.Add("Preview enabled.");
            }

            this.Text = string.Format(_titleTemplate, "-no file-");
        }

        private void LoadXmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog()
            {
                Filter = "Sitecore Config (*.config)|*.config|Xml Files (*.xml)|*.xml"
            };

            var dr = ofd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                try
                {
                    _manager.Initialise(ofd.FileName);
                    sourceTreeView.Nodes[0].Expand();
                    this.Text = string.Format(_titleTemplate, Path.GetFileName(ofd.FileName));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        //this, 
                        $"Exception trapped while loading \"{Path.GetFileName(ofd.FileName)}\"\n{ex.Message}",
                        "Unable to load that xml",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
        }

        private void ShowAddDialog(Func<TreeNode, IAddPatchForm> createForm)
        {
            var node = sourceTreeView.SelectedNode;
            if (node == null)
            {
                return;
            }

            var pdf = createForm(node);
            var dr = pdf.ShowDialog(this);

            if (dr == DialogResult.OK)
            {
                patchListBox.Items.Add(pdf.Patch);
            }
        }

        private void TreeMenu_Opening(object sender, CancelEventArgs e)
        {
            if (sourceTreeView.SelectedNode == null)
            {
                addAChildToolStripMenuItem.Enabled = false;
                return;
            }

            var hasChildren = sourceTreeView.SelectedNode.Nodes.Count == 0;
            addAChildToolStripMenuItem.Enabled = hasChildren;
        }

        private void AddAChildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAddDialog(n => new PatchNewChildForm(n));
        }

        private void PatchDeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAddDialog(n => new PatchDeleteForm(n));
        }

        private void PatchInsertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAddDialog(n => new PatchInsertForm(n));
        }

        private void PatchInsteadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAddDialog(n => new PatchInsteadForm(n));
        }

        private void AttributeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAddDialog(n => new PatchAttributeForm(n, AttributePatchTypes.Patch));
        }

        private void GeneratePatchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var generateDialog = new PatchGenerationForm(patchListBox, _manager.Source, _manager.SourceFileName)
            {
                RoleConfig = _manager.RoleConfig
            };
            generateDialog.ShowDialog(this);
            _manager.RoleConfig = generateDialog.RoleConfig;
        }

        private void FileMenuToolStripItem_DropDownOpening(object sender, EventArgs e)
        {
            bool ready = _manager.Source != null && patchListBox.Items.Count > 0;

            generatePatchesToolStripMenuItem.Enabled = ready;
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PatchMenu_Opening(object sender, CancelEventArgs e)
        {
            deleteToolStripMenuItem.Enabled = patchListBox.SelectedIndex != -1;
            editToolStripMenuItem.Enabled = patchListBox.SelectedIndex != -1;

            moveUpToolStripMenuItem.Enabled = patchListBox.SelectedIndex > 0;
            moveDownToolStripMenuItem.Enabled = patchListBox.SelectedIndex > -1 && patchListBox.SelectedIndex < patchListBox.Items.Count - 1;
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var item = patchListBox.SelectedItem;
            patchListBox.Items.Remove(item);
        }

        private void EditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(patchListBox.SelectedItem is PatchItem item))
            {
                return;
            }

            Func<PatchItem, IAddPatchForm> createDialog = null;
            if (item.Patch is PatchDelete)
            {
                createDialog = i => new PatchDeleteForm(i);
            }
            if (item.Patch is PatchInsert)
            {
                createDialog = i => new PatchInsertForm(i);
            }
            if (item.Patch is PatchInstead)
            {
                createDialog = i => new PatchInsteadForm(i);
            }
            if (item.Patch is PatchAttribute)
            {
                createDialog = i => new PatchAttributeForm(i);
            }
            if (item.Patch is SetAttribute)
            {
                createDialog = i => new PatchAttributeForm(i);
            }
            if (item.Patch is PatchNewChild)
            {
                createDialog = i => new PatchNewChildForm(i);
            }

            var dialog = createDialog(item);
            var dr = dialog.ShowDialog(this);

            if (dr == DialogResult.OK)
            {
                patchListBox.Items[patchListBox.SelectedIndex] = dialog.Patch;
            }
        }

        private void PatchListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            EditToolStripMenuItem_Click(sender, e);
        }

        private void OpenHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpSpawner.SpawnLocalFile(null);
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var af = new AboutForm();
            af.ShowDialog(this);
        }

        private void FoundABugToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpSpawner.SpawnUrl("https://github.com/jermdavis/PatchMaker/issues");
        }

        private void FilterBtn_Click(object sender, EventArgs e)
        {
            var txt = filterTextBox.Text;

            if (sourceTreeView.Nodes.Count > 0)
            {
                var root = sourceTreeView.Nodes[0];

                sourceTreeView.SuspendLayout();
                root.HighlightNodesRecursive(txt);

                if (sourceTreeView.SelectedNode != null)
                {
                    sourceTreeView.SelectedNode.EnsureVisible();
                }

                sourceTreeView.ResumeLayout();
            }
        }

        private void SourceTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node is HighlightableTreeNode)
            {
                (e.Node as HighlightableTreeNode).OnExpand();
            }
        }

        private void SourceTreeView_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node is HighlightableTreeNode)
            {
                (e.Node as HighlightableTreeNode).OnCollapse();
            }
        }

        private void FilterTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                FilterBtn_Click(sender, e);
            }
        }

        private void CollapseBtn_Click(object sender, EventArgs e)
        {
            if (sourceTreeView.Nodes.Count > 0)
            {
                sourceTreeView.CollapseAll();
                sourceTreeView.Nodes[0].Expand();
            }
        }

        private void PatchPlanningForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // This is necessary because otherwise you get odd over-draw
            // on the treeview when the app is exiting. Looks strange without this.
            sourceTreeView.DrawMode = TreeViewDrawMode.Normal;
        }

        private void MoveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            patchListBox.MoveUp();
        }

        private void MoveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            patchListBox.MoveDown();
        }
    }

}
