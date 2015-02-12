#pragma once

#include "ClientAdapter.h"
#include "ScriptCore.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::ComponentModel;
using namespace System::Windows::Forms;

namespace CefSharp
{
namespace WinForms
{
    public ref class WebView sealed : public Control, IWebBrowser
    {
    private:
        BrowserSettings^ _settings;

        MCefRefPtr<ClientAdapter> _clientAdapter;
        BrowserCore^ _browserCore;
        MCefRefPtr<ScriptCore> _scriptCore;

        void Initialize(String^ address, BrowserSettings^ settings);
        bool TryGetCefBrowser(CefRefPtr<CefBrowser>& browser);

    protected:
        virtual void OnHandleCreated(EventArgs^ e) override;
        virtual void OnSizeChanged(EventArgs^ e) override;
        virtual void OnGotFocus(EventArgs^ e) override;

    public:
        virtual event PropertyChangedEventHandler^ PropertyChanged
        {
            void add(PropertyChangedEventHandler^ handler)
            {
                _browserCore->PropertyChanged += handler;
            }

            void remove(PropertyChangedEventHandler^ handler)
            {
                _browserCore->PropertyChanged -= handler;
            }
        }

        virtual event ConsoleMessageEventHandler^ ConsoleMessage;
        virtual event KeyEventHandler^ BrowserKey;
        virtual event LoadCompletedEventHandler^ LoadCompleted;
        virtual event LoadStartedEventHandler^ LoadStarted;

        WebView()
        {
            Initialize(String::Empty, gcnew BrowserSettings);
        }

        WebView(String^ address, BrowserSettings^ settings)
        {
            Initialize(address, settings);
        }

        ~WebView()
        {
            CefRefPtr<CefBrowser> browser;
            if (TryGetCefBrowser(browser))
            {
                browser->CloseBrowser();
            }
        }

        virtual property bool IsBrowserInitialized
        {
            bool get() { return _browserCore->IsBrowserInitialized; }
        }

        virtual property bool IsLoading
        {
            bool get() { return _browserCore->IsLoading; }
        }

        virtual property bool CanGoBack
        { 
            bool get() { return _browserCore->CanGoBack; } 
        }

        virtual property bool CanGoForward
        { 
            bool get() { return _browserCore->CanGoForward; } 
        }

        virtual property int ContentsWidth
        {
            int get() { return _browserCore->ContentsWidth; }
            void set(int contentsWidth) { _browserCore->ContentsWidth = contentsWidth; }
        }

        virtual property int ContentsHeight
        {
            int get() { return _browserCore->ContentsHeight; }
            void set(int contentsHeight) { _browserCore->ContentsHeight = contentsHeight; }
        }

        virtual property String^ Address
        {
            String^ get() { return _browserCore->Address; }
            void set(String^ address) { _browserCore->Address = address; }
        }

        virtual property String^ Title
        {
            String^ get() { return _browserCore->Title; }
            void set(String^ title) { _browserCore->Title = title; }
        }

        virtual property String^ TooltipText
        {
            String^ get() { return _browserCore->TooltipText; }
            void set(String^ text) { _browserCore->TooltipText = text; }
        }

        virtual property ILifeSpanHandler^ LifeSpanHandler
        {
            ILifeSpanHandler^ get() { return _browserCore->LifeSpanHandler; }
            void set(ILifeSpanHandler^ handler) { _browserCore->LifeSpanHandler = handler; }
        }

        virtual property ILoadHandler^ LoadHandler
        {
            ILoadHandler^ get() { return _browserCore->LoadHandler; }
            void set(ILoadHandler^ handler) { _browserCore->LoadHandler = handler; }
        }

        virtual property IRequestHandler^ RequestHandler
        {
            IRequestHandler^ get() { return _browserCore->RequestHandler; }
            void set(IRequestHandler^ handler) { _browserCore->RequestHandler = handler; }
        }

        virtual property IMenuHandler^ MenuHandler
        {
            IMenuHandler^ get() { return _browserCore->MenuHandler; }
            void set(IMenuHandler^ handler) { _browserCore->MenuHandler = handler; }
        }

        virtual property IKeyboardHandler^ KeyboardHandler
        {
            IKeyboardHandler^ get() { return _browserCore->KeyboardHandler; }
            void set(IKeyboardHandler^ handler) { _browserCore->KeyboardHandler = handler; }
        }

        virtual property IJsDialogHandler^ JsDialogHandler
        {
            IJsDialogHandler^ get() { return _browserCore->JsDialogHandler; }
            void set(IJsDialogHandler^ handler) { _browserCore->JsDialogHandler = handler; }
        }

        virtual void OnInitialized();

        virtual void Load(String^ url);
        virtual void LoadHtml(String^ html);
        virtual void LoadHtml(String^ html, String ^url);
        virtual void Stop();
        virtual void Back();
        virtual void Forward();
        virtual void Reload();
        virtual void Reload(bool ignoreCache);
        virtual void ClearHistory();
        virtual void ShowDevTools();
        virtual void CloseDevTools();

        virtual void Undo();
        virtual void Redo();
        virtual void Cut();
        virtual void Copy();
        virtual void Paste();
        virtual void Delete();
        virtual void SelectAll();
        virtual void Print();

        void ExecuteScript(String^ script);
        Object^ EvaluateScript(String^ script);
        Object^ EvaluateScript(String^ script, TimeSpan timeout);

        virtual void SetNavState(bool isLoading, bool canGoBack, bool canGoForward);

        virtual void OnFrameLoadStart(String^ url, bool isMainFrame);
        virtual void OnFrameLoadEnd(String^ url, bool isMainFrame);
        virtual void OnTakeFocus(bool next);
        virtual void OnConsoleMessage(String^ message, String^ source, int line);

        virtual void RegisterJsObject(String^ name, Object^ objectToBind);
        virtual IDictionary<String^, Object^>^ GetBoundObjects();
    };
}}