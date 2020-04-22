using System.Threading;
using System.Windows.Forms;

namespace PatchMaker.App
{

    public class XmlFragmentTreeView : TreeView
    {
        public XmlFragmentTreeView()
        {
            Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        public TreeNode BuildTreeView(TreeNode node)
        {
            var nodes = buildNodes(node);

            var rootNode = nodes;
            while (rootNode.Parent != null)
            {
                rootNode = rootNode.Parent;
            }

            this.Nodes.Add(rootNode);
            this.ExpandAll();

            selectLeafNode(rootNode);

            return rootNode;
        }

        private void selectLeafNode(TreeNode rootNode)
        {
            var node = rootNode;
            while(node.Nodes.Count > 0)
            {
                node = node.FirstNode;
            }

            this.SelectedNode = node;
        }

        private TreeNode buildNodes(TreeNode sourceNode)
        {
            if (sourceNode.Parent == null)
            {
                var tn = new TreeNode("/");
                return tn;
            }
            else
            {
                var parent = buildNodes(sourceNode.Parent);
                var newNode = new TreeNode()
                {
                    Text = sourceNode.Text,
                    Name = sourceNode.Name,
                    Tag = sourceNode.Tag,
                };

                parent.Nodes.Add(newNode);

                return newNode;
            }
        }

    }

}
