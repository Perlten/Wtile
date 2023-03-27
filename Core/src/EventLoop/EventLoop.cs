using Wtile.Core.Entities;
using Wtile.Core.Utils;

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
        private readonly int msgNotify;
        public MessageWindow()
        {
            _wnd = this;
            _hwnd = Handle;
            _windowReadyEvent.Set();


            msgNotify = ExternalFunctions.RegisterWindowMessage("SHELLHOOK");
            ExternalFunctions.RegisterShellHookWindow(Handle);

        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == msgNotify)
            {
                var windowName = Window.GetName(m.LParam);
                if (m.WParam == 1 && windowName != "")
                {
                    Wtile.AddWindow(m.LParam);
                }
                if (m.WParam == 2 && windowName != "")
                {
                    Wtile.RemoveWindow(m.LParam);
                }
            }
            base.WndProc(ref m);
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(false);
        }
    }
}
