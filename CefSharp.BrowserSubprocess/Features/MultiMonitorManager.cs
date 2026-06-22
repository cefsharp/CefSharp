using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace CefSharp.BrowserSubprocess.Features
{
    /// <summary>
    /// Multi-monitor support: enumerate displays, move/place forms on any screen,
    /// DPI-aware positioning.
    /// </summary>
    public static class MultiMonitorManager
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        [DllImport("shcore.dll")]
        private static extern int SetProcessDpiAwareness(int value);

        static MultiMonitorManager()
        {
            try { SetProcessDpiAwareness(2); } // PROCESS_PER_MONITOR_DPI_AWARE
            catch { try { SetProcessDPIAware(); } catch { } }
        }

        public static Screen[] GetAllScreens()
        {
            return Screen.AllScreens;
        }

        public static int ScreenCount { get { return Screen.AllScreens.Length; } }

        /// <summary>
        /// Places a form on the specified monitor index (0-based), centered.
        /// </summary>
        public static void PlaceOnMonitor(Form form, int monitorIndex)
        {
            var screens = Screen.AllScreens;
            if (monitorIndex < 0 || monitorIndex >= screens.Length)
                monitorIndex = 0;

            var screen = screens[monitorIndex];
            var workArea = screen.WorkingArea;

            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(
                workArea.X + (workArea.Width - form.Width) / 2,
                workArea.Y + (workArea.Height - form.Height) / 2
            );
        }

        /// <summary>
        /// Maximizes a form on the specified monitor.
        /// </summary>
        public static void MaximizeOnMonitor(Form form, int monitorIndex)
        {
            var screens = Screen.AllScreens;
            if (monitorIndex < 0 || monitorIndex >= screens.Length)
                monitorIndex = 0;

            var screen = screens[monitorIndex];
            form.StartPosition = FormStartPosition.Manual;
            form.WindowState = FormWindowState.Normal;
            form.Location = screen.WorkingArea.Location;
            form.Size = screen.WorkingArea.Size;
        }

        /// <summary>
        /// Spans a form across all monitors.
        /// </summary>
        public static void SpanAcrossMonitors(Form form)
        {
            var virtualScreen = SystemInformation.VirtualScreen;
            form.StartPosition = FormStartPosition.Manual;
            form.WindowState = FormWindowState.Normal;
            form.Location = virtualScreen.Location;
            form.Size = virtualScreen.Size;
        }

        /// <summary>
        /// Gets a formatted string listing all monitors for UI display.
        /// </summary>
        public static string GetMonitorInfo()
        {
            var sb = new System.Text.StringBuilder();
            var screens = Screen.AllScreens;
            for (int i = 0; i < screens.Length; i++)
            {
                var s = screens[i];
                sb.AppendLine($"Monitor {i}: {s.DeviceName}");
                sb.AppendLine($"  Resolution: {s.Bounds.Width}x{s.Bounds.Height}");
                sb.AppendLine($"  Work Area:  {s.WorkingArea.Width}x{s.WorkingArea.Height}");
                sb.AppendLine($"  Primary:    {s.Primary}");
                sb.AppendLine($"  BPP:        {s.BitsPerPixel}");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns the monitor index that contains the given form.
        /// </summary>
        public static int GetMonitorIndex(Form form)
        {
            var screen = Screen.FromControl(form);
            var screens = Screen.AllScreens;
            for (int i = 0; i < screens.Length; i++)
            {
                if (screens[i].DeviceName == screen.DeviceName)
                    return i;
            }
            return 0;
        }
    }
}
