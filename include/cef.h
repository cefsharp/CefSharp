// Copyright (c) 2008-2009 Marshall A. Greenblatt. All rights reserved.
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


#ifndef _CEF_H
#define _CEF_H

#include <map>
#include <string>
#include <vector>
#include "cef_ptr.h"
#include "cef_types.h"

class CefBrowser;
class CefBrowserSettings;
class CefDownloadHandler;
class CefFrame;
class CefHandler;
class CefPopupFeatures;
class CefPostData;
class CefPostDataElement;
class CefRequest;
class CefSchemeHandler;
class CefSchemeHandlerFactory;
class CefSettings;
class CefStreamReader;
class CefStreamWriter;
class CefTask;
class CefV8Handler;
class CefV8Value;


// This function should be called once when the application is started to
// initialize CEF.  A return value of true indicates that it succeeded and
// false indicates that it failed.
/*--cef()--*/
bool CefInitialize(const CefSettings& settings,
                   const CefBrowserSettings& browser_defaults);

// This function should be called once before the application exits to shut down
// CEF.
/*--cef()--*/
void CefShutdown();

// Perform message loop processing.  Has no affect if the browser UI loop is
// running in a separate thread.
/*--cef()--*/
void CefDoMessageLoopWork();

// Register a new V8 extension with the specified JavaScript extension code and
// handler. Functions implemented by the handler are prototyped using the
// keyword 'native'. The calling of a native function is restricted to the scope
// in which the prototype of the native function is defined.
//
// Example JavaScript extension code:
//
//   // create the 'example' global object if it doesn't already exist.
//   if (!example)
//     example = {};
//   // create the 'example.test' global object if it doesn't already exist.
//   if (!example.test)
//     example.test = {};
//   (function() {
//     // Define the function 'example.test.myfunction'.
//     example.test.myfunction = function() {
//       // Call CefV8Handler::Execute() with the function name 'MyFunction'
//       // and no arguments.
//       native function MyFunction();
//       return MyFunction();
//     };
//     // Define the getter function for parameter 'example.test.myparam'.
//     example.test.__defineGetter__('myparam', function() {
//       // Call CefV8Handler::Execute() with the function name 'GetMyParam'
//       // and no arguments.
//       native function GetMyParam();
//       return GetMyParam();
//     });
//     // Define the setter function for parameter 'example.test.myparam'.
//     example.test.__defineSetter__('myparam', function(b) {
//       // Call CefV8Handler::Execute() with the function name 'SetMyParam'
//       // and a single argument.
//       native function SetMyParam();
//       if(b) SetMyParam(b);
//     });
//
//     // Extension definitions can also contain normal JavaScript variables
//     // and functions.
//     var myint = 0;
//     example.test.increment = function() {
//       myint += 1;
//       return myint;
//     };
//   })();
//
// Example usage in the page:
//
//   // Call the function.
//   example.test.myfunction();
//   // Set the parameter.
//   example.test.myparam = value;
//   // Get the parameter.
//   value = example.test.myparam;
//   // Call another function.
//   example.test.increment();
//
/*--cef()--*/
bool CefRegisterExtension(const CefString& extension_name,
                          const CefString& javascript_code,
                          CefRefPtr<CefV8Handler> handler);


// Register a custom scheme handler factory for the specified |scheme_name| and
// |host_name|. All URLs beginning with scheme_name://host_name/ can be handled
// by CefSchemeHandler instances returned by the factory. Specify an empty
// |host_name| value to match all host names.
/*--cef()--*/
bool CefRegisterScheme(const CefString& scheme_name,
                       const CefString& host_name,
                       CefRefPtr<CefSchemeHandlerFactory> factory);


typedef cef_thread_id_t CefThreadId;

// CEF maintains multiple internal threads that are used for handling different
// types of tasks. The UI thread creates the browser window and is used for all
// interaction with the WebKit rendering engine and V8 JavaScript engine (The
// UI thread will be the same as the main application thread if CefInitialize()
// was called with a |multi_threaded_message_loop| value of false.) The IO
// thread is used for handling schema and network requests. The FILE thread is
// used for the application cache and other miscellaneous activities. This
// method will return true if called on the specified thread.
/*--cef()--*/
bool CefCurrentlyOn(CefThreadId threadId);

// Post a task for execution on the specified thread.
/*--cef()--*/
bool CefPostTask(CefThreadId threadId, CefRefPtr<CefTask> task);

// Post a task for delayed execution on the specified thread.
/*--cef()--*/
bool CefPostDelayedTask(CefThreadId threadId, CefRefPtr<CefTask> task,
                        long delay_ms);


// Interface defining the the reference count implementation methods. All
// framework classes must implement the CefBase class.
class CefBase
{
public:
  // The AddRef method increments the reference count for the object. It should
  // be called for every new copy of a pointer to a given object. The resulting
  // reference count value is returned and should be used for diagnostic/testing
  // purposes only.
  virtual int AddRef() =0;

  // The Release method decrements the reference count for the object. If the
  // reference count on the object falls to 0, then the object should free
  // itself from memory.  The resulting reference count value is returned and
  // should be used for diagnostic/testing purposes only.
  virtual int Release() =0;

  // Return the current number of references.
  virtual int GetRefCt() = 0;
};


// Bring in platform-specific definitions.
#if defined(_WIN32)
#include "cef_win.h"
#elif defined(__APPLE__)
#include "cef_mac.h"
#elif defined(__linux__)
#include "cef_linux.h"
#endif


// Template that provides atomic implementations of AddRef() and Release()
// along with Lock() and Unlock() methods to protect critical sections of
// code from simultaneous access by multiple threads.
//
// The below example demonstrates how to use the CefThreadSafeBase template.
//
// class MyHandler : public CefThreadSafeBase<CefHandler>
// {
//    CefString m_title;
//
//    virtual RetVal HandleTitleChange(const CefString& title)
//    {
//       Lock();   // Begin protecting code
//       m_title = title;
//       Unlock(); // Done protecting code
//       return RV_HANDLED;
//    }
//    ...
// }
//
template <class ClassName>
class CefThreadSafeBase : public ClassName
{
public:
  CefThreadSafeBase()
  {
    m_dwRef = 0L;
  }
  virtual ~CefThreadSafeBase()
  {
  }

  // Atomic reference increment.
  virtual int AddRef()
  {
    return CefAtomicIncrement(&m_dwRef);
  }

  // Atomic reference decrement.  Delete this object when no references remain.
  virtual int Release()
  {
    int retval = CefAtomicDecrement(&m_dwRef);
    if(retval == 0)
      delete this;
    return retval;
  }

  // Return the current number of references.
  virtual int GetRefCt() { return m_dwRef; }

  // Use the Lock() and Unlock() methods to protect a section of code from
  // simultaneous access by multiple threads.
  void Lock() { m_critsec.Lock(); }
  void Unlock() { m_critsec.Unlock(); }

  // A helper class that acquires the lock for the given CefThreadSafeBase while
  // the CefAutoLock is in scope.
  class AutoLock
  {
  public:
    AutoLock(CefThreadSafeBase* base) : base_(base)
    {
      base_->Lock();
    }
    ~AutoLock()
    {
      base_->Unlock();
    }

  private:
    CefThreadSafeBase* base_;
  };

protected:
  long m_dwRef;
  CefCriticalSection m_critsec;
};


