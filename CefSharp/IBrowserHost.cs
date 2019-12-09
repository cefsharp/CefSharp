// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CefSharp.Callback;
using CefSharp.Enums;
using CefSharp.Structs;
using Size = CefSharp.Structs.Size;

namespace CefSharp
{
    /// <summary>
    /// Interface used to represent the browser process aspects of a browser window.
    /// They may be called on any thread in that process unless otherwise indicated in the comments. 
    /// </summary>
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
        /// Returns true if this browser currently has an associated DevTools browser.
        /// Must be called on the CEF UI thread.
        /// </summary>
        bool HasDevTools { get; }

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

        /// <summary>
        /// Call this method when the drag operation started by a <see cref="CefSharp.Internals.IRenderWebBrowser.StartDragging"/> call has ended either in a drop or by being cancelled.
        /// If the web view is both the drag source and the drag target then all DragTarget* methods should be called before DragSource* methods.
        /// This method is only used when window rendering is disabled. 
        /// </summary>
        /// <param name="x">x mouse coordinate relative to the upper-left corner of the view.</param>
        /// <param name="y">y mouse coordinate relative to the upper-left corner of the view.</param>
        /// <param name="op">Drag Operations mask</param>
        void DragSourceEndedAt(int x, int y, DragOperationsMask op);

        /// <summary>
        /// Call this method when the user drags the mouse out of the web view (after calling <see cref="DragTargetDragEnter"/>).
        /// This method is only used when window rendering is disabled.
        /// </summary>
        void DragTargetDragLeave();

        /// <summary>
        /// Call this method when the drag operation started by a <see cref="CefSharp.Internals.IRenderWebBrowser.StartDragging"/> call has completed.
        /// This method may be called immediately without first calling DragSourceEndedAt to cancel a drag operation.
        /// If the web view is both the drag source and the drag target then all DragTarget* methods should be called before DragSource* mthods.
        /// This method is only used when window rendering is disabled. 
        /// </summary>
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
        /// Returns the extension hosted in this browser or null if no extension is hosted. See <see cref="IRequestContext.LoadExtension"/> for details.
        /// </summary>
        IExtension Extension { get; }

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
        /// Gets the current zoom level. The default zoom level is 0.0. This method can only be called on the CEF UI thread. 
        /// </summary>
        /// <returns>zoom level (default is 0.0)</returns>
        double GetZoomLevel();

        /// <summary>
        /// Get the current zoom level. The default zoom level is 0.0. This method executes GetZoomLevel on the CEF UI thread
        /// in an async fashion.
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
        /// Returns true if this browser is hosting an extension background script. Background hosts do not have a window and are not displayable.
        /// See <see cref="IRequestContext.LoadExtension"/> for details.
        /// </summary>
        /// <returns>Returns true if this browser is hosting an extension background script.</returns>
        bool IsBackgroundHost { get; }

        /// <summary>
        /// Begins a new composition or updates the existing composition. Blink has a
        /// special node (a composition node) that allows the input method to change
        /// text without affecting other DOM nodes. 
        ///
        /// This method may be called multiple times as the composition changes. When
        /// the client is done making changes the composition should either be canceled
        /// or completed. To cancel the composition call ImeCancelComposition. To
        /// complete the composition call either ImeCommitText or
        /// ImeFinishComposingText. Completion is usually signaled when:
        /// The client receives a WM_IME_COMPOSITION message with a GCS_RESULTSTR
        /// flag (on Windows).
        /// This method is only used when window rendering is disabled. (WPF and OffScreen) 
        /// </summary>
        /// <param name="text">is the optional text that
        /// will be inserted into the composition node</param>
        /// <param name="underlines">is an optional set
        /// of ranges that will be underlined in the resulting text.</param>
        /// <param name="replacementRange">is an optional range of the existing text that will be replaced. (MAC OSX ONLY)</param>
        /// <param name="selectionRange"> is an optional range of the resulting text that
        /// will be selected after insertion or replacement. </param>
        void ImeSetComposition(string text, CompositionUnderline[] underlines, Range? replacementRange, Range? selectionRange);

