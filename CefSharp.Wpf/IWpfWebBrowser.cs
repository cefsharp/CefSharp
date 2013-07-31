using System.Windows.Input;

namespace CefSharp.Wpf
{
    public interface IWpfWebBrowser : IWebBrowser
    {
        /// <summary>
        /// Command which navigates to the previous page in the browser history. Will automatically be enabled/disabled depending
        /// on the availability of the command.
        /// </summary>
        ICommand BackCommand { get; }

        /// <summary>
        /// Command which navigates to the next page in the browser history. Will automatically be enabled/disabled depending on
        /// the availability of the command.
        /// </summary>
        ICommand ForwardCommand { get; }
    }
}
