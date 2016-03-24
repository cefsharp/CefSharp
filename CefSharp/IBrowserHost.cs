﻿// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Threading.Tasks;

namespace CefSharp
{
    public interface IBrowserHost : IDisposable
    {
        /// <summary>
        /// Add the specified word to the spelling dictionary.
        /// </summary>
        /// <param name="word"></param>
        void AddWordToDictionary(string word);

        /// <summary>
        /// Request that the browser close. The JavaScript 'onbeforeunload' event will be fired.
        /// </summary>
        /// <param name="forceClose">
        /// If forceClose is false the event handler, if any, will be allowed to prompt the user and the
        /// user can optionally cancel the close. If forceClose is true the prompt will not be displayed
        /// and the close will proceed. Results in a call to <see cref="ILifeSpanHandler.DoClose"/> if
        /// the event handler allows the close or if forceClose is true
        /// See <see cref="ILifeSpanHandler.DoClose"/> documentation for additional usage information.
        /// </param>
        void CloseBrowser(bool forceClose);

        /// <summary>
        /// Explicitly close the developer tools window if one exists for this browser instance.
        /// </summary>
        void CloseDevTools();

        /// <summary>
        /// Call this method when the user drags the mouse into the web view (before calling <see cref="DragTargetDragOver"/>/<see cref="DragTargetDragLeave"/>/<see cref="DragTargetDragDrop"/>).
        /// </summary>
        void DragTargetDragEnter(IDragData dragData, MouseEvent mouseEvent, DragOperationsMask allowedOperations);

        /// <summary>
        /// Call this method each time the mouse is moved across the web view during a drag operation (after calling <see cref="DragTargetDragEnter"/> and before calling <see cref="DragTargetDragLeave"/>/<see cref="DragTargetDragDrop"/>). 
        /// This method is only used when window rendering is disabled.
        /// </summary>
        void DragTargetDragOver(MouseEvent mouseEvent, DragOperationsMask allowedOperations);

        /// <summary>
        /// Call this method when the user completes the drag operation by dropping the object onto the web view (after calling <see cref="DragTargetDragEnter"/>). 
        /// The object being dropped is <see cref="IDragData"/>, given as an argument to the previous <see cref="DragTargetDragEnter"/> call. 
        /// This method is only used when window rendering is disabled.
        /// </summary>
        void DragTargetDragDrop(MouseEvent mouseEvent);

        void DragSourceEndedAt(int x, int y, DragOperationsMask op);

        /// <summary>
        /// Call this method when the user drags the mouse out of the web view (after calling <see cref="DragTargetDragEnter"/>). This method is only used when window rendering is disabled.
        /// </summary>
        void DragTargetDragLeave();
        
        void DragSourceSystemDragEnded();

        /// <summary>
        /// Search for text
        /// </summary>
        /// <param name="identifier">can be used to have multiple searches running simultaniously</param>
        /// <param name="searchText">text to search for</param>
        /// <param name="forward">indicates whether to search forward or backward within the page</param>
        /// <param name="matchCase">indicates whether the search should be case-sensitive</param>
        /// <param name="findNext">indicates whether this is the first request or a follow-up</param>
        /// <remarks>The IFindHandler instance, if any, will be called to report find results. </remarks>
        void Find(int identifier, string searchText, bool forward, bool matchCase, bool findNext);

        /// <summary>
        /// Retrieve the window handle of the browser that opened this browser.
        /// </summary>
        /// <returns>The handler</returns>
        IntPtr GetOpenerWindowHandle();

        /// <summary>
        /// Retrieve the window handle for this browser. 
        /// </summary>
        /// <returns>The handler</returns>
        IntPtr GetWindowHandle();

        /// <summary>
        /// Get the current zoom level. The default zoom level is 0.0. This method can only be called on the CEF UI thread. 
        /// </summary>
        /// <returns> a <see cref="Task{Double}"/> that when executed returns the zoom level as a double.</returns>
        Task<double> GetZoomLevelAsync();

        /// <summary>
        /// Invalidate the view. The browser will call CefRenderHandler::OnPaint asynchronously.
        /// This method is only used when window rendering is disabled (OSR). 
        /// </summary>
        /// <param name="type">indicates which surface to re-paint either View or Popup.</param>
        void Invalidate(PaintElementType type);

        /// <summary>
        /// Get/Set Mouse cursor change disabled
        /// </summary>
        bool MouseCursorChangeDisabled { get; set; }

        /// <summary>
        /// Notify the browser that the window hosting it is about to be moved or resized.
        /// </summary>
        void NotifyMoveOrResizeStarted();

        /// <summary>
        /// Send a notification to the browser that the screen info has changed.
        /// The browser will then call CefRenderHandler::GetScreenInfo to update the screen information with the new values.
        /// This simulates moving the webview window from one display to another, or changing the properties of the current display.
        /// This method is only used when window rendering is disabled. 
        /// </summary>
        void NotifyScreenInfoChanged();

        /// <summary>
        /// Print the current browser contents. 
        /// </summary>
        void Print();

        /// <summary>
        /// Asynchronously prints the current browser contents to the Pdf file specified.
        /// The caller is responsible for deleting the file when done.
        /// </summary>
        /// <param name="path">Output file location.</param>
        /// <param name="settings">Print Settings.</param>
        /// <returns>A task that represents the asynchronous print operation.
        /// The result is true on success or false on failure to generate the Pdf.</returns>
        Task<bool> PrintToPdfAsync(string path, PdfPrintSettings settings = null);

