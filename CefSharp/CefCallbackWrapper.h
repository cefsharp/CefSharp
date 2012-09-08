#pragma once

using namespace System;
using namespace System::Threading;
namespace CefSharp
{
	struct CEF_CALLBACK_INFO {
		int callCount;
		CefV8Value *callback;
		CefV8Context *context;
	};

	public ref class CefCallbackWrapper : IDisposable
	{
	public:
		CefCallbackWrapper(CefRefPtr<CefV8Value> callback);
		~CefCallbackWrapper(){
			if(cbInfo)
			{
				while(cbInfo->callCount)
				{
					Thread::Sleep(0);
				}
				cbInfo->callback->Release();
				cbInfo->context->Release();
				delete cbInfo;
				cbInfo = 0;
			}
		}
		void Call(...array<Object^> ^args);
	private:
		CEF_CALLBACK_INFO *cbInfo;
	};
}