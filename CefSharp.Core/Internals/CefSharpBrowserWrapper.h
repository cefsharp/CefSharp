// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include <include/cef_browser.h>
#include "StringUtils.h"

namespace CefSharp
{
    namespace Internals
    {
        public ref class CefSharpBrowserWrapper : IBrowser
        {
        private:
            MCefRefPtr<CefBrowser> _browser;
            IBrowserAdapter^ _browserAdapter;

        internal:
            CefSharpBrowserWrapper::CefSharpBrowserWrapper(CefRefPtr<CefBrowser> &browser, IBrowserAdapter^ browserAdapter)
                : _browser(browser), _browserAdapter(browserAdapter)
            {
            }

        public:
            ///
            // Returns the browser host object. This method can only be called in the
            // browser process.
            ///
            /*--cef()--*/
            virtual CefRefPtr<CefBrowserHost> GetHost();

            ///
            // Returns true if the browser can navigate backwards.
            ///
            /*--cef()--*/
            virtual bool CanGoBack();

            ///
            // Navigate backwards.
            ///
            /*--cef()--*/
            virtual void GoBack();

            ///
            // Returns true if the browser can navigate forwards.
            ///
            /*--cef()--*/
            virtual bool CanGoForward();

            ///
            // Navigate forwards.
            ///
            /*--cef()--*/
            virtual void GoForward();

            ///
            // Returns true if the browser is currently loading.
            ///
            /*--cef()--*/
            virtual bool IsLoading();

            ///
            // Reload the current page.
            ///
            /*--cef()--*/
            virtual void Reload();

            ///
            // Reload the current page ignoring any cached data.
            ///
            /*--cef()--*/
            virtual void ReloadIgnoreCache();

            ///
            // Stop loading the page.
            ///
            /*--cef()--*/
            virtual void StopLoad();

            ///
            // Returns the globally unique identifier for this browser.
            ///
            /*--cef()--*/
            virtual int GetIdentifier();

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
            virtual bool IsPopup();

            ///
            // Returns true if a document has been loaded in the browser.
            ///
            /*--cef()--*/
            virtual bool HasDocument();

            ///
            // Returns the main (top-level) frame for the browser window.
            ///
            /*--cef()--*/
            virtual IFrame^ GetMainFrame();

            ///
            // Returns the focused frame for the browser window.
            ///
            /*--cef()--*/
            virtual IFrame^ GetFocusedFrame();

            ///
            // Returns the frame with the specified identifier, or NULL if not found.
            ///
            /*--cef(capi_name=get_frame_byident)--*/
            virtual IFrame^ GetFrame(Int64 identifier);

            ///
            // Returns the frame with the specified name, or NULL if not found.
            ///
            /*--cef(optional_param=name)--*/
            virtual IFrame^ GetFrame(String^ name);

            ///
            // Returns the number of frames that currently exist.
            ///
            /*--cef()--*/
            virtual int GetFrameCount();

            ///
            // Returns the identifiers of all existing frames.
            ///
            /*--cef(count_func=identifiers:GetFrameCount)--*/
            virtual List<Int64>^ GetFrameIdentifiers();

            ///
            // Returns the names of all existing frames.
            ///
            /*--cef()--*/
            virtual List<String^>^ GetFrameNames();

            //
            // Send a message to the specified |target_process|. Returns true if the
            // message was sent successfully.
            ///
            /*--cef()--*/
            virtual bool SendProcessMessage(CefProcessId targetProcess, CefRefPtr<CefProcessMessage> message);

        };
    }
}