using System;
using System.Windows.Forms;
using CefSharp;

namespace CefTest
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

            Browser browser = new Browser();
            Application.Run(browser);
        }

    }
}
