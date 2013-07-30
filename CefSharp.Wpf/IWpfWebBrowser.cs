using System.Windows.Input;

namespace CefSharp.Wpf
{
    public interface IWpfWebBrowser : IWebBrowser
    {
        ICommand BackCommand { get; }
        ICommand ForwardCommand { get; }
    }
}
