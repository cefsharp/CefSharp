// Copyright © 2019 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include <include/cef_browser.h>

namespace CefSharp
{
    namespace BrowserSubprocess
    {
        private ref class Browser : public IBrowser
        {
        private:
            MCefRefPtr<CefBrowser> _browser;
            bool _disposed;

        internal:
            Browser(CefRefPtr<CefBrowser> &browser)
                : _browser(browser)
            {
            }

            !Browser()
            {
                // Release the reference.
                _browser = nullptr;
            }

            ~Browser()
            {
                this->!Browser();
                _disposed = true;
            }

        public:
            virtual property bool IsValid
            {
                bool get();
            }

            ///
            // Returns the browser host object. This method can only be called in the
            // browser process.
            ///
            /*--cef()--*/
            virtual IBrowserHost^ GetHost();

            ///
            // Returns true if the browser can navigate backwards.
            ///
            /*--cef()--*/
            virtual property bool CanGoBack
            {
                bool get();
            }

            ///
            // Navigate backwards.
            ///
            /*--cef()--*/
            virtual void GoBack();

            ///
            // Returns true if the browser can navigate forwards.
            ///
            /*--cef()--*/
            virtual property bool CanGoForward
            {
                bool get();
            }

            ///
            // Navigate forwards.
            ///
            /*--cef()--*/
            virtual void GoForward();

            ///
            // Returns true if the browser is currently loading.
            ///
            /*--cef()--*/
            virtual property bool IsLoading
            {
                bool get();
            }

            virtual void CloseBrowser(bool forceClose);

            ///
            // Reload the current page.
            ///
            /*--cef()--*/
            virtual void Reload(bool ignoreCache);

            ///
            // Stop loading the page.
            ///
            /*--cef()--*/
            virtual void StopLoad();

            ///
            // Returns the globally unique identifier for this browser.
            ///
            /*--cef()--*/
            virtual property int Identifier
            {
                int get();
            }

            ///
            // Returns true if this object is pointing to the same handle as |that|
            // object.
            ///
            /*--cef()--*/
            virtual bool IsSame(IBrowser^ that);

            ///
            // Returns true if the window is a popup window.
            ///
            /*--cef()--*/
            virtual property bool IsPopup
            {
                bool get();
            }

            ///
            // Returns true if a document has been loaded in the browser.
            ///
            /*--cef()--*/
            virtual property bool HasDocument
            {
                bool get();
            }

            ///
            // Returns the main (top-level) frame for the browser window.
            ///
            /*--cef()--*/
            virtual property IFrame^ MainFrame
            {
                IFrame^ get();
            }

            ///
            // Returns the focused frame for the browser window.
            ///
            /*--cef()--*/
            virtual property IFrame^ FocusedFrame
            {
                IFrame^ get();
            }

            ///
            // Returns the frame with the specified identifier, or NULL if not found.
            ///
            /*--cef(capi_name=get_frame_byident)--*/
            virtual IFrame^ GetFrameByIdentifier(String^ identifier);

            ///
            // Returns the frame with the specified name, or NULL if not found.
            ///
            /*--cef(optional_param=name)--*/
            virtual IFrame^ GetFrameByName(String^ name);

            ///
            // Returns the number of frames that currently exist.
            ///
            /*--cef()--*/
            virtual int GetFrameCount();

            ///
            // Returns the identifiers of all existing frames.
            ///
            /*--cef(count_func=identifiers:GetFrameCount)--*/
            virtual List<String^>^ GetFrameIdentifiers();

            ///
            // Returns the names of all existing frames.
            ///
            /*--cef()--*/
            virtual List<String^>^ GetFrameNames();

            virtual IReadOnlyCollection<IFrame^>^ GetAllFrames();

            virtual property bool IsDisposed
            {
                bool get();
            }
        };
    }
}
