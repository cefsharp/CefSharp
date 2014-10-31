using System;
using System.Linq;

namespace CefSharp.Example
{
    public static class CefExample
    {
        public const string DefaultUrl = "custom://cefsharp/home";

        // Use when debugging the actual SubProcess, to make breakpoints etc. inside that project work.
        private const bool debuggingSubProcess = false;

        public static void Init()
        {
            var settings = new CefSettings();
            settings.RemoteDebuggingPort = 8088;
            settings.LogSeverity = LogSeverity.Verbose;

            if (debuggingSubProcess)
            {
                var platform = IntPtr.Size == 4 ? "x86" : "x64";
                settings.BrowserSubprocessPath = string.Format(
                    "..\\..\\..\\..\\CefSharp.BrowserSubprocess\\bin\\{0}\\Debug\\CefSharp.BrowserSubprocess.exe", platform);
            }

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = CefSharpSchemeHandlerFactory.SchemeName,
                SchemeHandlerFactory = new CefSharpSchemeHandlerFactory()
            });

            if (!Cef.Initialize(settings))
            {
                if (Environment.GetCommandLineArgs().Contains("--type=renderer"))
                {
                    Environment.Exit(0);
                }
                else
                {
                    return;
                }
            }

            Cef.RegisterJsObject("bound", new BoundObject());
        }
    }
}
