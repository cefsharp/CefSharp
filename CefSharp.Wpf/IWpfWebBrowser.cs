using System.ComponentModel;
using System.Windows.Input;

namespace CefSharp.Wpf
{
    public interface IWpfWebBrowser : IWebBrowser, INotifyPropertyChanged
    {
        /// <summary>
        /// The address (URL) which the browser control is currently displaying. Can be set to a simplified URL
        /// (e.g. www.google.com) or a full URL (e.g. http://www.google.com). Will automatically be updated as the user
        /// navigates to another page (e.g. by clickig on a link).
        /// </summary>
        /// <remarks>This property is a Dependency Property and fully supports data binding.</remarks>
        string Address { get; set; }

        /// <summary>
        /// A flag that indicates whether the control is currently loading one or more web pages (true) or not (false).
        /// </summary>
        /// <remarks>This property is a Dependency Property and fully supports data binding.</remarks>
        bool IsLoading { get; set; }

        /// <summary>
        /// The title of the web page being currently displayed.
        /// </summary>
        /// <remarks>This property is a Dependency Property and fully supports data binding.</remarks>
        string Title { get; }

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
