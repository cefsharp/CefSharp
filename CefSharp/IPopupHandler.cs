using System.Collections.Generic;

namespace CefSharp
{
    /// <summary>
    /// Handler methods that get called AFTER the Popup is created.
    /// </summary>
    public interface IPopupHandler
    {
        /// <summary>
        /// Called before the passed popup window (browser) is closed.
        /// </summary>
        /// <param name="browserControl">The IWebBrowser control this popup is related to.</param>
        /// <param name="browser">The popup window instance that is closing.</param>
        void OnBeforeClose(IWebBrowser browserControl, IBrowser browser);

        /// <summary>
        /// Called after the paased popup window (browser) is created.
        /// </summary>
        /// <param name="browserControl">The IWebBrowser control this popup is related to.</param>
        /// <param name="browser">The popup window instance that was just created.</param>
        void OnAfterCreated(IWebBrowser browserControl, IBrowser browser);

        void OnFaviconUrlChange(IWebBrowser browserControl, IBrowser browser, List<string> iconUrls);

        void OnLoadingStateChange(IWebBrowser browserControl, IBrowser browser, bool isLoading, bool canGoBack, bool canGoForward);

        void OnStatusMessage(IWebBrowser browserControl, IBrowser browser, string message);

        void OnFrameLoadStart(IWebBrowser browserControl, IBrowser browser, FrameLoadStartEventArgs frameLoadStartArgs);

        void OnFrameLoadEnd(IWebBrowser browserControl, IBrowser browser, FrameLoadEndEventArgs frameLoadEndArgs);

        /// <summary>
        /// Called when the resource load for a navigation fails or is canceled.
        /// |errorCode| is the error code number, |errorText| is the error text and
        /// |failedUrl| is the URL that failed to load. See net\base\net_error_list.h
        /// for complete descriptions of the error codes.
        /// </summary>
        void OnLoadError(IWebBrowser browserControl, IBrowser browser, LoadErrorEventArgs loadErrorArgs);

        #region KeyBoardHandler methods

        bool OnKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int code, CefEventFlags modifiers, bool isSystemKey);

        bool OnPreKeyEvent(IWebBrowser browserControl, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut);
        #endregion

        #region RequestHandler methods

        /// <summary>
        /// Called before browser navigation.
        /// If the navigation is allowed <see cref="IWebBrowser.FrameLoadStart"/> and <see cref="IWebBrowser.FrameLoadEnd"/>
        /// will be called. If the navigation is canceled <see cref="IWebBrowser.LoadError"/> will be called with an ErrorCode
        /// value of <see cref="CefErrorCode.Aborted"/>. 
        /// </summary>
        /// <param name="browserControl">the browser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="request">the request object - cannot be modified in this callback</param>
        /// <param name="isRedirect">has the request been redirected</param>
        /// <param name="frame">The frame the request is coming from</param>
        /// <returns>Return true to cancel the navigation or false to allow the navigation to proceed.</returns>
        bool OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IRequest request, bool isRedirect, IFrame frame);

        #endregion
    }
}
