using System.Windows.Forms;
using System.Xml.Linq;

namespace PatchMaker.App
{

    public class HighlightableTreeNode : TreeNode
    {
        private bool _highlighted = false;

        public bool Highlighted => _highlighted;

        public HighlightableTreeNode(XElement element, ContextMenuStrip treeMenu)
        {
            Text = element.Name.ToString();
            Name = element.Name.LocalName;
            Tag = element;
            ContextMenuStrip = treeMenu;
            ImageIndex = 0;
            SelectedImageIndex = 0;
        }

        public void SetHighlight()
        {
            _highlighted = true;
            ImageIndex = 2;
            SelectedImageIndex = 2;
        }

        public void RemoveHighlight()
        {
            _highlighted = false;
            ImageIndex = 0;
            SelectedImageIndex = 0;
        }

        public void OnExpand()
        {
            if(_highlighted)
            {
                ImageIndex = 3;
                SelectedImageIndex = 3;
            }
            else
            {
                ImageIndex = 1;
                SelectedImageIndex = 1;
            }
        }

        public void OnCollapse()
        {
            if(_highlighted)
            {
                ImageIndex = 2;
                SelectedImageIndex = 2;
            }
            else
            {
                ImageIndex = 0;
                SelectedImageIndex = 0;
            }
        }
    }

}
