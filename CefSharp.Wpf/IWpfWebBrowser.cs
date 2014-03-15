using System.ComponentModel;
using System.Windows.Input;

namespace CefSharp.Wpf
{
    public interface IWpfWebBrowser : IWebBrowser
    {
        /// <summary>
        /// Command which navigates to the previous page in the browser history. Will automatically be enabled/disabled depending
        /// on the browser state.
        /// </summary>
        ICommand BackCommand { get; }

        /// <summary>
        /// Command which navigates to the next page in the browser history. Will automatically be enabled/disabled depending on
        /// the browser state.
        /// </summary>
        ICommand ForwardCommand { get; }

        /// <summary>
        /// Command which reloads the content of the current page. Will automatically be enabled/disabled depending on the
        /// browser state.
        /// </summary>
        ICommand ReloadCommand { get; }

        /// <summary>
        /// Opens up a new program window (using the default text editor) where the source code of the currently displayed web
        /// page is shown.
        /// </summary>
        void ViewSource();

        /// <summary>
        /// Attempts to give focus to the WebBrowser control.
        /// </summary>
        /// <returns><c>true</c> if keyboard focus and logical focus were set to this element; <c>false</c> if only logical focus
        /// was set to this element, or if the call to this method did not force the focus to change.</returns>
        bool Focus();
    }
}
