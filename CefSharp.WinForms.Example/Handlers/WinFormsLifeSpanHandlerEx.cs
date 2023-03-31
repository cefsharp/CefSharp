// Copyright © 2023 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Forms;
using CefSharp.WinForms.Host;

namespace CefSharp.WinForms.Example.Handlers
{
    /// <summary>
    /// A WinForms Specific <see cref="ILifeSpanHandler"/> implementation that demos
    /// the process of hosting a Popup using a <see cref="ChromiumWebBrowser"/> instance.
    /// This <see cref="ILifeSpanHandler"/> implementation returns true in <see cref="ILifeSpanHandler.DoClose(IWebBrowser, IBrowser)"/>
    /// so no WM_CLOSE message is sent, this differs from the default CEF behaviour.
    /// </summary>
    internal class WinFormsLifeSpanHandlerEx : CefSharp.Handler.LifeSpanHandler
    {
        private Action<ChromiumWebBrowser> onPopupBrowserCreated;
        private Action<ChromiumWebBrowser> onPopupDestroyed;
        private Action<ChromiumWebBrowser, string, string, CefSharp.Structs.Rect> onPopupCreated;

        /// <summary>
        /// The <paramref name="onPopupBrowserCreated"/> delegate will be called when the underlying CEF <see cref="IBrowser"/> has been
        /// created. The <see cref="IBrowser"/> instance is valid until <see cref="OnPopupDestroyed(Action{ChromiumWebBrowser})"/>
        /// is called. <see cref="IBrowser"/> provides low level access to the CEF Browser, you can access frames, view source,
        /// perform navigation (via frame) etc. This is equivilent to the <see cref="ILifeSpanHandler.OnAfterCreated(IWebBrowser, IBrowser)"/>.
        /// </summary>
        /// <param name="onPopupBrowserCreated">Action to be invoked when the <see cref="IBrowser"/> has been created.</param>
        /// <returns><see cref="WinFormsLifeSpanHandlerEx"/> instance allowing you to chain method calls together</returns>
        public WinFormsLifeSpanHandlerEx OnPopupBrowserCreated(Action<ChromiumWebBrowser> onPopupBrowserCreated)
        {
            this.onPopupBrowserCreated = onPopupBrowserCreated;

            return this;
        }

        /// <summary>
        /// The <paramref name="onPopupDestroyed"/> will be called when the <see cref="ChromiumWebBrowser"/> is to be
        /// removed from it's parent.
        /// When the <see cref="OnPopupDestroyedDelegate"/> is called you must remove/dispose of the <see cref="ChromiumWebBrowser"/>.
        /// </summary>
        /// <param name="onPopupDestroyed">Action to be invoked when the Popup is to be destroyed.</param>
        /// <returns><see cref="WinFormsLifeSpanHandlerEx"/> instance allowing you to chain method calls together</returns>
        public WinFormsLifeSpanHandlerEx OnPopupDestroyed(Action<ChromiumWebBrowser> onPopupDestroyed)
        {
            this.onPopupDestroyed = onPopupDestroyed;

            return this;
        }

        /// <summary>
        /// The <paramref name="onPopupCreated"/> will be called when the<see cref="ChromiumWebBrowser"/> has been
        /// created. When the <paramref name="onPopupCreated"/> is called you must add the control to it's intended parent.
        /// </summary>
        /// <param name="onPopupCreated">Action to be invoked when the Popup host has been created and is ready to be attached to it's parent.</param>
        /// <returns><see cref="WinFormsLifeSpanHandlerEx"/> instance allowing you to chain method calls together</returns>
        public WinFormsLifeSpanHandlerEx OnPopupCreated(Action<ChromiumWebBrowser, string, string, CefSharp.Structs.Rect> onPopupCreated)
        {
            this.onPopupCreated = onPopupCreated;

            return this;
        }

        /// <inheritdoc/>
        protected override bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            if (browser.IsPopup)
            {
                var control = ChromiumHostControlBase.FromBrowser<ChromiumWebBrowser>(browser);

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
                        control.BeginInvoke(new Action(() =>
                        {
                            onPopupDestroyed?.Invoke(control);

                            control.Dispose();
                        }));
                    }
                    catch (ObjectDisposedException)
                    {
                        // If the popup is being hosted on a Form that is being
                        // Closed/Disposed as we attempt to call Control.BeginInvoke
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
                var webBrowser = (ChromiumWebBrowser)chromiumWebBrowser;

                webBrowser.BeginInvoke((Action) (() =>
                {
                    var control = ChromiumHostControlBase.FromBrowser<ChromiumWebBrowser>(browser);

                    if (control != null)
                    {
                        onPopupBrowserCreated?.Invoke(control);
                    }
                }));
                
            }
        }

        /// <inheritdoc/>
        protected override void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            if (!browser.IsDisposed && browser.IsPopup)
            {
                
            }
        }

        /// <inheritdoc/>
        /// <remarks>
        /// NOTE: DevTools popups DO NOT trigger OnBeforePopup.
        /// </remarks>
        protected override bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            newBrowser = null;

            var webBrowser = (ChromiumWebBrowser)chromiumWebBrowser;

            ChromiumWebBrowser control = null;

            //We need to execute sync here so IWindowInfo.SetAsChild is called before we return false;
            webBrowser.Invoke(new Action(() =>
            {
                control = new ChromiumWebBrowser
                {
                    Dock = DockStyle.Fill
                };

                //NOTE: This is important and must be called before the handle is created
                control.SetAsPopup();
                control.LifeSpanHandler = this;

                control.CreateControl();

                var rect = control.ClientRectangle;

                var windowBounds = new CefSharp.Structs.Rect(rect.X, rect.Y, rect.Width, rect.Height);

                windowInfo.SetAsChild(control.Handle, windowBounds);

                onPopupCreated?.Invoke(control, targetUrl, targetFrameName, windowBounds);
            }));

            newBrowser = control;

            return false;
        }
    }
}
