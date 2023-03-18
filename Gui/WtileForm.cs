using System.Text;
using Wtile.Core.Keybind;
using Wtile.Core.Utils;

namespace Wtile.Gui
{
    public partial class WtileForm : Form
    {
        private readonly Core.Wtile _wtile;
        private bool _resizable = true;
        private readonly int msgNotify;

        public WtileForm(Core.Wtile wtile)
        {
            _wtile = wtile;
            InitializeComponent();

            var keys = new List<WtileKey> { WtileKey.LWin, WtileKey.H };
            var keybind = new WtileKeybind(keys, () => _resizable = !_resizable);
            KeybindManager.AddKeybind(keybind);

            System.Windows.Forms.Timer tmr = new()
            {
                Interval = 16
            };
            tmr.Tick += Update;
            tmr.Start();

            msgNotify = ExternalFunctions.RegisterWindowMessage("SHELLHOOK");
            ExternalFunctions.RegisterShellHookWindow(this.Handle);
        }

        private void Update(object? sender, EventArgs e)
        {
            TopMost = true;
            leftLabel.Text = _wtile.GetWtileString();
            ToggleResize();
            ShowInTaskbar = false;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == msgNotify)
            {
                var windowName = GetWindowName(m.LParam);
                if (m.WParam == 1 && windowName != "")
                {
                    _wtile.AddWindow(m.LParam);
                }
                if (m.WParam == 2 && windowName != "")
                {
                    _wtile.RemoveWindow(m.LParam);
                }
            }
            base.WndProc(ref m);
        }

        private string GetWindowName(IntPtr hwnd)
        {
            StringBuilder sb = new StringBuilder();
            int longi = ExternalFunctions.GetWindowTextLength(hwnd) + 1;
            sb.Capacity = longi;
            ExternalFunctions.GetWindowText(hwnd, sb, sb.Capacity);
            return sb.ToString();
        }

        private void ToggleResize()
        {
            if (_resizable)
                FormBorderStyle = FormBorderStyle.SizableToolWindow;
            else
                FormBorderStyle = FormBorderStyle.None;
        }

        protected override CreateParams CreateParams
        {
            // Ensures that the window does not show in alt+tab
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80;
                return cp;
            }
        }
        private void LeftLabelClick(object sender, EventArgs e) { }
    }

}
