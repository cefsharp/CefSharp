// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows.Input;

namespace CefSharp.Wpf.HwndHost
{
    /// <summary>
    /// Focus Handler
    /// The methods of this class will be called on the CEF UI thread. 
    /// </summary>
    public class FocusHandler : IFocusHandler
    {
        void IFocusHandler.OnGotFocus(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            OnGotFocus(chromiumWebBrowser, browser);
        }

        /// <summary>
        /// Called when the browser component has received focus.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        protected virtual void OnGotFocus(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            
        }

        bool IFocusHandler.OnSetFocus(IWebBrowser chromiumWebBrowser, IBrowser browser, CefFocusSource source)
        {
            return OnSetFocus(chromiumWebBrowser, browser, source);
        }

        /// <summary>
        /// Called when the browser component is requesting focus.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object, do not keep a reference to this object outside of this method</param>
        /// <param name="source">Indicates where the focus request is originating from.</param>
        /// <returns>Return false to allow the focus to be set or true to cancel setting the focus.</returns>
        protected virtual bool OnSetFocus(IWebBrowser chromiumWebBrowser, IBrowser browser, CefFocusSource source)
        {
            //We don't deal with popups as they're rendered by default entirely by CEF
            if (browser.IsPopup)
            {
                return false;
            }
            // Do not let the browser take focus when a Load method has been called
            return source == CefFocusSource.FocusSourceNavigation;
        }


        
        void IFocusHandler.OnTakeFocus(IWebBrowser chromiumWebBrowser, IBrowser browser, bool next)
        {
            OnTakeFocus(chromiumWebBrowser, browser, next);
        }

        /// <summary>
        /// Called when the browser component is about to lose focus.
        /// For instance, if focus was on the last HTML element and the user pressed the TAB key.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">the browser object</param>
        /// <param name="next">Will be true if the browser is giving focus to the next component
        /// and false if the browser is giving focus to the previous component.</param>
        protected virtual void OnTakeFocus(IWebBrowser chromiumWebBrowser, IBrowser browser, bool next)
        {
            //We don't deal with popups as they're rendered by default entirely by CEF
            if (browser.IsPopup)
            {
                return;
            }

            var hwndChromiumWebBrowser = (ChromiumWebBrowser)chromiumWebBrowser;

            var request = new TraversalRequest(next ? FocusNavigationDirection.Next : FocusNavigationDirection.Previous);

            // NOTE: OnTakeFocus means leaving focus / not taking focus
            hwndChromiumWebBrowser.UiThreadRunAsync(() => hwndChromiumWebBrowser.MoveFocus(request));
        }
    }
}
