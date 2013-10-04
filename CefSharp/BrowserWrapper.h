#pragma once

#include "Stdafx.h"
//#include "BrowserSettings.h"
//#include "MouseButtonType.h"

using namespace CefSharp::Internals;
using namespace CefSharp::Internals::JavascriptBinding;
using namespace System::ServiceModel;

namespace CefSharp
{
	private ref class BrowserWrapper
	{
	private:
		ClientAdapter* _clientAdapter;
		ScriptCore* _scriptCore;
		IJavascriptProxy^ _javaScriptProxy;

	public:
		BrowserWrapper(IWebBrowserInternal^ browserControl)
		{
			_clientAdapter = new ClientAdapter(browserControl);
			_scriptCore = new ScriptCore();

            // TODO: Address should certainly not be hardwired like this.
            auto channelFactory = gcnew ChannelFactory<IJavascriptProxy^>(
                gcnew NetNamedPipeBinding(),
                gcnew EndpointAddress("net.pipe://localhost/JavaScriptProxy")
            );
            
            _javaScriptProxy = channelFactory->CreateChannel();
		}

		~BrowserWrapper()
		{
			_clientAdapter = nullptr;
		}

		void CreateBrowser(BrowserSettings^ browserSettings, IntPtr^ sourceHandle, String^ address)
        {
            HWND hwnd = static_cast<HWND>(sourceHandle->ToPointer());
			RECT rect;
			GetClientRect(hwnd, &rect);
            CefWindowInfo window;
            window.SetAsChild(hwnd, rect);
            CefString addressNative = StringUtils::ToNative(address);

            CefBrowserHost::CreateBrowser(window, _clientAdapter, addressNative,
                *(CefBrowserSettings*) browserSettings->_internalBrowserSettings);
        }

		void LoadUrl(String^ address)
        {
			auto cefBrowser = _clientAdapter->GetCefBrowser().get();

            auto cefFrame = cefBrowser->GetMainFrame();

            if (cefFrame != nullptr)
            {
                cefFrame->LoadURL(StringUtils::ToNative(address));
            }
        }
	};
}