#include "Stdafx.h"
#include "CefSharpBrowserWrapper.h"
#include "JavascriptCallbackImpl.h"
#include "JavascriptCallbackImplFactory.h"

namespace CefSharp
{
    namespace Internals
    {
        JavascriptCallbackImplFactory::JavascriptCallbackImplFactory(Messaging::PendingTaskRepository<JavascriptResponse^>^ pendingTasks)
            :_pendingTasks(pendingTasks)
        {
        }

		void JavascriptCallbackImplFactory::BrowserWrapper::set(CefSharpBrowserWrapper^ browserWrapper)
		{
			_browserWrapper = gcnew WeakReference(browserWrapper);
		}

        IJavascriptCallback^ JavascriptCallbackImplFactory::Create(JavascriptCallback^ callback)
        {
            return gcnew JavascriptCallbackImpl(callback, _pendingTasks, _browserWrapper);
        }
    }
}