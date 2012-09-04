#pragma once

using namespace System;
namespace CefSharp
{
	struct CEF_CALLBACK_INFO {
		CefV8Value *callback;
		CefV8Context *context;
	};

	public ref class CefCallbackWrapper
	{
	public:
		CefCallbackWrapper(CefRefPtr<CefV8Value> callback);
		~CefCallbackWrapper(){
			cbInfo->callback->Release();
			cbInfo->context->Release();
			delete cbInfo;
		}
		void Call(...array<Object^> ^args);
	private:
		CEF_CALLBACK_INFO *cbInfo;
	};
}
