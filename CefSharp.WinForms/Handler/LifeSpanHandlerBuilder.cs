// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.WinForms.Handler
{
    /// <summary>
    /// Fluent LifeSpanHandler Builder
    /// </summary>
    public class LifeSpanHandlerBuilder
    {
        private readonly LifeSpanHandler handler = new LifeSpanHandler();

        /// <summary>
        /// The <see cref="OnPopupCreatedDelegate"/> will be called when the<see cref="ChromiumHostControl"/> has been
        /// created. When the <see cref="OnPopupCreatedDelegate"/> is called you must add the control to it's intended parent
        /// so the <see cref="Control.ClientRectangle"/> can be calculated to set the initial
        /// size correctly.
        /// </summary>
        /// <param name="onPopupCreated">Action to be invoked when the Popup is to be destroyed.</param>
        /// <returns><see cref="LifeSpanHandler"/> instance allowing you to chain method calls together</returns>
        public LifeSpanHandlerBuilder OnPopupCreated(OnPopupCreatedDelegate onPopupCreated)
        {
            handler.OnPopupCreated(onPopupCreated);

            return this;
        }

        /// <summary>
        /// The <see cref="OnPopupDestroyedDelegate"/> will be called when the <see cref="ChromiumHostControl"/> is to be
        /// removed from it's parent.
        /// When the <see cref="OnPopupDestroyedDelegate"/> is called you must remove/dispose of the <see cref="ChromiumHostControl"/>.
        /// </summary>
        /// <param name="onPopupDestroyed">Action to be invoked when the Popup is to be destroyed.</param>
        /// <returns><see cref="LifeSpanHandler"/> instance allowing you to chain method calls together</returns>
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
        /// <returns>a <see cref="IDownloadHandler"/> instance</returns>
        public ILifeSpanHandler Build()
        {
            return handler;
        }
    }
}