// Class representing a rectangle.
class CefRect : public cef_rect_t
{
public:
  CefRect()
  {
    x = y = width = height = 0;
  }
  CefRect(int x, int y, int width, int height)
  {
    set(x, y, width, height);
  }

  CefRect(const cef_rect_t& r)
  {
    set(r.x, r.y, r.width, r.height);
  }
  CefRect& operator=(const cef_rect_t& r)
  {
      x = r.x;
      y = r.y;
      width = r.width;
      height = r.height;
      return *this;
  }

  bool isEmpty() const { return width <= 0 || height <= 0; }
  void set(int x, int y, int width, int height)
  {
    this->x = x, this->y = y, this->width = width, this->height = height;
  }
};

inline bool operator==(const CefRect& a, const CefRect& b)
{
  return a.x == b.x && a.y == b.y && a.width == b.width && a.height == b.height;
}

inline bool operator!=(const CefRect& a, const CefRect& b)
{
  return !(a == b);
}

// Implement this interface for task execution.
/*--cef(source=client)--*/
class CefTask : public CefBase
{
public:
  // Method that will be executed. |threadId| is the thread executing the call.
  /*--cef()--*/
  virtual void Execute(CefThreadId threadId) =0;
};


// Class used to represent a browser window.  All methods exposed by this class
// should be thread safe.
/*--cef(source=library)--*/
class CefBrowser : public CefBase
{
public:
  // Create a new browser window using the window parameters specified
  // by |windowInfo|. All values will be copied internally and the actual
  // window will be created on the UI thread.  The |popup| parameter should
  // be true if the new window is a popup window. This method call will not
  // block.
  /*--cef()--*/
  static bool CreateBrowser(CefWindowInfo& windowInfo, bool popup,
                            CefRefPtr<CefHandler> handler,
                            const CefString& url);

  // Create a new browser window using the window parameters specified
  // by |windowInfo|. The |popup| parameter should be true if the new window is
  // a popup window. This method call will block and can only be used if
  // the |multi_threaded_message_loop| parameter to CefInitialize() is false.
  /*--cef()--*/
  static CefRefPtr<CefBrowser> CreateBrowserSync(CefWindowInfo& windowInfo,
                                                 bool popup,
                                                 CefRefPtr<CefHandler> handler,
                                                 const CefString& url);

  // Returns true if the browser can navigate backwards.
  /*--cef()--*/
  virtual bool CanGoBack() =0;
  // Navigate backwards.
  /*--cef()--*/
  virtual void GoBack() =0;
  // Returns true if the browser can navigate forwards.
  /*--cef()--*/
  virtual bool CanGoForward() =0;
  // Navigate backwards.
  /*--cef()--*/
  virtual void GoForward() =0;
  // Reload the current page.
  /*--cef()--*/
  virtual void Reload() =0;
  // Reload the current page ignoring any cached data.
  /*--cef()--*/
  virtual void ReloadIgnoreCache() =0;
  // Stop loading the page.
  /*--cef()--*/
  virtual void StopLoad() =0;

  // Set focus for the browser window.  If |enable| is true focus will be set
  // to the window.  Otherwise, focus will be removed.
  /*--cef()--*/
  virtual void SetFocus(bool enable) =0;

  // Retrieve the window handle for this browser.
  /*--cef()--*/
  virtual CefWindowHandle GetWindowHandle() =0;

  // Returns true if the window is a popup window.
  /*--cef()--*/
  virtual bool IsPopup() =0;

  // Returns the handler for this browser.
  /*--cef()--*/
  virtual CefRefPtr<CefHandler> GetHandler() =0;

  // Returns the main (top-level) frame for the browser window.
  /*--cef()--*/
  virtual CefRefPtr<CefFrame> GetMainFrame() =0;

  // Returns the focused frame for the browser window.
  /*--cef()--*/
  virtual CefRefPtr<CefFrame> GetFocusedFrame() =0;

  // Returns the frame with the specified name, or NULL if not found.
  /*--cef()--*/
  virtual CefRefPtr<CefFrame> GetFrame(const CefString& name) =0;

  // Returns the names of all existing frames.
  /*--cef()--*/
  virtual void GetFrameNames(std::vector<CefString>& names) =0;

  // Search for |searchText|. |identifier| can be used to have multiple searches
  // running simultaniously. |forward| indicates whether to search forward or
  // backward within the page. |matchCase| indicates whether the search should
  // be case-sensitive. |findNext| indicates whether this is the first request
  // or a follow-up.
  /*--cef()--*/
  virtual void Find(int identifier, const CefString& searchText,
                    bool forward, bool matchCase, bool findNext) =0;

  // Cancel all searches that are currently going on.
  /*--cef()--*/
  virtual void StopFinding(bool clearSelection) =0;
};


// Class used to represent a frame in the browser window.  All methods exposed
// by this class should be thread safe.
/*--cef(source=library)--*/
class CefFrame : public CefBase
{
public:
  // Execute undo in this frame.
  /*--cef()--*/
  virtual void Undo() =0;
  // Execute redo in this frame.
  /*--cef()--*/
  virtual void Redo() =0;
  // Execute cut in this frame.
  /*--cef()--*/
  virtual void Cut() =0;
  // Execute copy in this frame.
  /*--cef()--*/
  virtual void Copy() =0;
  // Execute paste in this frame.
  /*--cef()--*/
  virtual void Paste() =0;
  // Execute delete in this frame.
  /*--cef(capi_name=del)--*/
  virtual void Delete() =0;
  // Execute select all in this frame.
  /*--cef()--*/
  virtual void SelectAll() =0;

  // Execute printing in the this frame.  The user will be prompted with the
  // print dialog appropriate to the operating system.
  /*--cef()--*/
  virtual void Print() =0;

  // Save this frame's HTML source to a temporary file and open it in the
  // default text viewing application.
  /*--cef()--*/
  virtual void ViewSource() =0;

  // Returns this frame's HTML source as a string.
  /*--cef()--*/
  virtual CefString GetSource() =0;

  // Returns this frame's display text as a string.
  /*--cef()--*/
  virtual CefString GetText() =0;

  // Load the request represented by the |request| object.
  /*--cef()--*/
  virtual void LoadRequest(CefRefPtr<CefRequest> request) =0;

  // Load the specified |url|.
  /*--cef()--*/
  virtual void LoadURL(const CefString& url) =0;
  
  // Load the contents of |string| with the optional dummy target |url|.
  /*--cef()--*/
  virtual void LoadString(const CefString& string,
                          const CefString& url) =0;

  // Load the contents of |stream| with the optional dummy target |url|.
  /*--cef()--*/
  virtual void LoadStream(CefRefPtr<CefStreamReader> stream,
                          const CefString& url) =0;

  // Execute a string of JavaScript code in this frame. The |script_url|
  // parameter is the URL where the script in question can be found, if any.
  // The renderer may request this URL to show the developer the source of the
  // error.  The |start_line| parameter is the base line number to use for error
  // reporting.
  /*--cef()--*/
  virtual void ExecuteJavaScript(const CefString& jsCode, 
                                 const CefString& scriptUrl,
                                 int startLine) =0;

  // Returns true if this is the main frame.
  /*--cef()--*/
  virtual bool IsMain() =0;

  // Returns true if this is the focused frame.
  /*--cef()--*/
  virtual bool IsFocused() =0;

  // Returns this frame's name.
  /*--cef()--*/
  virtual CefString GetName() =0;

