using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wtile.Core.Entities;

namespace Wtile.Core.Utils
{

    internal interface IWindowHandler
    {
        static abstract List<Window> GetNewWindows();
    }

    internal class WindowHandler : IWindowHandler
    {
        private static List<IntPtr> lastWindowPtr = new();
        public static List<Window> GetNewWindows()
        {
            IntPtr shellWindow = ExternalFunctions.GetShellWindow();
            List<IntPtr> windowPtr = new();

            ExternalFunctions.EnumWindows(delegate (IntPtr IntPtr, int lParam)
            {
                if (IntPtr == shellWindow) return true;
                if (!ExternalFunctions.IsWindowVisible(IntPtr)) return true;

                windowPtr.Add(IntPtr);
                return true;
            }, 0);

            var newWindows = windowPtr
                .Where(x => !lastWindowPtr.Contains(x))
                .Select(x => new Window(x))
                .Where(x => x.Name != "" && x.Name != "Task Switching").ToList();

            lastWindowPtr = windowPtr;

            return newWindows;
        }
    }
}
