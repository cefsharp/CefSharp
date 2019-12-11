using System.IO;
using System.Windows;

namespace CefSharp.Wpf.HwndHost.Example
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var settings = new CefSettings();
            settings.BrowserSubprocessPath = Path.GetFullPath(@"x86\CefSharp.BrowserSubprocess.exe");

            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            Cef.Shutdown();
        }
    }
}
