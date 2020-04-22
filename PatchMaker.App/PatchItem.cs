using System;
using System.Windows.Forms;

namespace PatchMaker.App
{
    public class PatchItem : ListViewItem
    {

        public BasePatch Patch { get; set; }
        public TreeNode RelatedTreeNode { get; set; }

        public PatchItem(BasePatch patch, TreeNode relatedTreeNode)
        {
            Patch = patch ?? throw new ArgumentNullException(nameof(patch));
            RelatedTreeNode = relatedTreeNode ?? throw new ArgumentNullException(nameof(relatedTreeNode));
        }

        public override string ToString()
        {
            return Patch.ToString();
        }
    }

}
