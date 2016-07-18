using System;

namespace CefSharp.Wpf.Example
{
    class Startup
    {
        [STAThread]
        static void Main()
        {
            var dom = AppDomain.CreateDomain("sdsfdsdf");
            dom.DoCallBack(() =>
            {
                var app = new App();
                app.InitializeComponent();
                app.Run();
            });
        }
    }
}
