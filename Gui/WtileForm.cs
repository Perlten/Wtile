using NAudio.CoreAudioApi;
using System.Diagnostics;
using Wtile.Core.Config;
using Wtile.Core.Keybind;
using Wtile.Core.Utils;

namespace Wtile.Gui
{
    public partial class WtileForm : Form
    {
        private PerformanceCounter _cpuCounter;

        public WtileForm()
        {
            InitializeComponent();

            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

            StartPosition = FormStartPosition.Manual;
            Top = ConfigManager.Config.Gui.Top;
            Left = ConfigManager.Config.Gui.Left;
            Width = ConfigManager.Config.Gui.Width;
            Height = ConfigManager.Config.Gui.Height;
            leftLabel.Font = ConfigManager.Config.Gui.Font;
            rightLabel.Font = ConfigManager.Config.Gui.Font;

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

            var config = ConfigManager.Config.Gui;
            config.X = Location.X;
            config.Y = Location.Y;
            config.Width = Width;
            config.Height = Height;
            config.Top = Top;
            config.Left = Left;
            if (!State.RUNNING)
                Opacity = 0;
            else
                Opacity = 100;

        }

        private void UpdateRightLabel(object? sender, EventArgs e)
        {
            rightLabel.Text = GetRightLabelText();
        }

        private string GetRightLabelText()
        {
            var timeString = DateTime.Now.ToString("ddd dd/MM/yyyy HH:mm:ss"); ;

            var deviceEnumerator = new MMDeviceEnumerator();
            var defaultDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            var isMute = defaultDevice.AudioEndpointVolume.Mute;
            int volume = (int)(defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100);
            string volumeString = isMute ? "Mute" : $"{volume}%";

            int cpuUsage = (int)_cpuCounter.NextValue();

            return $"CPU: {cpuUsage}% | Volume: {volumeString} | {timeString}";
        }

        private void ToggleResize()
        {
            if (State.RESIZEABLE)
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
