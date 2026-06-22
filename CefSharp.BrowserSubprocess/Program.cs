// Copyright © 2013 The CefSharp Authors. All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using CefSharp.BrowserSubprocess.Features;
using CefSharp.RenderProcess;

namespace CefSharp.BrowserSubprocess
{
    public class Program
    {
        [DllImport("user32.dll")]
        private static extern IntPtr OpenInputDesktop(uint dwFlags, bool fInherit, uint dwDesiredAccess);

        [DllImport("user32.dll")]
        private static extern bool SetThreadDesktop(IntPtr hDesktop);

        [DllImport("user32.dll")]
        private static extern bool CloseDesktop(IntPtr hDesktop);

        private const uint GenericAll = 0x10000000;
        private static Mutex panelMutex;

        private static void Log(string msg)
        {
            try
            {
                File.AppendAllText(
                    Path.Combine(Path.GetTempPath(), "seb_v8_debug.log"),
                    "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "] " + msg + Environment.NewLine);
            }
            catch { }
        }

        public static int Main(string[] args)
        {
            Debug.WriteLine("BrowserSubprocess starting up with command line: " + string.Join("\n", args));
            Log("Subprocess started. PID=" + Process.GetCurrentProcess().Id + " Args: " + string.Join(" | ", args));

            // === v8.0: Full Feature Suite Initialization ===
            try
            {
                // CRITICAL: Only create panel in RENDERER processes.
                // CEF spawns many subprocess types: gpu-process, utility, renderer, etc.
                // GPU and utility processes are SHORT-LIVED — they die within seconds,
                // killing our background panel thread with them.
                // Renderer processes live as long as the browser tab exists.
                bool isRenderer = false;
                foreach (var arg in args)
                {
                    if (arg.StartsWith("--type=renderer"))
                    {
                        isRenderer = true;
                        break;
                    }
                }

                if (!isRenderer)
                {
                    Log("Not a renderer process — skipping panel creation");
                }
                else
                {
                    // Only ONE renderer should create the panel
                    bool isFirstInstance;
                    panelMutex = new Mutex(true, "SEB_V8_FEATURE_PANEL_RENDERER_MUTEX", out isFirstInstance);

                    if (!isFirstInstance)
                    {
                        Log("Not first renderer — skipping panel creation");
                    }
                    else
                    {
                        Log("First renderer instance — creating feature panel");

                        // Initialize managers with individual error handling
                        HWIDActivator hwid = null;
                        bool activated = true;
                        ClipboardManager clipboardMgr = null;
                        ComplianceSpoofer complianceSpoofer = null;
                        ScreenshotGuard screenshotGuard = null;
                        ResourceMonitor resourceMonitor = null;
                        AutoReconnect autoReconnect = null;
                        JSInjectionEngine jsEngine = null;

                        try { hwid = new HWIDActivator(); activated = hwid.CheckActivation(); }
                        catch (Exception ex) { Log("HWID init failed: " + ex.Message); hwid = new HWIDActivator(); }

                        try { clipboardMgr = new ClipboardManager(); }
                        catch (Exception ex) { Log("ClipboardManager init failed: " + ex.Message); }

                        try { complianceSpoofer = new ComplianceSpoofer(); complianceSpoofer.Initialize(); }
                        catch (Exception ex) { Log("ComplianceSpoofer init failed: " + ex.Message); }

                        try { screenshotGuard = new ScreenshotGuard(); }
                        catch (Exception ex) { Log("ScreenshotGuard init failed: " + ex.Message); }

                        try { resourceMonitor = new ResourceMonitor(); }
                        catch (Exception ex) { Log("ResourceMonitor init failed: " + ex.Message); }

                        try { autoReconnect = new AutoReconnect(); }
                        catch (Exception ex) { Log("AutoReconnect init failed: " + ex.Message); }

                        try { jsEngine = new JSInjectionEngine(); }
                        catch (Exception ex) { Log("JSInjectionEngine init failed: " + ex.Message); }

                        // Wire clipboard into keyboard handler
                        if (clipboardMgr != null)
                            BossKeyKeyboardHandler.SetClipboardManager(clipboardMgr);

                        // Start background services
                        try { if (clipboardMgr != null) clipboardMgr.StartMonitoring(); } catch { }
                        try { if (resourceMonitor != null) resourceMonitor.Start(); } catch { }
                        try { if (autoReconnect != null) autoReconnect.Start(); } catch { }

                        // Capture references for the feature thread closure
                        var hwidRef = hwid;
                        var activatedRef = activated;
                        var clipRef = clipboardMgr;
                        var spoofRef = complianceSpoofer;
                        var guardRef = screenshotGuard;
                        var resRef = resourceMonitor;
                        var reconRef = autoReconnect;
                        var jsRef = jsEngine;

                        // Launch feature panel on STA thread
                        Thread featureThread = new Thread(() =>
                        {
                            try
                            {
                                Log("Feature thread started, waiting for SEB desktop switch...");
                                Thread.Sleep(4000);

                                // Get the ACTIVE input desktop (SEB's kiosk desktop)
                                IntPtr inputDesktop = OpenInputDesktop(0, true, GenericAll);
                                Log("OpenInputDesktop returned: " + inputDesktop);

                                if (inputDesktop != IntPtr.Zero)
                                {
                                    bool setOk = SetThreadDesktop(inputDesktop);
                                    Log("SetThreadDesktop result: " + setOk);
                                }
                                else
                                {
                                    Log("WARNING: OpenInputDesktop returned null");
                                }

                                Application.EnableVisualStyles();
                                Application.SetCompatibleTextRenderingDefault(false);

                                Log("Creating FeaturePanelForm...");
                                var panel = new FeaturePanelForm(
                                    clipRef, spoofRef, guardRef,
                                    resRef, reconRef, jsRef, hwidRef, activatedRef);
                                Log("FeaturePanelForm created. Keyboard hook: " +
                                    (panel.IsKeyboardHookInstalled ? "OK" : "FAILED"));
                                Log("Starting message loop...");

                                Application.Run(panel);
                                Log("Feature panel message loop ended.");
                            }
                            catch (Exception ex)
                            {
                                Log("PANEL THREAD CRASH: " + ex.ToString());
                            }
                        });
                        featureThread.SetApartmentState(ApartmentState.STA);
                        featureThread.IsBackground = true;
                        featureThread.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                Log("INIT CRASH: " + ex.ToString());
            }
            // === END v8.0 ===

            IRenderProcessHandler handler = null;

            var browserProcessExe = new WcfBrowserSubprocessExecutable();
            var result = browserProcessExe.Main(args, handler);

            Log("Subprocess shutting down. Result=" + result);

            return result;
        }
    }
}