  // Return the URL currently loaded in this frame.
  /*--cef()--*/
  virtual CefString GetURL() =0;
};


// Interface that should be implemented to handle events generated by the
// browser window.  All methods exposed by this class should be thread safe.
// Each method in the interface returns a RetVal value.
/*--cef(source=client)--*/
class CefHandler : public CefBase
{
public:
  // Define handler return value types. Returning RV_HANDLED indicates
  // that the implementation completely handled the method and that no further
  // processing is required.  Returning RV_CONTINUE indicates that the
  // implementation did not handle the method and that the default handler
  // should be called.
  typedef cef_retval_t RetVal;

  // Event called before a new window is created. The |parentBrowser| parameter
  // will point to the parent browser window, if any. The |popup| parameter
  // will be true if the new window is a popup window, in which case
  // |popupFeatures| will contain information about the style of popup window
  // requested. If you create the window yourself you should populate the window
  // handle member of |createInfo| and return RV_HANDLED.  Otherwise, return
  // RV_CONTINUE and the framework will create the window.  By default, a newly
  // created window will recieve the same handler as the parent window.  To
  // change the handler for the new window modify the object that |handler|
  // points to.
  /*--cef()--*/
  virtual RetVal HandleBeforeCreated(CefRefPtr<CefBrowser> parentBrowser,
                                     CefWindowInfo& windowInfo, bool popup,
                                     const CefPopupFeatures& popupFeatures,
                                     CefRefPtr<CefHandler>& handler,
                                     CefString& url,
                                     CefBrowserSettings& settings) =0;

  // Event called after a new window is created. The return value is currently
  // ignored.
  /*--cef()--*/
  virtual RetVal HandleAfterCreated(CefRefPtr<CefBrowser> browser) =0;

  // Event called when a frame's address has changed. The return value is
  // currently ignored.
  /*--cef()--*/
  virtual RetVal HandleAddressChange(CefRefPtr<CefBrowser> browser,
                                     CefRefPtr<CefFrame> frame,
                                     const CefString& url) =0;

  // Event called when the page title changes. The return value is currently
  // ignored.
  /*--cef()--*/
  virtual RetVal HandleTitleChange(CefRefPtr<CefBrowser> browser,
                                   const CefString& title) =0;

  // Various browser navigation types supported by chrome.
  typedef cef_handler_navtype_t NavType;

  // Event called before browser navigation. The client has an opportunity to
  // modify the |request| object if desired.  Return RV_HANDLED to cancel
  // navigation.
  /*--cef()--*/
  virtual RetVal HandleBeforeBrowse(CefRefPtr<CefBrowser> browser,
                                    CefRefPtr<CefFrame> frame,
                                    CefRefPtr<CefRequest> request,
                                    NavType navType, bool isRedirect) =0;

  // Event called when the browser begins loading a page.  The |frame| pointer
  // will be empty if the event represents the overall load status and not the
  // load status for a particular frame.  The return value is currently ignored.
  /*--cef()--*/
  virtual RetVal HandleLoadStart(CefRefPtr<CefBrowser> browser,
                                 CefRefPtr<CefFrame> frame) =0;

  // Event called when the browser is done loading a page. The |frame| pointer
  // will be empty if the event represents the overall load status and not the
  // load status for a particular frame. This event will be generated
  // irrespective of whether the request completes successfully. The return
  // value is currently ignored.
  /*--cef()--*/
  virtual RetVal HandleLoadEnd(CefRefPtr<CefBrowser> browser,
                               CefRefPtr<CefFrame> frame) =0;

  // Supported error code values. See net\base\net_error_list.h for complete
  // descriptions of the error codes.
  typedef cef_handler_errorcode_t ErrorCode;

  // Called when the browser fails to load a resource.  |errorCode| is the
  // error code number and |failedUrl| is the URL that failed to load.  To
  // provide custom error text assign the text to |errorText| and return
  // RV_HANDLED.  Otherwise, return RV_CONTINUE for the default error text.
  /*--cef()--*/
  virtual RetVal HandleLoadError(CefRefPtr<CefBrowser> browser,
                                 CefRefPtr<CefFrame> frame,
                                 ErrorCode errorCode,
                                 const CefString& failedUrl,
                                 CefString& errorText) =0;

  // Event called before a resource is loaded.  To allow the resource to load
  // normally return RV_CONTINUE. To redirect the resource to a new url
  // populate the |redirectUrl| value and return RV_CONTINUE.  To specify
  // data for the resource return a CefStream object in |resourceStream|, set
  // |mimeType| to the resource stream's mime type, and return RV_CONTINUE.
  // To cancel loading of the resource return RV_HANDLED.  Any modifications
  // to |request| will be observed.  If the URL in |request| is changed and
  // |redirectUrl| is also set, the URL in |request| will be used.
  /*--cef()--*/
  virtual RetVal HandleBeforeResourceLoad(CefRefPtr<CefBrowser> browser,
                                     CefRefPtr<CefRequest> request,
                                     CefString& redirectUrl,
                                     CefRefPtr<CefStreamReader>& resourceStream,
                                     CefString& mimeType,
                                     int loadFlags) =0;

  // Called when a server indicates via the 'Content-Disposition' header that a
  // response represents a file to download. |mimeType| is the mime type for
  // the download, |fileName| is the suggested target file name and
  // |contentLength| is either the value of the 'Content-Size' header or -1 if
  // no size was provided. Set |handler| to the CefDownloadHandler instance that
  // will recieve the file contents.  Return RV_CONTINUE to download the file
  // or RV_HANDLED to cancel the file download.
  /*--cef()--*/
  virtual RetVal HandleDownloadResponse(CefRefPtr<CefBrowser> browser,
                                    const CefString& mimeType,
                                    const CefString& fileName,
                                    int64 contentLength,
                                    CefRefPtr<CefDownloadHandler>& handler) =0;

  // Structure representing menu information.
  typedef cef_handler_menuinfo_t MenuInfo;

  // Event called before a context menu is displayed.  To cancel display of the
  // default context menu return RV_HANDLED.
  /*--cef()--*/
  virtual RetVal HandleBeforeMenu(CefRefPtr<CefBrowser> browser,
                                  const MenuInfo& menuInfo) =0;

  // Supported menu ID values.
  typedef cef_handler_menuid_t MenuId;

  // Event called to optionally override the default text for a context menu
  // item.  |label| contains the default text and may be modified to substitute
  // alternate text.  The return value is currently ignored.
  /*--cef()--*/
  virtual RetVal HandleGetMenuLabel(CefRefPtr<CefBrowser> browser,
                                    MenuId menuId, CefString& label) =0;

  // Event called when an option is selected from the default context menu.
  // Return RV_HANDLED to cancel default handling of the action.
  /*--cef()--*/
  virtual RetVal HandleMenuAction(CefRefPtr<CefBrowser> browser,
                                  MenuId menuId) =0;

  // Structure representing print options.
  typedef cef_print_options_t CefPrintOptions;
  
  // Event called to allow customization of standard print options before the
  // print dialog is displayed. |printOptions| allows specification of paper
  // size, orientation and margins. Note that the specified margins may be
  // adjusted if they are outside the range supported by the printer. All units
  // are in inches. Return RV_CONTINUE to display the default print options or
  // RV_HANDLED to display the modified |printOptions|.
  /*--cef()--*/
  virtual RetVal HandlePrintOptions(CefRefPtr<CefBrowser> browser,
                                    CefPrintOptions& printOptions) = 0;

