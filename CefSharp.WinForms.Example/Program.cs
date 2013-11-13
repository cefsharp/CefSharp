using System;
using System.Windows.Forms;
using CefSharp.Example;

namespace CefSharp.WinForms.Example
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            ExamplePresenter.Init();

            var browser = new BrowserForm();
            Application.Run(browser);
        }
    }
}
