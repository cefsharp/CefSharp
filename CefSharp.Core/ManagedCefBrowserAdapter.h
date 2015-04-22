// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include <include/cef_runnable.h>

#include "BrowserSettings.h"
#include "PaintElementType.h"
#include "Internals/ClientAdapter.h"
#include "Internals/CefDragDataWrapper.h"
#include "Internals/RenderClientAdapter.h"
#include "Internals/MCefRefPtr.h"
#include "Internals/StringVisitor.h"

using namespace CefSharp::Internals;
using namespace System::Diagnostics;
using namespace System::ServiceModel;
using namespace System::Threading;
using namespace System::Threading::Tasks;

namespace CefSharp
{
    public ref class ManagedCefBrowserAdapter : public DisposableResource
    {
        MCefRefPtr<ClientAdapter> _clientAdapter;
        BrowserProcessServiceHost^ _browserProcessServiceHost;
        IWebBrowserInternal^ _webBrowserInternal;
        JavascriptObjectRepository^ _javaScriptObjectRepository;
      
    protected:
        virtual void DoDispose(bool isDisposing) override
        {
            CloseAllPopups(true);
            Close(true);

            _clientAdapter = nullptr;

            // Guard managed only member derefs by isDisposing:
            if (isDisposing && _browserProcessServiceHost != nullptr)
            {
                _browserProcessServiceHost->Close();
                _browserProcessServiceHost = nullptr;
            }

            _webBrowserInternal = nullptr;
            _javaScriptObjectRepository = nullptr;

            DisposableResource::DoDispose(isDisposing);
        };

    public:
        ManagedCefBrowserAdapter(IWebBrowserInternal^ webBrowserInternal, bool offScreenRendering)
        {
            if (offScreenRendering)
            {
                _clientAdapter = new RenderClientAdapter(webBrowserInternal,
                    gcnew Action<int>(this, &ManagedCefBrowserAdapter::OnAfterBrowserCreated));
            }
            else
            {
                _clientAdapter = new ClientAdapter(webBrowserInternal,
                    gcnew Action<int>(this, &ManagedCefBrowserAdapter::OnAfterBrowserCreated));
            }

            _webBrowserInternal = webBrowserInternal;
            _javaScriptObjectRepository = gcnew JavascriptObjectRepository();
        }

        void CreateOffscreenBrowser(IntPtr windowHandle, BrowserSettings^ browserSettings, String^ address);
        void CreateBrowser(BrowserSettings^ browserSettings, IntPtr sourceHandle, String^ address);

        void Close(bool forceClose);
        void CloseAllPopups(bool forceClose);

        void OnAfterBrowserCreated(int browserId);

        void LoadUrl(String^ address);
        void LoadHtml(String^ html, String^ url);

        void WasResized();
        void WasHidden(bool hidden);

        void Invalidate(PaintElementType type);

        // Private keyboard functions:
    private:
        bool isKeyDown(WPARAM wparam)
        {
            return (GetKeyState(wparam) & 0x8000) != 0;
        }

        int GetCefKeyboardModifiers(WPARAM wparam, LPARAM lparam);

    public:
        void SendFocusEvent(bool isFocused);
        void SetFocus(bool isFocused);
        bool SendKeyEvent(int message, int wParam, int lParam);

        void OnMouseMove(int x, int y, bool mouseLeave, CefEventFlags modifiers);
        void OnMouseButton(int x, int y, int mouseButtonType, bool mouseUp, int clickCount, CefEventFlags modifiers);
        void OnMouseWheel(int x, int y, int deltaX, int deltaY);

        void Stop();

        void GoBack();
        void GoForward();

        void Print();

        void Find(int identifier, String^ searchText, bool forward, bool matchCase, bool findNext);
        void StopFinding(bool clearSelection);

        void Reload()
        {
            Reload(false);
        }

        void Reload(bool ignoreCache);

        void ViewSource();

        void GetSource(IStringVisitor^ visitor);
        void GetText(IStringVisitor^ visitor);

        void Cut();
        void Copy();
        void Paste();
        void Delete();
        void SelectAll();
        void Undo();
        void Redo();

        void ExecuteScriptAsync(String^ script);
        Task<JavascriptResponse^>^ EvaluateScriptAsync(String^ script, Nullable<TimeSpan> timeout);

    private:
        double GetZoomLevelOnUI();

    public:
        Task<double>^ GetZoomLevelAsync();

        void SetZoomLevel(double zoomLevel);

        void ShowDevTools();
        void CloseDevTools();

        void Resize(int width, int height);
        void NotifyMoveOrResizeStarted();
        void NotifyScreenInfoChanged();

        void RegisterJsObject(String^ name, Object^ object, bool lowerCaseJavascriptNames);

        void ReplaceMisspelling(String^ word);
        void AddWordToDictionary(String^ word);

        CefMouseEvent GetCefMouseEvent(MouseEvent^ mouseEvent);

        void OnDragTargetDragEnter(CefDragDataWrapper^ dragData, MouseEvent^ mouseEvent, DragOperationsMask allowedOperations);
        void OnDragTargetDragOver(MouseEvent^ mouseEvent, DragOperationsMask allowedOperations);
        void OnDragTargetDragLeave();
        void OnDragTargetDragDrop(MouseEvent^ mouseEvent);
    };
}
