// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CefSharp.WinForms.Host;
using CefSharp.WinForms.Internals;

namespace CefSharp.WinForms.Handler
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
    public delegate PopupCreation OnBeforePopupCreatedDelegate(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IBrowserSettings browserSettings);

    /// <summary>
    /// Called when the <see cref="ChromiumHostControl"/> has been created.
    /// When called you must add the control to it's intended parent
    /// so the <see cref="Control.ClientRectangle"/> can be calculated to set the initial
    /// size correctly.
    /// </summary>
    /// <param name="control">popup host control</param>
    /// <param name="url">url</param>
    public delegate void OnPopupCreatedDelegate(ChromiumHostControl control, string url);

    /// <summary>
    /// Called when the <see cref="IBrowser"/> instance has been created.
    /// The <see cref="IBrowser"/> reference will be valid until <see cref="OnPopupDestroyedDelegate"/> is called
    /// </summary>
    /// <param name="control">popup host control, maybe null if Browser is hosted in a native Popup window.
    /// DevTools by default will be hosted in a native popup window.</param>
    /// <param name="browser">browser</param>
    public delegate void OnPopupBrowserCreatedDelegate(ChromiumHostControl control, IBrowser browser);

    /// <summary>
    /// Called when the <see cref="ChromiumHostControl"/> is to be removed from it's parent.
    /// When called you must remove/dispose of the <see cref="ChromiumHostControl"/>.
    /// </summary>
    /// <param name="control">popup host control</param>
    /// <param name="browser">browser</param>
    public delegate void OnPopupDestroyedDelegate(ChromiumHostControl control, IBrowser browser);

    /// <summary>
    /// Called to create a new instance of <see cref="ChromiumHostControl"/>. Allows creation of a derived
    /// implementation of <see cref="ChromiumHostControl"/>.
    /// </summary>
    /// <returns>A custom instance of <see cref="ChromiumHostControl"/>.</returns>
    public delegate ChromiumHostControl CreatePopupChromiumHostControl();

    /// <summary>
    /// A WinForms Specific <see cref="ILifeSpanHandler"/> implementation that simplifies
    /// the process of hosting a Popup as a Control/Tab.
    /// This <see cref="ILifeSpanHandler"/> implementation returns true in <see cref="ILifeSpanHandler.DoClose(IWebBrowser, IBrowser)"/>
    /// so no WM_CLOSE message is sent, this differs from the default CEF behaviour.
    /// </summary>
    public class LifeSpanHandler : CefSharp.Handler.LifeSpanHandler
    {
        private readonly Dictionary<int, ParentFormMessageInterceptor> popupParentFormMessageInterceptors = new Dictionary<int, ParentFormMessageInterceptor>();
        private OnBeforePopupCreatedDelegate onBeforePopupCreated;
        private OnPopupDestroyedDelegate onPopupDestroyed;
        private OnPopupBrowserCreatedDelegate onPopupBrowserCreated;
        private OnPopupCreatedDelegate onPopupCreated;
        private CreatePopupChromiumHostControl chromiumHostControlCreatedDelegate;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="chromiumHostControlCreatedDelegate">Optional delegate used to create custom <see cref="ChromiumHostControl"/> instances.</param>
        public LifeSpanHandler(CreatePopupChromiumHostControl chromiumHostControlCreatedDelegate = null)
        {
            this.chromiumHostControlCreatedDelegate = chromiumHostControlCreatedDelegate;
        }

        /// <inheritdoc/>
        protected override bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            if (browser.IsPopup)
            {
                var control = ChromiumHostControl.FromBrowser(browser);

                //We don't have a parent control so we allow the default behaviour, required to close
                //default popups e.g. DevTools
                if (control == null)
                {
                    return false;
                }

                //If the main browser is disposed or the handle has been released then we don't
                //need to remove the popup (likely removed from menu)
                if (!control.IsDisposed && control.IsHandleCreated)
                {
                    try
                    {
                        //We need to invoke in a sync fashion so our IBrowser object is still in scope
                        //Calling in an async fashion leads to the IBrowser being disposed before we
                        //can access it.
                        control.InvokeSyncOnUiThreadIfRequired(new Action(() =>
                        {
                            onPopupDestroyed?.Invoke(control, browser);

                            control.Dispose();
                        }));
                    }
                    catch (ObjectDisposedException)
                    {
                        // If the popup is being hosted on a Form that is being
                        // Closed/Disposed as we attempt to call Control.Invoke
                        // we can end up with an ObjectDisposedException
                        // return false (Default behaviour).
                        return false;
                    }
                }
            }

            //No WM_CLOSE message will be sent, manually handle closing
            return true;
        }

        /// <inheritdoc/>
        protected override void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            if (browser.IsPopup)
            {
                //WinForms will kindly lookup the child control from it's handle
                //If no parentControl then likely it's a native popup created by CEF
                //(Devtools by default will open as a popup, at this point the Url hasn't been set, so 
                // we're going with this assumption as it fits the use case currently)
                var control = ChromiumHostControl.FromBrowser(browser);

                //If control is null then we'll treat as a native popup (do nothing)
                //If control is disposed there's nothing for us to do either.
                if (control != null && !control.IsDisposed)
                {
                    control.BrowserHwnd = browser.GetHost().GetWindowHandle();

                    control.InvokeOnUiThreadIfRequired(() =>
                    {
                        var interceptor = new ParentFormMessageInterceptor(control);
                        interceptor.Moving += (sender, args) =>
                        {
                            if (!browser.IsDisposed)
                            {
                                browser?.GetHost()?.NotifyMoveOrResizeStarted();
                            }
                        };

                        popupParentFormMessageInterceptors.Add(browser.Identifier, interceptor);
                    });

                    control.BrowserCore = browser;
                    control.RaiseIsBrowserInitializedChangedEvent();
                }

                onPopupBrowserCreated?.Invoke(control, browser);
            }
        }

        /// <inheritdoc/>
        protected override void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            if (!browser.IsDisposed && browser.IsPopup)
            {
                ParentFormMessageInterceptor interceptor;

                if (popupParentFormMessageInterceptors.TryGetValue(browser.Identifier, out interceptor))
                {
                    popupParentFormMessageInterceptors.Remove(browser.Identifier);
                    interceptor?.Dispose();
                }
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// NOTE: DevTools popups DO NOT trigger OnBeforePopup.
        /// </remarks>
        protected override bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            newBrowser = null;

            PopupCreation userAction = onBeforePopupCreated?.Invoke(chromiumWebBrowser, browser, frame, targetUrl, targetFrameName, targetDisposition, userGesture, browserSettings) ?? PopupCreation.Continue;

            //Cancel popup creation
            if(userAction == PopupCreation.Cancel)
            {
                return true;
            }

            if(userAction == PopupCreation.ContinueWithJavascriptDisabled)
            {
                noJavascriptAccess = true;
            }

            //No action so we'll go with the default behaviour.
            if (onPopupCreated == null)
            {
                return false;
            }

            var webBrowser = (ChromiumWebBrowser)chromiumWebBrowser;

            //Load and Display Handlers are used to trigger the relevant events.
            //If they are already assigned we'll leave the user preference in place
            if (webBrowser.LoadHandler == null)
            {
                webBrowser.LoadHandler = new LoadHandler();
            }
            if (webBrowser.DisplayHandler == null)
            {
                webBrowser.DisplayHandler = new DisplayHandler();
            }

            //We need to execute sync here so IWindowInfo.SetAsChild is called before we return false;
            webBrowser.InvokeSyncOnUiThreadIfRequired(new Action(() =>
            {
                ChromiumHostControl control = chromiumHostControlCreatedDelegate?.Invoke();

                if (control == null)
                {
                    control = new ChromiumHostControl
                    {
                        Dock = DockStyle.Fill
                    };
                }
                control.CreateControl();

                onPopupCreated?.Invoke(control, targetUrl);

                var rect = control.ClientRectangle;

                var windowBounds = new CefSharp.Structs.Rect(rect.X, rect.Y, rect.Width, rect.Height);

                windowInfo.SetAsChild(control.Handle, windowBounds);
            }));

            return false;
        }

        /// <summary>
        /// The <see cref="OnBeforePopupCreatedDelegate"/> will be called <b>before</b> the popup has been created and
        /// can be used to cancel popup creation if required or modify <see cref="IBrowserSettings"/>.
        /// </summary>
        /// <param name="onBeforePopupCreated">Action to be invoked before popup is created.</param>
        /// <returns><see cref="LifeSpanHandler"/> instance allowing you to chain method calls together</returns>
        public LifeSpanHandler OnBeforePopupCreated(OnBeforePopupCreatedDelegate onBeforePopupCreated)
        {
            this.onBeforePopupCreated = onBeforePopupCreated;

            return this;
        }

        /// <summary>
        /// The <see cref="OnPopupCreatedDelegate"/> will be called when the<see cref="ChromiumHostControl"/> has been
        /// created. When the <see cref="OnPopupCreatedDelegate"/> is called you must add the control to it's intended parent
        /// so the <see cref="Control.ClientRectangle"/> can be calculated to set the initial
        /// size correctly.
        /// </summary>
        /// <param name="onPopupCreated">Action to be invoked when the Popup host has been created and is ready to be attached to it's parent.</param>
        /// <returns><see cref="LifeSpanHandler"/> instance allowing you to chain method calls together</returns>
        public LifeSpanHandler OnPopupCreated(OnPopupCreatedDelegate onPopupCreated)
        {
            this.onPopupCreated = onPopupCreated;

            return this;
        }

        /// <summary>
        /// The <see cref="OnPopupBrowserCreatedDelegate"/> will be called when the<see cref="IBrowser"/> has been
        /// created. The <see cref="IBrowser"/> instance is valid until <see cref="OnPopupDestroyed(OnPopupDestroyedDelegate)"/>
        /// is called. <see cref="IBrowser"/> provides low level access to the CEF Browser, you can access frames, view source,
        /// perform navigation (via frame) etc.
        /// </summary>
        /// <param name="onPopupBrowserCreated">Action to be invoked when the <see cref="IBrowser"/> has been created.</param>
        /// <returns><see cref="LifeSpanHandler"/> instance allowing you to chain method calls together</returns>
        public LifeSpanHandler OnPopupBrowserCreated(OnPopupBrowserCreatedDelegate onPopupBrowserCreated)
        {
            this.onPopupBrowserCreated = onPopupBrowserCreated;

            return this;
        }

        /// <summary>
        /// The <see cref="OnPopupDestroyedDelegate"/> will be called when the <see cref="ChromiumHostControl"/> is to be
        /// removed from it's parent.
        /// When the <see cref="OnPopupDestroyedDelegate"/> is called you must remove/dispose of the <see cref="ChromiumHostControl"/>.
        /// </summary>
        /// <param name="onPopupDestroyed">Action to be invoked when the Popup is to be destroyed.</param>
        /// <returns><see cref="LifeSpanHandler"/> instance allowing you to chain method calls together</returns>
        public LifeSpanHandler OnPopupDestroyed(OnPopupDestroyedDelegate onPopupDestroyed)
        {
            this.onPopupDestroyed = onPopupDestroyed;

            return this;
        }

        /// <summary>
        /// Create a new instance of the <see cref="LifeSpanHandlerBuilder"/>
        /// which can be used to create a WinForms specific <see cref="ILifeSpanHandler"/>
        /// implementation that simplifies the process of hosting a Popup as a Control/Tab.
        /// In scnarios where you also need to implement <see cref="ILoadHandler"/> then instead
        /// of implementing directly you will need to inherit from <see cref="CefSharp.WinForms.Handler.LoadHandler"/>.
        /// As it provides base functionality required to make <see cref="ChromiumHostControl"/> events work correctly.
        /// </summary>
        /// <returns>
        /// A <see cref="LifeSpanHandlerBuilder"/> which can be used to fluently create an <see cref="ILifeSpanHandler"/>.
        /// Call <see cref="LifeSpanHandlerBuilder.Build"/> to create the actual instance after you have call
        /// <see cref="LifeSpanHandlerBuilder.OnPopupCreated(OnPopupCreatedDelegate)"/> etc.
        /// </returns>
        public static LifeSpanHandlerBuilder Create(CreatePopupChromiumHostControl chromiumHostControlCreatedDelegate = null)
        {
            return new LifeSpanHandlerBuilder(chromiumHostControlCreatedDelegate);
        }
    }
}
