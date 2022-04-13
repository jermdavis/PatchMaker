using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace PatchMaker.App
{
    public partial class RoleConfigForm : Form
    {
        private class ConfigItem
        {
            public string Key { get; set; }
            public string Value { get; set; }

            public ConfigItem()
            {
                Key = "Key";
                Value = "Value";
            }

            public ConfigItem(KeyValuePair<string,string> source)
            {
                Key = source.Key;
                Value = source.Value;
            }

            public override string ToString()
            {
                return $"{Key}: {Value}";
            }
        }

        public Dictionary<string, string> RoleConfig { get; set; } = new Dictionary<string, string>();

        public RoleConfigForm()
        {
            InitializeComponent();
            this.ConfigureDialog();
        }

        public void Initialise(Dictionary<string,string> roleConfig)
        {
            RoleConfig = roleConfig;

            roleListBox.Items.Clear();
            foreach(var role in RoleConfig)
            {
                var i = new ConfigItem(role);
                roleListBox.Items.Add(i);
            }
        }

        private void RoleConfigForm_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            HelpSpawner.SpawnLocalFile("roleconfig");
        }

        private void RoleListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (roleListBox.SelectedIndex != -1)
            {
                var itm = roleListBox.SelectedItem as ConfigItem;
                keyTextBox.Text = itm.Key;
                valueTextBox.Text = itm.Value;
            }
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            var newItm = new ConfigItem();
            roleListBox.Items.Add(newItm);
            roleListBox.SelectedItem = newItm;
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            if (roleListBox.SelectedIndex != -1)
            {
                roleListBox.Items.RemoveAt(roleListBox.SelectedIndex);
            }
        }

        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            if (roleListBox.SelectedIndex != -1)
            {
                var itm = roleListBox.SelectedItem as ConfigItem;
                var idx = roleListBox.SelectedIndex;
                roleListBox.Items.RemoveAt(idx);
                itm.Key = keyTextBox.Text;
                itm.Value = valueTextBox.Text;
                roleListBox.Items.Insert(idx, itm);
            }
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            var newCfg = new Dictionary<string, string>();

            foreach(ConfigItem itm in roleListBox.Items)
            {
                newCfg.Add(itm.Key, itm.Value);
            }

            this.RoleConfig = newCfg;
        }
    }
}
