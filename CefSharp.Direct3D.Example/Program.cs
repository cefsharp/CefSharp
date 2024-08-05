using System;
using System.IO;
using System.Windows.Forms;
using CefSharp;
using CefSharp.OffScreen;

namespace DirectX
{
    internal static class Program
    {


        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!CefSharp.Cef.IsInitialized)
            {
                // Monitor parent process exit and close subprocesses if parent process exits first
                CefSharpSettings.SubprocessExitIfParentProcessClosed = true;

                CefSettings settings = new CefSettings();
                //settings.SetOffScreenRenderingBestPerformanceArgs();
                settings.EnableAudio();
                settings.MultiThreadedMessageLoop = true;
                settings.CachePath = Path.GetFullPath("cache");
                settings.IgnoreCertificateErrors = true; // Doesn't work. RequestHandler is still required.
                settings.CefCommandLineArgs.Add("allow-running-insecure-content");
                settings.PersistSessionCookies = true;
                settings.CefCommandLineArgs.Add("disable-web-security");
                settings.CefCommandLineArgs["touch-events"] = "enabled";
                settings.CefCommandLineArgs["autoplay-policy"] = "no-user-gesture-required";
                settings.CefCommandLineArgs["disable-features"] = "SameSiteByDefaultCookies";

                //CefSharp.Cef.EnableHighDPISupport();

                CefSharp.Cef.Initialize(settings);

                //CefSharp.Cef.GetGlobalCookieManager().SetStoragePath(Path.Combine(settings.CachePath, "Cookies"), true);
            }

            Application.Run(new DXForm());
        }
    }
}
