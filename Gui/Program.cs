namespace Wtile.Gui
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var wtile = new Core.Wtile();
            Thread wtileThread = new(wtile.Start);
            wtileThread.Start();

            ApplicationConfiguration.Initialize();
            Application.Run(new WtileForm(wtile));
        }
    }
}