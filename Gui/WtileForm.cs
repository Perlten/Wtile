using System.Text;
using Wtile.Core;
using Wtile.Core.Keybind;
using Wtile.Core.Utils;

namespace Wtile.Gui
{
    public partial class WtileForm : Form
    {
        private bool _resizable = true;

        public WtileForm()
        {
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
        }

        private void Update(object? sender, EventArgs e)
        {
            TopMost = true;
            leftLabel.Text = Core.Wtile.GetWtileString();
            ToggleResize();
            ShowInTaskbar = false;
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
