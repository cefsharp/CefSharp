#include "StdAfx.h"
#include "CefCallbackWrapper.h"
#include "include/cef_runnable.h"

using namespace System;

namespace CefSharp
{
	static void _call(CefV8Value* callback, CefV8Context* context, gcroot<array<Object^>^> args)
	{
		context->Enter();

		CefV8ValueList arguments = CefV8ValueList(args->Length);
		for(int i = 0; i < args->Length; i++)
		{
			arguments[i] = convertToCef(args[i], args[i]->GetType());
		}
		
		callback->ExecuteFunction(CefV8Value::CreateUndefined() , arguments);
		callback->Release();

		context->Exit();
		context->Release();
	}

	CefCallbackWrapper::CefCallbackWrapper(CefRefPtr<CefV8Value> callback)
	{
		this->cbInfo = new CEF_CALLBACK_INFO();
		this->cbInfo->callback = callback.get();
		this->cbInfo->callback->AddRef();

		CefRefPtr<CefV8Context> pC = CefV8Context::GetEnteredContext();
		this->cbInfo->context = pC.get();
		this->cbInfo->context->AddRef();
	}

	void CefCallbackWrapper::Call(...array<Object^> ^args)
	{
		if(CefCurrentlyOn(TID_UI)){
			_call(this->cbInfo->callback, this->cbInfo->context, (gcroot<array<Object^>^>) args);
		}
		else
		{
			CefPostTask(TID_UI, NewCefRunnableFunction(&_call, this->cbInfo->callback, this->cbInfo->context, (gcroot<array<Object^>^>) args));
		}
	}
}