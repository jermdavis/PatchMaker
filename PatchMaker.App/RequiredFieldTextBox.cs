using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PatchMaker.App
{
    public class RequiredFieldTextBox : TextBox
    {
        [DllImport("user32")]
        private static extern IntPtr GetWindowDC(IntPtr hwnd);

        private const int WM_NCPAINT = 0x85;
        private ToolTip _toolTip;
        private bool _errorState = false;

        public Action<string> PerformValidation;

        public RequiredFieldTextBox()
        {
            this.Font = new System.Drawing.Font("Consolas", 8.25F);
        }

        public string EmptyFieldValidationMessage { get; set; } = "Field can't be empty";

        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);

            if (string.IsNullOrWhiteSpace(this.Text))
            {
                e.Cancel = true;
                _errorState = true;
                AddValidationError(EmptyFieldValidationMessage);
                this.Invalidate(true); // needed to ensure validation message shown if "ok" clicked
                return;
            }

            if (PerformValidation != null)
            {
                try
                {
                    PerformValidation?.Invoke(this.Text);
                }
                catch (Exception ex)
                {
                    e.Cancel = true;
                    _errorState = true;
                    AddValidationError(ex.Message);
                    this.Invalidate(true); // needed to ensure validation message shown if "ok" clicked
                    return;
                }
            }

            _errorState = false;
            RemoveValidationError();
        }

        // https://stackoverflow.com/a/8511075/847953
        private void AddValidationError(string message)
        {
            var p = new Point(15, this.Height - 10);

            _toolTip = new ToolTip
            {
                InitialDelay = 0,
                IsBalloon = true
            };
            _toolTip.Show(string.Empty, this, p);
            _toolTip.Show(message, this, p);
        }

        private void RemoveValidationError()
        {
            if (_toolTip != null)
            {
                _toolTip.Dispose();
                _toolTip = null;
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            _toolTip?.Dispose();
        }

        // https://stackoverflow.com/a/38405319/847953
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCPAINT && _errorState)
            {
                var dc = GetWindowDC(Handle);
                using (Graphics g = Graphics.FromHdc(dc))
                {
                    g.DrawRectangle(Pens.Red, 0, 0, Width - 1, Height - 1);
                }
            }
        }
    }

}   