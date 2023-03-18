
namespace Wtile.Gui
{
    public partial class WtileForm : Form
    {
        private readonly Core.Wtile _wtile;

        public WtileForm(Core.Wtile wtile)
        {
            _wtile = wtile;
            InitializeComponent();

            System.Windows.Forms.Timer tmr = new();
            tmr.Interval = 16;
            tmr.Tick += Update;
            tmr.Start();
        }

        private void Update(object? sender, EventArgs e)
        {
            this.leftLabel.Text = _wtile.GetWtileString();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
