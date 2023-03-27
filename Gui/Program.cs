namespace Wtile.Gui
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Thread wtileThread = new(Core.Wtile.Start);
            wtileThread.Start();

            ApplicationConfiguration.Initialize();
            Application.Run(new WtileForm());
        }
    }
}