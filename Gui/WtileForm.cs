
using System.Collections.Generic;
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
        }

        private void ToggleResize()
        {
            if (_resizable)
                FormBorderStyle = FormBorderStyle.Sizable;
            else
                FormBorderStyle = FormBorderStyle.None;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
