// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

namespace CefSharp
{
    /// <summary>
    /// CefSharp interface for CefBrowser.
    /// </summary>
    public interface IBrowser : IDisposable
    {
        /// <summary>
        /// Returns True if this object is currently valid. This will return false after
        /// <see cref="ILifeSpanHandler.OnBeforeClose(IWebBrowser, IBrowser)"/> is called.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Returns the browser host object. This method can only be called in the browser process.
        /// </summary>
        /// <returns>the browser host object</returns>
        IBrowserHost GetHost();

        /// <summary>
        /// Returns true if the browser can navigate backwards.
        /// </summary>
        bool CanGoBack { get; }

        /// <summary>
        /// Navigate backwards.
        /// </summary>
        void GoBack();

        /// <summary>
        /// Returns true if the browser can navigate forwards.
        /// </summary>
        bool CanGoForward { get; }

        /// <summary>
        /// Navigate forwards.
        /// </summary>
        void GoForward();

        /// <summary>
        /// Returns true if the browser is currently loading.
        /// </summary>
        bool IsLoading { get; }

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
        /// Reload the current page.
        /// </summary>
        /// <param name="ignoreCache">
        /// <c>true</c> a reload is performed ignoring browser cache; <c>false</c> a reload is
        /// performed using files from the browser cache, if available.
        /// </param>
        void Reload(bool ignoreCache = false);

        /// <summary>
        /// Stop loading the page.
        /// </summary>
        void StopLoad();

        /// <summary>
        /// Returns the globally unique identifier for this browser.
        /// </summary>
        int Identifier { get; }

        /// <summary>
        /// Returns true if this object is pointing to the same handle as that object.
        /// </summary>
        /// <param name="that">compare browser instances</param>
        /// <returns>returns true if the same instance</returns>
        bool IsSame(IBrowser that);

        /// <summary>
        /// Returns true if the window is a popup window.
        /// </summary>
        bool IsPopup { get; }

        /// <summary>
        /// Returns true if a document has been loaded in the browser.
        /// </summary>
        bool HasDocument { get; }

        /// <summary>
        /// Returns the main (top-level) frame for the browser window.
        /// Returns null if there is currently no MainFrame.
        /// </summary>
        IFrame MainFrame { get; }

        /// <summary>
        /// Returns the focused frame for the browser window or null.
        /// </summary>
        IFrame FocusedFrame { get; }

        /// <summary>
        /// Returns the frame with the specified identifier, or NULL if not found.
        /// </summary>
        /// <param name="identifier">identifier</param>
        /// <returns>frame or null</returns>
        IFrame GetFrameByIdentifier(string identifier);

        /// <summary>
        /// Returns the frame with the specified name, or NULL if not found.
        /// </summary>
        /// <param name="name">name of frame</param>
        /// <returns>frame or null</returns>
        IFrame GetFrameByName(string name);

        /// <summary>
        /// Returns the number of frames that currently exist.
        /// </summary>
        /// <returns>the number of frames</returns>
        int GetFrameCount();

        /// <summary>
        /// Returns the identifiers of all existing frames.
        /// </summary>
        /// <returns>list of frame identifiers</returns>
        List<string> GetFrameIdentifiers();

        /// <summary>
        /// Returns the names of all existing frames.
        /// </summary>
        /// <returns>frame names</returns>
        List<string> GetFrameNames();

        /// <summary>
        /// Gets a value indicating whether the browser has been disposed of.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Gets a collection of all the current frames.
        /// </summary>
        /// <returns>frames</returns>
        IReadOnlyCollection<IFrame> GetAllFrames();
    }
}