  // Event called to format print headers and footers.  |printInfo| contains
  // platform-specific information about the printer context.  |url| is the
  // URL if the currently printing page, |title| is the title of the currently
  // printing page, |currentPage| is the current page number and |maxPages| is
  // the total number of pages.  Six default header locations are provided
  // by the implementation: top left, top center, top right, bottom left,
  // bottom center and bottom right.  To use one of these default locations
  // just assign a string to the appropriate variable.  To draw the header
  // and footer yourself return RV_HANDLED.  Otherwise, populate the approprate
  // variables and return RV_CONTINUE.
  /*--cef()--*/
  virtual RetVal HandlePrintHeaderFooter(CefRefPtr<CefBrowser> browser,
                                         CefRefPtr<CefFrame> frame,
                                         CefPrintInfo& printInfo,
                                         const CefString& url,
                                         const CefString& title,
                                         int currentPage, int maxPages,
                                         CefString& topLeft,
                                         CefString& topCenter,
                                         CefString& topRight,
                                         CefString& bottomLeft,
                                         CefString& bottomCenter,
                                         CefString& bottomRight) =0;

  // Run a JS alert message.  Return RV_CONTINUE to display the default alert
  // or RV_HANDLED if you displayed a custom alert.
  /*--cef()--*/
  virtual RetVal HandleJSAlert(CefRefPtr<CefBrowser> browser,
                               CefRefPtr<CefFrame> frame,
                               const CefString& message) =0;

  // Run a JS confirm request.  Return RV_CONTINUE to display the default alert
  // or RV_HANDLED if you displayed a custom alert.  If you handled the alert
  // set |retval| to true if the user accepted the confirmation.
  /*--cef()--*/
  virtual RetVal HandleJSConfirm(CefRefPtr<CefBrowser> browser,
                                 CefRefPtr<CefFrame> frame,
                                 const CefString& message, bool& retval) =0;

  // Run a JS prompt request.  Return RV_CONTINUE to display the default prompt
  // or RV_HANDLED if you displayed a custom prompt.  If you handled the prompt
  // set |retval| to true if the user accepted the prompt and request and
  // |result| to the resulting value.
  /*--cef()--*/
  virtual RetVal HandleJSPrompt(CefRefPtr<CefBrowser> browser,
                                CefRefPtr<CefFrame> frame,
                                const CefString& message,
                                const CefString& defaultValue,
                                bool& retval,
                                CefString& result) =0;

  // Event called for adding values to a frame's JavaScript 'window' object. The
  // return value is currently ignored.
  /*--cef()--*/
  virtual RetVal HandleJSBinding(CefRefPtr<CefBrowser> browser,
                                 CefRefPtr<CefFrame> frame,
                                 CefRefPtr<CefV8Value> object) =0;

  // Called just before a window is closed. The return value is currently
  // ignored.
  /*--cef()--*/
  virtual RetVal HandleBeforeWindowClose(CefRefPtr<CefBrowser> browser) =0;

  // Called when the browser component is about to loose focus. For instance,
  // if focus was on the last HTML element and the user pressed the TAB key.
  // The return value is currently ignored.
  /*--cef()--*/
  virtual RetVal HandleTakeFocus(CefRefPtr<CefBrowser> browser,
                                 bool reverse) =0;

  // Called when the browser component is requesting focus. |isWidget| will be
  // true if the focus is requested for a child widget of the browser window.
  // Return RV_CONTINUE to allow the focus to be set or RV_HANDLED to cancel
  // setting the focus.
  /*--cef()--*/
  virtual RetVal HandleSetFocus(CefRefPtr<CefBrowser> browser,
                                bool isWidget) =0;

  // Supported keyboard event types.
  typedef cef_handler_keyevent_type_t KeyEventType;

  // Called when the browser component receives a keyboard event.
  // |type| is the type of keyboard event (see |KeyEventType|).
  // |code| is the windows scan-code for the event.
  // |modifiers| is a set of bit-flags describing any pressed modifier keys.
  // |isSystemKey| is set if Windows considers this a 'system key' message;
  //   (see http://msdn.microsoft.com/en-us/library/ms646286(VS.85).aspx)
  // Return RV_HANDLED if the keyboard event was handled or RV_CONTINUE
  // to allow the browser component to handle the event.
  /*--cef()--*/
  virtual RetVal HandleKeyEvent(CefRefPtr<CefBrowser> browser,
                                KeyEventType type,
                                int code,
                                int modifiers,
                                bool isSystemKey) =0;

  // Event called when the browser is about to display a tooltip.  |text|
  // contains the text that will be displayed in the tooltip.  To handle
  // the display of the tooltip yourself return RV_HANDLED.  Otherwise,
  // you can optionally modify |text| and then return RV_CONTINUE to allow
  // the browser to display the tooltip.
  /*--cef()--*/
  virtual RetVal HandleTooltip(CefRefPtr<CefBrowser> browser,
                               CefString& text) =0;

  // Called to display a console message. Return RV_HANDLED to stop the message
  // from being output to the console.
  /*--cef()--*/
  virtual RetVal HandleConsoleMessage(CefRefPtr<CefBrowser> browser,
                                      const CefString& message,
                                      const CefString& source, int line) =0;

  // Called to report find results returned by CefBrowser::Find(). |identifer|
  // is the identifier passed to CefBrowser::Find(), |count| is the number of
  // matches currently identified, |selectionRect| is the location of where the
  // match was found (in window coordinates), |activeMatchOrdinal| is the
  // current position in the search results, and |finalUpdate| is true if this
  // is the last find notification.  The return value is currently ignored.
  /*--cef()--*/
  virtual RetVal HandleFindResult(CefRefPtr<CefBrowser> browser,
                                  int identifier, int count,
                                  const CefRect& selectionRect,
                                  int activeMatchOrdinal, bool finalUpdate) =0;
};


// Class used to represent a web request.
/*--cef(source=library)--*/
class CefRequest : public CefBase
{
public:
  typedef std::map<CefString,CefString> HeaderMap;

  // Create a new CefRequest object.
  /*--cef()--*/
  static CefRefPtr<CefRequest> CreateRequest();

  // Fully qualified URL to load.
  /*--cef()--*/
  virtual CefString GetURL() =0;
  /*--cef()--*/
  virtual void SetURL(const CefString& url) =0;

  // Optional request method type, defaulting to POST if post data is provided
  // and GET otherwise.
  /*--cef()--*/
  virtual CefString GetMethod() =0;
  /*--cef()--*/
  virtual void SetMethod(const CefString& method) =0;

  // Optional post data.
  /*--cef()--*/
  virtual CefRefPtr<CefPostData> GetPostData() =0;
  /*--cef()--*/
  virtual void SetPostData(CefRefPtr<CefPostData> postData) =0;

  // Optional header values.
  /*--cef()--*/
  virtual void GetHeaderMap(HeaderMap& headerMap) =0;
  /*--cef()--*/
  virtual void SetHeaderMap(const HeaderMap& headerMap) =0;

  // Set all values at one time.
  /*--cef()--*/
  virtual void Set(const CefString& url,
                   const CefString& method,
                   CefRefPtr<CefPostData> postData,
                   const HeaderMap& headerMap) =0;
};


// Class used to represent post data for a web request.
/*--cef(source=library)--*/
class CefPostData : public CefBase
{
public:
  typedef std::vector<CefRefPtr<CefPostDataElement> > ElementVector;

