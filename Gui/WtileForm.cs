using NAudio.CoreAudioApi;
using System.Diagnostics;
using Wtile.Core.Config;
using Wtile.Core.Keybind;

namespace Wtile.Gui
{
    public partial class WtileForm : Form
    {
        private bool _resizable = false;
        private bool _visible = true;
        private PerformanceCounter _cpuCounter;

        public WtileForm()
        {
            InitializeComponent();

            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            StartPosition = FormStartPosition.Manual;
            Top = ConfigManager.Config.Top;
            Left = ConfigManager.Config.Left;
            Width = ConfigManager.Config.Width;
            Height = ConfigManager.Config.Height;

            var resizeKeys = new List<WtileKey> { WtileKey.H, WtileKey.LControlKey, WtileKey.LShiftKey };
            var resizeKeybind = new WtileKeybind(resizeKeys, WtileModKey.LWin, () => _resizable = !_resizable);
            KeybindManager.AddKeybind(resizeKeybind);

            var visibilityKeys = new List<WtileKey> { WtileKey.H };
            var visibilityKeybind = new WtileKeybind(visibilityKeys, WtileModKey.LWin, () => _visible = !_visible);
            KeybindManager.AddKeybind(visibilityKeybind);


            System.Windows.Forms.Timer mainTimer = new()
            {
                Interval = 16
            };
            mainTimer.Tick += MainUpdate;
            mainTimer.Start();

            System.Windows.Forms.Timer rightLabelTimer = new()
            {
                Interval = 1000
            };
            rightLabelTimer.Tick += UpdateRightLabel;
            rightLabelTimer.Start();

            leftLabel.Text = Core.Wtile.GetWtileString();
            rightLabel.Text = GetRightLabelText();

        }

        private void MainUpdate(object? sender, EventArgs e)
        {
            TopMost = true;
            ToggleResize();
            ShowInTaskbar = false;

            leftLabel.Text = Core.Wtile.GetWtileString();

            var config = ConfigManager.Config;
            config.X = Location.X;
            config.Y = Location.Y;
            config.Width = Width;
            config.Height = Height;
            config.Top = Top;
            config.Left = Left;
            if (_visible != Visible)
            {
                Visible = _visible;
            }
        }

        private void UpdateRightLabel(object? sender, EventArgs e)
        {
            rightLabel.Text = GetRightLabelText();
        }

        private string GetRightLabelText()
        {
            var timeString = DateTime.Now.ToString("ddd dd/MM/yyyy HH:mm:ss"); ;

            var deviceEnumerator = new MMDeviceEnumerator();
            var defaultDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia); int volume = (int)(defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100);

            int cpuUsage = (int)_cpuCounter.NextValue();


            return $"CPU: {cpuUsage}% | Volume: {volume} | {timeString}";
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
        private void RightLabelClick(object sender, EventArgs e) { }
    }

}
