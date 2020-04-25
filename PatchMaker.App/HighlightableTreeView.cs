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

            if(e.Node is HighlightableTreeNode)
            {
                var node = e.Node as HighlightableTreeNode;

                var foreColour = e.Node.TreeView.ForeColor;

                if(e.Node == e.Node.TreeView.SelectedNode)
                {
                    var backColour = SystemBrushes.Highlight;
                    foreColour = SystemColors.HighlightText;

                    if(node.Highlighted)
                    {
                        backColour = Brushes.Red;
                    }

                    e.Graphics.FillRectangle(backColour, e.Bounds);
                }
                else
                {
                    if (node.Highlighted)
                    {
                        foreColour = Color.Red;
                    }
                }

                var bounds = e.Bounds;
                bounds.Y += 1;

                var font = e.Node.NodeFont ?? e.Node.TreeView.Font;
                TextRenderer.DrawText(e.Graphics, e.Node.Text, font, bounds, foreColour, TextFormatFlags.GlyphOverhangPadding);
            }
        }
    }

}