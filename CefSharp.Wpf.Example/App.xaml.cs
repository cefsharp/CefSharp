using System.Windows;
using CefSharp.Example;

namespace CefSharp.Wpf.Example
{
    public partial class App : Application
    {
        private App()
        {
            ExamplePresenter.Init();
        }
    }
}