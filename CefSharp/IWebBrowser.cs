// Copyright © 2011 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;

namespace CefSharp
{
    /// <summary>
    /// ChromiumWebBrowser implementations implement this interface. Can be cast to
    /// the concrete implementation to access UI specific features.
    /// </summary>
    public interface IWebBrowser : IChromiumWebBrowserBase
    {
        /// <summary>
        /// Event handler that will get called when the message that originates from CefSharp.PostMessage
        /// </summary>
        event EventHandler<JavascriptMessageReceivedEventArgs> JavascriptMessageReceived;

        /// <summary>
        /// Loads the specified <paramref name="url"/> in the Main Frame.
        /// If <see cref="IChromiumWebBrowserBase.IsDisposed"/> is true then the method call will be ignored.
        /// Same as calling <see cref="IChromiumWebBrowserBase.LoadUrl(string)"/>
        /// </summary>
        /// <param name="url">The URL to be loaded.</param>
        void Load(string url);

        /// <summary>
        /// Wait for the Browser to finish loading the initial web page.
        /// </summary>
        /// <returns>
        /// A <see cref="Task{LoadUrlAsyncResponse}"/> that can be awaited which returns the HttpStatusCode and <see cref="CefErrorCode"/>.
        /// A HttpStatusCode equal to 200 and <see cref="CefErrorCode.None"/> is considered a success.
        /// </returns>
        Task<LoadUrlAsyncResponse> WaitForInitialLoadAsync();

        /// <summary>
        /// The javascript object repository, one repository per ChromiumWebBrowser instance.
        /// </summary>
        IJavascriptObjectRepository JavascriptObjectRepository { get; }

        /// <summary>
        /// Implement <see cref="IDialogHandler" /> and assign to handle dialog events.
        /// </summary>
        /// <value>The dialog handler.</value>
        IDialogHandler DialogHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IRequestHandler" /> and assign to handle events related to browser requests.
        /// </summary>
        /// <value>The request handler.</value>
        IRequestHandler RequestHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IDisplayHandler" /> and assign to handle events related to browser display state.
        /// </summary>
        /// <value>The display handler.</value>
        IDisplayHandler DisplayHandler { get; set; }

        /// <summary>
        /// Implement <see cref="ILoadHandler" /> and assign to handle events related to browser load status.
        /// </summary>
        /// <value>The load handler.</value>
        ILoadHandler LoadHandler { get; set; }

        /// <summary>
        /// Implement <see cref="ILifeSpanHandler" /> and assign to handle events related to popups.
        /// </summary>
        /// <value>The life span handler.</value>
        ILifeSpanHandler LifeSpanHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IKeyboardHandler" /> and assign to handle events related to key press.
        /// </summary>
        /// <value>The keyboard handler.</value>
        IKeyboardHandler KeyboardHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IJsDialogHandler" /> and assign to handle events related to JavaScript Dialogs.
        /// </summary>
        /// <value>The js dialog handler.</value>
        IJsDialogHandler JsDialogHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IDragHandler" /> and assign to handle events related to dragging.
        /// </summary>
        /// <value>The drag handler.</value>
        IDragHandler DragHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IDownloadHandler" /> and assign to handle events related to downloading files.
        /// </summary>
        /// <value>The download handler.</value>
        IDownloadHandler DownloadHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IContextMenuHandler" /> and assign to handle events related to the browser context menu
        /// </summary>
        /// <value>The menu handler.</value>
        IContextMenuHandler MenuHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IFocusHandler" /> and assign to handle events related to the browser component's focus
        /// </summary>
        /// <value>The focus handler.</value>
        IFocusHandler FocusHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IResourceRequestHandlerFactory" /> and control the loading of resources
        /// </summary>
        /// <value>The resource handler factory.</value>
        IResourceRequestHandlerFactory ResourceRequestHandlerFactory { get; set; }

        /// <summary>
        /// Implement <see cref="IRenderProcessMessageHandler" /> and assign to handle messages from the render process.
        /// </summary>
        /// <value>The render process message handler.</value>
        IRenderProcessMessageHandler RenderProcessMessageHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IFindHandler" /> to handle events related to find results.
        /// </summary>
        /// <value>The find handler.</value>
        IFindHandler FindHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IAudioHandler" /> to handle audio events.
        /// </summary>
        IAudioHandler AudioHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IFrameHandler" /> to handle frame events.
        /// </summary>
        IFrameHandler FrameHandler { get; set; }

        /// <summary>
        /// Implement <see cref="IPermissionHandler" /> to handle events related to permission requests.
        /// </summary>
        IPermissionHandler PermissionHandler { get; set; }

        /// <summary>
        /// The text that will be displayed as a ToolTip
        /// </summary>
        /// <value>The tooltip text.</value>
        string TooltipText { get; }

        /// <summary>
        /// A flag that indicates if you can execute javascript in the main frame.
        /// Flag is set to true in IRenderProcessMessageHandler.OnContextCreated.
        /// and false in IRenderProcessMessageHandler.OnContextReleased
        /// </summary>
        bool CanExecuteJavascriptInMainFrame { get; }

        /// <summary>
        /// Gets the custom request context assigned to this browser instance
        /// If no instance was assigned this will be null and the global
        /// request context will have been used for this browser. 
        /// You can access the global request context through Cef.GetGlobalRequestContext()
        /// </summary>
        IRequestContext RequestContext { get; }

        /// <summary>
        /// Returns the current CEF Browser Instance
        /// </summary>
        /// <returns>browser instance or null</returns>
        IBrowser GetBrowser();

        /// <summary>
        /// Try and get a reference to the <see cref="IBrowser"/> instance that matches the <paramref name="browserId"/>.
        /// Primarily used for geting a reference to the <see cref="IBrowser"/> used by popups.
        /// </summary>
        /// <param name="browserId">browser Id</param>
        /// <param name="browser">When this method returns, contains the <see cref="IBrowser"/> object reference that matches the specified <paramref name="browserId"/>, or null if no matching instance found.</param>
        /// <returns>true if a <see cref="IBrowser"/> instance was found matching <paramref name="browserId"/>; otherwise, false.</returns>
        bool TryGetBrowserCoreById(int browserId, out IBrowser browser);

        /// <summary>
        /// Size of scrollable area in CSS pixels
        /// </summary>
        /// <returns>A task that can be awaited to get the size of the scrollable area in CSS pixels.</returns>
        Task<CefSharp.Structs.DomRect> GetContentSizeAsync();
    }
}