  // Create a new CefPostData object.
  /*--cef()--*/
  static CefRefPtr<CefPostData> CreatePostData();

  // Returns the number of existing post data elements.
  /*--cef()--*/
  virtual size_t GetElementCount() =0;

  // Retrieve the post data elements.
  /*--cef()--*/
  virtual void GetElements(ElementVector& elements) =0;

  // Remove the specified post data element.  Returns true if the removal
  // succeeds.
  /*--cef()--*/
  virtual bool RemoveElement(CefRefPtr<CefPostDataElement> element) =0;

  // Add the specified post data element.  Returns true if the add succeeds.
  /*--cef()--*/
  virtual bool AddElement(CefRefPtr<CefPostDataElement> element) =0;

  // Remove all existing post data elements.
  /*--cef()--*/
  virtual void RemoveElements() =0;
};


// Class used to represent a single element in the request post data.
/*--cef(source=library)--*/
class CefPostDataElement : public CefBase
{
public:
  // Post data elements may represent either bytes or files.
  typedef cef_postdataelement_type_t Type;

  // Create a new CefPostDataElement object.
  /*--cef()--*/
  static CefRefPtr<CefPostDataElement> CreatePostDataElement();

  // Remove all contents from the post data element.
  /*--cef()--*/
  virtual void SetToEmpty() =0;

  // The post data element will represent a file.
  /*--cef()--*/
  virtual void SetToFile(const CefString& fileName) =0;

  // The post data element will represent bytes.  The bytes passed
  // in will be copied.
  /*--cef()--*/
  virtual void SetToBytes(size_t size, const void* bytes) =0;

  // Return the type of this post data element.
  /*--cef()--*/
  virtual Type GetType() =0;

  // Return the file name.
  /*--cef()--*/
  virtual CefString GetFile() =0;

  // Return the number of bytes.
  /*--cef()--*/
  virtual size_t GetBytesCount() =0;

  // Read up to |size| bytes into |bytes| and return the number of bytes
  // actually read.
  /*--cef()--*/
  virtual size_t GetBytes(size_t size, void* bytes) =0;
};


// Interface the client can implement to provide a custom stream reader.
/*--cef(source=client)--*/
class CefReadHandler : public CefBase
{
public:
  // Read raw binary data.
  /*--cef()--*/
  virtual size_t Read(void* ptr, size_t size, size_t n) =0;
	
  // Seek to the specified offset position. |whence| may be any one of
  // SEEK_CUR, SEEK_END or SEEK_SET.
  /*--cef()--*/
  virtual int Seek(long offset, int whence) =0;
	
  // Return the current offset position.
  /*--cef()--*/
  virtual long Tell() =0;

  // Return non-zero if at end of file.
  /*--cef()--*/
  virtual int Eof() =0;
};


// Class used to read data from a stream.
/*--cef(source=library)--*/
class CefStreamReader : public CefBase
{
public:
  // Create a new CefStreamReader object.
  /*--cef()--*/
  static CefRefPtr<CefStreamReader> CreateForFile(const CefString& fileName);
  /*--cef()--*/
  static CefRefPtr<CefStreamReader> CreateForData(void* data, size_t size);
   /*--cef()--*/
  static CefRefPtr<CefStreamReader> CreateForHandler(CefRefPtr<CefReadHandler> handler);

  // Read raw binary data.
  /*--cef()--*/
  virtual size_t Read(void* ptr, size_t size, size_t n) =0;
	
  // Seek to the specified offset position. |whence| may be any one of
  // SEEK_CUR, SEEK_END or SEEK_SET. Returns zero on success and non-zero on
  // failure.
  /*--cef()--*/
  virtual int Seek(long offset, int whence) =0;
	
  // Return the current offset position.
  /*--cef()--*/
  virtual long Tell() =0;

  // Return non-zero if at end of file.
  /*--cef()--*/
  virtual int Eof() =0;
};


// Interface the client can implement to provide a custom stream writer.
/*--cef(source=client)--*/
class CefWriteHandler : public CefBase
{
public:
   // Write raw binary data.
  /*--cef()--*/
  virtual size_t Write(const void* ptr, size_t size, size_t n) =0;
	
  // Seek to the specified offset position. |whence| may be any one of
  // SEEK_CUR, SEEK_END or SEEK_SET.
  /*--cef()--*/
  virtual int Seek(long offset, int whence) =0;
	
  // Return the current offset position.
  /*--cef()--*/
  virtual long Tell() =0;

  // Flush the stream.
  /*--cef()--*/
  virtual int Flush() =0;
};


// Class used to write data to a stream.
/*--cef(source=library)--*/
class CefStreamWriter : public CefBase
{
public:
  // Create a new CefStreamWriter object.
  /*--cef()--*/
  static CefRefPtr<CefStreamWriter> CreateForFile(const CefString& fileName);
  /*--cef()--*/
  static CefRefPtr<CefStreamWriter> CreateForHandler(CefRefPtr<CefWriteHandler> handler);

  // Write raw binary data.
  /*--cef()--*/
  virtual size_t Write(const void* ptr, size_t size, size_t n) =0;
	
  // Seek to the specified offset position. |whence| may be any one of
  // SEEK_CUR, SEEK_END or SEEK_SET.
  /*--cef()--*/
  virtual int Seek(long offset, int whence) =0;
	
  // Return the current offset position.
  /*--cef()--*/
  virtual long Tell() =0;

  // Flush the stream.
  /*--cef()--*/
  virtual int Flush() =0;
};


typedef std::vector<CefRefPtr<CefV8Value> > CefV8ValueList;

// Interface that should be implemented to handle V8 function calls.
/*--cef(source=client)--*/
class CefV8Handler : public CefBase
{
public:
  // Execute with the specified argument list and return value.  Return true if
  // the method was handled.
  /*--cef()--*/
  virtual bool Execute(const CefString& name,
                       CefRefPtr<CefV8Value> object,
                       const CefV8ValueList& arguments,
                       CefRefPtr<CefV8Value>& retval,
                       CefString& exception) =0;
};


// Class representing a V8 value.
/*--cef(source=library)--*/
class CefV8Value : public CefBase
{
public:
  // Create a new CefV8Value object of the specified type.  These methods
  // should only be called from within the JavaScript context -- either in a
  // CefV8Handler::Execute() callback or a CefHandler::HandleJSBinding()
  // callback.
  /*--cef()--*/
  static CefRefPtr<CefV8Value> CreateUndefined();
  /*--cef()--*/
  static CefRefPtr<CefV8Value> CreateNull();
  /*--cef()--*/
  static CefRefPtr<CefV8Value> CreateBool(bool value);
  /*--cef()--*/
  static CefRefPtr<CefV8Value> CreateInt(int value);
  /*--cef()--*/
  static CefRefPtr<CefV8Value> CreateDouble(double value);
  /*--cef()--*/
  static CefRefPtr<CefV8Value> CreateString(const CefString& value);
  /*--cef()--*/
  static CefRefPtr<CefV8Value> CreateObject(CefRefPtr<CefBase> user_data);
  /*--cef()--*/
  static CefRefPtr<CefV8Value> CreateArray();
  /*--cef()--*/
  static CefRefPtr<CefV8Value> CreateFunction(const CefString& name,
                                              CefRefPtr<CefV8Handler> handler);

