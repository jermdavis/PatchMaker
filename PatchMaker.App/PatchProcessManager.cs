using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PatchMaker.App
{

    public class PatchProcessManager
    {
        private TreeView _patchTreeView;
        private ListBox _patchesList;
        private ContextMenuStrip _treeMenu;
        private XDocument _sourceXml;
        private string _sourceFileName;

        public XDocument Source => _sourceXml;
        public string SourceFileName => _sourceFileName;
        public Dictionary<string, string> RoleConfig { get; set; } = new Dictionary<string, string>() {
            { "role","Standalone|ContentDelivery"},
            { "search","Solr"}
        };

        public void ConfigureControls(TreeView patchTreeView, ListBox patchesList, ContextMenuStrip treeMenu)
        {
            _patchTreeView = patchTreeView ?? throw new ArgumentNullException(nameof(patchTreeView));
            _patchesList = patchesList ?? throw new ArgumentNullException(nameof(patchesList));
            _treeMenu = treeMenu ?? throw new ArgumentNullException(nameof(treeMenu));
        }

        public void Initialise(string sourceXmlPath)
        {
            if(string.IsNullOrWhiteSpace(sourceXmlPath))
            {
                throw new ArgumentNullException(sourceXmlPath);
            }
            _sourceFileName = sourceXmlPath;

            _patchTreeView.SuspendLayout();
            ClearControls();
            LoadXml(sourceXmlPath);
            MapTreeView();
            _patchTreeView.ResumeLayout();
        }

        private void MapTreeView()
        {
            MapTreeView(_sourceXml.Root, _patchTreeView, _treeMenu);
        }

        public static void MapTreeView(XElement root, TreeView patchTreeView, ContextMenuStrip treeMenu)
        {
            var xElement = root;
            var rootNode = new TreeNode("/");
            patchTreeView.Nodes.Add(rootNode);

            MapNode(xElement, rootNode, treeMenu);
        }

        public static void MapNode(XElement element, TreeNode parentTreeNode, ContextMenuStrip treeMenu)
        {
            var treeNode = new HighlightableTreeNode(element, treeMenu);
            
            foreach(var attr in element.Attributes())
            {
                treeNode.Text += $" {attr.Name}='{attr.Value}'";
            }
            
            parentTreeNode.Nodes.Add(treeNode);

            foreach(var child in element.Elements())
            {
                MapNode(child, treeNode, treeMenu);
            }
        }

        private void LoadXml(string file)
        {
            _sourceXml = XDocument.Load(file);
        }

        private void ClearControls()
        {
            _patchTreeView.Nodes.Clear();
            _patchesList.Items.Clear();
        }
    }

}
