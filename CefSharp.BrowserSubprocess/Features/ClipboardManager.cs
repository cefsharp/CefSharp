using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CefSharp.BrowserSubprocess.Features
{
    public class ClipboardManager : IDisposable
    {
        #region P/Invoke

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        private static extern bool EmptyClipboard();

        [DllImport("user32.dll")]
        private static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

        [DllImport("user32.dll")]
        private static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("user32.dll")]
        private static extern bool IsClipboardFormatAvailable(uint format);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        private static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        private static extern UIntPtr GlobalSize(IntPtr hMem);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        // INPUT struct must be exactly 40 bytes on x64.
        // type (4) + padding (4) + union (32) = 40
        // The union must be 32 bytes to match MOUSEINPUT (the largest member).
        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public uint type;
            public INPUTUNION u;
        }

        [StructLayout(LayoutKind.Explicit, Size = 32)]
        private struct INPUTUNION
        {
            [FieldOffset(0)] public KEYBDINPUT ki;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        private const uint InputKeyboard = 1;
        private const uint KeyeventfKeyup = 0x0002;

        private const uint CfUnicodetext = 13;
        private const uint CfBitmap = 2;
        private const uint CfHdrop = 15;
        private const uint GmemMoveable = 0x0002;
        private const int WmClipboardupdate = 0x031D;

        private const byte VkControl = 0x11;
        private const byte VkV = 0x56;

        #endregion

        private const int MaxHistory = 500;
        private readonly string persistPath;

        private readonly List<ClipboardEntry> history = new List<ClipboardEntry>();
        private readonly object syncLock = new object();
        private ClipboardListenerWindow listenerWindow;
        private Thread listenerThread;
        private bool monitoring;
        private volatile bool suppressNextCapture;  // Prevents feedback loop when WE set clipboard

        public event Action<ClipboardEntry> EntryAdded;
        public event Action HistoryCleared;

        public ClipboardManager()
        {
            persistPath = Path.Combine(Path.GetTempPath(), "seb_clip_v8.dat");
            LoadHistory();
        }

        public IReadOnlyList<ClipboardEntry> History
        {
            get { lock (syncLock) return history.ToList().AsReadOnly(); }
        }

        public int Count
        {
            get { lock (syncLock) return history.Count; }
        }

        /// <summary>
        /// Adds an entry directly to history without going through the OS clipboard.
        /// Used by Files tab "Copy Content" button to store file contents.
        /// </summary>
        public void AddEntryManually(ClipboardEntry entry)
        {
            if (entry == null) return;
            lock (syncLock)
            {
                history.Insert(0, entry);
                while (history.Count > MaxHistory)
                    history.RemoveAt(history.Count - 1);
            }
            EntryAdded?.Invoke(entry);
        }

        public void StartMonitoring()
        {
            if (monitoring) return;
            monitoring = true;

            listenerThread = new Thread(() =>
            {
                listenerWindow = new ClipboardListenerWindow(this);
                Application.Run(listenerWindow);
            });
            listenerThread.SetApartmentState(ApartmentState.STA);
            listenerThread.IsBackground = true;
            listenerThread.Start();
        }

        public void StopMonitoring()
        {
            monitoring = false;
            try { listenerWindow?.Invoke(new Action(() => listenerWindow?.Close())); }
            catch { }
        }

        internal void OnClipboardChanged()
        {
            // If WE set the clipboard (ForcePaste/ForceCopy), skip capture
            if (suppressNextCapture)
            {
                suppressNextCapture = false;
                return;
            }

            try
            {
                var entry = CaptureClipboard();
                if (entry != null)
                {
                    lock (syncLock)
                    {
                        history.Insert(0, entry);
                        while (history.Count > MaxHistory)
                            history.RemoveAt(history.Count - 1);
                    }
                    EntryAdded?.Invoke(entry);
                }
            }
            catch { }
        }

        private ClipboardEntry CaptureClipboard()
        {
            ClipboardEntry entry = null;

            string sourceApp = "";
            try
            {
                var hwnd = GetForegroundWindow();
                var sb = new StringBuilder(256);
                GetWindowText(hwnd, sb, 256);
                sourceApp = sb.ToString();
            }
            catch { }

            if (!OpenClipboard(IntPtr.Zero)) return null;
            try
            {
                if (IsClipboardFormatAvailable(CfBitmap))
                {
                    try
                    {
                        var img = Clipboard.GetImage();
                        if (img != null)
                        {
                            entry = new ClipboardEntry
                            {
                                EntryType = ClipboardEntryType.Image,
                                SourceApp = sourceApp
                            };
                            using (var ms = new MemoryStream())
                            {
                                img.Save(ms, ImageFormat.Png);
                                entry.ImageData = ms.ToArray();
                            }
                            img.Dispose();
                        }
                    }
                    catch { }
                }

                if (entry == null && IsClipboardFormatAvailable(CfHdrop))
                {
                    try
                    {
                        var files = Clipboard.GetFileDropList();
                        if (files != null && files.Count > 0)
                        {
                            var paths = new string[files.Count];
                            files.CopyTo(paths, 0);
                            entry = new ClipboardEntry
                            {
                                EntryType = ClipboardEntryType.Files,
                                FilePaths = paths,
                                Text = string.Join("\r\n", paths),
                                SourceApp = sourceApp
                            };
                        }
                    }
                    catch { }
                }

                if (entry == null && IsClipboardFormatAvailable(CfUnicodetext))
                {
                    try
                    {
                        var hGlobal = GetClipboardData(CfUnicodetext);
                        if (hGlobal != IntPtr.Zero)
                        {
                            var ptr = GlobalLock(hGlobal);
                            if (ptr != IntPtr.Zero)
                            {
                                try
                                {
                                    string text = Marshal.PtrToStringUni(ptr);
                                    if (!string.IsNullOrEmpty(text))
                                    {
                                        entry = new ClipboardEntry
                                        {
                                            EntryType = ClipboardEntryType.Text,
                                            Text = text,
                                            SourceApp = sourceApp
                                        };
                                    }
                                }
                                finally { GlobalUnlock(hGlobal); }
                            }
                        }
                    }
                    catch { }
                }

                if (entry != null && entry.EntryType == ClipboardEntryType.Text)
                {
                    try
                    {
                        if (Clipboard.ContainsText(TextDataFormat.Html))
                        {
                            entry.Html = Clipboard.GetText(TextDataFormat.Html);
                            entry.EntryType = ClipboardEntryType.Html;
                        }
                    }
                    catch { }
                }
            }
            finally
            {
                CloseClipboard();
            }

            return entry;
        }

        public void ForcePasteEntry(ClipboardEntry entry)
        {
            if (entry == null) return;
            try
            {
                suppressNextCapture = true;  // Don't record our own clipboard set
                ForceSetClipboard(entry);
                Thread.Sleep(80);

                // Use SendInput instead of keybd_event — more reliable in SEB kiosk
                var inputs = new INPUT[4];
                // Ctrl down
                inputs[0].type = InputKeyboard;
                inputs[0].u.ki.wVk = VkControl;
                // V down
                inputs[1].type = InputKeyboard;
                inputs[1].u.ki.wVk = VkV;
                // V up
                inputs[2].type = InputKeyboard;
                inputs[2].u.ki.wVk = VkV;
                inputs[2].u.ki.dwFlags = KeyeventfKeyup;
                // Ctrl up
                inputs[3].type = InputKeyboard;
                inputs[3].u.ki.wVk = VkControl;
                inputs[3].u.ki.dwFlags = KeyeventfKeyup;

                SendInput(4, inputs, Marshal.SizeOf(typeof(INPUT)));
            }
            catch { }
        }

        public void ForceCopyToClipboard(ClipboardEntry entry)
        {
            if (entry == null) return;
            suppressNextCapture = true;  // Don't record our own clipboard set
            ForceSetClipboard(entry);
        }

        private void ForceSetClipboard(ClipboardEntry entry)
        {
            if (!OpenClipboard(IntPtr.Zero)) return;
            try
            {
                EmptyClipboard();

                switch (entry.EntryType)
                {
                    case ClipboardEntryType.Text:
                    case ClipboardEntryType.Html:
                        if (!string.IsNullOrEmpty(entry.Text))
                        {
                            var bytes = Encoding.Unicode.GetBytes(entry.Text + "\0");
                            var hGlobal = GlobalAlloc(GmemMoveable, (UIntPtr)bytes.Length);
                            var ptr = GlobalLock(hGlobal);
                            Marshal.Copy(bytes, 0, ptr, bytes.Length);
                            GlobalUnlock(hGlobal);
                            SetClipboardData(CfUnicodetext, hGlobal);
                        }
                        break;

                    case ClipboardEntryType.Image:
                        if (entry.ImageData != null)
                        {
                            var img = entry.GetImage();
                            if (img != null)
                            {
                                Clipboard.SetImage(img);
                                img.Dispose();
                            }
                        }
                        break;

                    case ClipboardEntryType.Files:
                        if (entry.FilePaths != null)
                        {
                            var coll = new System.Collections.Specialized.StringCollection();
                            coll.AddRange(entry.FilePaths);
                            Clipboard.SetFileDropList(coll);
                        }
                        break;
                }
            }
            finally
            {
                CloseClipboard();
            }
        }

        public List<ClipboardEntry> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return History.ToList();

            lock (syncLock)
            {
                return history.Where(e =>
                    (e.Text != null && e.Text.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (e.Html != null && e.Html.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (e.FilePaths != null && e.FilePaths.Any(f => f.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0))
                ).ToList();
            }
        }

        public void ClearHistory()
        {
            lock (syncLock) history.Clear();
            HistoryCleared?.Invoke();
        }

        public void RemoveEntry(ClipboardEntry entry)
        {
            lock (syncLock) history.Remove(entry);
        }

        public string ExportHistory()
        {
            var sb = new StringBuilder();
            lock (syncLock)
            {
                foreach (var e in history)
                {
                    sb.AppendLine("[" + e.Timestamp.ToString("yyyy-MM-dd HH:mm:ss") + "] [" + e.EntryType + "] " + e.GetPreview(200));
                    sb.AppendLine("---");
                }
            }
            return sb.ToString();
        }

        public void SaveHistory()
        {
            try
            {
                lock (syncLock)
                {
                    using (var fs = new FileStream(persistPath, FileMode.Create))
                    {
                        var bf = new BinaryFormatter();
                        bf.Serialize(fs, history);
                    }
                }
            }
            catch { }
        }

        private void LoadHistory()
        {
            try
            {
                if (File.Exists(persistPath))
                {
                    using (var fs = new FileStream(persistPath, FileMode.Open))
                    {
                        var bf = new BinaryFormatter();
                        var loaded = bf.Deserialize(fs) as List<ClipboardEntry>;
                        if (loaded != null)
                        {
                            lock (syncLock)
                            {
                                history.Clear();
                                history.AddRange(loaded);
                            }
                        }
                    }
                }
            }
            catch { }
        }

        public void Dispose()
        {
            StopMonitoring();
            SaveHistory();
        }

        #region Hidden Listener Window

        private class ClipboardListenerWindow : Form
        {
            private readonly ClipboardManager mgr;

            public ClipboardListenerWindow(ClipboardManager manager)
            {
                mgr = manager;
                this.FormBorderStyle = FormBorderStyle.None;
                this.ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;
                this.Visible = false;
                this.Load += (s, e) =>
                {
                    this.Size = new Size(1, 1);
                    this.Location = new Point(-9999, -9999);
                    AddClipboardFormatListener(this.Handle);
                };
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WmClipboardupdate)
                {
                    mgr.OnClipboardChanged();
                }
                base.WndProc(ref m);
            }

            protected override void OnFormClosing(FormClosingEventArgs e)
            {
                RemoveClipboardFormatListener(this.Handle);
                base.OnFormClosing(e);
            }
        }

        #endregion
    }
}
