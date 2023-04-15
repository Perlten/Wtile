using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wtile.Core.Utils;


public static class Logging
{
    private readonly static string LogFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".wtile");
    private readonly static string LogFilePath = Path.Combine(LogFolderPath, "log.txt");


    public static void WriteToLog(string message)
    {
        if (!Directory.Exists(LogFolderPath))
            Directory.CreateDirectory(LogFolderPath);

        if (!File.Exists(LogFilePath))
            File.Create(LogFilePath).Dispose();

        using StreamWriter writer = new(LogFilePath, true);
        var datetimeMessage = DateTime.Now.ToString() + ": " + message;
        writer.WriteLine(datetimeMessage);
    }

    public static void WriteUnhandledExceptionToLog(object sender, UnhandledExceptionEventArgs e)
    {
        var message = e.ExceptionObject.ToString();
        if (message == null)
            return;
        WriteToLog(message);
    }

}
