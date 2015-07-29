#include "stdafx.h"
#include "JavascriptAsyncRootObjectWrapper.h"

using namespace System::Linq;

namespace CefSharp
{
    namespace Internals
    {
        namespace Async
        {
			void JavascriptAsyncRootObjectWrapper::Bind(const CefRefPtr<CefV8Value>& v8Value)
            {
				auto promiseCreator = v8Value->GetValue("cefsharp_CreatePromise");
                auto memberObjects = _rootObject->MemberObjects;
                for each (JavascriptObject^ obj in Enumerable::OfType<JavascriptObject^>(memberObjects))
                {
                    auto wrapperObject = gcnew JavascriptAsyncObjectWrapper(obj, _callbackRegistry, _methodCallbackSave);
					wrapperObject->Bind(v8Value, promiseCreator);

                    _wrappedObjects->Add(wrapperObject);
                }
            }
        }
    }
}