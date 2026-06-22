using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CefSharp.BrowserSubprocess.Features
{
    public class ScreenshotGuard : IDisposable
    {
        #region P/Invoke

        [DllImport("user32.dll")]
        private static extern bool SetWindowDisplayAffinity(IntPtr hWnd, uint dwAffinity);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private const int WhKeyboardLl = 13;
        private const int WmKeydown = 0x0100;
        private const int WmSyskeydown = 0x0104;
        private const int VkSnapshot = 0x2C;
        private const uint WdaNone = 0x00000000;
        private const uint WdaExcludeFromCapture = 0x00000011;

        #endregion

        private IntPtr hookId = IntPtr.Zero;
        private LowLevelKeyboardProc hookProc;
        private readonly System.Collections.Generic.List<IntPtr> protectedWindows = new System.Collections.Generic.List<IntPtr>();
        private bool enabled;

        public bool PreventionEnabled { get { return enabled; } }

        public event Action<string> LogEvent;

        public void Toggle()
        {
            if (enabled)
                Disable();
            else
                Enable();
        }

        public void Enable()
        {
            if (enabled) return;
            enabled = true;

            hookProc = HookCallback;
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                hookId = SetWindowsHookEx(WhKeyboardLl, hookProc, GetModuleHandle(curModule.ModuleName), 0);
            }

            foreach (var hwnd in protectedWindows)
            {
                SetWindowDisplayAffinity(hwnd, WdaExcludeFromCapture);
            }

            Log("Screenshot prevention ENABLED");
        }

        public void Disable()
        {
            if (!enabled) return;
            enabled = false;

            if (hookId != IntPtr.Zero)
            {
                UnhookWindowsHookEx(hookId);
                hookId = IntPtr.Zero;
            }

            foreach (var hwnd in protectedWindows)
            {
                SetWindowDisplayAffinity(hwnd, WdaNone);
            }

            Log("Screenshot prevention DISABLED");
        }

        public void ProtectWindow(IntPtr hwnd)
        {
            if (!protectedWindows.Contains(hwnd))
                protectedWindows.Add(hwnd);

            if (enabled)
                SetWindowDisplayAffinity(hwnd, WdaExcludeFromCapture);
        }

        public void UnprotectWindow(IntPtr hwnd)
        {
            protectedWindows.Remove(hwnd);
            SetWindowDisplayAffinity(hwnd, WdaNone);
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && enabled)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (vkCode == VkSnapshot)
                {
                    Log("Blocked PrintScreen key press");
                    return (IntPtr)1;
                }
            }
            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        private void Log(string msg)
        {
            LogEvent?.Invoke("[" + DateTime.Now.ToString("HH:mm:ss") + "] [GUARD] " + msg);
        }

        public void Dispose()
        {
            Disable();
        }
    }
}
