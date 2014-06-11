using System;

namespace CefSharp
{
    /// <summary>
    /// Event arguments to the LoadError event handler set up in IWebBrowser.
    /// </summary>
    public class LoadErrorEventArgs : EventArgs
    {
        public LoadErrorEventArgs(string failedUrl, CefErrorCode errorCode, string errorText)
        {
            FailedUrl = failedUrl;
            ErrorCode = errorCode;
            ErrorText = errorText;
        }

        /// <summary>
        /// The URL that failed to load.
        /// </summary>
        public string FailedUrl { get; private set; }

        /// <summary>
        /// The error code.
        /// </summary>
        public CefErrorCode ErrorCode { get; private set; }

        /// <summary>
        /// The error text.
        /// </summary>
        public string ErrorText { get; private set; }
    }
}
