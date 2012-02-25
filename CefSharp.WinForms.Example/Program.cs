using System;
using System.Windows.Forms;
using CefSharp.Example;

namespace CefSharp.WinForms.Example
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ExamplePresenter.Init();

            Browser browser = new Browser();
            Application.Run(browser);
        }
    }
}
