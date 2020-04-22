namespace PatchMaker.App.PatchForms
{
    partial class PatchInsertForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatchInsertForm));
            this.cancelBtn = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.parentXPathTextBox = new PatchMaker.App.RequiredFieldTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.orderXPathTextBox = new PatchMaker.App.RequiredFieldTextBox();
            this.positionComboBox = new System.Windows.Forms.ComboBox();
            this.newElementTextBox = new PatchMaker.App.XmlTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.treeView = new PatchMaker.App.XmlFragmentTreeView();
            this.SuspendLayout();
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.CausesValidation = false;
            this.cancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelBtn.Location = new System.Drawing.Point(605, 308);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(75, 23);
            this.cancelBtn.TabIndex = 6;
            this.cancelBtn.Text = "Cancel";
            this.cancelBtn.UseVisualStyleBackColor = true;
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(524, 308);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tree:";
            // 
            // parentXPathTextBox
            // 
            this.parentXPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parentXPathTextBox.EmptyFieldValidationMessage = "Field can\'t be empty";
            this.parentXPathTextBox.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.parentXPathTextBox.Location = new System.Drawing.Point(60, 115);
            this.parentXPathTextBox.Name = "parentXPathTextBox";
            this.parentXPathTextBox.Size = new System.Drawing.Size(620, 20);
            this.parentXPathTextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Parent:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Order:";
            // 
            // orderXPathTextBox
            // 
            this.orderXPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.orderXPathTextBox.EmptyFieldValidationMessage = "Field can\'t be empty";
            this.orderXPathTextBox.Font = new System.Drawing.Font("Consolas", 8.25F);
            this.orderXPathTextBox.Location = new System.Drawing.Point(60, 168);
            this.orderXPathTextBox.Name = "orderXPathTextBox";
            this.orderXPathTextBox.Size = new System.Drawing.Size(620, 20);
            this.orderXPathTextBox.TabIndex = 3;
            // 
            // positionComboBox
            // 
            this.positionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.positionComboBox.FormattingEnabled = true;
            this.positionComboBox.Location = new System.Drawing.Point(60, 141);
            this.positionComboBox.Name = "positionComboBox";
            this.positionComboBox.Size = new System.Drawing.Size(242, 21);
            this.positionComboBox.TabIndex = 2;
            // 
            // newElementTextBox
            // 
            this.newElementTextBox.AcceptsReturn = true;
            this.newElementTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newElementTextBox.EmptyFieldValidationMessage = "Field can\'t be empty";
            this.newElementTextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newElementTextBox.Location = new System.Drawing.Point(60, 194);
            this.newElementTextBox.Multiline = true;
            this.newElementTextBox.Name = "newElementTextBox";
            this.newElementTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.newElementTextBox.Size = new System.Drawing.Size(620, 108);
            this.newElementTextBox.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 197);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(27, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Xml:";
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView.Location = new System.Drawing.Point(60, 12);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(620, 97);
            this.treeView.TabIndex = 0;
            // 
            // PatchInsertForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 343);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.newElementTextBox);
            this.Controls.Add(this.positionComboBox);
            this.Controls.Add(this.orderXPathTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.parentXPathTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelBtn);
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PatchInsertForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Insert element";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.PatchInsertForm_HelpButtonClicked);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cancelBtn;
        private System.Windows.Forms.Button okButton;
        private XmlFragmentTreeView treeView;
        private System.Windows.Forms.Label label1;
        private RequiredFieldTextBox parentXPathTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private RequiredFieldTextBox orderXPathTextBox;
        private System.Windows.Forms.ComboBox positionComboBox;
        private System.Windows.Forms.Label label5;
        private XmlTextBox newElementTextBox;
    }
}