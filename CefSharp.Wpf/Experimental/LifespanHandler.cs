// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

namespace CefSharp.Wpf.Experimental
{
    /// <summary>
    /// Called <b>before</b>the popup is created, can be used to cancel popup creation if required
    /// or modify <see cref="IBrowserSettings"/>.
    /// It's important to note that the methods of this interface are called on a CEF UI thread,
    /// which by default is not the same as your application UI thread.
    /// </summary>
    /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
    /// <param name="browser">The browser instance that launched this popup.</param>
    /// <param name="frame">The HTML frame that launched this popup.</param>
    /// <param name="targetUrl">The URL of the popup content. (This may be empty/null)</param>
    /// <param name="targetFrameName">The name of the popup. (This may be empty/null)</param>
    /// <param name="targetDisposition">The value indicates where the user intended to
    /// open the popup (e.g. current tab, new tab, etc)</param>
    /// <param name="userGesture">The value will be true if the popup was opened via explicit user gesture
    /// (e.g. clicking a link) or false if the popup opened automatically (e.g. via the DomContentLoaded event).</param>
    /// <param name="browserSettings">browser settings, defaults to source browsers</param>
    /// <returns>To cancel creation of the popup return true otherwise return false.</returns>
    public delegate PopupCreation LifeSpanHandlerOnBeforePopupCreatedDelegate(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IBrowserSettings browserSettings);

    /// <summary>
    /// Called when the <see cref="ChromiumWebBrowser"/> has been created.
    /// When called you must add the control to it's intended parent.
    /// </summary>
    /// <param name="control">popup host control</param>
    /// <param name="url">url</param>
    /// <param name="targetFrameName">target frame name</param>
    /// <param name="windowInfo">WindowInfo</param>
    public delegate void LifeSpanHandlerOnPopupCreatedDelegate(ChromiumWebBrowser control, string url, string targetFrameName, IWindowInfo windowInfo);

    /// <summary>
    /// Called when the <see cref="IBrowser"/> instance has been created.
    /// The <see cref="IBrowser"/> reference will be valid until <see cref="LifeSpanHandlerOnPopupDestroyedDelegate"/> is called
    /// </summary>
    /// <param name="control">popup ChromiumWebBrowser control, maybe null if Browser is hosted in a native Popup window.
    /// DevTools by default will be hosted in a native popup window.</param>
    /// <param name="browser">browser</param>
    public delegate void LifeSpanHandlerOnPopupBrowserCreatedDelegate(ChromiumWebBrowser control, IBrowser browser);

    /// <summary>
    /// Called when the <see cref="ChromiumWebBrowser"/> is to be removed from it's parent.
    /// When called you must remove/dispose of the <see cref="ChromiumWebBrowser"/>.
    /// </summary>
    /// <param name="control">popup ChromiumWebBrowser control</param>
    /// <param name="browser">browser</param>
    public delegate void LifeSpanHandlerOnPopupDestroyedDelegate(ChromiumWebBrowser control, IBrowser browser);

    /// <summary>
    /// Called to create a new instance of <see cref="ChromiumWebBrowser"/>. Allows creation of a derived/custom
    /// implementation of <see cref="ChromiumWebBrowser"/>.
    /// </summary>
    /// <returns>A custom instance of <see cref="ChromiumWebBrowser"/>.</returns>
    public delegate ChromiumWebBrowser LifeSpanHandlerCreatePopupChromiumWebBrowser();

    /// <summary>
    /// WPF - EXPERIMENTAL LifeSpanHandler implementation that can be used to host a popup using a new <see cref="ChromiumWebBrowser"/> instance.
    /// </summary>
    public class LifeSpanHandler : CefSharp.Handler.LifeSpanHandler
    {
        private LifeSpanHandlerOnBeforePopupCreatedDelegate onBeforePopupCreated;
        private LifeSpanHandlerOnPopupDestroyedDelegate onPopupDestroyed;
        private LifeSpanHandlerOnPopupBrowserCreatedDelegate onPopupBrowserCreated;
        private LifeSpanHandlerOnPopupCreatedDelegate onPopupCreated;
        private LifeSpanHandlerCreatePopupChromiumWebBrowser chromiumWebBrowserCreatedDelegate;
        private Dictionary<IntPtr, ChromiumWebBrowser> chromiumWebBrowserMap = new Dictionary<IntPtr, ChromiumWebBrowser>();
        private ChromiumWebBrowser pendingChromiumWebBrowser;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="chromiumWebBrowserCreatedDelegate">optional delegate to create a custom <see cref="ChromiumWebBrowser" /> instance.</param>
        public LifeSpanHandler(LifeSpanHandlerCreatePopupChromiumWebBrowser chromiumWebBrowserCreatedDelegate = null)
        {
            this.chromiumWebBrowserCreatedDelegate = chromiumWebBrowserCreatedDelegate;
        }

