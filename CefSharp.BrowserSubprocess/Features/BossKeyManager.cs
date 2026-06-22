using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CefSharp.BrowserSubprocess.Features
{
    /// <summary>
    /// Boss key manager v2: hide/show all managed windows with Alt+Q.
    /// Also handles panel toggle via PanelToggleRequested event.
    /// </summary>
    public static class BossKeyManager
    {
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        private const int SwHide = 0;
        private const int SwShow = 5;
        private const int SwRestore = 9;

        // Default: Alt+Q
        public static Keys HotKey { get; set; } = Keys.Q;
        public static bool UseAlt { get; set; } = true;
        public static bool UseCtrl { get; set; } = false;
        public static bool UseShift { get; set; } = false;

        public static bool IsHidden { get; private set; } = false;

        private static readonly List<IntPtr> managedWindows = new List<IntPtr>();
        private static readonly List<Form> managedForms = new List<Form>();
        private static IntPtr lastForeground = IntPtr.Zero;

        public static event Action BossKeyTriggered;
        public static event Action<bool> VisibilityChanged;
        public static event Action<string> LogEvent;

        /// <summary>
        /// Fired when the boss key is pressed — panel should toggle.
        /// This is the ONLY event the panel should listen to for toggle.
        /// </summary>
        public static event Action PanelToggleRequested;

        /// <summary>
        /// Request the feature panel to toggle visibility.
        /// Called from BossKeyKeyboardHandler when Alt+Q is pressed in CEF.
        /// </summary>
        public static void RequestPanelToggle()
        {
            Log("Panel toggle requested via boss key");
            PanelToggleRequested?.Invoke();
        }

        /// <summary>
        /// Register a window handle to be managed (hidden/shown) by boss key.
        /// </summary>
        public static void RegisterWindow(IntPtr hwnd)
        {
            if (!managedWindows.Contains(hwnd))
                managedWindows.Add(hwnd);
        }

        /// <summary>
        /// Register a Form to be managed by boss key.
        /// </summary>
        public static void RegisterForm(Form form)
        {
            if (!managedForms.Contains(form))
                managedForms.Add(form);
            RegisterWindow(form.Handle);
        }

        /// <summary>
        /// Unregister a window handle.
        /// </summary>
        public static void UnregisterWindow(IntPtr hwnd)
        {
            managedWindows.Remove(hwnd);
        }

        /// <summary>
        /// Toggle visibility of all managed windows.
        /// </summary>
        public static void Trigger()
        {
            IsHidden = !IsHidden;

            if (IsHidden)
                HideAll();
            else
                ShowAll();

            BossKeyTriggered?.Invoke();
            VisibilityChanged?.Invoke(IsHidden);
            Log(IsHidden ? "All windows HIDDEN (boss key)" : "All windows RESTORED");
        }

        private static void HideAll()
        {
            lastForeground = GetForegroundWindow();

            foreach (var hwnd in managedWindows)
            {
                try { ShowWindow(hwnd, SwHide); }
                catch { }
            }

            foreach (var form in managedForms)
            {
                try
                {
                    if (form.InvokeRequired)
                        form.Invoke(new Action(() => { form.Hide(); form.WindowState = FormWindowState.Minimized; }));
                    else
                    {
                        form.Hide();
                        form.WindowState = FormWindowState.Minimized;
                    }
                }
                catch { }
            }
        }

        private static void ShowAll()
        {
            foreach (var hwnd in managedWindows)
            {
                try { ShowWindow(hwnd, SwShow); }
                catch { }
            }

            foreach (var form in managedForms)
            {
                try
                {
                    if (form.InvokeRequired)
                    {
                        form.Invoke(new Action(() =>
                        {
                            form.Show();
                            form.WindowState = FormWindowState.Normal;
                            form.BringToFront();
                        }));
                    }
                    else
                    {
                        form.Show();
                        form.WindowState = FormWindowState.Normal;
                        form.BringToFront();
                    }
                }
                catch { }
            }

            if (lastForeground != IntPtr.Zero)
            {
                try { SetForegroundWindow(lastForeground); }
                catch { }
            }
        }

        /// <summary>
        /// Check if a key event matches the configured boss key combo.
        /// </summary>
        public static bool MatchesHotkey(int windowsKeyCode, CefEventFlags modifiers)
        {
            bool keyMatch = windowsKeyCode == (int)HotKey;
            bool altMatch = !UseAlt || modifiers.HasFlag(CefEventFlags.AltDown);
            bool ctrlMatch = !UseCtrl || modifiers.HasFlag(CefEventFlags.ControlDown);
            bool shiftMatch = !UseShift || modifiers.HasFlag(CefEventFlags.ShiftDown);

            return keyMatch && altMatch && ctrlMatch && shiftMatch;
        }

        /// <summary>
        /// Returns modifiers and VK code for RegisterHotKey API via out params.
        /// </summary>
        public static void GetHotkeyRegistration(out int modifiers, out int vk)
        {
            modifiers = 0;
            if (UseAlt) modifiers |= 0x0001;   // MOD_ALT
            if (UseCtrl) modifiers |= 0x0002;  // MOD_CONTROL
            if (UseShift) modifiers |= 0x0004; // MOD_SHIFT

            vk = (int)HotKey;
        }

        public static string GetHotkeyDisplayString()
        {
            var parts = new List<string>();
            if (UseCtrl) parts.Add("Ctrl");
            if (UseAlt) parts.Add("Alt");
            if (UseShift) parts.Add("Shift");
            parts.Add(HotKey.ToString());
            return string.Join(" + ", parts);
        }

        private static void Log(string msg)
        {
            LogEvent?.Invoke("[" + DateTime.Now.ToString("HH:mm:ss") + "] [BOSS] " + msg);
        }
    }
}
