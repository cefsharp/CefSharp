#pragma once

namespace CefSharp
{
    namespace Internals
    {
		ref class CefSharpBrowserWrapper;

        ref class JavascriptCallbackImplFactory : public IJavascriptCallbackFactory
        {
        private:
            WeakReference^ _browserWrapper;
            Messaging::PendingTaskRepository<JavascriptResponse^>^ _pendingTasks;
        public:
            JavascriptCallbackImplFactory(Messaging::PendingTaskRepository<JavascriptResponse^>^ pendingTasks);

			property CefSharpBrowserWrapper^ BrowserWrapper
			{
				void set(CefSharpBrowserWrapper^ browserWrapper);
			};

            virtual IJavascriptCallback^ Create(JavascriptCallback^ callback);
        };
    }
}