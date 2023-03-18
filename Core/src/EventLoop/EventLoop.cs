using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wtile.Core.Keybind;

namespace Wtile.Core.EventLoop;

internal static class EventLoop
{

    public static volatile MessageWindow _wnd;
    private static ManualResetEvent _windowReadyEvent = new(false);
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052:Remove unread private members", Justification = "Exception otherwise")]
    private static volatile IntPtr _hwnd;
    public static uint ThreadId { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    static EventLoop()
    {
        Thread messageLoop = new(delegate ()
          {
              Application.Run(new MessageWindow());
          });
        ThreadId = (uint)messageLoop.ManagedThreadId;
        messageLoop.Name = "WtileHotkeyMessageLoopThread";
        messageLoop.IsBackground = true;
        messageLoop.Start();
    }

    public static bool AddToEventLoop(Delegate method)
    {
        _windowReadyEvent.WaitOne();
        _wnd.Invoke(method);
        return true;
    }

    public class MessageWindow : Form
    {
        public MessageWindow()
        {
            _wnd = this;
            _hwnd = this.Handle;
            _windowReadyEvent.Set();
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(false);
        }
    }
}
