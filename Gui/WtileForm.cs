using System.Diagnostics;
using System.Text;
using Wtile.Core;
using Wtile.Core.Config;
using Wtile.Core.Keybind;
using Wtile.Core.Utils;

namespace Wtile.Gui
{
    public partial class WtileForm : Form
    {
        private bool _resizable = false;

        public WtileForm()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.Manual;
            Top = ConfigManager.Config.Top;
            Left = ConfigManager.Config.Left;
            Width = ConfigManager.Config.Width;


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
            var config = ConfigManager.Config;

            config.X = Location.X;
            config.Y = Location.Y;
            config.Width = Width;
            config.Height = Height;
            config.Top = Top;
            config.Left = Left;
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
