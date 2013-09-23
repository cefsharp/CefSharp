// Copyright (c) 2012 Marshall A. Greenblatt. All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
//    * Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following disclaimer
// in the documentation and/or other materials provided with the
// distribution.
//    * Neither the name of Google Inc. nor the name Chromium Embedded
// Framework nor the names of its contributors may be used to endorse
// or promote products derived from this software without specific prior
// written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
// ---------------------------------------------------------------------------
//
// The contents of this file must follow a specific format in order to
// support the CEF translator tool. See the translator.README.txt file in the
// tools directory for more information.
//

#ifndef CEF_INCLUDE_CEF_LIFE_SPAN_HANDLER_H_
#define CEF_INCLUDE_CEF_LIFE_SPAN_HANDLER_H_
#pragma once

#include "include/cef_base.h"
#include "include/cef_browser.h"

class CefClient;

///
// Implement this interface to handle events related to browser life span. The
// methods of this class will be called on the UI thread unless otherwise
// indicated.
///
/*--cef(source=client)--*/
class CefLifeSpanHandler : public virtual CefBase {
 public:
  ///
  // Called on the IO thread before a new popup window is created. The |browser|
  // and |frame| parameters represent the source of the popup request. The
  // |target_url| and |target_frame_name| values may be empty if none were
  // specified with the request. The |popupFeatures| structure contains
  // information about the requested popup window. To allow creation of the
  // popup window optionally modify |windowInfo|, |client|, |settings| and
  // |no_javascript_access| and return false. To cancel creation of the popup
  // window return true. The |client| and |settings| values will default to the
  // source browser's values. The |no_javascript_access| value indicates whether
  // the new browser window should be scriptable and in the same process as the
  // source browser.
  /*--cef(optional_param=target_url,optional_param=target_frame_name)--*/
  virtual bool OnBeforePopup(CefRefPtr<CefBrowser> browser,
                             CefRefPtr<CefFrame> frame,
                             const CefString& target_url,
                             const CefString& target_frame_name,
                             const CefPopupFeatures& popupFeatures,
                             CefWindowInfo& windowInfo,
                             CefRefPtr<CefClient>& client,
                             CefBrowserSettings& settings,
                             bool* no_javascript_access) {
    return false;
  }

  ///
  // Called after a new browser is created.
  ///
  /*--cef()--*/
  virtual void OnAfterCreated(CefRefPtr<CefBrowser> browser) {}

  ///
  // Called when a modal window is about to display and the modal loop should
  // begin running. Return false to use the default modal loop implementation or
  // true to use a custom implementation.
  ///
  /*--cef()--*/
  virtual bool RunModal(CefRefPtr<CefBrowser> browser) { return false; }

  ///
  // Called when a browser has recieved a request to close. This may result
  // directly from a call to CefBrowserHost::CloseBrowser() or indirectly if the
  // browser is a top-level OS window created by CEF and the user attempts to
  // close the window. This method will be called after the JavaScript
  // 'onunload' event has been fired. It will not be called for browsers after
  // the associated OS window has been destroyed (for those browsers it is no
  // longer possible to cancel the close).
  //
  // If CEF created an OS window for the browser returning false will send an OS
  // close notification to the browser window's top-level owner (e.g. WM_CLOSE
  // on Windows, performClose: on OS-X and "delete_event" on Linux). If no OS
  // window exists (window rendering disabled) returning false will cause the
  // browser object to be destroyed immediately. Return true if the browser is
  // parented to another window and that other window needs to receive close
  // notification via some non-standard technique.
  //
  // If an application provides its own top-level window it should handle OS
  // close notifications by calling CefBrowserHost::CloseBrowser(false) instead
  // of immediately closing (see the example below). This gives CEF an
  // opportunity to process the 'onbeforeunload' event and optionally cancel the
  // close before DoClose() is called.
  //
  // The CefLifeSpanHandler::OnBeforeClose() method will be called immediately
  // before the browser object is destroyed. The application should only exit
  // after OnBeforeClose() has been called for all existing browsers.
  //
  // If the browser represents a modal window and a custom modal loop
  // implementation was provided in CefLifeSpanHandler::RunModal() this callback
  // should be used to restore the opener window to a usable state.
  //
  // By way of example consider what should happen during window close when the
  // browser is parented to an application-provided top-level OS window.
  // 1.  User clicks the window close button which sends an OS close
  //     notification (e.g. WM_CLOSE on Windows, performClose: on OS-X and
  //     "delete_event" on Linux).
  // 2.  Application's top-level window receives the close notification and:
  //     A. Calls CefBrowserHost::CloseBrowser(false).
  //     B. Cancels the window close.
  // 3.  JavaScript 'onbeforeunload' handler executes and shows the close
  //     confirmation dialog (which can be overridden via
  //     CefJSDialogHandler::OnBeforeUnloadDialog()).
  // 4.  User approves the close.
  // 5.  JavaScript 'onunload' handler executes.
  // 6.  Application's DoClose() handler is called. Application will:
  //     A. Call CefBrowserHost::ParentWindowWillClose() to notify CEF that the
  //        parent window will be closing.
  //     B. Set a flag to indicate that the next close attempt will be allowed.
  //     C. Return false.
  // 7.  CEF sends an OS close notification.
  // 8.  Application's top-level window receives the OS close notification and
  //     allows the window to close based on the flag from #6B.
  // 9.  Browser OS window is destroyed.
  // 10. Application's CefLifeSpanHandler::OnBeforeClose() handler is called and
  //     the browser object is destroyed.
  // 11. Application exits by calling CefQuitMessageLoop() if no other browsers
  //     exist.
  ///
  /*--cef()--*/
  virtual bool DoClose(CefRefPtr<CefBrowser> browser) { return false; }

  ///
  // Called just before a browser is destroyed. Release all references to the
  // browser object and do not attempt to execute any methods on the browser
  // object after this callback returns. If this is a modal window and a custom
  // modal loop implementation was provided in RunModal() this callback should
  // be used to exit the custom modal loop. See DoClose() documentation for
  // additional usage information.
  ///
  /*--cef()--*/
  virtual void OnBeforeClose(CefRefPtr<CefBrowser> browser) {}
};

#endif  // CEF_INCLUDE_CEF_LIFE_SPAN_HANDLER_H_