        /// <summary>
        /// The <see cref="LifeSpanHandlerOnBeforePopupCreatedDelegate"/> will be called <b>before</b> the popup has been created and
        /// can be used to cancel popup creation if required or modify <see cref="IBrowserSettings"/>.
        /// </summary>
        /// <param name="onBeforePopupCreated">Action to be invoked before popup is created.</param>
        /// <returns><see cref="LifeSpanHandler"/> instance allowing you to chain method calls together</returns>
        public LifeSpanHandler OnBeforePopupCreated(LifeSpanHandlerOnBeforePopupCreatedDelegate onBeforePopupCreated)
        {
            this.onBeforePopupCreated = onBeforePopupCreated;

            return this;
        }

        /// <summary>
        /// The <see cref="LifeSpanHandlerOnPopupCreatedDelegate"/> will be called when the<see cref="ChromiumWebBrowser"/> has been
        /// created. When the <see cref="LifeSpanHandlerOnPopupCreatedDelegate"/> is called you must add the control to it's intended parent.
        /// </summary>
        /// <param name="onPopupCreated">Action to be invoked when the Popup host has been created and is ready to be attached to it's parent.</param>
        /// <returns><see cref="LifeSpanHandler"/> instance allowing you to chain method calls together</returns>
        public LifeSpanHandler OnPopupCreated(LifeSpanHandlerOnPopupCreatedDelegate onPopupCreated)
        {
            this.onPopupCreated = onPopupCreated;

            return this;
        }

        /// <summary>
        /// The <see cref="LifeSpanHandlerOnPopupBrowserCreatedDelegate"/> will be called when the<see cref="IBrowser"/> has been
        /// created. The <see cref="IBrowser"/> instance is valid until <see cref="OnPopupDestroyed(LifeSpanHandlerOnPopupDestroyedDelegate)"/>
        /// is called. <see cref="IBrowser"/> provides low level access to the CEF Browser, you can access frames, view source,
        /// perform navigation (via frame) etc.
        /// </summary>
        /// <param name="onPopupBrowserCreated">Action to be invoked when the <see cref="IBrowser"/> has been created.</param>
        /// <returns><see cref="LifeSpanHandler"/> instance allowing you to chain method calls together</returns>
        public LifeSpanHandler OnPopupBrowserCreated(LifeSpanHandlerOnPopupBrowserCreatedDelegate onPopupBrowserCreated)
        {
            this.onPopupBrowserCreated = onPopupBrowserCreated;

            return this;
        }

        /// <summary>
        /// The <see cref="LifeSpanHandlerOnPopupDestroyedDelegate"/> will be called when the <see cref="ChromiumWebBrowser"/> is to be
        /// removed from it's parent.
        /// When the <see cref="LifeSpanHandlerOnPopupDestroyedDelegate"/> is called you must remove/dispose of the <see cref="ChromiumWebBrowser"/>.
        /// </summary>
        /// <param name="onPopupDestroyed">Action to be invoked when the Popup is to be destroyed.</param>
        /// <returns><see cref="LifeSpanHandler"/> instance allowing you to chain method calls together</returns>
        public LifeSpanHandler OnPopupDestroyed(LifeSpanHandlerOnPopupDestroyedDelegate onPopupDestroyed)
        {
            this.onPopupDestroyed = onPopupDestroyed;

            return this;
        }