        /// <summary>
        /// Completes the existing composition by optionally inserting the specified
        /// text into the composition node.
        /// This method is only used when window rendering is disabled. (WPF and OffScreen) 
        /// </summary>
        /// <param name="text">text that will be committed</param>
        /// <param name="replacementRange">is an optional range of the existing text that will be replaced. (MAC OSX ONLY)</param>
        /// <param name="relativeCursorPos">is where the cursor will be positioned relative to the current cursor position. (MAC OSX ONLY)</param>
        void ImeCommitText(string text, Range? replacementRange, int relativeCursorPos);

        /// <summary>
        /// Completes the existing composition by applying the current composition node
        /// contents. See comments on ImeSetComposition for usage.
        /// This method is only used when window rendering is disabled. (WPF and OffScreen) 
        /// </summary>
        /// <param name="keepSelection">If keepSelection is false the current selection, if any, will be discarded.</param>
        void ImeFinishComposingText(bool keepSelection);

        /// <summary>
        /// Cancels the existing composition and discards the composition node
        /// contents without applying them. See comments on ImeSetComposition for
        /// usage.
        /// This method is only used when window rendering is disabled. (WPF and OffScreen) 
        /// </summary>
        void ImeCancelComposition();

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
        /// <param name="settings">Print Settings, can be null</param>
        /// <param name="callback">Callback executed when printing complete</param>
        void PrintToPdf(string path, PdfPrintSettings settings, IPrintToPdfCallback callback);

        /// <summary>
        /// If a misspelled word is currently selected in an editable node calling this method will replace it with the specified word.
        /// </summary>
        /// <param name="word">word to be replaced</param>
        void ReplaceMisspelling(string word);

        /// <summary>
        /// Call to run a file chooser dialog. Only a single file chooser dialog may be pending at any given time.
        /// The dialog will be initiated asynchronously on the CEF UI thread.
        /// </summary>
        /// <param name="mode">represents the type of dialog to display</param>
        /// <param name="title">to the title to be used for the dialog and may be empty to show the default title ("Open" or "Save" depending on the mode)</param>
        /// <param name="defaultFilePath">is the path with optional directory and/or file name component that will be initially selected in the dialog</param>
        /// <param name="acceptFilters">are used to restrict the selectable file types and may any combination of (a) valid lower-cased MIME types (e.g. "text/*" or "image/*"), (b) individual file extensions (e.g. ".txt" or ".png"), or (c) combined description and file extension delimited using "|" and ";" (e.g. "Image Types|.png;.gif;.jpg")</param>
        /// <param name="selectedAcceptFilter">is the 0-based index of the filter that will be selected by default</param>
        /// <param name="callback">will be executed after the dialog is dismissed or immediately if another dialog is already pending.</param>
        void RunFileDialog(CefFileDialogMode mode, string title, string defaultFilePath, IList<string> acceptFilters, int selectedAcceptFilter, IRunFileDialogCallback callback);

        /// <summary>
        /// Returns the request context for this browser.
        /// </summary>
        IRequestContext RequestContext { get; }

        /// <summary>
        /// Issue a BeginFrame request to Chromium.
        /// Only valid when <see cref="IWindowInfo.ExternalBeginFrameEnabled"/> is set to true.
        /// </summary>
        void SendExternalBeginFrame();

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
        /// <param name="mouseEvent">mouse event - x, y and modifiers</param>
        /// <param name="mouseButtonType">Mouse ButtonType</param>
        /// <param name="mouseUp">mouse up</param>
        /// <param name="clickCount">click count</param>
        void SendMouseClickEvent(MouseEvent mouseEvent, MouseButtonType mouseButtonType, bool mouseUp, int clickCount);

