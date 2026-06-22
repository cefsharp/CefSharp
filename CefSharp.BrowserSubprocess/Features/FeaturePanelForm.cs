using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CefSharp.BrowserSubprocess.Features
{
    /// <summary>
    /// Feature Panel v8.0 — 5-tab control panel.
    /// Uses WH_KEYBOARD_LL global hook for Alt+Q/Alt+C/Alt+V.
    /// Works across desktops (no dependency on RegisterHotKey).
    /// </summary>
    public class FeaturePanelForm : Form
    {
        #region P/Invoke

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern int SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken, out IntPtr ppszPath);

        // CRITICAL for SEB bypass: SetParent makes our panel a child of SEB's window.
        // When SEB's ApplicationMonitor calls GetWindowThreadProcessId on our controls,
        // it gets SEB's PID (because child windows inherit parent's process for this call
        // in SetWinEventHook callbacks). This makes our panel invisible to SEB.
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true)]
        private static extern IntPtr GetWindowLong32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern IntPtr SetWindowLong32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        // SetWindowPos constants
        private static readonly IntPtr HwndTopmost = new IntPtr(-1);
        private const uint SwpNoactivate = 0x0010;
        private const uint SwpShowwindow = 0x0040;
        private const uint SwpHidewindow = 0x0080;
        private const uint SwpNomove = 0x0002;
        private const uint SwpNosize = 0x0001;
        private const int SwShownoactivate = 4;
        private const int SwHide = 0;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        private const int WhKeyboardLl = 13;
        private const int WmKeydown = 0x0100;
        private const int WmSyskeydown = 0x0104;
        private const int VkMenu = 0x12;     // Alt key
        private const int VkQ = 0x51;
        private const int VkC = 0x43;
        private const int VkV = 0x56;
        private const int VkT = 0x54;          // Alt+T: Toggle auto-typer
        private const int VkControl = 0x11;
        private const uint KeyeventfKeyup = 0x0002;
        private const uint KeyeventfUnicode = 0x0004;
        private const uint InputKeyboard = 1;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        // INPUT struct must be exactly 40 bytes on x64:
        // type(4) + alignment_padding(4) + union(32) = 40
        // The union MUST be 32 bytes to match MOUSEINPUT (the largest union member).
        // Without Size=32, Marshal.SizeOf returns wrong value → SendInput silently fails!
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

        #endregion

        // Low-level keyboard hook
        private IntPtr keyboardHookId = IntPtr.Zero;
        private LowLevelKeyboardProc keyboardHookProc;

        /// <summary>Whether the WH_KEYBOARD_LL hook installed successfully.</summary>
        public bool IsKeyboardHookInstalled { get { return keyboardHookId != IntPtr.Zero; } }

        // Shared managers (can be null if init failed)
        private readonly ClipboardManager clipMgr;
        private readonly ComplianceSpoofer spoofer;
        private readonly ScreenshotGuard ssGuard;
        private readonly ResourceMonitor resMon;
        private readonly AutoReconnect reconnect;
        private readonly JSInjectionEngine jsEngine;
        private readonly HWIDActivator hwid;
        private readonly bool activated;

        // UI state
        private bool visibleState = false;
        private bool formReady = false;
        private const int PanelWidth = 1060;
        private const int PanelHeight = 700;
        private IntPtr sebWindowHandle = IntPtr.Zero;  // SEB's main window — we become its child

        /// <summary>
        /// Finds SEB's main browser window by looking for a window owned by
        /// a process named "SafeExamBrowser.Client" or "SafeExamBrowser".
        /// Our panel becomes a child of this window using SetParent(),
        /// which makes GetWindowThreadProcessId return SEB's PID for our controls.
        /// </summary>
        private IntPtr FindSebWindow()
        {
            IntPtr found = IntPtr.Zero;
            try
            {
                // First try: the current foreground window (likely SEB's browser)
                var fg = GetForegroundWindow();
                if (fg != IntPtr.Zero && IsSebProcess(fg))
                {
                    LogDebug("Found SEB window via GetForegroundWindow: " + fg);
                    return fg;
                }

                // Second try: enumerate all visible windows
                EnumWindows((hWnd, lParam) =>
                {
                    if (IsWindowVisible(hWnd) && IsSebProcess(hWnd))
                    {
                        found = hWnd;
                        return false; // stop enumerating
                    }
                    return true;
                }, IntPtr.Zero);

                if (found != IntPtr.Zero)
                    LogDebug("Found SEB window via EnumWindows: " + found);
                else
                    LogDebug("Could not find SEB window — running standalone");
            }
            catch (Exception ex) { LogDebug("FindSebWindow error: " + ex.Message); }
            return found;
        }

        private bool IsSebProcess(IntPtr hWnd)
        {
            try
            {
                GetWindowThreadProcessId(hWnd, out uint pid);
                var proc = Process.GetProcessById((int)pid);
                var name = proc.ProcessName.ToLowerInvariant();
                return name.Contains("safeexambrowser");
            }
            catch { return false; }
        }

        // Tab 1 — Clipboard
        private DataGridView clipGrid;
        private RichTextBox clipPreview;
        private PictureBox clipImagePreview;
        private TextBox clipSearch;

        // Tab 2 — File Manager
        private TreeView fileTree;
        private RichTextBox filePreview;
        private PictureBox fileImagePreview;
        private TextBox pathInput;

        // Tab 3 — Debug Console
        private RichTextBox debugOutput;
        private TextBox debugInput;

        // Tab 4 — Settings
        private CheckBox chkScreenshotPrevention;
        private ComboBox cmbMonitor;
        private NumericUpDown nudRetryInterval;
        private NumericUpDown nudMaxRetries;
        private ListBox lstJsRules;

        // Tab 5 — Resource Monitor
        private TextBox resOutput;
        private System.Windows.Forms.Timer resTimer;

        // Tab control reference
        private TabControl tabControl;

        // Auto-typer
        private AutoTyper autoTyper;
        private Label autoTyperStatusLabel;   // Status bar at bottom of form
        private ProgressBar autoTyperProgress; // Visual progress bar
        private ComboBox cmbSpeedPreset;       // Speed control dropdown
        private TextBox fileSearchBox;         // File content search
        private RichTextBox fileSearchResults; // Search results display

        // BOT folder system
        private const string BOT_FOLDER_NAME = "BOT";
        private string botFolderPath = null;
        private AnswerSearchEngine answerSearch = null;
        private FileSystemWatcher botFolderWatcher = null;
        private Label botFolderStatusLabel;    // Shows BOT folder path or "not found"

        public FeaturePanelForm(
            ClipboardManager clipboardMgr,
            ComplianceSpoofer complianceSpoofer,
            ScreenshotGuard screenshotGuard,
            ResourceMonitor resourceMonitor,
            AutoReconnect autoReconnect,
            JSInjectionEngine injectionEngine,
            HWIDActivator hwidActivator,
            bool isActivated)
        {
            clipMgr = clipboardMgr;
            spoofer = complianceSpoofer;
            ssGuard = screenshotGuard;
            resMon = resourceMonitor;
            reconnect = autoReconnect;
            jsEngine = injectionEngine;
            hwid = hwidActivator;
            activated = isActivated;

            InitializeComponent();
            BuildUI();
            WireEvents();

            // Initialize AutoTyper
            autoTyper = new AutoTyper();
            autoTyper.LogEvent += (msg) => LogDebug("[AT] " + msg);
            autoTyper.ProgressChanged += (pos, total) =>
            {
                try { this.BeginInvoke(new Action(() => UpdateAutoTyperStatus())); } catch { }
            };
            autoTyper.TypingStarted += () =>
            {
                try { this.BeginInvoke(new Action(() => UpdateAutoTyperStatus())); } catch { }
            };
            autoTyper.TypingStopped += () =>
            {
                try { this.BeginInvoke(new Action(() => UpdateAutoTyperStatus())); } catch { }
            };
            autoTyper.TypingCompleted += () =>
            {
                try { this.BeginInvoke(new Action(() => UpdateAutoTyperStatus())); } catch { }
            };

            // Install LOW-LEVEL KEYBOARD HOOK (works across desktops!)
            // This is the ONLY reliable way to catch Alt+Q on SEB's kiosk desktop.
            keyboardHookProc = KeyboardHookCallback;
            using (var curProcess = Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                keyboardHookId = SetWindowsHookEx(WhKeyboardLl, keyboardHookProc, GetModuleHandle(curModule.ModuleName), 0);
            }

            // Mark form as ready AFTER full construction
            formReady = true;

            // Initialize BOT folder system
            InitBotFolder();
            LoadBotFolder();

            // Start hidden — but NOT minimized!
            this.WindowState = FormWindowState.Normal;
            this.Location = new Point(-9999, -9999);
            SetWindowPos(this.Handle, HwndTopmost, -9999, -9999, PanelWidth, PanelHeight,
                SwpNoactivate | SwpHidewindow);

            LogDebug("Feature Panel v10.0 initialized — BOT folder + AutoSearch edition");
            LogDebug("HWID: " + (activated ? "ACTIVATED" : "DEV MODE"));
            LogDebug("Keyboard Hook: " + (keyboardHookId != IntPtr.Zero ? "INSTALLED" : "FAILED"));
            LogDebug("Alt+Q = Panel  |  Alt+T = AutoType ON/OFF  |  Alt+C = Copy + Auto-Search");
            LogDebug("BOT Folder: " + (botFolderPath ?? "NOT FOUND"));
            LogDebug("Compliance: AUTO-ENABLED (all spoofs active)");
        }

        #region Form Setup + Keyboard Hook

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Double buffering to prevent flicker (do NOT use UserPaint — causes black screen)
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1060, 700);
            this.Name = "FeaturePanelForm";
            this.Text = "";  // CRITICAL: Empty title — SEB's GetOpenWindows skips windows where GetWindowTextLength == 0
            this.FormBorderStyle = FormBorderStyle.None;  // No chrome — stealth overlay
            this.TopMost = true;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F);
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.FromArgb(220, 220, 220);

            // CRITICAL: If SEB's ApplicationMonitor sends WM_CLOSE, cancel and hide instead
            this.FormClosing += (s, e) =>
            {
                if (e.CloseReason == CloseReason.UserClosing ||
                    e.CloseReason == CloseReason.TaskManagerClosing ||
                    e.CloseReason == CloseReason.None)
                {
                    e.Cancel = true;
                    HidePanel();
                }
            };

            this.ResumeLayout(false);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                // DO NOT use WS_EX_COMPOSITED (0x02000000) — causes black screen with TreeView/RichTextBox
                cp.ExStyle |= 0x08000000; // WS_EX_NOACTIVATE — prevents EVENT_SYSTEM_FOREGROUND on show
                cp.ExStyle |= 0x00000080; // WS_EX_TOOLWINDOW — hidden from Alt+Tab, taskbar
                return cp;
            }
        }

        private const int WmMouseactivate = 0x0021;
        private const int MaNoactivate = 3;
        private const int MaActivate = 1;
        private const int WsExNoactivate = 0x08000000;
        private const int GwlExstyle = -20;
        private const int WmShowwindow = 0x0018;
        private const int WmWindowposchanging = 0x0046;
        private const int WmClose = 0x0010;
        private const uint SwpHidewindowFlag = 0x0080;

        /// <summary>
        /// Removes WS_EX_NOACTIVATE from the window's extended style.
        /// This allows the form to be activated when the user clicks on it,
        /// which is REQUIRED for TextBox/RichTextBox to receive keyboard input.
        /// </summary>
        private void RemoveNoActivateStyle()
        {
            try
            {
                IntPtr exStyle = IntPtr.Size == 4
                    ? GetWindowLong32(this.Handle, GwlExstyle)
                    : GetWindowLongPtr64(this.Handle, GwlExstyle);
                IntPtr newStyle = (IntPtr)(exStyle.ToInt64() & ~WsExNoactivate);
                if (IntPtr.Size == 4)
                    SetWindowLong32(this.Handle, GwlExstyle, newStyle);
                else
                    SetWindowLongPtr64(this.Handle, GwlExstyle, newStyle);
            }
            catch { }
        }

        /// <summary>
        /// Re-adds WS_EX_NOACTIVATE to the window's extended style.
        /// Called when hiding the panel so it doesn't steal activation while hidden.
        /// </summary>
        private void AddNoActivateStyle()
        {
            try
            {
                IntPtr exStyle = IntPtr.Size == 4
                    ? GetWindowLong32(this.Handle, GwlExstyle)
                    : GetWindowLongPtr64(this.Handle, GwlExstyle);
                IntPtr newStyle = (IntPtr)(exStyle.ToInt64() | WsExNoactivate);
                if (IntPtr.Size == 4)
                    SetWindowLong32(this.Handle, GwlExstyle, newStyle);
                else
                    SetWindowLongPtr64(this.Handle, GwlExstyle, newStyle);
            }
            catch { }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WINDOWPOS
        {
            public IntPtr hwnd;
            public IntPtr hwndInsertAfter;
            public int x, y, cx, cy;
            public uint flags;
        }

        /// <summary>
        /// Nuclear counter to SEB's ApplicationMonitor:
        /// 
        /// SEB detects our panel via EVENT_SYSTEM_CAPTURESTART when controls are clicked.
        /// It then calls ShowWindow(handle, SW_HIDE) on the detected window handle.
        /// 
        /// We counter this by:
        /// 1. Intercepting WM_WINDOWPOSCHANGING and removing SWP_HIDEWINDOW flag
        /// 2. Intercepting WM_SHOWWINDOW with show=false and ignoring it
        /// 3. Intercepting WM_CLOSE and cancelling it
        /// 4. Returning MA_NOACTIVATE on WM_MOUSEACTIVATE to prevent foreground change
        /// 
        /// This creates a "zombie" window that SEB cannot hide or close.
        /// Only our own HidePanel() can hide it (via direct SetWindowPos with our flag).
        /// </summary>
        private bool allowHide = false;  // Only OUR code sets this true

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WmMouseactivate:
                    // When the panel is visible, allow activation so TextBoxes can receive keyboard input.
                    // When parented to SEB via SetParent, activating the child doesn't change the
                    // foreground window (SEB's top-level window stays foreground), so no EVENT_SYSTEM_FOREGROUND fires.
                    if (visibleState)
                        m.Result = (IntPtr)MaActivate;
                    else
                        m.Result = (IntPtr)MaNoactivate;
                    return;

                case WmShowwindow:
                    // lParam == 0 means the call is from ShowWindow()
                    // wParam == 0 means hide, 1 means show
                    if ((int)m.WParam == 0 && !allowHide)
                    {
                        // SEB is trying to hide us — BLOCK IT
                        m.Result = IntPtr.Zero;
                        return;
                    }
                    break;

                case WmWindowposchanging:
                    if (!allowHide)
                    {
                        // Remove the HIDE flag from WINDOWPOSCHANGING
                        // SEB calls SetWindowPos with SWP_HIDEWINDOW — we strip it
                        var pos = (WINDOWPOS)Marshal.PtrToStructure(m.LParam, typeof(WINDOWPOS));
                        if ((pos.flags & SwpHidewindowFlag) != 0)
                        {
                            pos.flags &= ~SwpHidewindowFlag;
                            Marshal.StructureToPtr(pos, m.LParam, true);
                        }
                    }
                    break;

                case WmClose:
                    if (!allowHide)
                    {
                        // SEB's Close(window) sends WM_CLOSE — block it
                        m.Result = IntPtr.Zero;
                        return;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// Low-level keyboard hook callback. Runs on ANY desktop.
        /// Catches Alt+Q (toggle), Alt+C (copy + auto-search), Alt+T (toggle AutoTyper).
        ///
        /// KEYBOARD INPUT FIX:
        /// When the panel is visible and a TextBox/RichTextBox/ComboBox has focus,
        /// ALL keystrokes pass through to WinForms — EXCEPT Alt+Q (to hide the panel).
        /// This fixes the blocker where panel text fields couldn't receive input.
        /// </summary>
        private IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && ((int)wParam == WmKeydown || (int)wParam == WmSyskeydown))
            {
                int vkCode = Marshal.ReadInt32(lParam);
                bool altPressed = (GetAsyncKeyState(VkMenu) & 0x8000) != 0;

                if (altPressed)
                {
                    if (vkCode == VkQ)
                    {
                        // Alt+Q: Toggle panel
                        try { this.BeginInvoke(new Action(TogglePanel)); } catch { }
                        return (IntPtr)1;
                    }
                    else if (vkCode == VkC)
                    {
                        // Alt+C: Copy — release Alt, then simulate Ctrl+C, then auto-search
                        ThreadPool.QueueUserWorkItem(delegate
                        {
                            keybd_event((byte)VkMenu, 0, KeyeventfKeyup, UIntPtr.Zero);
                            Thread.Sleep(50);
                            keybd_event((byte)VkControl, 0, 0, UIntPtr.Zero);
                            keybd_event((byte)VkC, 0, 0, UIntPtr.Zero);
                            keybd_event((byte)VkC, 0, KeyeventfKeyup, UIntPtr.Zero);
                            keybd_event((byte)VkControl, 0, KeyeventfKeyup, UIntPtr.Zero);

                            // Wait for clipboard to update, then auto-search
                            Thread.Sleep(200);
                            AutoSearchAndLoadAnswer();
                        });
                        return (IntPtr)1;
                    }
                    else if (vkCode == VkT)
                    {
                        // Alt+T: Toggle AutoTyper ON/OFF
                        ThreadPool.QueueUserWorkItem(delegate
                        {
                            keybd_event((byte)VkMenu, 0, KeyeventfKeyup, UIntPtr.Zero);
                            Thread.Sleep(50);
                            ToggleAutoTyper();
                        });
                        return (IntPtr)1;
                    }
                }
            }
            return CallNextHookEx(keyboardHookId, nCode, wParam, lParam);
        }

        /// <summary>
        /// Checks if any input control on the form currently has focus.
        /// Used by the keyboard hook to decide whether to intercept keystrokes.
        /// </summary>
        private bool HasFocusedInputControl()
        {
            try
            {
                if (this.InvokeRequired)
                    return false; // Can't check from non-UI thread safely

                var focused = GetFocusedControlRecursive(this);
                return focused is TextBox ||
                       focused is RichTextBox ||
                       focused is ComboBox ||
                       focused is NumericUpDown ||
                       focused is ListBox;
            }
            catch { return false; }
        }

        /// <summary>
        /// Recursively finds the currently focused control within the form's control tree.
        /// </summary>
        private Control GetFocusedControlRecursive(Control parent)
        {
            if (parent == null) return null;
            if (parent.Focused && !(parent is ContainerControl)) return parent;

            foreach (Control child in parent.Controls)
            {
                // Check container controls that manage focus internally
                if (child is ContainerControl cc && cc.ActiveControl != null)
                {
                    var found = GetFocusedControlRecursive(cc.ActiveControl);
                    if (found != null) return found;
                }

                var found2 = GetFocusedControlRecursive(child);
                if (found2 != null) return found2;
            }
            return null;
        }

        /// <summary>
        /// Toggles the AutoTyper on/off. If no text is loaded, loads from latest clipboard entry.
        /// Hides panel before typing so the browser textarea has focus.
        /// </summary>
        private void ToggleAutoTyper()
        {
            try
            {
                if (autoTyper == null) return;

                if (autoTyper.IsTyping)
                {
                    autoTyper.Stop();
                    LogDebug("AutoTyper PAUSED at " + autoTyper.Position + "/" + autoTyper.TotalLength);
                    return;
                }

                // If no text loaded or fully typed, try auto-search first
                if (!autoTyper.HasText || autoTyper.Position >= autoTyper.TotalLength)
                {
                    // Try to auto-search from clipboard
                    if (clipMgr != null && clipMgr.Count > 0)
                    {
                        var entry = clipMgr.History[0];
                        if (entry != null && !string.IsNullOrEmpty(entry.Text))
                        {
                            // Check if we should search for an answer
                            if (DetectIsQuestion(entry.Text) && answerSearch != null)
                            {
                                AutoSearchAndLoadAnswer();
                                Thread.Sleep(300); // Give search time to complete
                            }
                            else
                            {
                                autoTyper.LoadText(entry.Text);
                            }
                        }
                    }

                    if (!autoTyper.HasText)
                    {
                        LogDebug("AutoTyper: no text loaded and clipboard empty");
                        return;
                    }
                }

                // Hide panel so browser gets focus
                if (visibleState)
                {
                    try { this.BeginInvoke(new Action(HidePanel)); } catch { }
                }
                Thread.Sleep(400); // Wait for browser to regain focus

                // Start typing
                autoTyper.Start();
            }
            catch (Exception ex) { LogDebug("ToggleAutoTyper error: " + ex.Message); }
        }

        /// <summary>
        /// Loads text into AutoTyper from a specific string.
        /// </summary>
        private void LoadTextToAutoTyper(string text)
        {
            if (autoTyper == null || string.IsNullOrEmpty(text)) return;
            autoTyper.LoadText(text);
            LogDebug("Loaded " + text.Length + " chars to AutoTyper");
            UpdateAutoTyperStatus();
        }

        #region BOT Folder + Auto-Search

        /// <summary>
        /// Finds the BOT folder by searching Desktop, Documents, Downloads, then all drive roots.
        /// </summary>
        private string FindBotFolder()
        {
            // Search order: Desktop > Documents > Downloads > All drive roots
            var searchPaths = new[]
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), BOT_FOLDER_NAME),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), BOT_FOLDER_NAME),
                Path.Combine(KnownFolderPath(new Guid("374DE290-123F-4565-9164-39C4925E467B")), BOT_FOLDER_NAME),
            };

            foreach (var p in searchPaths)
            {
                if (!string.IsNullOrEmpty(p) && Directory.Exists(p))
                {
                    LogDebug("BOT folder found: " + p);
                    return p;
                }
            }

            // Fallback: scan drive roots
            foreach (var drive in DriveInfo.GetDrives())
            {
                try
                {
                    if (!drive.IsReady) continue;
                    var botPath = Path.Combine(drive.RootDirectory.FullName, BOT_FOLDER_NAME);
                    if (Directory.Exists(botPath))
                    {
                        LogDebug("BOT folder found on drive: " + botPath);
                        return botPath;
                    }
                }
                catch { }
            }

            LogDebug("No BOT folder found — searched Desktop, Documents, Downloads, all drives");
            return null;
        }

        /// <summary>
        /// Initializes the BOT folder system: finds folder, creates search engine, sets up watcher.
        /// </summary>
        private void InitBotFolder()
        {
            botFolderPath = FindBotFolder();

            if (botFolderPath != null)
            {
                answerSearch = new AnswerSearchEngine(botFolderPath);
                answerSearch.LogEvent += (msg) => LogDebug("[Search] " + msg);

                // Set up FileSystemWatcher for auto-reload
                try
                {
                    botFolderWatcher = new FileSystemWatcher(botFolderPath)
                    {
                        IncludeSubdirectories = true,
                        NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite,
                        EnableRaisingEvents = true
                    };
                    botFolderWatcher.Changed += (s, e) => ScheduleBotReload();
                    botFolderWatcher.Created += (s, e) => ScheduleBotReload();
                    botFolderWatcher.Deleted += (s, e) => ScheduleBotReload();
                    botFolderWatcher.Renamed += (s, e) => ScheduleBotReload();
                    LogDebug("FileSystemWatcher active on BOT folder");
                }
                catch (Exception ex)
                {
                    LogDebug("FileSystemWatcher failed: " + ex.Message);
                }

                if (botFolderStatusLabel != null)
                {
                    botFolderStatusLabel.Text = "  📁 BOT: " + botFolderPath;
                    botFolderStatusLabel.ForeColor = Color.FromArgb(100, 255, 100);
                }
            }
            else
            {
                if (botFolderStatusLabel != null)
                {
                    botFolderStatusLabel.Text = "  ⚠ No BOT folder found — create a folder named 'BOT' on your Desktop";
                    botFolderStatusLabel.ForeColor = Color.FromArgb(255, 180, 100);
                }
            }
        }

        private System.Windows.Forms.Timer botReloadTimer = null;

        /// <summary>
        /// Debounces BOT folder reload events to avoid spamming on multi-file changes.
        /// </summary>
        private void ScheduleBotReload()
        {
            try
            {
                this.BeginInvoke(new Action(() =>
                {
                    if (botReloadTimer != null)
                    {
                        botReloadTimer.Stop();
                        botReloadTimer.Dispose();
                    }
                    botReloadTimer = new System.Windows.Forms.Timer { Interval = 500 };
                    botReloadTimer.Tick += (s, e) =>
                    {
                        botReloadTimer.Stop();
                        botReloadTimer.Dispose();
                        botReloadTimer = null;
                        LoadBotFolder();
                        LogDebug("BOT folder reloaded (file change detected)");
                    };
                    botReloadTimer.Start();
                }));
            }
            catch { }
        }

        /// <summary>
        /// Loads the BOT folder contents into the file tree, expanding all nodes.
        /// </summary>
        private void LoadBotFolder()
        {
            if (fileTree == null) return;

            fileTree.Nodes.Clear();

            if (string.IsNullOrEmpty(botFolderPath) || !Directory.Exists(botFolderPath))
            {
                var node = new TreeNode("⚠ No BOT folder found")
                {
                    ForeColor = Color.FromArgb(255, 180, 100),
                    Tag = ""
                };
                fileTree.Nodes.Add(node);
                if (botFolderStatusLabel != null)
                {
                    botFolderStatusLabel.Text = "  ⚠ No BOT folder — create 'BOT' on Desktop";
                    botFolderStatusLabel.ForeColor = Color.FromArgb(255, 180, 100);
                }
                return;
            }

            var root = new TreeNode("📁 BOT (" + botFolderPath + ")")
            {
                Tag = botFolderPath,
                ForeColor = Color.FromArgb(100, 255, 100)
            };
            PopulateBotNode(root, botFolderPath);
            root.ExpandAll();
            fileTree.Nodes.Add(root);

            if (pathInput != null)
                pathInput.Text = botFolderPath;
        }

        /// <summary>
        /// Recursively populates tree nodes for the BOT folder, showing full nested structure.
        /// </summary>
        private void PopulateBotNode(TreeNode parent, string path)
        {
            try
            {
                // Directories first
                foreach (var dir in Directory.GetDirectories(path).OrderBy(d => d))
                {
                    var node = new TreeNode("📂 " + Path.GetFileName(dir)) { Tag = dir };
                    PopulateBotNode(node, dir);
                    parent.Nodes.Add(node);
                }

                // Then files
                foreach (var file in Directory.GetFiles(path).OrderBy(f => f))
                {
                    var info = new FileInfo(file);
                    var sizeStr = info.Length < 1024 ? info.Length + "B" :
                                  info.Length < 1048576 ? (info.Length / 1024) + "KB" :
                                  (info.Length / 1048576) + "MB";
                    var node = new TreeNode("📄 " + Path.GetFileName(file) + " [" + sizeStr + "]") { Tag = file };
                    parent.Nodes.Add(node);
                }
            }
            catch (Exception ex)
            {
                LogDebug("PopulateBotNode error: " + ex.Message);
            }
        }

        /// <summary>
        /// Auto-searches BOT folder files for an answer matching the current clipboard content.
        /// If found, loads the answer into AutoTyper automatically.
        /// </summary>
        private void AutoSearchAndLoadAnswer()
        {
            try
            {
                if (answerSearch == null)
                {
                    LogDebug("Auto-search: BOT folder not initialized");
                    return;
                }

                // Get the latest clipboard entry
                string searchText = "";
                if (clipMgr != null && clipMgr.Count > 0)
                {
                    var entry = clipMgr.History[0];
                    searchText = entry.Text ?? "";
                }

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    LogDebug("Auto-search: clipboard is empty");
                    return;
                }

                // Smart detection: is this a question or an answer?
                bool isQuestion = DetectIsQuestion(searchText);
                LogDebug("Auto-search: text classified as " + (isQuestion ? "QUESTION" : "ANSWER"));

                if (!isQuestion)
                {
                    // It's an answer — load directly to AutoTyper
                    autoTyper.LoadText(searchText);
                    LogDebug("Auto-search: loaded answer directly (" + searchText.Length + " chars)");
                    try { this.BeginInvoke(new Action(UpdateAutoTyperStatus)); } catch { }
                    ShowAnswerToast("Loaded clipboard as answer", searchText);
                    return;
                }

                // Search for matching answer
                var result = answerSearch.Search(searchText);

                if (result != null && !string.IsNullOrEmpty(result.Answer))
                {
                    autoTyper.LoadText(result.Answer);
                    LogDebug("Auto-search: FOUND answer (" + result.Answer.Length + " chars) from " +
                             System.IO.Path.GetFileName(result.SourceFile) + " (score: " + result.Score.ToString("F2") + ")");
                    try { this.BeginInvoke(new Action(UpdateAutoTyperStatus)); } catch { }
                    ShowAnswerToast("Found answer: " + result.Preview, result.Answer);
                }
                else
                {
                    LogDebug("Auto-search: no matching answer found in BOT folder");
                    ShowAnswerToast("No answer found", "Create Q:/A: formatted files in BOT folder");
                }
            }
            catch (Exception ex)
            {
                LogDebug("AutoSearchAndLoadAnswer error: " + ex.Message);
            }
        }

        /// <summary>
        /// Detects whether the given text is a question (should be searched) or an answer (load directly).
        /// </summary>
        private bool DetectIsQuestion(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return false;

            var trimmed = text.Trim();

            // Contains a question mark → question
            if (trimmed.Contains("?")) return true;

            // Starts with common question words
            var lower = trimmed.ToLowerInvariant();
            string[] questionStarters = { "what ", "why ", "how ", "define ", "explain ", "describe ",
                                           "compare ", "contrast ", "list ", "name ", "identify ",
                                           "calculate ", "solve ", "find ", "determine ", "state ",
                                           "give ", "write ", "draw ", "sketch ", "outline " };
            foreach (var starter in questionStarters)
            {
                if (lower.StartsWith(starter)) return true;
            }

            // Short text (< 200 chars) with no newlines → likely a question
            if (trimmed.Length < 200 && !trimmed.Contains("\n")) return true;

            // Long paragraph → likely an answer
            return false;
        }

        /// <summary>
        /// Shows a brief toast notification with the found answer preview.
        /// </summary>
        private void ShowAnswerToast(string title, string body)
        {
            try
            {
                this.BeginInvoke(new Action(() =>
                {
                    if (autoTyperStatusLabel != null)
                    {
                        var preview = body.Length > 60 ? body.Substring(0, 60) + "..." : body;
                        autoTyperStatusLabel.Text = "  ⌨ " + title + ": " + preview + "  |  Alt+T to type";
                        autoTyperStatusLabel.ForeColor = Color.FromArgb(100, 255, 100);

                        // Reset color after 5 seconds
                        var toastTimer = new System.Windows.Forms.Timer { Interval = 5000 };
                        toastTimer.Tick += (s, e) =>
                        {
                            toastTimer.Stop();
                            toastTimer.Dispose();
                            UpdateAutoTyperStatus();
                        };
                        toastTimer.Start();
                    }
                }));
            }
            catch { }
        }

        #endregion

        /// <summary>
        /// Updates the auto-typer status bar label.
        /// </summary>
        private void UpdateAutoTyperStatus()
        {
            if (autoTyperStatusLabel == null) return;
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(UpdateAutoTyperStatus));
                    return;
                }
                autoTyperStatusLabel.Text = "  ⌨ AutoTyper: " + autoTyper.StatusText + "  |  Alt+T = Toggle";
                autoTyperStatusLabel.ForeColor = autoTyper.IsTyping
                    ? Color.FromArgb(100, 255, 100)  // Green when typing
                    : Color.FromArgb(180, 200, 220); // Dim when idle

                // Update progress bar
                if (autoTyperProgress != null)
                {
                    int pct = (int)autoTyper.Progress;
                    if (pct < 0) pct = 0;
                    if (pct > 100) pct = 100;
                    autoTyperProgress.Value = pct;
                }
            }
            catch { }
        }

        // SimulateCtrlV removed — replaced by AutoTyper (KEYEVENTF_UNICODE bypass)

        private void TogglePanel()
        {
            if (!formReady) return;
            LogDebug("Alt+Q detected — " + (visibleState ? "HIDING" : "SHOWING") + " panel");

            if (visibleState)
                HidePanel();
            else
                ShowPanel();
        }

        private void ShowPanel()
        {
            visibleState = true;

            // CRITICAL: Remove WS_EX_NOACTIVATE so the form can be activated when clicked.
            // Without this, TextBox/RichTextBox/ComboBox cannot receive keyboard input.
            // SWP_NOACTIVATE in SetWindowPos below prevents activation on SHOW,
            // but the user can click to activate (WM_MOUSEACTIVATE returns MA_ACTIVATE).
            // When parented to SEB via SetParent, activating the child doesn't change
            // the foreground window, so EVENT_SYSTEM_FOREGROUND doesn't fire.
            RemoveNoActivateStyle();

            // CRITICAL: Find SEB's main window and make our panel its child.
            // SEB's ApplicationMonitor checks the PROCESS that owns each window.
            // Our panel runs in CefSharp.BrowserSubprocess.exe (not SEB).
            // By using SetParent, our panel becomes a child window of SEB.
            // SetWinEventHook callbacks report the OWNER window's process,
            // so our controls are now "owned by SEB" in the monitor's eyes.
            if (sebWindowHandle == IntPtr.Zero)
            {
                sebWindowHandle = FindSebWindow();
                if (sebWindowHandle != IntPtr.Zero)
                {
                    SetParent(this.Handle, sebWindowHandle);
                    LogDebug("SetParent: Panel is now child of SEB window " + sebWindowHandle);
                }
            }

            // Calculate center position on current screen
            var screen = Screen.FromHandle(this.Handle);
            int x, y;

            if (sebWindowHandle != IntPtr.Zero)
            {
                // As a child window, coordinates are relative to parent's client area
                // Center within the parent
                x = (screen.WorkingArea.Width - PanelWidth) / 2;
                y = (screen.WorkingArea.Height - PanelHeight) / 2;
            }
            else
            {
                // Standalone mode (CefSharp example) — screen coordinates
                x = screen.WorkingArea.Left + (screen.WorkingArea.Width - PanelWidth) / 2;
                y = screen.WorkingArea.Top + (screen.WorkingArea.Height - PanelHeight) / 2;
            }

            // Show the panel. When parented to SEB, we don't need TOPMOST
            // (child windows are always on top within their parent).
            // SWP_NOACTIVATE still prevents EVENT_SYSTEM_FOREGROUND.
            SetWindowPos(
                this.Handle,
                sebWindowHandle != IntPtr.Zero ? IntPtr.Zero : HwndTopmost,
                x, y, PanelWidth, PanelHeight,
                SwpNoactivate | SwpShowwindow);

            this.Visible = true;
            this.Invalidate(true);  // Force full repaint including children
            this.Update();
            if (tabControl != null)
            {
                tabControl.Invalidate(true);
                tabControl.Update();
            }

            // Delayed forced repaint — on SEB's kiosk desktop, the first paint
            // may fail because the graphics context isn't fully ready yet.
            // A timer ensures controls render even if the initial paint was missed.
            var repaintTimer = new System.Windows.Forms.Timer { Interval = 150 };
            repaintTimer.Tick += (ts, te) =>
            {
                repaintTimer.Stop();
                repaintTimer.Dispose();
                try
                {
                    this.Refresh();
                    if (tabControl != null) tabControl.Refresh();
                    foreach (Control c in this.Controls)
                    {
                        c.Refresh();
                        foreach (Control cc in c.Controls)
                            cc.Refresh();
                    }
                }
                catch { }
            };
            repaintTimer.Start();

            LogDebug("Panel SHOWN at (" + x + "," + y + ") size " + PanelWidth + "x" + PanelHeight);
        }

        private void HidePanel()
        {
            visibleState = false;
            allowHide = true;  // Let OUR hide through the WndProc guard

            // Re-add WS_EX_NOACTIVATE so the hidden panel doesn't interfere with activation
            AddNoActivateStyle();
            // Move off-screen and hide — no activation change
            SetWindowPos(
                this.Handle,
                HwndTopmost,
                -9999, -9999, PanelWidth, PanelHeight,
                SwpNoactivate | SwpHidewindow);
            this.Visible = false;
            allowHide = false;  // Re-arm the guard
            LogDebug("Panel HIDDEN");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (keyboardHookId != IntPtr.Zero)
            {
                UnhookWindowsHookEx(keyboardHookId);
                keyboardHookId = IntPtr.Zero;
            }
            if (resTimer != null) resTimer.Stop();
            if (botFolderWatcher != null) { botFolderWatcher.EnableRaisingEvents = false; botFolderWatcher.Dispose(); }
            if (autoTyper != null) autoTyper.Dispose();
            if (clipMgr != null) clipMgr.SaveHistory();
            if (jsEngine != null) jsEngine.SaveRules();
            base.OnFormClosing(e);
        }

        #endregion

        #region Build UI — 5 Tabs (no Compliance)

        private void BuildUI()
        {
            // Custom title bar (since FormBorderStyle.None removes native chrome)
            var titleBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 32,
                BackColor = Color.FromArgb(20, 20, 20),
                Cursor = Cursors.SizeAll,
            };

            var titleLabel = new Label
            {
                Text = "SEB Enhanced v10.0",
                ForeColor = Color.FromArgb(100, 200, 255),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(8, 0, 0, 0),
            };

            var closeBtn = new Button
            {
                Text = "\u2715",
                ForeColor = Color.FromArgb(180, 180, 180),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F),
                Size = new Size(40, 32),
                Dock = DockStyle.Right,
                Cursor = Cursors.Hand,
            };
            closeBtn.FlatAppearance.BorderSize = 0;
            closeBtn.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 50, 50);
            closeBtn.Click += (s, e) => HidePanel();

            // Drag support for the title bar
            bool dragging = false;
            Point dragStart = Point.Empty;
            titleBar.MouseDown += (s, e) => { dragging = true; dragStart = e.Location; };
            titleBar.MouseMove += (s, e) =>
            {
                if (dragging)
                {
                    this.Left += e.X - dragStart.X;
                    this.Top += e.Y - dragStart.Y;
                }
            };
            titleBar.MouseUp += (s, e) => { dragging = false; };
            titleLabel.MouseDown += (s, e) => { dragging = true; dragStart = e.Location; };
            titleLabel.MouseMove += (s, e) =>
            {
                if (dragging)
                {
                    this.Left += e.X - dragStart.X;
                    this.Top += e.Y - dragStart.Y;
                }
            };
            titleLabel.MouseUp += (s, e) => { dragging = false; };

            titleBar.Controls.Add(titleLabel);
            titleBar.Controls.Add(closeBtn);
            this.Controls.Add(titleBar);

            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 9.5F),
            };
            this.Controls.Add(tabControl);

            // IMPORTANT: Dock order matters — tabControl fills space BELOW titleBar
            tabControl.BringToFront();

            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.DrawItem += (s, e) =>
            {
                var g = e.Graphics;
                var tabPage = tabControl.TabPages[e.Index];
                var tabBounds = tabControl.GetTabRect(e.Index);
                var isSelected = (tabControl.SelectedIndex == e.Index);
                using (var bgBrush = new SolidBrush(isSelected ? Color.FromArgb(50, 50, 50) : Color.FromArgb(35, 35, 35)))
                using (var fgBrush = new SolidBrush(isSelected ? Color.FromArgb(100, 200, 255) : Color.FromArgb(180, 180, 180)))
                {
                    g.FillRectangle(bgBrush, tabBounds);
                    var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString(tabPage.Text, tabControl.Font, fgBrush, tabBounds, sf);
                }
            };

            // 5 tabs: Clipboard, Files, Debug, Settings, Resources
            tabControl.TabPages.Add(BuildClipboardTab());
            tabControl.TabPages.Add(BuildFileManagerTab());
            tabControl.TabPages.Add(BuildDebugTab());
            tabControl.TabPages.Add(BuildSettingsTab());
            tabControl.TabPages.Add(BuildResourceTab());

            // Bottom panel: AutoTyper status + progress bar + speed control
            var bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 52,
                BackColor = Color.FromArgb(25, 30, 35),
                Padding = new Padding(4, 2, 4, 2)
            };

            // Status label (top line of bottom panel)
            autoTyperStatusLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 18,
                Text = "  ⌨ AutoTyper: No text loaded  |  Alt+T = Toggle",
                ForeColor = Color.FromArgb(180, 200, 220),
                Font = new Font("Consolas", 8.5F),
                TextAlign = ContentAlignment.MiddleLeft,
                BorderStyle = BorderStyle.None
            };

            // Progress bar (fills middle of bottom panel)
            autoTyperProgress = new ProgressBar
            {
                Dock = DockStyle.Fill,
                Style = ProgressBarStyle.Continuous,
                ForeColor = Color.FromArgb(100, 255, 100),
                BackColor = Color.FromArgb(40, 45, 50),
                Minimum = 0,
                Maximum = 100,
                Value = 0,
                Height = 14
            };

            // Speed control dropdown (bottom-right)
            var speedLabel = new Label
            {
                Text = "Speed:",
                ForeColor = Color.FromArgb(150, 170, 200),
                Font = new Font("Segoe UI", 8.5F),
                Dock = DockStyle.Right,
                Width = 50,
                TextAlign = ContentAlignment.MiddleRight
            };
            cmbSpeedPreset = new ComboBox
            {
                Dock = DockStyle.Right,
                Width = 90,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 8.5F)
            };
            cmbSpeedPreset.Items.AddRange(new object[] { "Slow", "Normal", "Fast", "Instant" });
            cmbSpeedPreset.SelectedIndex = 1; // Normal
            cmbSpeedPreset.SelectedIndexChanged += (s, e) =>
            {
                if (autoTyper == null) return;
                var preset = (AutoTyper.SpeedPreset)cmbSpeedPreset.SelectedIndex;
                autoTyper.SetSpeedPreset(preset);
                LogDebug("Speed preset changed: " + preset);
            };

            // BOT folder status (bottom-left)
            botFolderStatusLabel = new Label
            {
                Text = "  📁 BOT: searching...",
                ForeColor = Color.FromArgb(180, 180, 180),
                Font = new Font("Segoe UI", 8F),
                Dock = DockStyle.Left,
                Width = 300,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Layout: [BOT status .... progress bar .... speed label + dropdown]
            bottomPanel.Controls.Add(autoTyperProgress);
            bottomPanel.Controls.Add(speedLabel);
            bottomPanel.Controls.Add(cmbSpeedPreset);
            bottomPanel.Controls.Add(botFolderStatusLabel);
            bottomPanel.Controls.Add(autoTyperStatusLabel);

            this.Controls.Add(bottomPanel);
            bottomPanel.SendToBack();
        }

        #endregion

        #region Tab 1 — Clipboard

        private TabPage BuildClipboardTab()
        {
            var tab = new TabPage("Clipboard") { BackColor = Color.FromArgb(30, 30, 30) };

            clipSearch = new TextBox
            {
                Dock = DockStyle.Top,
                Height = 28,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F),
                Text = "",
            };
            clipSearch.TextChanged += (s, e) => RefreshClipboardGrid();

            clipGrid = new DataGridView
            {
                Dock = DockStyle.Left,
                Width = 460,
                BackgroundColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.White,
                GridColor = Color.FromArgb(60, 60, 60),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(35, 35, 35),
                    ForeColor = Color.White,
                    SelectionBackColor = Color.FromArgb(60, 80, 120),
                    SelectionForeColor = Color.White,
                    Font = new Font("Consolas", 9F)
                },
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.FromArgb(50, 50, 50),
                    ForeColor = Color.FromArgb(180, 220, 255),
                    Font = new Font("Segoe UI Semibold", 9F)
                },
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BorderStyle = BorderStyle.None
            };
            clipGrid.Columns.Add("Type", "Type");
            clipGrid.Columns.Add("Time", "Time");
            clipGrid.Columns.Add("Preview", "Preview");
            clipGrid.Columns["Type"].Width = 40;
            clipGrid.Columns["Time"].Width = 65;
            clipGrid.SelectionChanged += ClipGridSelectionChanged;
            clipGrid.CellDoubleClick += (s, e) => LoadSelectedToAutoTyper();

            var previewPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(25, 25, 25) };
            clipPreview = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BackColor = Color.FromArgb(25, 25, 25),
                ForeColor = Color.FromArgb(200, 230, 255),
                Font = new Font("Consolas", 10F),
                BorderStyle = BorderStyle.None
            };
            clipImagePreview = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(25, 25, 25),
                Visible = false
            };
            previewPanel.Controls.Add(clipPreview);
            previewPanel.Controls.Add(clipImagePreview);

            // Hint label
            var hintLabel = new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                Text = "  Alt+C = Copy + Auto-Search  |  Double-click or 'Load' = Send to AutoTyper  |  Alt+T = Type",
                ForeColor = Color.FromArgb(120, 160, 200),
                BackColor = Color.FromArgb(40, 40, 40),
                Font = new Font("Segoe UI", 8.5F),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.FromArgb(35, 35, 35),
                Padding = new Padding(4)
            };

            var btnLoadTyper = MakeButton("⌨ Load to AutoTyper", Color.FromArgb(50, 130, 80));
            btnLoadTyper.Click += (s, e) => LoadSelectedToAutoTyper();

            var btnResetTyper = MakeButton("Reset Position", Color.FromArgb(80, 120, 80));
            btnResetTyper.Click += (s, e) => { if (autoTyper != null) { autoTyper.ResetPosition(); UpdateAutoTyperStatus(); } };

            var btnCopy = MakeButton("Copy to OS", Color.FromArgb(60, 90, 140));
            btnCopy.Click += (s, e) => CopySelectedToOS();

            var btnDelete = MakeButton("Delete", Color.FromArgb(140, 60, 60));
            btnDelete.Click += (s, e) => DeleteSelectedClip();

            var btnClear = MakeButton("Clear All", Color.FromArgb(100, 60, 60));
            btnClear.Click += (s, e) => { clipMgr.ClearHistory(); RefreshClipboardGrid(); };

            var btnExport = MakeButton("Export", Color.FromArgb(80, 80, 120));
            btnExport.Click += (s, e) => ExportClipHistory();

            btnPanel.Controls.AddRange(new Control[] { btnLoadTyper, btnResetTyper, btnCopy, btnDelete, btnClear, btnExport });

            tab.Controls.Add(previewPanel);
            tab.Controls.Add(clipGrid);
            tab.Controls.Add(btnPanel);
            tab.Controls.Add(hintLabel);
            tab.Controls.Add(clipSearch);

            RefreshClipboardGrid();
            return tab;
        }

        private void ClipGridSelectionChanged(object sender, EventArgs e)
        {
            if (clipGrid.SelectedRows.Count == 0) return;
            var idx = clipGrid.SelectedRows[0].Index;
            var entries = GetFilteredClipEntries();
            if (idx < 0 || idx >= entries.Count) return;

            var entry = entries[idx];

            if (entry.EntryType == ClipboardEntryType.Image && entry.ImageData != null)
            {
                var img = entry.GetImage();
                if (img != null)
                {
                    if (clipImagePreview.Image != null) clipImagePreview.Image.Dispose();
                    clipImagePreview.Image = img;
                    clipImagePreview.Visible = true;
                    clipPreview.Visible = false;
                    return;
                }
            }

            clipImagePreview.Visible = false;
            clipPreview.Visible = true;

            if (!string.IsNullOrEmpty(entry.Html))
                clipPreview.Text = entry.Html;
            else if (!string.IsNullOrEmpty(entry.Text))
                clipPreview.Text = entry.Text;
            else if (entry.FilePaths != null)
                clipPreview.Text = string.Join("\r\n", entry.FilePaths);
            else
                clipPreview.Text = "[No preview available]";
        }

        private void RefreshClipboardGrid()
        {
            if (clipGrid == null) return;
            if (this.InvokeRequired) { this.BeginInvoke(new Action(RefreshClipboardGrid)); return; }

            clipGrid.Rows.Clear();
            var entries = GetFilteredClipEntries();
            foreach (var e in entries)
            {
                clipGrid.Rows.Add(e.GetTypeIcon(), e.Timestamp.ToString("HH:mm:ss"), e.GetPreview(70));
            }
        }

        private List<ClipboardEntry> GetFilteredClipEntries()
        {
            var query = clipSearch != null ? clipSearch.Text : "";
            return string.IsNullOrWhiteSpace(query) ? clipMgr.History.ToList() : clipMgr.Search(query);
        }

        private void LoadSelectedToAutoTyper()
        {
            if (clipGrid == null || clipGrid.SelectedRows.Count == 0) return;
            var idx = clipGrid.SelectedRows[0].Index;
            var entries = GetFilteredClipEntries();
            if (idx >= 0 && idx < entries.Count)
            {
                var entry = entries[idx];
                if (string.IsNullOrEmpty(entry.Text))
                {
                    LogDebug("AutoTyper: entry has no text content");
                    return;
                }
                LoadTextToAutoTyper(entry.Text);
            }
        }

        private void CopySelectedToOS()
        {
            if (clipGrid.SelectedRows.Count == 0) return;
            var idx = clipGrid.SelectedRows[0].Index;
            var entries = GetFilteredClipEntries();
            if (idx >= 0 && idx < entries.Count)
            {
                clipMgr.ForceCopyToClipboard(entries[idx]);
                LogDebug("Copied to OS clipboard: " + entries[idx].GetPreview(40));
            }
        }

        private void DeleteSelectedClip()
        {
            if (clipGrid.SelectedRows.Count == 0) return;
            var idx = clipGrid.SelectedRows[0].Index;
            var entries = GetFilteredClipEntries();
            if (idx >= 0 && idx < entries.Count)
            {
                clipMgr.RemoveEntry(entries[idx]);
                RefreshClipboardGrid();
            }
        }

        private void ExportClipHistory()
        {
            try
            {
                var path = Path.Combine(Path.GetTempPath(), "seb_clip_export_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt");
                File.WriteAllText(path, clipMgr.ExportHistory());
                LogDebug("Clipboard exported to: " + path);
                MessageBox.Show("Exported to:\n" + path, "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { LogDebug("Export failed: " + ex.Message); }
        }

        #endregion

        #region Tab 2 — File Manager

        private TabPage BuildFileManagerTab()
        {
            var tab = new TabPage("Files") { BackColor = Color.FromArgb(30, 30, 30) };

            // Path input bar at top
            pathInput = new TextBox
            {
                Dock = DockStyle.Top,
                Height = 28,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F),
                Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };
            pathInput.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) LoadDirectory(pathInput.Text);
            };

            // File content search bar — searches WITHIN the currently opened file
            var searchPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 28,
                BackColor = Color.FromArgb(35, 35, 35)
            };
            var searchLabel = new Label
            {
                Text = "Search:",
                Width = 55,
                Dock = DockStyle.Left,
                ForeColor = Color.FromArgb(150, 180, 220),
                Font = new Font("Segoe UI", 9F),
                TextAlign = ContentAlignment.MiddleCenter
            };
            fileSearchBox = new TextBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(50, 50, 55),
                ForeColor = Color.FromArgb(220, 240, 255),
                Font = new Font("Segoe UI", 9.5F),
                BorderStyle = BorderStyle.None,
                Text = ""
            };
            // Live search as user types
            fileSearchBox.TextChanged += (s, e) =>
            {
                SearchInOpenFile(fileSearchBox.Text);
            };
            searchPanel.Controls.Add(fileSearchBox);
            searchPanel.Controls.Add(searchLabel);

            // Left panel: tree view
            fileTree = new TreeView
            {
                Dock = DockStyle.Left,
                Width = 340,
                BackColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.FromArgb(200, 220, 255),
                Font = new Font("Consolas", 9.5F),
                BorderStyle = BorderStyle.None,
                ShowLines = true,
                ShowPlusMinus = true,
                HideSelection = false
            };
            fileTree.AfterSelect += FileTreeAfterSelect;
            fileTree.BeforeExpand += FileTreeBeforeExpand;

            // Right panel: file preview + search results
            var previewPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(25, 25, 25) };
            filePreview = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BackColor = Color.FromArgb(25, 25, 25),
                ForeColor = Color.FromArgb(200, 230, 200),
                Font = new Font("Consolas", 10F),
                BorderStyle = BorderStyle.None,
                WordWrap = false
            };
            fileImagePreview = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(25, 25, 25),
                Visible = false
            };
            fileSearchResults = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BackColor = Color.FromArgb(20, 25, 30),
                ForeColor = Color.FromArgb(180, 220, 255),
                Font = new Font("Consolas", 9.5F),
                BorderStyle = BorderStyle.None,
                WordWrap = true,
                Visible = false
            };
            previewPanel.Controls.Add(filePreview);
            previewPanel.Controls.Add(fileImagePreview);
            previewPanel.Controls.Add(fileSearchResults);

            // Bottom button panel
            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 40,
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = Color.FromArgb(35, 35, 35),
                Padding = new Padding(4)
            };

            var btnLoad = MakeButton("Refresh BOT", Color.FromArgb(60, 90, 140));
            btnLoad.Click += (s, e) => LoadBotFolder();

            var btnSearchAnswer = MakeButton("🔍 Search Answer", Color.FromArgb(90, 60, 140));
            btnSearchAnswer.Click += (s, e) =>
            {
                // Search for the currently selected clip entry
                if (clipMgr != null && clipMgr.Count > 0)
                {
                    AutoSearchAndLoadAnswer();
                }
                else
                {
                    LogDebug("No clipboard content to search for");
                }
            };

            var btnUpload = MakeButton("Go To Path", Color.FromArgb(50, 130, 80));
            btnUpload.Click += (s, e) => OpenFileDialog();

            var btnCopyContent = MakeButton("Copy Content", Color.FromArgb(80, 130, 60));
            btnCopyContent.Click += (s, e) =>
            {
                try
                {
                    if (fileTree.SelectedNode == null || fileTree.SelectedNode.Tag == null) return;
                    var filePath = fileTree.SelectedNode.Tag.ToString();
                    if (!File.Exists(filePath)) { LogDebug("Not a file: " + filePath); return; }

                    var content = File.ReadAllText(filePath, Encoding.UTF8);
                    if (content.Length > 50000) content = content.Substring(0, 50000);

                    if (clipMgr != null)
                    {
                        var entry = new ClipboardEntry
                        {
                            EntryType = ClipboardEntryType.Text,
                            Text = content,
                            SourceApp = "File: " + Path.GetFileName(filePath)
                        };
                        clipMgr.ForceCopyToClipboard(entry);
                        clipMgr.AddEntryManually(entry);
                        RefreshClipboardGrid();
                    }
                    LogDebug("Copied file content: " + Path.GetFileName(filePath) + " (" + content.Length + " chars)");
                }
                catch (Exception ex) { LogDebug("Copy content failed: " + ex.Message); }
            };

            // NEW: Load file content directly to auto-typer
            var btnLoadToTyper = MakeButton("⌨ Load to AutoTyper", Color.FromArgb(50, 140, 90));
            btnLoadToTyper.Click += (s, e) =>
            {
                try
                {
                    if (fileTree.SelectedNode == null || fileTree.SelectedNode.Tag == null) return;
                    var filePath = fileTree.SelectedNode.Tag.ToString();
                    if (!File.Exists(filePath)) { LogDebug("Not a file: " + filePath); return; }

                    var content = File.ReadAllText(filePath, Encoding.UTF8);
                    if (content.Length > 50000) content = content.Substring(0, 50000);
                    LoadTextToAutoTyper(content);
                }
                catch (Exception ex) { LogDebug("Load to AutoTyper failed: " + ex.Message); }
            };

            var btnSaveAs = MakeButton("Save As", Color.FromArgb(80, 80, 120));
            btnSaveAs.Click += (s, e) => { try { SaveFileAs(); } catch (Exception ex) { LogDebug("SaveAs failed: " + ex.Message); } };

            btnPanel.Controls.AddRange(new Control[] { btnLoad, btnSearchAnswer, btnUpload, btnCopyContent, btnLoadToTyper, btnSaveAs });

            tab.Controls.Add(previewPanel);
            tab.Controls.Add(fileTree);
            tab.Controls.Add(btnPanel);
            tab.Controls.Add(searchPanel);
            tab.Controls.Add(pathInput);

            try { LoadBotFolder(); }
            catch (Exception ex) { LogDebug("LoadBotFolder failed: " + ex.Message); }
            return tab;
        }

        // Stores the raw content of the currently previewed file for in-file search
        private string currentFileContent = "";
        private string currentFilePath = "";

        /// <summary>
        /// Searches within the currently opened/previewed file content.
        /// Shows matching lines with line numbers highlighted.
        /// </summary>
        private void SearchInOpenFile(string query)
        {
            if (filePreview == null || fileSearchResults == null) return;

            if (string.IsNullOrWhiteSpace(query))
            {
                // Clear search — show normal preview
                fileSearchResults.Visible = false;
                filePreview.Visible = true;
                return;
            }

            if (string.IsNullOrEmpty(currentFileContent))
            {
                fileSearchResults.Text = "No file open. Select a file from the tree first.";
                fileSearchResults.Visible = true;
                filePreview.Visible = false;
                return;
            }

            var lines = currentFileContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            var results = new StringBuilder();
            int matchCount = 0;

            results.AppendLine($"Searching in: {Path.GetFileName(currentFilePath)}");
            results.AppendLine($"Query: \"{query}\"");
            results.AppendLine(new string('─', 50));
            results.AppendLine();

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    matchCount++;
                    var lineText = lines[i].TrimEnd();
                    if (lineText.Length > 150) lineText = lineText.Substring(0, 150) + "...";
                    results.AppendLine($"  Line {i + 1}:  {lineText}");
                    results.AppendLine();

                    if (matchCount >= 200) { results.AppendLine("... (200 matches limit)"); break; }
                }
            }

            if (matchCount == 0)
                results.AppendLine("  No matches found.");
            else
                results.Insert(results.ToString().IndexOf('\n') + 1, $"Found {matchCount} matches\n");

            fileSearchResults.Text = results.ToString();
            fileSearchResults.Visible = true;
            filePreview.Visible = false;
        }

        private void LoadDefaultDirs()
        {
            fileTree.Nodes.Clear();
            try
            {
                var paths = new[]
                {
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    KnownFolderPath(new Guid("374DE290-123F-4565-9164-39C4925E467B")),
                    Path.GetTempPath()
                };

                foreach (var p in paths)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(p) && Directory.Exists(p))
                        {
                            var node = new TreeNode(Path.GetFileName(p) + " (" + p + ")") { Tag = p };
                            node.Nodes.Add(new TreeNode("..."));
                            fileTree.Nodes.Add(node);
                        }
                    }
                    catch { }
                }

                if (fileTree.Nodes.Count == 0)
                {
                    // If all special folders failed, at least show temp
                    var tempPath = Path.GetTempPath();
                    if (Directory.Exists(tempPath))
                    {
                        var node = new TreeNode("Temp (" + tempPath + ")") { Tag = tempPath };
                        node.Nodes.Add(new TreeNode("..."));
                        fileTree.Nodes.Add(node);
                    }
                }
            }
            catch (Exception ex)
            {
                LogDebug("LoadDefaultDirs error: " + ex.Message);
            }
        }

        private void LoadAllDrives()
        {
            fileTree.Nodes.Clear();
            foreach (var drive in DriveInfo.GetDrives())
            {
                try
                {
                    if (drive.IsReady)
                    {
                        var node = new TreeNode(drive.Name + " [" + drive.DriveType + " - " + (drive.TotalSize / (1024 * 1024 * 1024)) + "GB]")
                        { Tag = drive.RootDirectory.FullName };
                        node.Nodes.Add(new TreeNode("..."));
                        fileTree.Nodes.Add(node);
                    }
                }
                catch { }
            }
        }

        private void LoadDirectory(string path)
        {
            if (!Directory.Exists(path)) return;
            fileTree.Nodes.Clear();
            PopulateNode(null, path);
        }

        private void PopulateNode(TreeNode parent, string path)
        {
            try
            {
                foreach (var dir in Directory.GetDirectories(path))
                {
                    var node = new TreeNode(Path.GetFileName(dir)) { Tag = dir };
                    node.Nodes.Add(new TreeNode("..."));
                    if (parent == null) fileTree.Nodes.Add(node);
                    else parent.Nodes.Add(node);
                }
                foreach (var file in Directory.GetFiles(path))
                {
                    var info = new FileInfo(file);
                    var sizeStr = info.Length < 1024 ? info.Length + "B" :
                                  info.Length < 1048576 ? (info.Length / 1024) + "KB" :
                                  (info.Length / 1048576) + "MB";
                    var node = new TreeNode(Path.GetFileName(file) + " [" + sizeStr + "]") { Tag = file };
                    if (parent == null) fileTree.Nodes.Add(node);
                    else parent.Nodes.Add(node);
                }
            }
            catch (Exception ex)
            {
                LogDebug("Failed to browse '" + path + "': " + ex.Message);
            }
        }

        private void FileTreeBeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                var node = e.Node;
                if (node.Nodes.Count == 1 && node.Nodes[0].Text == "...")
                {
                    node.Nodes.Clear();
                    var path = node.Tag != null ? node.Tag.ToString() : "";
                    if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                        PopulateNode(node, path);
                }
            }
            catch (Exception ex)
            {
                LogDebug("TreeExpand error: " + ex.Message);
                e.Cancel = true;
            }
        }

        private void FileTreeAfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                var path = e.Node != null && e.Node.Tag != null ? e.Node.Tag.ToString() : "";
                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                {
                    if (Directory.Exists(path))
                    {
                        pathInput.Text = path;
                        filePreview.Text = "Directory: " + path + "\r\n\r\n[Double-click or expand to browse]";
                        currentFileContent = "";
                        currentFilePath = "";
                    }
                    return;
                }

                pathInput.Text = path;
                var ext = Path.GetExtension(path).ToLower();

                string[] imageExts = { ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".ico", ".webp" };
                if (Array.IndexOf(imageExts, ext) >= 0)
                {
                    try
                    {
                        if (fileImagePreview.Image != null) fileImagePreview.Image.Dispose();
                        using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                            fileImagePreview.Image = Image.FromStream(stream);
                        fileImagePreview.Visible = true;
                        filePreview.Visible = false;
                        fileSearchResults.Visible = false;
                        currentFileContent = "";
                        currentFilePath = path;
                        return;
                    }
                    catch { }
                }

                fileImagePreview.Visible = false;
                filePreview.Visible = true;

                string[] textExts = { ".txt", ".log", ".cs", ".js", ".html", ".css", ".json", ".xml", ".md", ".py",
                                      ".cpp", ".h", ".cfg", ".ini", ".bat", ".cmd", ".ps1", ".csv", ".seb" };
                if (Array.IndexOf(textExts, ext) >= 0)
                {
                    try
                    {
                        var content = File.ReadAllText(path, Encoding.UTF8);
                        currentFileContent = content;  // Store for search
                        currentFilePath = path;
                        if (content.Length > 100000) content = content.Substring(0, 100000) + "\r\n\r\n... [truncated]";
                        filePreview.Text = content;
                        fileSearchResults.Visible = false;
                        filePreview.Visible = true;
                        return;
                    }
                    catch (Exception ex)
                    {
                        filePreview.Text = "Cannot read file: " + ex.Message;
                        return;
                    }
                }

                try
                {
                    filePreview.Text = "File: " + path + "\r\nSize: " + new FileInfo(path).Length + " bytes\r\nType: " + ext + "\r\n\r\n[Binary file]";
                }
                catch
                {
                    filePreview.Text = "File: " + path + "\r\nType: " + ext + "\r\n\r\n[Cannot read file info]";
                }
            }
            catch (Exception ex)
            {
                LogDebug("FileSelect error: " + ex.Message);
                try { filePreview.Text = "Error: " + ex.Message; } catch { }
            }
        }

        private void OpenFileDialog()
        {
            // SEB kills Explorer — standard OpenFileDialog crashes.
            // Use a simple path input instead.
            var result = ShowPathInputDialog("Open File", "Enter full file path:", "");
            if (!string.IsNullOrEmpty(result))
            {
                if (File.Exists(result))
                {
                    pathInput.Text = result;
                    LogDebug("Opened file: " + result);
                    // Navigate to the file in the tree
                    var dir = Path.GetDirectoryName(result);
                    if (!string.IsNullOrEmpty(dir)) LoadDirectory(dir);
                }
                else if (Directory.Exists(result))
                {
                    LoadDirectory(result);
                }
                else
                {
                    LogDebug("Path not found: " + result);
                }
            }
        }

        private void SaveFileAs()
        {
            var sourcePath = fileTree.SelectedNode != null && fileTree.SelectedNode.Tag != null ? fileTree.SelectedNode.Tag.ToString() : "";
            if (string.IsNullOrEmpty(sourcePath) || !File.Exists(sourcePath)) return;

            // Default to Downloads folder
            var downloadsPath = KnownFolderPath(new Guid("374DE290-123F-4565-9164-39C4925E467B"));
            if (string.IsNullOrEmpty(downloadsPath)) downloadsPath = Path.GetTempPath();
            var defaultDest = Path.Combine(downloadsPath, Path.GetFileName(sourcePath));

            var result = ShowPathInputDialog("Save As", "Enter destination path:", defaultDest);
            if (!string.IsNullOrEmpty(result))
            {
                try
                {
                    var destDir = Path.GetDirectoryName(result);
                    if (!string.IsNullOrEmpty(destDir) && !Directory.Exists(destDir))
                        Directory.CreateDirectory(destDir);
                    File.Copy(sourcePath, result, true);
                    LogDebug("Saved file to: " + result);
                }
                catch (Exception ex) { LogDebug("Save failed: " + ex.Message); }
            }
        }

        /// <summary>
        /// Shows a simple path input dialog that works without Explorer shell.
        /// </summary>
        private string ShowPathInputDialog(string title, string prompt, string defaultValue)
        {
            string result = null;
            var dlg = new Form
            {
                Text = title,
                Size = new Size(560, 180),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.White,
                MaximizeBox = false,
                MinimizeBox = false,
                TopMost = true,
                ShowInTaskbar = false,
            };

            var lbl = new Label
            {
                Text = prompt,
                Location = new Point(12, 12),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F),
            };

            var txt = new TextBox
            {
                Text = defaultValue,
                Location = new Point(12, 40),
                Size = new Size(520, 28),
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F),
            };

            var btnOk = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(370, 90),
                Size = new Size(80, 32),
                BackColor = Color.FromArgb(60, 120, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(458, 90),
                Size = new Size(80, 32),
                BackColor = Color.FromArgb(120, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
            };

            dlg.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnCancel });
            dlg.AcceptButton = btnOk;
            dlg.CancelButton = btnCancel;

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                result = txt.Text.Trim();
            }
            dlg.Dispose();
            return result;
        }

        private string KnownFolderPath(Guid folderId)
        {
            try
            {
                IntPtr ptr;
                if (SHGetKnownFolderPath(folderId, 0, IntPtr.Zero, out ptr) == 0)
                {
                    string path = Marshal.PtrToStringUni(ptr);
                    Marshal.FreeCoTaskMem(ptr);
                    return path;
                }
            }
            catch { }
            return "";
        }

        #endregion

        #region Tab 3 — Debug Console

        private TabPage BuildDebugTab()
        {
            var tab = new TabPage("Debug") { BackColor = Color.FromArgb(30, 30, 30) };

            debugOutput = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BackColor = Color.FromArgb(15, 15, 15),
                ForeColor = Color.FromArgb(180, 220, 180),
                Font = new Font("Consolas", 9.5F),
                BorderStyle = BorderStyle.None,
                WordWrap = true
            };

            debugInput = new TextBox
            {
                Dock = DockStyle.Bottom,
                Height = 28,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.FromArgb(100, 255, 100),
                Font = new Font("Consolas", 10F)
            };
            debugInput.KeyDown += DebugInputKeyDown;

            var infoPanel = new Panel { Dock = DockStyle.Top, Height = 30, BackColor = Color.FromArgb(35, 35, 35) };
            var lblHelp = new Label
            {
                Text = "Commands: hwid | spoof status | clip count | net status | sysinfo | inject <js> | clear | help",
                Dock = DockStyle.Fill,
                ForeColor = Color.FromArgb(130, 150, 180),
                Font = new Font("Consolas", 8.5F),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(6, 0, 0, 0)
            };
            infoPanel.Controls.Add(lblHelp);

            tab.Controls.Add(debugOutput);
            tab.Controls.Add(debugInput);
            tab.Controls.Add(infoPanel);
            return tab;
        }

        private void DebugInputKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;

            var cmd = debugInput.Text.Trim().ToLower();
            debugInput.Clear();

            if (string.IsNullOrEmpty(cmd)) return;

            LogDebug("> " + cmd, Color.FromArgb(100, 200, 255));

            if (cmd == "hwid")
            {
                LogDebug(hwid.GetDisplayInfo(), Color.FromArgb(255, 220, 100));
            }
            else if (cmd == "spoof status")
            {
                LogDebug(spoofer.GetStatus(), Color.FromArgb(255, 180, 100));
            }
            else if (cmd == "clip count")
            {
                LogDebug("Clipboard history: " + clipMgr.Count + " entries", Color.FromArgb(180, 220, 255));
            }
            else if (cmd == "net status")
            {
                LogDebug(reconnect.GetStatus(), Color.FromArgb(180, 255, 180));
            }
            else if (cmd == "sysinfo")
            {
                LogDebug(GetSystemInfo(), Color.FromArgb(220, 200, 255));
            }
            else if (cmd.StartsWith("inject "))
            {
                var js = cmd.Substring(7).Trim();
                if (!string.IsNullOrEmpty(js))
                {
                    jsEngine.InjectNow(js);
                    LogDebug("Injected JS: " + js.Substring(0, Math.Min(80, js.Length)) + "...", Color.FromArgb(255, 200, 100));
                }
            }
            else if (cmd == "clear")
            {
                debugOutput.Clear();
            }
            else if (cmd == "help")
            {
                LogDebug("Available commands:", Color.FromArgb(200, 200, 200));
                LogDebug("  hwid          - Show HWID hash and activation status");
                LogDebug("  spoof status  - Show compliance spoofer state");
                LogDebug("  clip count    - Show clipboard history count");
                LogDebug("  net status    - Show auto-reconnect status");
                LogDebug("  sysinfo       - Show system information");
                LogDebug("  inject <js>   - Inject JavaScript code");
                LogDebug("  clear         - Clear debug output");
            }
            else
            {
                LogDebug("Unknown command: " + cmd, Color.FromArgb(255, 100, 100));
            }
        }

        private string GetSystemInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== System Information ===");
            sb.AppendLine("  OS:          " + Environment.OSVersion);
            sb.AppendLine("  64-bit OS:   " + Environment.Is64BitOperatingSystem);
            sb.AppendLine("  64-bit Proc: " + Environment.Is64BitProcess);
            sb.AppendLine("  Machine:     " + Environment.MachineName);
            sb.AppendLine("  User:        " + Environment.UserName);
            sb.AppendLine("  Processors:  " + Environment.ProcessorCount);
            sb.AppendLine("  CLR Version: " + Environment.Version);
            sb.AppendLine("  Sys Dir:     " + Environment.SystemDirectory);
            sb.AppendLine();

            if (resMon.LatestSnapshot != null)
                sb.Append(resMon.GetFormattedStatus());

            sb.AppendLine();
            sb.Append(MultiMonitorManager.GetMonitorInfo());

            return sb.ToString();
        }

        #endregion

        #region Tab 4 — Settings

        private TabPage BuildSettingsTab()
        {
            var tab = new TabPage("Settings") { BackColor = Color.FromArgb(30, 30, 30) };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 10,
                Padding = new Padding(16)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));

            var lblBoss = new Label { Text = "Boss Key Combo:", ForeColor = Color.FromArgb(100, 200, 255), Font = new Font("Segoe UI Semibold", 10F), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            layout.Controls.Add(lblBoss, 0, 0);
            var bossLabel = new Label
            {
                Text = BossKeyManager.GetHotkeyDisplayString(),
                ForeColor = Color.FromArgb(100, 255, 100),
                Font = new Font("Consolas", 11F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            layout.Controls.Add(bossLabel, 1, 0);

            // Copy/Paste keys info
            var lblKeys = new Label { Text = "Copy / Paste:", ForeColor = Color.FromArgb(100, 200, 255), Font = new Font("Segoe UI Semibold", 10F), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            layout.Controls.Add(lblKeys, 0, 1);
            var keysLabel = new Label
            {
                Text = "Alt+C / Alt+V",
                ForeColor = Color.FromArgb(100, 255, 100),
                Font = new Font("Consolas", 11F, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            layout.Controls.Add(keysLabel, 1, 1);

            // Compliance status
            var lblComp = new Label { Text = "Compliance:", ForeColor = Color.FromArgb(100, 200, 255), Font = new Font("Segoe UI Semibold", 10F), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            layout.Controls.Add(lblComp, 0, 2);
            var compLabel = new Label
            {
                Text = "AUTO (all spoofs active)",
                ForeColor = Color.FromArgb(100, 255, 100),
                Font = new Font("Consolas", 10F),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            layout.Controls.Add(compLabel, 1, 2);

            chkScreenshotPrevention = MakeCheckBox("Screenshot Prevention (block PrintScreen)");
            chkScreenshotPrevention.CheckedChanged += (s, e) =>
            {
                ssGuard.Toggle();
                LogDebug("Screenshot prevention: " + (ssGuard.PreventionEnabled ? "ON" : "OFF"));
            };
            layout.Controls.Add(chkScreenshotPrevention, 0, 3);
            layout.SetColumnSpan(chkScreenshotPrevention, 2);

            var lblMon = new Label { Text = "Panel Monitor:", ForeColor = Color.FromArgb(180, 180, 180), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            layout.Controls.Add(lblMon, 0, 4);
            cmbMonitor = new ComboBox
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };
            for (int i = 0; i < MultiMonitorManager.ScreenCount; i++)
                cmbMonitor.Items.Add("Monitor " + i + " (" + Screen.AllScreens[i].Bounds.Width + "x" + Screen.AllScreens[i].Bounds.Height + ")");
            if (cmbMonitor.Items.Count > 0) cmbMonitor.SelectedIndex = 0;
            cmbMonitor.SelectedIndexChanged += (s, e) =>
            {
                if (cmbMonitor.SelectedIndex >= 0)
                    MultiMonitorManager.PlaceOnMonitor(this, cmbMonitor.SelectedIndex);
            };
            layout.Controls.Add(cmbMonitor, 1, 4);

            var lblRetry = new Label { Text = "Reconnect Interval (ms):", ForeColor = Color.FromArgb(180, 180, 180), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            layout.Controls.Add(lblRetry, 0, 5);
            nudRetryInterval = new NumericUpDown
            {
                Dock = DockStyle.Fill,
                Minimum = 1000,
                Maximum = 60000,
                Value = reconnect.RetryIntervalMs,
                Increment = 1000,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            nudRetryInterval.ValueChanged += (s, e) => reconnect.RetryIntervalMs = (int)nudRetryInterval.Value;
            layout.Controls.Add(nudRetryInterval, 1, 5);

            var lblMax = new Label { Text = "Max Retries:", ForeColor = Color.FromArgb(180, 180, 180), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            layout.Controls.Add(lblMax, 0, 6);
            nudMaxRetries = new NumericUpDown
            {
                Dock = DockStyle.Fill,
                Minimum = 1,
                Maximum = 999,
                Value = reconnect.MaxRetries,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White
            };
            nudMaxRetries.ValueChanged += (s, e) => reconnect.MaxRetries = (int)nudMaxRetries.Value;
            layout.Controls.Add(nudMaxRetries, 1, 6);

            var lblJs = new Label { Text = "JS Injection Rules:", ForeColor = Color.FromArgb(100, 200, 255), Font = new Font("Segoe UI Semibold", 10F), Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            layout.Controls.Add(lblJs, 0, 7);
            layout.SetColumnSpan(lblJs, 2);

            lstJsRules = new ListBox
            {
                Dock = DockStyle.Fill,
                Height = 120,
                BackColor = Color.FromArgb(35, 35, 35),
                ForeColor = Color.FromArgb(200, 220, 255),
                Font = new Font("Consolas", 9F),
                BorderStyle = BorderStyle.None
            };
            layout.Controls.Add(lstJsRules, 0, 8);
            layout.SetColumnSpan(lstJsRules, 2);
            RefreshJsRulesList();

            var jsBtnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Height = 36,
                BackColor = Color.FromArgb(30, 30, 30)
            };
            var btnAddRule = MakeButton("+ Add Rule", Color.FromArgb(50, 130, 80));
            btnAddRule.Click += (s, e) => AddJsRule();
            var btnRemoveRule = MakeButton("- Remove", Color.FromArgb(140, 60, 60));
            btnRemoveRule.Click += (s, e) => RemoveSelectedJsRule();
            var btnToggleRule = MakeButton("Toggle", Color.FromArgb(60, 90, 140));
            btnToggleRule.Click += (s, e) => ToggleSelectedJsRule();
            jsBtnPanel.Controls.AddRange(new Control[] { btnAddRule, btnRemoveRule, btnToggleRule });
            layout.Controls.Add(jsBtnPanel, 0, 9);
            layout.SetColumnSpan(jsBtnPanel, 2);

            tab.Controls.Add(layout);
            return tab;
        }

        private void RefreshJsRulesList()
        {
            if (lstJsRules == null) return;
            lstJsRules.Items.Clear();
            foreach (var rule in jsEngine.Rules)
            {
                lstJsRules.Items.Add("[" + (rule.Enabled ? "ON" : "OFF") + "] " + rule.Name + " | " + rule.UrlPattern + " | " + rule.Script.Substring(0, Math.Min(40, rule.Script.Length)) + "...");
            }
        }

        private void AddJsRule()
        {
            var urlPattern = Microsoft.VisualBasic.Interaction.InputBox("URL Pattern (* for all):", "Add JS Rule", "*");
            if (string.IsNullOrEmpty(urlPattern)) return;
            var script = Microsoft.VisualBasic.Interaction.InputBox("JavaScript code:", "Add JS Rule", "console.log('injected');");
            if (string.IsNullOrEmpty(script)) return;
            var name = Microsoft.VisualBasic.Interaction.InputBox("Rule name:", "Add JS Rule", "Rule_" + (jsEngine.Rules.Count + 1));

            jsEngine.AddRule(urlPattern, script, name);
            RefreshJsRulesList();
            LogDebug("Added JS rule: " + name + " -> " + urlPattern);
        }

        private void RemoveSelectedJsRule()
        {
            if (lstJsRules.SelectedIndex < 0) return;
            var rulesList = jsEngine.Rules;
            if (lstJsRules.SelectedIndex < rulesList.Count)
            {
                jsEngine.RemoveRule(rulesList[lstJsRules.SelectedIndex].Id);
                RefreshJsRulesList();
            }
        }

        private void ToggleSelectedJsRule()
        {
            if (lstJsRules.SelectedIndex < 0) return;
            var rulesList = jsEngine.Rules;
            if (lstJsRules.SelectedIndex < rulesList.Count)
            {
                jsEngine.ToggleRule(rulesList[lstJsRules.SelectedIndex].Id);
                RefreshJsRulesList();
            }
        }

        #endregion

        #region Tab 5 — Resource Monitor

        private TabPage BuildResourceTab()
        {
            var tab = new TabPage("Resources") { BackColor = Color.FromArgb(30, 30, 30) };

            resOutput = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                BackColor = Color.FromArgb(15, 15, 15),
                ForeColor = Color.FromArgb(100, 255, 180),
                Font = new Font("Consolas", 10F),
                BorderStyle = BorderStyle.None
            };

            resTimer = new System.Windows.Forms.Timer { Interval = 2500 };
            resTimer.Tick += (s, e) =>
            {
                if (resMon != null && resMon.LatestSnapshot != null)
                    resOutput.Text = resMon.GetFormattedStatus() + "\r\n" + MultiMonitorManager.GetMonitorInfo();
            };
            resTimer.Start();

            var btnRefresh = MakeButton("Refresh Now", Color.FromArgb(60, 90, 140));
            btnRefresh.Dock = DockStyle.Bottom;
            btnRefresh.Click += (s, e) =>
            {
                if (resMon != null && resMon.LatestSnapshot != null)
                    resOutput.Text = resMon.GetFormattedStatus() + "\r\n" + MultiMonitorManager.GetMonitorInfo();
            };

            var btnProcesses = MakeButton("Process List", Color.FromArgb(80, 80, 120));
            btnProcesses.Dock = DockStyle.Bottom;
            btnProcesses.Click += (s, e) =>
            {
                var sb = new StringBuilder();
                sb.AppendLine("=== Process List ===");
                foreach (var proc in Process.GetProcesses().OrderByDescending(p => { try { return p.WorkingSet64; } catch { return 0; } }).Take(30))
                {
                    try { sb.AppendLine("  [" + proc.Id.ToString().PadLeft(6) + "] " + proc.ProcessName.PadRight(30) + " " + (proc.WorkingSet64 / (1024 * 1024)).ToString().PadLeft(6) + "MB"); }
                    catch { }
                }
                resOutput.Text = sb.ToString();
            };

            tab.Controls.Add(resOutput);
            tab.Controls.Add(btnProcesses);
            tab.Controls.Add(btnRefresh);
            return tab;
        }

        #endregion

        #region Wire Events

        private void WireEvents()
        {
            // All managers can be null if their init failed — null-safe wiring
            if (clipMgr != null)
            {
                clipMgr.EntryAdded += (entry) =>
                {
                    try { this.BeginInvoke(new Action(RefreshClipboardGrid)); } catch { }
                };
                clipMgr.HistoryCleared += () =>
                {
                    try { this.BeginInvoke(new Action(RefreshClipboardGrid)); } catch { }
                };
            }

            if (spoofer != null) spoofer.SpoofEvent += (msg) => LogDebug(msg, Color.FromArgb(255, 180, 100));
            if (ssGuard != null) ssGuard.LogEvent += (msg) => LogDebug(msg, Color.FromArgb(255, 100, 100));
            if (reconnect != null) reconnect.LogEvent += (msg) => LogDebug(msg, Color.FromArgb(100, 255, 180));
            if (jsEngine != null) jsEngine.LogEvent += (msg) => LogDebug(msg, Color.FromArgb(255, 220, 100));
            if (resMon != null) resMon.LogEvent += (msg) => LogDebug(msg, Color.FromArgb(180, 180, 255));
            BossKeyManager.LogEvent += (msg) => LogDebug(msg, Color.FromArgb(255, 150, 255));

            if (ssGuard != null) this.HandleCreated += (s, e) => ssGuard.ProtectWindow(this.Handle);
        }

        #endregion

        #region Helpers

        private void LogDebug(string msg, Color? color = null)
        {
            if (debugOutput == null) return;

            try
            {
                if (debugOutput.InvokeRequired)
                {
                    debugOutput.BeginInvoke(new Action(() => LogDebug(msg, color)));
                    return;
                }

                var c = color.HasValue ? color.Value : Color.FromArgb(180, 220, 180);
                debugOutput.SelectionStart = debugOutput.TextLength;
                debugOutput.SelectionLength = 0;
                debugOutput.SelectionColor = c;
                debugOutput.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + msg + "\r\n");
                debugOutput.ScrollToCaret();
            }
            catch { }

            try
            {
                File.AppendAllText(
                    Path.Combine(Path.GetTempPath(), "seb_panel_v8.log"),
                    "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + msg + "\n");
            }
            catch { }
        }

        private Button MakeButton(string text, Color backColor)
        {
            return new Button
            {
                Text = text,
                Width = 140,
                Height = 32,
                FlatStyle = FlatStyle.Flat,
                BackColor = backColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand,
                Margin = new Padding(3)
            };
        }

        private CheckBox MakeCheckBox(string text)
        {
            return new CheckBox
            {
                Text = text,
                ForeColor = Color.FromArgb(220, 220, 220),
                Font = new Font("Segoe UI", 10F),
                Dock = DockStyle.Fill,
                AutoSize = false,
                Height = 32
            };
        }

        #endregion
    }
}
