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
    /// A WinForms Specific <see cref="ILifeSpanHandler"/> implementation that simplifies
    /// the process of hosting a Popup as a Control/Tab.
    /// </summary>
    public class LifeSpanHandler : CefSharp.Handler.LifeSpanHandler
    {
        private readonly Dictionary<int, ParentFormMessageInterceptor> popupParentFormMessageInterceptors = new Dictionary<int, ParentFormMessageInterceptor>();
        //TODO: Do we use ChromiumHostControl for this type instead of Control?
        private Action<ChromiumHostControl, IBrowser> onPopupDestroyed;
        private Action<ChromiumHostControl, string> onPopupCreated;
        
        /// <inheritdoc/>
        protected override bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            if (browser.IsPopup)
            {
                var windowHandle = browser.GetHost().GetWindowHandle();
                var control = Control.FromChildHandle(windowHandle) as ChromiumHostControl;

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
                    //We need to invoke in a sync fashion so our IBrowser object is still in scope
                    //Calling in an async fashion leads to the IBrowser being disposed before we
                    //can access it.
                    control.InvokeSyncOnUiThreadIfRequired(new Action(() =>
                    {
                        onPopupDestroyed?.Invoke(control, browser);
                    }));
                }
            }

            //No WM_CLOSE message will be sent, manually handle closing
            return true;
        }

        /// <summary>
        /// Register an Action that will be called when the Popup Host Control is to be
        /// removed from it's parent.
        /// When the Action is called you must remove/dispose of the Popup Host Control.
        /// </summary>
        /// <param name="onPopupDestroyed">Action to be invoked when the Popup is to be destroyed.</param>
        /// <returns><see cref="LifeSpanHandler"/> instance allowing you to chain method calls together</returns>
        public LifeSpanHandler RegisterPopupDestroyed(Action<ChromiumHostControl, IBrowser> onPopupDestroyed)
        {
            this.onPopupDestroyed = onPopupDestroyed;

            return this;
        }

        /// <inheritdoc/>
        protected override void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            if (browser.IsPopup)
            {
                var windowHandle = browser.GetHost().GetWindowHandle();

                //WinForms will kindly lookup the child control from it's handle
                //If no parentControl then likely it's a native popup created by CEF
                //(Devtools by default will open as a popup, at this point the Url hasn't been set, so 
                // we're going with this assumption as it fits the use case currently)
                var control = Control.FromChildHandle(windowHandle) as ChromiumHostControl;

                //On WinForms parent control so we'll treat this as a native popup and do nothing
                if (control != null && !control.IsDisposed)
                {
                    control.BrowserHwnd = windowHandle;

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
                }
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
        protected override bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            newBrowser = null;

            //No action so we'll go with the default behaviour.
            if(onPopupCreated == null)
            {
                return false;
            }

            //NOTE: DevTools popups DO NOT trigger OnBeforePopup.
            //Use IWindowInfo.SetAsChild to specify the parent handle
            //NOTE: use ParentFormMessageInterceptor to handle Form move and Control resize etc.
            var webBrowser = (ChromiumWebBrowser)chromiumWebBrowser;

            //We need to execute sync here so IWindowInfo.SetAsChild is called before we return false;
            webBrowser.InvokeSyncOnUiThreadIfRequired(new Action(() =>
            {
                var control = new ChromiumHostControl
                {
                    Dock = DockStyle.Fill
                };
                control.CreateControl();

                onPopupCreated?.Invoke(control, targetUrl);

                var rect = control.ClientRectangle;

                windowInfo.SetAsChild(control.Handle, rect.Left, rect.Top, rect.Right, rect.Bottom);
            }));

            return false;
        }

        /// <summary>
        /// Register an Action that will be called when the Popup Host Control has been
        /// created. When the Action is called you must add the control to it's intended parent
        /// so the <see cref="Control.ClientRectangle"/> can be calculated to set the initial
        /// size correctly.
        /// </summary>
        /// <param name="onPopupCreated">Action to be invoked when the Popup is to be destroyed.</param>
        /// <returns><see cref="LifeSpanHandler"/> instance allowing you to chain method calls together</returns>
        public LifeSpanHandler RegisterPopupCreated(Action<ChromiumHostControl, string> onPopupCreated)
        {
            this.onPopupCreated = onPopupCreated;

            return this;
        }

        /// <summary>
        /// Create a new instance of the  WinForms specific lifespan handler
        /// </summary>
        /// <returns>LifeSpanHandler</returns>
        public static LifeSpanHandler Create()
        {
            return new LifeSpanHandler();
        }
    }
}