  // Check the value type.
  /*--cef()--*/
  virtual bool IsUndefined() =0;
  /*--cef()--*/
  virtual bool IsNull() =0;
  /*--cef()--*/
  virtual bool IsBool() =0;
  /*--cef()--*/
  virtual bool IsInt() =0;
  /*--cef()--*/
  virtual bool IsDouble() =0;
  /*--cef()--*/
  virtual bool IsString() =0;
  /*--cef()--*/
  virtual bool IsObject() =0;
  /*--cef()--*/
  virtual bool IsArray() =0;
  /*--cef()--*/
  virtual bool IsFunction() =0;
  
  // Return a primitive value type.  The underlying data will be converted to
  // the requested type if necessary.
  /*--cef()--*/
  virtual bool GetBoolValue() =0;
  /*--cef()--*/
  virtual int GetIntValue() =0;
  /*--cef()--*/
  virtual double GetDoubleValue() =0;
  /*--cef()--*/
  virtual CefString GetStringValue() =0;


  // OBJECT METHODS - These methods are only available on objects. Arrays and
  // functions are also objects. String- and integer-based keys can be used
  // interchangably with the framework converting between them as necessary.
  // Keys beginning with "Cef::" and "v8::" are reserved by the system.

  // Returns true if the object has a value with the specified identifier.
  /*--cef(capi_name=has_value_bykey)--*/
  virtual bool HasValue(const CefString& key) =0;
  /*--cef(capi_name=has_value_byindex)--*/
  virtual bool HasValue(int index) =0;

  // Delete the value with the specified identifier.
  /*--cef(capi_name=delete_value_bykey)--*/
  virtual bool DeleteValue(const CefString& key) =0;
  /*--cef(capi_name=delete_value_byindex)--*/
  virtual bool DeleteValue(int index) =0;

  // Returns the value with the specified identifier.
  /*--cef(capi_name=get_value_bykey)--*/
  virtual CefRefPtr<CefV8Value> GetValue(const CefString& key) =0;
  /*--cef(capi_name=get_value_byindex)--*/
  virtual CefRefPtr<CefV8Value> GetValue(int index) =0;

  // Associate value with the specified identifier.
  /*--cef(capi_name=set_value_bykey)--*/
  virtual bool SetValue(const CefString& key, CefRefPtr<CefV8Value> value) =0;
  /*--cef(capi_name=set_value_byindex)--*/
  virtual bool SetValue(int index, CefRefPtr<CefV8Value> value) =0;

  // Read the keys for the object's values into the specified vector. Integer-
  // based keys will also be returned as strings.
  /*--cef()--*/
  virtual bool GetKeys(std::vector<CefString>& keys) =0;

  // Returns the user data, if any, specified when the object was created.
  /*--cef()--*/
  virtual CefRefPtr<CefBase> GetUserData() =0;


  // ARRAY METHODS - These methods are only available on arrays.

  // Returns the number of elements in the array.
  /*--cef()--*/
  virtual int GetArrayLength() =0;


  // FUNCTION METHODS - These methods are only available on functions.

  // Returns the function name.
  /*--cef()--*/
  virtual CefString GetFunctionName() =0;

  // Returns the function handler or NULL if not a CEF-created function.
  /*--cef()--*/
  virtual CefRefPtr<CefV8Handler> GetFunctionHandler() =0;
  
  // Execute the function.
  /*--cef()--*/
  virtual bool ExecuteFunction(CefRefPtr<CefV8Value> object,
                               const CefV8ValueList& arguments,
                               CefRefPtr<CefV8Value>& retval,
                               CefString& exception) =0;
};


// Class that creates CefSchemeHandler instances.
/*--cef(source=client)--*/
class CefSchemeHandlerFactory : public CefBase
{
public:
  // Return a new scheme handler instance to handle the request.
  /*--cef()--*/
  virtual CefRefPtr<CefSchemeHandler> Create() =0;
};


// Class used to represent a custom scheme handler interface.
/*--cef(source=client)--*/
class CefSchemeHandler : public CefBase
{
public:
  // Process the request. All response generation should take place in this
  // method. If there is no response set |response_length| to zero and
  // ReadResponse() will not be called. If the response length is not known then
  // set |response_length| to -1 and ReadResponse() will be called until it
  // returns false or until the value of |bytes_read| is set to 0. Otherwise,
  // set |response_length| to a positive value and ReadResponse() will be called
  // until it returns false, the value of |bytes_read| is set to 0 or the
  // specified number of bytes have been read. If there is a response set
  // |mime_type| to the mime type for the response.
  /*--cef()--*/
  virtual bool ProcessRequest(CefRefPtr<CefRequest> request,
                              CefString& mime_type, int* response_length) =0;

  // Cancel processing of the request.
  /*--cef()--*/
  virtual void Cancel() =0;

  // Copy up to |bytes_to_read| bytes into |data_out|. If the copy succeeds
  // set |bytes_read| to the number of bytes copied and return true. If the
  // copy fails return false and ReadResponse() will not be called again.
  /*--cef()--*/
  virtual bool ReadResponse(void* data_out, int bytes_to_read,
                            int* bytes_read) =0;
};


// Class used to handle file downloads.
/*--cef(source=client)--*/
class CefDownloadHandler : public CefBase
{
public:
  // A portion of the file contents have been received. This method will be
  // called multiple times until the download is complete. Return |true| to
  // continue receiving data and |false| to cancel.
  /*--cef()--*/
  virtual bool ReceivedData(void* data, int data_size) =0;

  // The download is complete.
  /*--cef()--*/
  virtual void Complete() =0;
};


// Class that supports the reading of XML data via the libxml streaming API.
/*--cef(source=library)--*/
class CefXmlReader : public CefBase
{
public:
  typedef cef_xml_encoding_type_t EncodingType;
  typedef cef_xml_node_type_t NodeType;

  // Create a new CefXmlReader object. The returned object's methods can only
  // be called from the thread that created the object.
  /*--cef()--*/
  static CefRefPtr<CefXmlReader> Create(CefRefPtr<CefStreamReader> stream,
                                        EncodingType encodingType,
                                        const CefString& URI);

  // Moves the cursor to the next node in the document. This method must be
  // called at least once to set the current cursor position. Returns true if
  // the cursor position was set successfully.
  /*--cef()--*/
  virtual bool MoveToNextNode() =0;

  // Close the document. This should be called directly to ensure that cleanup
  // occurs on the correct thread.
  /*--cef()--*/
  virtual bool Close() =0;

  // Returns true if an error has been reported by the XML parser.
  /*--cef()--*/
  virtual bool HasError() =0;

  // Returns the error string.
  /*--cef()--*/
  virtual CefString GetError() =0;


  // The below methods retrieve data for the node at the current cursor
  // position.

  // Returns the node type.
  /*--cef()--*/
  virtual NodeType GetType() =0;

  // Returns the node depth. Depth starts at 0 for the root node.
  /*--cef()--*/
  virtual int GetDepth() =0;

  // Returns the local name. See
  // http://www.w3.org/TR/REC-xml-names/#NT-LocalPart for additional details.
  /*--cef()--*/
  virtual CefString GetLocalName() =0;

  // Returns the namespace prefix. See http://www.w3.org/TR/REC-xml-names/ for
  // additional details.
  /*--cef()--*/
  virtual CefString GetPrefix() =0;

  // Returns the qualified name, equal to (Prefix:)LocalName. See
  // http://www.w3.org/TR/REC-xml-names/#ns-qualnames for additional details.
  /*--cef()--*/
  virtual CefString GetQualifiedName() =0;

