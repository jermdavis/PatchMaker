using System.Windows.Forms;

namespace PatchMaker.App
{
    public static class FormExtensions
    {
        public static void ConfigureDialog(this Form form)
        {
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.SizeGripStyle = SizeGripStyle.Show;
            form.StartPosition = FormStartPosition.CenterParent;
            form.MinimumSize = new System.Drawing.Size(400, 300);
        }
    }

}
