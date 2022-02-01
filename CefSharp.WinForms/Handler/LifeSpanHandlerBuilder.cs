// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.WinForms.Handler
{
    /// <summary>
    /// Fluent <see cref="CefSharp.WinForms.Handler.LifeSpanHandler"/> Builder
    /// </summary>
    public class LifeSpanHandlerBuilder
    {
        private readonly LifeSpanHandler handler;

        /// <summary>
        /// LifeSpanHandlerBuilder
        /// </summary>
        /// <param name="chromiumHostControlCreatedDelegate">
        /// When specified the delegate will be used to create the <see cref="Host.ChromiumHostControl"/>
        /// instance. Allowing users to create their own custom instance that extends <see cref="Host.ChromiumHostControl"/>
        /// </param>
        public LifeSpanHandlerBuilder(CreatePopupChromiumHostControl chromiumHostControlCreatedDelegate)
        {
            handler = new LifeSpanHandler(chromiumHostControlCreatedDelegate);
        }

        /// <summary>
        /// The <see cref="OnBeforePopupCreatedDelegate"/> will be called <b>before</b> the popup has been created and
        /// can be used to cancel popup creation if required, modify <see cref="IBrowserSettings"/> and disable javascript.
        /// </summary>
        /// <param name="onBeforePopupCreated">Action to be invoked before popup is created.</param>
        /// <returns><see cref="LifeSpanHandlerBuilder"/> instance allowing you to chain method calls together</returns>
        public LifeSpanHandlerBuilder OnBeforePopupCreated(OnBeforePopupCreatedDelegate onBeforePopupCreated)
        {
            handler.OnBeforePopupCreated(onBeforePopupCreated);

            return this;
        }

        /// <summary>
        /// The <see cref="OnPopupCreatedDelegate"/> will be called when the<see cref="Host.ChromiumHostControl"/> has been
        /// created. When the <see cref="OnPopupCreatedDelegate"/> is called you must add the control to it's intended parent
        /// so the <see cref="System.Windows.Forms.Control.ClientRectangle"/> can be calculated to set the initial
        /// size correctly.
        /// </summary>
        /// <param name="onPopupCreated">Action to be invoked when the Popup is to be destroyed.</param>
        /// <returns><see cref="LifeSpanHandlerBuilder"/> instance allowing you to chain method calls together</returns>
        public LifeSpanHandlerBuilder OnPopupCreated(OnPopupCreatedDelegate onPopupCreated)
        {
            handler.OnPopupCreated(onPopupCreated);

            return this;
        }

        /// <summary>
        /// The <see cref="OnPopupBrowserCreatedDelegate"/> will be called when the<see cref="IBrowser"/> has been
        /// created. The <see cref="IBrowser"/> instance is valid until <see cref="OnPopupDestroyed(OnPopupDestroyedDelegate)"/>
        /// is called. <see cref="IBrowser"/> provides low level access to the CEF Browser, you can access frames, view source,
        /// perform navigation (via frame) etc.
        /// </summary>
        /// <param name="onPopupBrowserCreated">Action to be invoked when the <see cref="IBrowser"/> has been created.</param>
        /// <returns><see cref="LifeSpanHandlerBuilder"/> instance allowing you to chain method calls together</returns>
        public LifeSpanHandlerBuilder OnPopupBrowserCreated(OnPopupBrowserCreatedDelegate onPopupBrowserCreated)
        {
            handler.OnPopupBrowserCreated(onPopupBrowserCreated);

            return this;
        }

        /// <summary>
        /// The <see cref="OnPopupDestroyedDelegate"/> will be called when the <see cref="Host.ChromiumHostControl"/> is to be
        /// removed from it's parent.
        /// When the <see cref="OnPopupDestroyedDelegate"/> is called you must remove/dispose of the <see cref="Host.ChromiumHostControl"/>.
        /// </summary>
        /// <param name="onPopupDestroyed">Action to be invoked when the Popup is to be destroyed.</param>
        /// <returns><see cref="LifeSpanHandlerBuilder"/> instance allowing you to chain method calls together</returns>
        public LifeSpanHandlerBuilder OnPopupDestroyed(OnPopupDestroyedDelegate onPopupDestroyed)
        {
            handler.OnPopupDestroyed(onPopupDestroyed);

            return this;
        }

        /// <summary>
        /// Creates an <see cref="ILifeSpanHandler"/> implementation
        /// that can be used to host popups as tabs/controls. The resulting
        /// <see cref="ILifeSpanHandler"/> returns true in <see cref="ILifeSpanHandler.DoClose(IWebBrowser, IBrowser)"/>
        /// so no WM_CLOSE message is sent, this differs from the default CEF behaviour.
        /// </summary>
        /// <returns>a <see cref="ILifeSpanHandler"/> instance</returns>
        public ILifeSpanHandler Build()
        {
            return handler;
        }
    }
}