  // Returns the URI defining the namespace associated with the node. See
  // http://www.w3.org/TR/REC-xml-names/ for additional details.
  /*--cef()--*/
  virtual CefString GetNamespaceURI() =0;

  // Returns the base URI of the node. See http://www.w3.org/TR/xmlbase/ for
  // additional details.
  /*--cef()--*/
  virtual CefString GetBaseURI() =0;

  // Returns the xml:lang scope within which the node resides. See
  // http://www.w3.org/TR/REC-xml/#sec-lang-tag for additional details.
  /*--cef()--*/
  virtual CefString GetXmlLang() =0;

  // Returns true if the node represents an empty element. <a/> is considered
  // empty but <a></a> is not.
  /*--cef()--*/
  virtual bool IsEmptyElement() =0;

  // Returns true if the node has a text value.
  /*--cef()--*/
  virtual bool HasValue() =0;

  // Returns the text value.
  /*--cef()--*/
  virtual CefString GetValue() =0;

  // Returns true if the node has attributes.
  /*--cef()--*/
  virtual bool HasAttributes() =0;

  // Returns the number of attributes.
  /*--cef()--*/
  virtual size_t GetAttributeCount() =0;

  // Returns the value of the attribute at the specified 0-based index.
  /*--cef(capi_name=get_attribute_byindex)--*/
  virtual CefString GetAttribute(int index) =0;

  // Returns the value of the attribute with the specified qualified name.
  /*--cef(capi_name=get_attribute_byqname)--*/
  virtual CefString GetAttribute(const CefString& qualifiedName) =0;

  // Returns the value of the attribute with the specified local name and
  // namespace URI.
  /*--cef(capi_name=get_attribute_bylname)--*/
  virtual CefString GetAttribute(const CefString& localName,
                                    const CefString& namespaceURI) =0;

  // Returns an XML representation of the current node's children.
  /*--cef()--*/
  virtual CefString GetInnerXml() =0;

  // Returns an XML representation of the current node including its children.
  /*--cef()--*/
  virtual CefString GetOuterXml() =0;

  // Returns the line number for the current node.
  /*--cef()--*/
  virtual int GetLineNumber() =0;

  
  // Attribute nodes are not traversed by default. The below methods can be
  // used to move the cursor to an attribute node. MoveToCarryingElement() can
  // be called afterwards to return the cursor to the carrying element. The
  // depth of an attribute node will be 1 + the depth of the carrying element.

  // Moves the cursor to the attribute at the specified 0-based index. Returns
  // true if the cursor position was set successfully.
  /*--cef(capi_name=move_to_attribute_byindex)--*/
  virtual bool MoveToAttribute(int index) =0;

  // Moves the cursor to the attribute with the specified qualified name.
  // Returns true if the cursor position was set successfully.
  /*--cef(capi_name=move_to_attribute_byqname)--*/
  virtual bool MoveToAttribute(const CefString& qualifiedName) =0;

  // Moves the cursor to the attribute with the specified local name and
  // namespace URI. Returns true if the cursor position was set successfully.
  /*--cef(capi_name=move_to_attribute_bylname)--*/
  virtual bool MoveToAttribute(const CefString& localName,
                               const CefString& namespaceURI) =0;

  // Moves the cursor to the first attribute in the current element. Returns
  // true if the cursor position was set successfully.
  /*--cef()--*/
  virtual bool MoveToFirstAttribute() =0;

  // Moves the cursor to the next attribute in the current element. Returns
  // true if the cursor position was set successfully.
  /*--cef()--*/
  virtual bool MoveToNextAttribute() =0;

  // Moves the cursor back to the carrying element. Returns true if the cursor
  // position was set successfully.
  /*--cef()--*/
  virtual bool MoveToCarryingElement() =0;
};


// Class that supports the reading of zip archives via the zlib unzip API.
/*--cef(source=library)--*/
class CefZipReader : public CefBase
{
public:
  // Create a new CefZipReader object. The returned object's methods can only
  // be called from the thread that created the object.
  /*--cef()--*/
  static CefRefPtr<CefZipReader> Create(CefRefPtr<CefStreamReader> stream);

  // Moves the cursor to the first file in the archive. Returns true if the
  // cursor position was set successfully.
  /*--cef()--*/
  virtual bool MoveToFirstFile() =0;

  // Moves the cursor to the next file in the archive. Returns true if the
  // cursor position was set successfully.
  /*--cef()--*/
  virtual bool MoveToNextFile() =0;

  // Moves the cursor to the specified file in the archive. If |caseSensitive|
  // is true then the search will be case sensitive. Returns true if the cursor
  // position was set successfully. 
  /*--cef()--*/
  virtual bool MoveToFile(const CefString& fileName, bool caseSensitive) =0;

  // Closes the archive. This should be called directly to ensure that cleanup
  // occurs on the correct thread.
  /*--cef()--*/
  virtual bool Close() =0;


  // The below methods act on the file at the current cursor position.

  // Returns the name of the file.
  /*--cef()--*/
  virtual CefString GetFileName() =0;

  // Returns the uncompressed size of the file.
  /*--cef()--*/
  virtual long GetFileSize() =0;

  // Returns the last modified timestamp for the file.
  /*--cef()--*/
  virtual time_t GetFileLastModified() =0;

  // Opens the file for reading of uncompressed data. A read password may
  // optionally be specified.
  /*--cef()--*/
  virtual bool OpenFile(const CefString& password) =0;

  // Closes the file.
  /*--cef()--*/
  virtual bool CloseFile() =0;

  // Read uncompressed file contents into the specified buffer. Returns < 0 if
  // an error occurred, 0 if at the end of file, or the number of bytes read.
  /*--cef()--*/
  virtual int ReadFile(void* buffer, size_t bufferSize) =0;

  // Returns the current offset in the uncompressed file contents.
  /*--cef()--*/
  virtual long Tell() =0;

  // Returns true if at end of the file contents.
  /*--cef()--*/
  virtual bool Eof() =0;
};


// Class representing popup window features.
class CefPopupFeatures : public cef_popup_features_t
{
public:
  CefPopupFeatures()
  {
    Init();
  }
  virtual ~CefPopupFeatures()
  {
    Reset();
  }

  CefPopupFeatures(const CefPopupFeatures& r)
  {
    Init();
    *this = r;
  }
  CefPopupFeatures(const cef_popup_features_t& r)
  {
    Init();
    *this = r;
  }

  void Reset()
  {
    if(additionalFeatures)
      cef_string_list_free(additionalFeatures);
  }

  void Attach(const cef_popup_features_t& r)
  {
    Reset();
    *static_cast<cef_popup_features_t*>(this) = r;
  }

  void Detach()
  {
    Init();
  }

  CefPopupFeatures& operator=(const CefPopupFeatures& r)
  {
    return operator=(static_cast<const cef_popup_features_t&>(r));
  }

