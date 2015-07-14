#include "stdafx.h"
#include "JavascriptAsyncRootObjectWrapper.h"

using namespace System::Linq;

namespace CefSharp
{
    namespace Internals
    {
        namespace Async
        {
            void JavascriptAsyncRootObjectWrapper::Bind()
            {
                auto promiseCreator = V8Value->GetValue("cefsharp_CreatePromise");
                auto memberObjects = _rootObject->MemberObjects;
                for each (JavascriptObject^ obj in Enumerable::OfType<JavascriptObject^>(memberObjects))
                {
                    auto wrapperObject = gcnew JavascriptAsyncObjectWrapper(obj);
                    wrapperObject->CallbackRegistry = CallbackRegistry;
                    wrapperObject->MethodCallbackSave = MethodCallbackSave;
                    wrapperObject->PromiseCreator = promiseCreator;
                    wrapperObject->Bind(V8Value.get());

                    _wrappedObjects->Add(wrapperObject);
                }
            }
        }
    }
}