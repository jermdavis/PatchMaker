using System.Drawing;
using System.Windows.Forms;

namespace PatchMaker.App
{
    public class HighlightableTreeView : TreeView
    {
        public HighlightableTreeView()
        {
            this.DrawMode = TreeViewDrawMode.OwnerDrawText;
            this.HideSelection = false;
        }

        protected override void OnDrawNode(DrawTreeNodeEventArgs e)
        {
            if (e.Node == null)
            {
                return;
            }

            if (e.DrawDefault || e.Node.TreeView.DrawMode != TreeViewDrawMode.OwnerDrawText)
            {
                return;
            }

            var bounds = e.Bounds;
            bounds.Y += 1;
            var foreColour = e.Node.TreeView.ForeColor;
            var backColour = Brushes.Transparent;

            if (e.Node == e.Node.TreeView.SelectedNode)
            {
                backColour = SystemBrushes.Highlight;
                foreColour = SystemColors.HighlightText;
            }

            if (e.Node is HighlightableTreeNode)
            {
                var node = e.Node as HighlightableTreeNode;

                if (e.Node == e.Node.TreeView.SelectedNode)
                {
                    if (node.Highlighted)
                    {
                        backColour = Brushes.Red;
                    }
                }
                else
                {
                    if (node.Highlighted)
                    {
                        foreColour = Color.Red;
                    }
                }
            }

            e.Graphics.FillRectangle(backColour, e.Bounds);

            var font = e.Node.NodeFont ?? e.Node.TreeView.Font;
            TextRenderer.DrawText(e.Graphics, e.Node.Text, font, bounds, foreColour, TextFormatFlags.GlyphOverhangPadding);
        }
    }

}