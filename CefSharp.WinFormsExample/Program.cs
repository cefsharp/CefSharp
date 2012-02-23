using System;
using System.Windows.Forms;
using CefSharp;

namespace CefSharp.WinForms.Example
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Settings settings = new Settings();

            if(!CEF.Initialize(settings))
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
