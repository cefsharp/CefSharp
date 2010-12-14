using System;
using System.Windows.Forms;
using CefSharp;

namespace CefSharp.Example
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Settings settings = new Settings();
            BrowserSettings browserSettings = new BrowserSettings();

            if(!CEF.Initialize(settings, browserSettings))
            {
                Console.WriteLine("Couldn't initialise CEF");
                return;
            }

            CEF.RegisterScheme("test", new TestSchemeHandlerFactory());
            CEF.RegisterJsObject("bound", new BoundObject());

            Browser browser = new Browser();
            Application.Run(browser);
        }

    }
}
