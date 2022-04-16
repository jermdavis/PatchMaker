using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PatchMaker.App.PatchForms
{
    public partial class RuleBasedConfigForm : Form
    {

        public RuleBasedConfigForm()
        {
            InitializeComponent();
            this.ConfigureDialog();
        }

        public void InitialiseRuleList(ConfigRule[] rules)
        {
            if (rules != null)
            {
                foreach (var rule in rules)
                {
                    RuleList.Items.Add(new ConfigRule(rule.Namespace, rule.Name, rule.Value));
                }
            }

            RuleList_SelectedIndexChanged(null, null);

            OutOfEditing();
        }

        public ConfigRule[] FetchRuleList()
        {
            var rules = new List<ConfigRule>();
            foreach(ConfigRule rule in RuleList.Items)
            {
                rules.Add(new ConfigRule(rule.Namespace, rule.Name, rule.Value));
            }

            return rules.ToArray();
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            var rule = new ConfigRule("", "require", "");
            RuleList.Items.Add(rule);
            RuleList.SelectedItem = rule;

            OkBtn.Enabled = false;
        }

        private void RuleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RuleList.SelectedIndex != -1)
            {
                var rule = RuleList.SelectedItem as ConfigRule;

                NamespaceCombo.Text = rule.Namespace.NamespaceName;
                NameEdit.Text = rule.Name;
                RuleEdit.Text = rule.Value;

                NamespaceCombo.Enabled = true;
                NameEdit.Enabled = true;
                RuleEdit.Enabled = true;

                OutOfEditing();

                SaveBtn.Enabled = true;
                DeleteBtn.Enabled = true;
            }
            else
            {
                NamespaceCombo.Text = "";
                NameEdit.Text = "";
                RuleEdit.Text = "";

                OutOfEditing();

                SaveBtn.Enabled = false;
                DeleteBtn.Enabled = false;
            }
        }

        private void InEditing()
        {
            OkBtn.Enabled = false;
            SaveBtn.Enabled = true;
        }

        private void OutOfEditing()
        {
            OkBtn.Enabled = true;
            SaveBtn.Enabled = false;
        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (RuleList.SelectedIndex != -1)
            {
                var rule = RuleList.SelectedItem as ConfigRule;

                rule.Namespace = (XNamespace)NamespaceCombo.Text;
                rule.Name = NameEdit.Text;
                rule.Value = RuleEdit.Text;

                var idx = RuleList.SelectedIndex;
                RuleList.Items.RemoveAt(idx);
                RuleList.Items.Insert(idx, rule);

                OutOfEditing();
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (RuleList.SelectedIndex != -1)
            {
                RuleList.Items.RemoveAt(RuleList.SelectedIndex);
            }
        }

        private void NamespaceCombo_TextChanged(object sender, EventArgs e)
        {
            InEditing();
        }

        private void NameEdit_TextChanged(object sender, EventArgs e)
        {
            InEditing();
        }

        private void RuleEdit_TextChanged(object sender, EventArgs e)
        {
            InEditing();
        }

        private void RuleBasedConfigForm_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            HelpSpawner.SpawnLocalFile("rbc");
        }
    }
}
