
using System.Collections.Generic;
using System.Diagnostics;
using Wtile.Core.Keybind;

namespace Wtile.Gui
{
    public partial class WtileForm : Form
    {
        private readonly Core.Wtile _wtile;
        private bool _resizable = true;

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
        }

        private void Update(object? sender, EventArgs e)
        {
            TopMost = true;
            leftLabel.Text = _wtile.GetWtileString();
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
