using System.Windows.Forms;

namespace PatchMaker.App
{
    public interface IAddPatchForm
    {
        PatchItem Patch { get; }
        DialogResult ShowDialog(IWin32Window form);
    }

}
