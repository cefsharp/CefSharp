// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "include/cef_browser.h"
#include "include/cef_v8.h"

#include "TypeUtils.h"
#include "Stdafx.h"
#include "JavascriptRootObjectWrapper.h"

using namespace CefSharp::Internals::Async;
using namespace System::ServiceModel;
using namespace System::Threading;
using namespace System::Threading::Tasks;

namespace CefSharp
{
    // "Master class" for wrapping everything that the Cef Subprocess needs 
    // for ONE CefBrowser.
    public ref class CefBrowserWrapper
    {
    private:
        MCefRefPtr<CefBrowser> _cefBrowser;
    
    internal:
        //Frame Identifier is used as Key
        property ConcurrentDictionary<int64, JavascriptRootObjectWrapper^>^ JavascriptRootObjectWrappers;

    public:
        CefBrowserWrapper(CefRefPtr<CefBrowser> cefBrowser)
        {
            _cefBrowser = cefBrowser;
            BrowserId = cefBrowser->GetIdentifier();
            IsPopup = cefBrowser->IsPopup();

            JavascriptRootObjectWrappers = gcnew ConcurrentDictionary<int64, JavascriptRootObjectWrapper^>();
        }
        
        !CefBrowserWrapper()
        {
            _cefBrowser = nullptr;
        }

        ~CefBrowserWrapper()
        {
            this->!CefBrowserWrapper();

            if (JavascriptRootObjectWrappers != nullptr)
            {
                for each(KeyValuePair<int64, JavascriptRootObjectWrapper^> entry in JavascriptRootObjectWrappers)
                {
                    delete entry.Value;
                }

                JavascriptRootObjectWrappers = nullptr;
            }
        }

        property int BrowserId;
        property bool IsPopup;

        // This allows us to create the WCF proxies back to our parent process.
        property ChannelFactory<IBrowserProcess^>^ ChannelFactory;

        // The WCF proxy to the parent process.
        property IBrowserProcess^ BrowserProcess;
    };
}
