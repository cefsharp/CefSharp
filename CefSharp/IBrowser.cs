// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;

namespace CefSharp
{
    /// <summary>
    /// CefSharp interface for CefBrowser.
    /// </summary>
    public interface IBrowser
    {
        ///
        // Returns the browser host object. This method can only be called in the
        // browser process.
        ///
        /*--cef()--*/
        //virtual CefRefPtr<CefBrowserHost> GetHost() =0;

        ///
        // Returns true if the browser can navigate backwards.
        ///
        /*--cef()--*/
        bool CanGoBack { get; }

        ///
        // Navigate backwards.
        ///
        /*--cef()--*/
        void GoBack();

        ///
        // Returns true if the browser can navigate forwards.
        ///
        /*--cef()--*/
        bool CanGoForward { get; }

        ///
        // Navigate forwards.
        ///
        /*--cef()--*/
        void GoForward();

        ///
        // Returns true if the browser is currently loading.
        ///
        /*--cef()--*/
        bool IsLoading { get; }

        ///
        // Reload the current page.
        ///
        /*--cef()--*/
        void Reload();

        ///
        // Reload the current page ignoring any cached data.
        ///
        /*--cef()--*/
        void ReloadIgnoreCache();

        ///
        // Stop loading the page.
        ///
        /*--cef()--*/
        void StopLoad();

        ///
        // Returns the globally unique identifier for this browser.
        ///
        /*--cef()--*/
        int Identifier { get; }

        ///
        // Returns true if this object is pointing to the same handle as |that|
        // object.
        ///
        /*--cef()--*/
        bool IsSame(IBrowser that);

        ///
        // Returns true if the window is a popup window.
        ///
        /*--cef()--*/
        bool IsPopup { get; }

        ///
        // Returns true if a document has been loaded in the browser.
        ///
        /*--cef()--*/
        bool HasDocument { get; }

        ///
        // Returns the main (top-level) frame for the browser window.
        ///
        /*--cef()--*/
        IFrame MainFrame { get; }

        ///
        // Returns the focused frame for the browser window.
        ///
        /*--cef()--*/
        IFrame FocusedFrame { get; }

        ///
        // Returns the frame with the specified identifier, or NULL if not found.
        ///
        /*--cef(capi_name=get_frame_byident)--*/
        IFrame GetFrame(Int64 identifier);

        ///
        // Returns the frame with the specified name, or NULL if not found.
        ///
        /*--cef(optional_param=name)--*/
        IFrame GetFrame(string name);

        ///
        // Returns the number of frames that currently exist.
        ///
        /*--cef()--*/
        int GetFrameCount();

        ///
        // Returns the identifiers of all existing frames.
        ///
        /*--cef(count_func=identifiers:GetFrameCount)--*/
        List<Int64> GetFrameIdentifiers();

        ///
        // Returns the names of all existing frames.
        ///
        /*--cef()--*/
        List<string> GetFrameNames();

        //
        // Send a message to the specified |target_process|. Returns true if the
        // message was sent successfully.
        //
        /*--cef()--*/
        //virtual bool SendProcessMessage(CefProcessId target_process,
        //                                CefRefPtr<CefProcessMessage> message) =0;

    }
}
