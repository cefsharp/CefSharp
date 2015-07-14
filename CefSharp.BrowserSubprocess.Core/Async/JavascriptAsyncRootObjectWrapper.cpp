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
                auto memberObjects = _rootObject->MemberObjects;
                for each (JavascriptObject^ obj in Enumerable::OfType<JavascriptObject^>(memberObjects))
                {
                    auto wrapperObject = gcnew JavascriptAsyncObjectWrapper(_browser, obj);
                    wrapperObject->CallbackRegistry = CallbackRegistry;
                    wrapperObject->Bind(V8Value.get());

                    _wrappedObjects->Add(wrapperObject);
                }
            }
        }
    }
}