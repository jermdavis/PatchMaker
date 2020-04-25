namespace PatchMaker.App
{
    partial class PatchPlanningForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatchPlanningForm));
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenuToolStripItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadXmlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.generatePatchesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.foundABugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.sourceTreeView = new PatchMaker.App.HighlightableTreeView();
            this.treeImageList = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.collapseBtn = new System.Windows.Forms.Button();
            this.filterBtn = new System.Windows.Forms.Button();
            this.filterTextBox = new System.Windows.Forms.TextBox();
            this.patchListBox = new System.Windows.Forms.ListBox();
            this.patchMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.moveUpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.treeMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.attributeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.patchDeleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.patchInsertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.patchInsteadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.patchMenu.SuspendLayout();
            this.treeMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 463);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(742, 22);
            this.statusStrip.TabIndex = 0;
            this.statusStrip.Text = "statusStrip1";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuToolStripItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(742, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileMenuToolStripItem
            // 
            this.fileMenuToolStripItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadXmlToolStripMenuItem,
            this.toolStripMenuItem2,
            this.generatePatchesToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileMenuToolStripItem.Name = "fileMenuToolStripItem";
            this.fileMenuToolStripItem.Size = new System.Drawing.Size(37, 20);
            this.fileMenuToolStripItem.Text = "&File";
            this.fileMenuToolStripItem.DropDownOpening += new System.EventHandler(this.fileMenuToolStripItem_DropDownOpening);
            // 
            // loadXmlToolStripMenuItem
            // 
            this.loadXmlToolStripMenuItem.Name = "loadXmlToolStripMenuItem";
            this.loadXmlToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.loadXmlToolStripMenuItem.Text = "&Load Xml";
            this.loadXmlToolStripMenuItem.Click += new System.EventHandler(this.loadXmlToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(162, 6);
            // 
            // generatePatchesToolStripMenuItem
            // 
            this.generatePatchesToolStripMenuItem.Name = "generatePatchesToolStripMenuItem";
            this.generatePatchesToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.generatePatchesToolStripMenuItem.Text = "&Generate Patches";
            this.generatePatchesToolStripMenuItem.Click += new System.EventHandler(this.generatePatchesToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(162, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openHelpToolStripMenuItem,
            this.foundABugToolStripMenuItem,
            this.toolStripMenuItem5,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // openHelpToolStripMenuItem
            // 
            this.openHelpToolStripMenuItem.Name = "openHelpToolStripMenuItem";
            this.openHelpToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.openHelpToolStripMenuItem.Text = "&Open Help";
            this.openHelpToolStripMenuItem.Click += new System.EventHandler(this.openHelpToolStripMenuItem_Click);
            // 
            // foundABugToolStripMenuItem
            // 
            this.foundABugToolStripMenuItem.Name = "foundABugToolStripMenuItem";
            this.foundABugToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.foundABugToolStripMenuItem.Text = "Found a bug?";
            this.foundABugToolStripMenuItem.Click += new System.EventHandler(this.foundABugToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(143, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.sourceTreeView);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.patchListBox);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Size = new System.Drawing.Size(742, 439);
            this.splitContainer1.SplitterDistance = 400;
            this.splitContainer1.TabIndex = 2;
            // 
            // sourceTreeView
            // 
            this.sourceTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceTreeView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.sourceTreeView.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceTreeView.HideSelection = false;
            this.sourceTreeView.ImageIndex = 0;
            this.sourceTreeView.ImageList = this.treeImageList;
            this.sourceTreeView.Location = new System.Drawing.Point(0, 33);
            this.sourceTreeView.Name = "sourceTreeView";
            this.sourceTreeView.SelectedImageIndex = 0;
            this.sourceTreeView.Size = new System.Drawing.Size(400, 406);
            this.sourceTreeView.TabIndex = 2;
            this.sourceTreeView.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.sourceTreeView_BeforeCollapse);
            this.sourceTreeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.sourceTreeView_BeforeExpand);
            // 
            // treeImageList
            // 
            this.treeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeImageList.ImageStream")));
            this.treeImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.treeImageList.Images.SetKeyName(0, "FolderClosed_16x.png");
            this.treeImageList.Images.SetKeyName(1, "FolderOpened_16x.png");
            this.treeImageList.Images.SetKeyName(2, "SearchFolderClosed_16x.png");
            this.treeImageList.Images.SetKeyName(3, "SearchFolderOpened_16x.png");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.collapseBtn);
            this.panel1.Controls.Add(this.filterBtn);
            this.panel1.Controls.Add(this.filterTextBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(400, 33);
            this.panel1.TabIndex = 1;
            // 
            // collapseBtn
            // 
            this.collapseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.collapseBtn.Image = ((System.Drawing.Image)(resources.GetObject("collapseBtn.Image")));
            this.collapseBtn.Location = new System.Drawing.Point(370, 5);
            this.collapseBtn.Name = "collapseBtn";
            this.collapseBtn.Size = new System.Drawing.Size(25, 23);
            this.collapseBtn.TabIndex = 2;
            this.collapseBtn.UseVisualStyleBackColor = true;
            this.collapseBtn.Click += new System.EventHandler(this.collapseBtn_Click);
            // 
            // filterBtn
            // 
            this.filterBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.filterBtn.Image = ((System.Drawing.Image)(resources.GetObject("filterBtn.Image")));
            this.filterBtn.Location = new System.Drawing.Point(341, 5);
            this.filterBtn.Name = "filterBtn";
            this.filterBtn.Size = new System.Drawing.Size(25, 23);
            this.filterBtn.TabIndex = 1;
            this.filterBtn.UseVisualStyleBackColor = true;
            this.filterBtn.Click += new System.EventHandler(this.filterBtn_Click);
            // 
            // filterTextBox
            // 
            this.filterTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filterTextBox.Location = new System.Drawing.Point(8, 6);
            this.filterTextBox.Name = "filterTextBox";
            this.filterTextBox.Size = new System.Drawing.Size(327, 20);
            this.filterTextBox.TabIndex = 0;
            this.filterTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.filterTextBox_KeyPress);
            // 
            // patchListBox
            // 
            this.patchListBox.ContextMenuStrip = this.patchMenu;
            this.patchListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.patchListBox.FormattingEnabled = true;
            this.patchListBox.IntegralHeight = false;
            this.patchListBox.Location = new System.Drawing.Point(0, 33);
            this.patchListBox.Name = "patchListBox";
            this.patchListBox.Size = new System.Drawing.Size(338, 406);
            this.patchListBox.TabIndex = 3;
            this.patchListBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.patchListBox_MouseDoubleClick);
            // 
            // patchMenu
            // 
            this.patchMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripMenuItem4,
            this.moveUpToolStripMenuItem,
            this.moveDownToolStripMenuItem});
            this.patchMenu.Name = "patchMenu";
            this.patchMenu.Size = new System.Drawing.Size(181, 120);
            this.patchMenu.Opening += new System.ComponentModel.CancelEventHandler(this.patchMenu_Opening);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.editToolStripMenuItem.Text = "Edit patch";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.deleteToolStripMenuItem.Text = "Delete patch";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(177, 6);
            // 
            // moveUpToolStripMenuItem
            // 
            this.moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            this.moveUpToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.moveUpToolStripMenuItem.Text = "Move up";
            this.moveUpToolStripMenuItem.Click += new System.EventHandler(this.moveUpToolStripMenuItem_Click);
            // 
            // moveDownToolStripMenuItem
            // 
            this.moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            this.moveDownToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.moveDownToolStripMenuItem.Text = "Move down";
            this.moveDownToolStripMenuItem.Click += new System.EventHandler(this.moveDownToolStripMenuItem_Click);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(338, 33);
            this.panel2.TabIndex = 0;
            // 
            // treeMenu
            // 
            this.treeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.attributeToolStripMenuItem,
            this.toolStripMenuItem3,
            this.patchDeleteToolStripMenuItem,
            this.patchInsertToolStripMenuItem,
            this.patchInsteadToolStripMenuItem});
            this.treeMenu.Name = "treeMenu";
            this.treeMenu.Size = new System.Drawing.Size(166, 98);
            // 
            // attributeToolStripMenuItem
            // 
            this.attributeToolStripMenuItem.Name = "attributeToolStripMenuItem";
            this.attributeToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.attributeToolStripMenuItem.Text = "Change Attribute";
            this.attributeToolStripMenuItem.Click += new System.EventHandler(this.attributeToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(162, 6);
            // 
            // patchDeleteToolStripMenuItem
            // 
            this.patchDeleteToolStripMenuItem.Name = "patchDeleteToolStripMenuItem";
            this.patchDeleteToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.patchDeleteToolStripMenuItem.Text = "Delete Element";
            this.patchDeleteToolStripMenuItem.Click += new System.EventHandler(this.patchDeleteToolStripMenuItem_Click);
            // 
            // patchInsertToolStripMenuItem
            // 
            this.patchInsertToolStripMenuItem.Name = "patchInsertToolStripMenuItem";
            this.patchInsertToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.patchInsertToolStripMenuItem.Text = "Insert Element";
            this.patchInsertToolStripMenuItem.Click += new System.EventHandler(this.patchInsertToolStripMenuItem_Click);
            // 
            // patchInsteadToolStripMenuItem
            // 
            this.patchInsteadToolStripMenuItem.Name = "patchInsteadToolStripMenuItem";
            this.patchInsteadToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.patchInsteadToolStripMenuItem.Text = "Replace Element";
            this.patchInsteadToolStripMenuItem.Click += new System.EventHandler(this.patchInsteadToolStripMenuItem_Click);
            // 
            // PatchPlanningForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 485);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "PatchPlanningForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PatchPlanningForm_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.patchMenu.ResumeLayout(false);
            this.treeMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenuToolStripItem;
        private System.Windows.Forms.ToolStripMenuItem loadXmlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generatePatchesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ContextMenuStrip treeMenu;
        private System.Windows.Forms.ToolStripMenuItem attributeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem patchDeleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem patchInsertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem patchInsteadToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ContextMenuStrip patchMenu;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveUpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveDownToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openHelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem foundABugToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button filterBtn;
        private System.Windows.Forms.TextBox filterTextBox;
        private HighlightableTreeView sourceTreeView;
        private System.Windows.Forms.ImageList treeImageList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button collapseBtn;
        private System.Windows.Forms.ListBox patchListBox;
    }
}

