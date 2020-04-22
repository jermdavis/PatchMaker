using System;
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

            clearControls();
            loadXml(sourceXmlPath);
            mapTreeView();
        }

        private void mapTreeView()
        {
            var xElement = _sourceXml.Root;
            var rootNode = new TreeNode("/");
            _patchTreeView.Nodes.Add(rootNode);

            mapNode(xElement, rootNode);
        }

        private void mapNode(XElement element, TreeNode parentTreeNode)
        {
            var treeNode = new TreeNode() {
                Text = element.Name.ToString(),
                Name = element.Name.LocalName,
                Tag = element,
                ContextMenuStrip = _treeMenu
            };
            
            foreach(var attr in element.Attributes())
            {
                treeNode.Text += $" {attr.Name}='{attr.Value}'";
            }
            
            parentTreeNode.Nodes.Add(treeNode);

            foreach(var child in element.Elements())
            {
                mapNode(child, treeNode);
            }
        }

        private void loadXml(string file)
        {
            _sourceXml = XDocument.Load(file);
        }

        private void clearControls()
        {
            _patchTreeView.Nodes.Clear();
            _patchesList.Items.Clear();
        }
    }

}