        /// <summary>
        /// Send a mouse wheel event to the browser.
        /// </summary>
        /// <param name="mouseEvent">mouse event - x, y and modifiers</param>
        /// <param name="deltaX">Movement delta for X direction.</param>
        /// <param name="deltaY">movement delta for Y direction.</param>
        void SendMouseWheelEvent(MouseEvent mouseEvent, int deltaX, int deltaY);

        /// <summary>
        /// Send a touch event to the browser.
        /// WPF and OffScreen browsers only
        /// </summary>
        /// <param name="evt">touch event</param>
        void SendTouchEvent(TouchEvent evt);

        /// <summary>
        /// Set accessibility state for all frames.  If accessibilityState is Default then accessibility will be disabled by default
        /// and the state may be further controlled with the "force-renderer-accessibility" and "disable-renderer-accessibility"
        /// command-line switches. If accessibilityState is STATE_ENABLED then accessibility will be enabled.
        /// If accessibilityState is STATE_DISABLED then accessibility will be completely disabled. For windowed browsers
        /// accessibility will be enabled in Complete mode (which corresponds to kAccessibilityModeComplete in Chromium).
        /// In this mode all platform accessibility objects will be created and managed by Chromium's internal implementation.
        /// The client needs only to detect the screen reader and call this method appropriately. For example, on Windows the
        /// client can handle WM_GETOBJECT with OBJID_CLIENT to detect accessibility readers. For windowless browsers accessibility
        /// will be enabled in TreeOnly mode (which corresponds to kAccessibilityModeWebContentsOnly in Chromium). In this mode
        /// renderer accessibility is enabled, the full tree is computed, and events are passed to IAccessibiltyHandler,
        /// but platform accessibility objects are not created. The client may implement platform accessibility objects using
        /// IAccessibiltyHandler callbacks if desired. 
        /// </summary>
        /// <param name="accessibilityState">may be default, enabled or disabled.</param>
        void SetAccessibilityState(CefState accessibilityState);

        /// <summary>
        /// Enable notifications of auto resize via IDisplayHandler.OnAutoResize. Notifications are disabled by default.
        /// </summary>
        /// <param name="enabled">enable auto resize</param>
        /// <param name="minSize">minimum size</param>
        /// <param name="maxSize">maximum size</param>
        void SetAutoResizeEnabled(bool enabled, Size minSize, Size maxSize);

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
        /// Send a mouse move event to the browser, coordinates, 
        /// </summary>
        /// <param name="mouseEvent">mouse information, x and y values are relative to upper-left corner of view</param>
        /// <param name="mouseLeave">mouse leave</param>
        void SendMouseMoveEvent(MouseEvent mouseEvent, bool mouseLeave);

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
        /// Retrieve a snapshot of current navigation entries as values sent to the
        /// specified visitor. 
        /// </summary>
        /// <param name="visitor">visitor</param>
        /// <param name="currentOnly">If true only the current navigation
        /// entry will be sent, otherwise all navigation entries will be sent.</param>
        void GetNavigationEntries(INavigationEntryVisitor visitor, bool currentOnly);

        /// <summary>
        /// Returns the current visible navigation entry for this browser. This method
        /// can only be called on the CEF UI thread which by default is not the same
        /// as your application UI thread.
        /// </summary>
        /// <returns>the current navigation entry</returns>
        NavigationEntry GetVisibleNavigationEntry();

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
        /// Set whether the browser's audio is muted.
        /// </summary>
        /// <param name="mute">true or false</param>
        void SetAudioMuted(bool mute);

        /// <summary>
        /// Returns true if the browser's audio is muted.
        /// This method can only be called on the CEF UI thread.
        /// </summary>
        bool IsAudioMuted { get; }

        /// <summary>
        /// Gets a value indicating whether the browserHost has been disposed of.
        /// </summary>
        bool IsDisposed { get; }
    }
}