        /// <summary>
        /// Create a new instance of the <see cref="LifeSpanHandlerBuilder"/>
        /// which can be used to create a WinForms specific <see cref="ILifeSpanHandler"/>
        /// implementation that simplifies the process of hosting a Popup as a Control/Tab.
        /// </summary>
        /// <returns>
        /// A <see cref="LifeSpanHandlerBuilder"/> which can be used to fluently create an <see cref="ILifeSpanHandler"/>.
        /// Call <see cref="LifeSpanHandlerBuilder.Build"/> to create the actual instance after you have call
        /// <see cref="LifeSpanHandlerBuilder.OnPopupCreated(LifeSpanHandlerOnPopupCreatedDelegate)"/> etc.
        /// </returns>
        public static LifeSpanHandlerBuilder Create(LifeSpanHandlerCreatePopupChromiumWebBrowser chromiumWebBrowserCreatedDelegate = null)
        {
            return new LifeSpanHandlerBuilder(chromiumWebBrowserCreatedDelegate);
        }

        /// <inheritdoc/>
        protected override bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            newBrowser = null;

            var userAction = onBeforePopupCreated?.Invoke(browserControl, browser, frame, targetUrl, targetFrameName, targetDisposition, userGesture, browserSettings) ?? PopupCreation.Continue;

            // Cancel popup creation
            if (userAction == PopupCreation.Cancel)
            {
                return true;
            }

            if (userAction == PopupCreation.ContinueWithJavascriptDisabled)
            {
                noJavascriptAccess = true;
            }

            //No action so we'll go with the default behaviour.
            if (onPopupCreated == null)
            {
                return false;
            }

            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;

            ChromiumWebBrowser control = null;

            // Invoke onto the WPF UI Thread to create a new ChromiumWebBrowser
            chromiumWebBrowser.Dispatcher.Invoke(() =>
            {
                control = chromiumWebBrowserCreatedDelegate?.Invoke();

                if (control == null)
                {
                    control = new ChromiumWebBrowser();
                }

                control.SetAsPopup();
                control.LifeSpanHandler = this;

                // Current assumption is that popups are created in sequence
                pendingChromiumWebBrowser = control;

                onPopupCreated?.Invoke(control, targetUrl, targetFrameName, windowInfo);

                var owner = System.Windows.Window.GetWindow(control);

                if (owner == null)
                {
                    windowInfo.SetAsWindowless(IntPtr.Zero);
                }
                else
                {
                    var windowInteropHelper = new System.Windows.Interop.WindowInteropHelper(owner);
                    //Ensure the Window handle has been created (In WPF there's only one handle per window, not per control)
                    var handle = windowInteropHelper.EnsureHandle();

                    //The parentHandle value will be used to identify monitor info and to act as the parent window for dialogs,
                    //context menus, etc. If parentHandle is not provided then the main screen monitor will be used and some
                    //functionality that requires a parent window may not function correctly.
                    windowInfo.SetAsWindowless(parentHandle: handle);
                }
            });

            newBrowser = control;

            return false;
        }

        /// <inheritdoc/>
        protected override void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {
            // For DevTools/Native Popup windows pendingChromiumWebBrowser should be null
            if (browser.IsPopup && pendingChromiumWebBrowser != null)
            {
                chromiumWebBrowserMap.Add(browser.GetHost().GetWindowHandle(), pendingChromiumWebBrowser);

                onPopupBrowserCreated?.Invoke(pendingChromiumWebBrowser, browser);

                pendingChromiumWebBrowser = null;
            }
        }

        /// <inheritdoc/>
        protected override bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            if (browser.IsPopup && !browser.IsDisposed)
            {
                var handle = browser.GetHost().GetWindowHandle();

                if (chromiumWebBrowserMap.TryGetValue(handle, out ChromiumWebBrowser control))
                {
                    // Control is null, use default behaviour
                    if (control == null)
                    {
                        return false;
                    }

                    if (!control.Dispatcher.HasShutdownStarted)
                    {
                        //We need to invoke in a sync fashion so our IBrowser object is still in scope
                        //Calling in an async fashion leads to the IBrowser being disposed before we
                        //can access it.
                        control.Dispatcher.Invoke(new Action(() =>
                        {
                            onPopupDestroyed?.Invoke(control, browser);

                            if (!control.IsDisposed)
                            {
                                control.Dispose();
                            }
                        }));
                    }

                    chromiumWebBrowserMap.Remove(handle);

                    return true;
                }
            }

            // We didn't find the control, so we use default behaviour
            // default popups e.g. DevTools need to return false
            // so WM_CLOSE is sent to close the window.
            return false;
        }
    }
}