        /// <summary>
        /// If a misspelled word is currently selected in an editable node calling this method will replace it with the specified word.
        /// </summary>
        /// <param name="word">word to be replaced</param>
        void ReplaceMisspelling(string word);

        /// <summary>
        /// Returns the request context for this browser.
        /// </summary>
        IRequestContext RequestContext { get; }

        /// <summary>
        /// Send a capture lost event to the browser.
        /// </summary>
        void SendCaptureLostEvent();

        /// <summary>
        /// Send a focus event to the browser. . (Used for OSR Rendering e.g. WPF or OffScreen)
        /// </summary>
        /// <param name="setFocus">set focus</param>
        void SendFocusEvent(bool setFocus);

        /// <summary>
        ///  Send a key event to the browser.
        /// </summary>
        /// <param name="keyEvent">represents keyboard event</param>
        void SendKeyEvent(KeyEvent keyEvent);

        /// <summary>
        /// Send key event to browser based on operating system message
        /// </summary>
        /// <param name="message">message</param>
        /// <param name="wParam">wParam</param>
        /// <param name="lParam">lParam</param>
        void SendKeyEvent(int message, int wParam, int lParam);

        /// <summary>
        /// Send a mouse click event to the browser.
        /// </summary>
        /// <param name="x">x coordinate - relative to upper-left corner of view</param>
        /// <param name="y">y coordinate - relative to upper-left corner of view</param>
        /// <param name="mouseButtonType">Mouse ButtonType</param>
        /// <param name="mouseUp">mouse up</param>
        /// <param name="clickCount">click count</param>
        /// <param name="modifiers">click modifiers e.g. Ctrl</param>
        void SendMouseClickEvent(int x, int y, MouseButtonType mouseButtonType, bool mouseUp, int clickCount, CefEventFlags modifiers);

        /// <summary>
        /// Send a mouse wheel event to the browser.
        /// </summary>
        /// <param name="x">X-Axis coordinate relative to the upper-left corner of the view.</param>
        /// <param name="y">Y-Axis coordinate relative to the upper-left corner of the view.</param>
        /// <param name="deltaX">Movement delta for X direction.</param>
        /// <param name="deltaY">movement delta for Y direction.</param>
        /// /// <param name="modifiers">click modifiers e.g. Ctrl</param>
        void SendMouseWheelEvent(int x, int y, int deltaX, int deltaY, CefEventFlags modifiers);

        /// <summary>
        /// Set whether the browser is focused. (Used for Normal Rendering e.g. WinForms)
        /// </summary>
        /// <param name="focus">set focus</param>
        void SetFocus(bool focus);

        /// <summary>
        /// Change the zoom level to the specified value. Specify 0.0 to reset the zoom level.
        /// If called on the CEF UI thread the change will be applied immediately.
        /// Otherwise, the change will be applied asynchronously on the UI thread. 
        /// </summary>
        /// <param name="zoomLevel">zoom level</param>
        void SetZoomLevel(double zoomLevel);

        /// <summary>
        /// Open developer tools in its own window. If inspectElementAtX and/or inspectElementAtY  are specified then
        /// the element at the specified (x,y) location will be inspected.
        /// </summary>
        /// <param name="windowInfo">window info used for showing dev tools</param>
        /// <param name="inspectElementAtX">x coordinate (used for inspectElement)</param>
        /// <param name="inspectElementAtY">y coordinate (used for inspectElement)</param>
        void ShowDevTools(IWindowInfo windowInfo = null, int inspectElementAtX = 0, int inspectElementAtY = 0);
        
        /// <summary>
        /// Download the file at url using IDownloadHandler. 
        /// </summary>
        /// <param name="url">url to download</param>
        void StartDownload(string url);

        /// <summary>
        /// Cancel all searches that are currently going on. 
        /// </summary>
        /// <param name="clearSelection">clear the selection</param>
        void StopFinding(bool clearSelection);

        /// <summary>
        /// Send a mouse move event to the browser
        /// </summary>
        /// <param name="x">x coordinate - relative to upper-left corner of view</param>
        /// <param name="y">y coordinate - relative to upper-left corner of view</param>
        /// <param name="mouseLeave">mouse leave</param>
        /// <param name="modifiers">click modifiers .e.g Ctrl</param>
        void SendMouseMoveEvent(int x, int y, bool mouseLeave, CefEventFlags modifiers);

        /// <summary>
        /// Notify the browser that it has been hidden or shown.
        /// Layouting and rendering notification will stop when the browser is hidden.
        /// This method is only used when window rendering is disabled (WPF/OffScreen). 
        /// </summary>
        /// <param name="hidden"></param>
        void WasHidden(bool hidden);

        /// <summary>
        /// Notify the browser that the widget has been resized.
        /// The browser will first call CefRenderHandler::GetViewRect to get the new size and then call CefRenderHandler::OnPaint asynchronously with the updated regions.
        /// This method is only used when window rendering is disabled. 
        /// </summary>
        void WasResized();

        /// <summary>
        /// Gets/sets the maximum rate in frames per second (fps) that CefRenderHandler::
        /// OnPaint will be called for a windowless browser. The actual fps may be
        /// lower if the browser cannot generate frames at the requested rate. The
        /// minimum value is 1 and the maximum value is 60 (default 30). This method
        /// can only be called on the UI thread. Can also be set at browser creation
        /// via BrowserSettings.WindowlessFrameRate.
        /// </summary>
        int WindowlessFrameRate { get; set; }

        /// <summary>
        /// Returns true if window rendering is disabled.
        /// </summary>
        bool WindowRenderingDisabled { get; }

        /// <summary>
        /// Gets a value indicating whether the browserHost has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}
