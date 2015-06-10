using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CefSharp.Example
{
    public class KeyboardHandler : IKeyboardHandler
    {
        /// <inheritdoc/>>
        public bool OnPreKeyEvent(IWebBrowser browserControl, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
        {
            const int WM_SYSKEYDOWN = 0x104;
            const int WM_KEYDOWN = 0x100;
            const int WM_KEYUP = 0x101;
            const int WM_SYSKEYUP = 0x105;
            const int WM_CHAR = 0x102;
            const int WM_SYSCHAR = 0x106;
            const int VK_TAB = 0x9;

            bool result = false;

            isKeyboardShortcut = false;

            // Don't deal with TABs by default:
            // TODO: Are there any additional ones we need to be careful of?
            // i.e. Escape, Return, etc...?
            if (windowsKeyCode == VK_TAB)
            {
                return result;
            }

            Control control = browserControl as Control;
            int msgType = 0;
            switch (type)
            {
            case KeyType.RawKeyDown:
                if (isSystemKey)
                {
                    msgType = WM_SYSKEYDOWN;
                }
                else
                {
                    msgType = WM_KEYDOWN;
                }
                break;
            case KeyType.KeyUp:
                if (isSystemKey)
                {
                    msgType = WM_SYSKEYUP;
                }
                else
                {
                    msgType = WM_KEYUP;
                }
                break;
            case KeyType.Char:
                if (isSystemKey)
                {
                    msgType = WM_SYSCHAR;
                }
                else
                {
                    msgType = WM_CHAR;
                }
                break;
            default:
                Trace.Assert(false);
                break;
            }
            PreProcessControlState state = PreProcessControlState.MessageNotNeeded;
            control.Invoke(new Action(() =>
            {
                Message msg = new Message() { HWnd = control.Handle, Msg = msgType, WParam = new IntPtr(windowsKeyCode), LParam = new IntPtr(nativeKeyCode) };
                bool processed = Application.FilterMessage(ref msg);
                if (processed)
                {
                    state = PreProcessControlState.MessageProcessed;
                }
                else
                {
                    state = control.PreProcessControlMessage(ref msg);
                }
            }));
            if (state == PreProcessControlState.MessageNeeded)
            {
                // TODO: Determine how to track MessageNeeded for OnKeyEvent.
                isKeyboardShortcut = true;
            }
            else if (state == PreProcessControlState.MessageProcessed)
            {
                result = true;
            }
            Debug.WriteLine(String.Format("OnPreKeyEvent: KeyType: {0} 0x{1:X} Modifiers: {2}", type, windowsKeyCode, modifiers));
            Debug.WriteLine(String.Format("OnPreKeyEvent PreProcessControlState: {0}", state));
            return result;
        }

        /// <inheritdoc/>>
        public bool OnKeyEvent(IWebBrowser browserControl, KeyType type, int windowsKeyCode, CefEventFlags modifiers, bool isSystemKey)
        {
            bool result = false;
            Debug.WriteLine(String.Format("OnKeyEvent: KeyType: {0} 0x{1:X} Modifiers: {2}", type, windowsKeyCode, modifiers));
            return result;
        }
    }
}