  CefPopupFeatures& operator=(const cef_popup_features_t& r)
  {
    if(additionalFeatures)
      cef_string_list_free(additionalFeatures);
    additionalFeatures = r.additionalFeatures ?
        cef_string_list_copy(r.additionalFeatures) : NULL;

    x = r.x;
    xSet = r.xSet;
    y = r.y;
    ySet = r.ySet;
    width = r.width;
    widthSet = r.widthSet;
    height = r.height;
    heightSet = r.heightSet;
    menuBarVisible = r.menuBarVisible;
    statusBarVisible = r.statusBarVisible;
    toolBarVisible = r.toolBarVisible;
    locationBarVisible = r.locationBarVisible;
    scrollbarsVisible = r.scrollbarsVisible;
    resizable = r.resizable;
    fullscreen = r.fullscreen;
    dialog = r.dialog;
    return *this;
  }

protected:
  void Init()
  {
    x = 0;
    xSet = false;
    y = 0;
    ySet = false;
    width = 0;
    widthSet = false;
    height = 0;
    heightSet = false;

    menuBarVisible = true;
    statusBarVisible = true;
    toolBarVisible = true;
    locationBarVisible = true;
    scrollbarsVisible = true;
    resizable = true;

    fullscreen = false;
    dialog = false;
    additionalFeatures = NULL;
  }
};


// Class representing initialization settings.
class CefSettings : public cef_settings_t
{
public:
  CefSettings()
  {
    Init();
  }
  virtual ~CefSettings()
  {
    Reset();
  }

  CefSettings(const CefSettings& r)
  {
    Init();
    *this = r;
  }
  CefSettings(const cef_settings_t& r)
  {
    Init();
    *this = r;
  }

  void Reset()
  {
    cef_string_clear(&cache_path);
    cef_string_clear(&user_agent);
    cef_string_clear(&product_version);
    cef_string_clear(&locale);
    if(extra_plugin_paths)
      cef_string_list_free(extra_plugin_paths);
    Init();
  }

  void Attach(const cef_settings_t& r)
  {
    Reset();
    *static_cast<cef_settings_t*>(this) = r;
  }

  void Detach()
  {
    Init();
  }

  CefSettings& operator=(const CefSettings& r)
  {
    return operator=(static_cast<const cef_settings_t&>(r));
  }

  CefSettings& operator=(const cef_settings_t& r)
  {
    multi_threaded_message_loop = r.multi_threaded_message_loop;

    cef_string_copy(r.cache_path.str, r.cache_path.length, &cache_path);
    cef_string_copy(r.user_agent.str, r.user_agent.length, &user_agent);
    cef_string_copy(r.product_version.str, r.product_version.length,
        &product_version);
    cef_string_copy(r.locale.str, r.locale.length, &locale);

    if(extra_plugin_paths)
      cef_string_list_free(extra_plugin_paths);
    extra_plugin_paths = r.extra_plugin_paths ?
        cef_string_list_copy(r.extra_plugin_paths) : NULL;

    return *this;
  }

protected:
  void Init()
  {
    memset(static_cast<cef_settings_t*>(this), 0, sizeof(cef_settings_t));
    size = sizeof(cef_settings_t);
  }
};


// Class representing browser initialization settings.
class CefBrowserSettings : public cef_browser_settings_t
{
public:
  CefBrowserSettings()
  {
    Init();
  }
  virtual ~CefBrowserSettings()
  {
    Reset();
  }

  CefBrowserSettings(const CefBrowserSettings& r)
  {
    Init();
    *this = r;
  }
  CefBrowserSettings(const cef_browser_settings_t& r)
  {
    Init();
    *this = r;
  }

  void Reset()
  {
    cef_string_clear(&standard_font_family);
    cef_string_clear(&fixed_font_family);
    cef_string_clear(&serif_font_family);
    cef_string_clear(&sans_serif_font_family);
    cef_string_clear(&cursive_font_family);
    cef_string_clear(&fantasy_font_family);
    cef_string_clear(&default_encoding);
    cef_string_clear(&user_style_sheet_location);
    Init();
  }

  void Attach(const cef_browser_settings_t& r)
  {
    Reset();
    *static_cast<cef_browser_settings_t*>(this) = r;
  }

  void Detach()
  {
    Init();
  }

  CefBrowserSettings& operator=(const CefBrowserSettings& r)
  {
    return operator=(static_cast<const cef_browser_settings_t&>(r));
  }

  CefBrowserSettings& operator=(const cef_browser_settings_t& r)
  {
    drag_drop_disabled = r.drag_drop_disabled;

    cef_string_copy(r.standard_font_family.str, r.standard_font_family.length,
        &standard_font_family);
    cef_string_copy(r.fixed_font_family.str, r.fixed_font_family.length,
        &fixed_font_family);
    cef_string_copy(r.serif_font_family.str, r.serif_font_family.length,
        &serif_font_family);
    cef_string_copy(r.sans_serif_font_family.str,
        r.sans_serif_font_family.length, &sans_serif_font_family);
    cef_string_copy(r.cursive_font_family.str, r.cursive_font_family.length,
        &cursive_font_family);
    cef_string_copy(r.fantasy_font_family.str, r.fantasy_font_family.length,
        &fantasy_font_family);

    default_font_size = r.default_font_size;
    default_fixed_font_size = r.default_fixed_font_size;
    minimum_font_size = r.minimum_font_size;
    minimum_logical_font_size = r.minimum_logical_font_size;
    remote_fonts_disabled = r.remote_fonts_disabled;

    cef_string_copy(r.default_encoding.str, r.default_encoding.length,
        &default_encoding);

    encoding_detector_enabled = r.encoding_detector_enabled;
    javascript_disabled = r.javascript_disabled;
    javascript_open_windows_disallowed = r.javascript_open_windows_disallowed;
    javascript_close_windows_disallowed = r.javascript_close_windows_disallowed;
    javascript_access_clipboard_disallowed =
        r.javascript_access_clipboard_disallowed;
    dom_paste_disabled = r.dom_paste_disabled;
    caret_browsing_enabled = r.caret_browsing_enabled;
    java_disabled = r.java_disabled;
    plugins_disabled = r.plugins_disabled;
    universal_access_from_file_urls_allowed =
        r.universal_access_from_file_urls_allowed;
    file_access_from_file_urls_allowed = r.file_access_from_file_urls_allowed;
    web_security_disabled = r.web_security_disabled;
    xss_auditor_enabled = r.xss_auditor_enabled;
    image_load_disabled = r.image_load_disabled;
    shrink_standalone_images_to_fit = r.shrink_standalone_images_to_fit;
    site_specific_quirks_disabled = r.site_specific_quirks_disabled;
    text_area_resize_disabled = r.text_area_resize_disabled;
    page_cache_disabled = r.page_cache_disabled;
    tab_to_links_disabled = r.tab_to_links_disabled;
    hyperlink_auditing_disabled = r.hyperlink_auditing_disabled;
    user_style_sheet_enabled = r.user_style_sheet_enabled;

    cef_string_copy(r.user_style_sheet_location.str,
        r.user_style_sheet_location.length, &user_style_sheet_location);

    author_and_user_styles_disabled = r.author_and_user_styles_disabled;
    local_storage_disabled = r.local_storage_disabled;
    databases_disabled = r.databases_disabled;
    application_cache_disabled = r.application_cache_disabled;
    experimental_webgl_enabled = r.experimental_webgl_enabled;
    accelerated_compositing_disabled = r.accelerated_compositing_disabled;
    accelerated_2d_canvas_disabled = r.accelerated_2d_canvas_disabled;

    return *this;
  }

protected:
  void Init()
  {
    memset(static_cast<cef_browser_settings_t*>(this), 0,
        sizeof(cef_browser_settings_t));
    size = sizeof(cef_browser_settings_t);
  }
};

#endif // _CEF_H
