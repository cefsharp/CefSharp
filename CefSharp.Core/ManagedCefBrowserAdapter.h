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
#include "Internals/CefFrameWrapper.h"

using namespace CefSharp::Internals;
using namespace System::Diagnostics;
using namespace System::ServiceModel;
using namespace System::Threading;
using namespace System::Threading::Tasks;

namespace CefSharp
{
    public ref class ManagedCefBrowserAdapter : public DisposableResource, IBrowserAdapter
    {
        MCefRefPtr<ClientAdapter> _clientAdapter;
        BrowserProcessServiceHost^ _browserProcessServiceHost;
        IWebBrowserInternal^ _webBrowserInternal;
        JavascriptObjectRepository^ _javaScriptObjectRepository;

    private:
        // Private keyboard functions:
        bool IsKeyDown(WPARAM wparam)
        {
            return (GetKeyState(wparam) & 0x8000) != 0;
        }

        int GetCefKeyboardModifiers(WPARAM wparam, LPARAM lparam);

        // Misc. private functions:
        void OnAfterBrowserCreated(int browserId);
        CefMouseEvent GetCefMouseEvent(MouseEvent^ mouseEvent);

        // Private methods for async tasks:
        double GetZoomLevelOnUI();

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
        void LoadUrl(String^ address);
        void LoadHtml(String^ html, String^ url);
        void WasResized();
        void WasHidden(bool hidden);
        void Invalidate(PaintElementType type);
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
        virtual Task<JavascriptResponse^>^ EvaluateScriptAsync(int browserId, Int64 frameId, String^ script, Nullable<TimeSpan> timeout);
        virtual Task<JavascriptResponse^>^ EvaluateScriptAsync(String^ script, Nullable<TimeSpan> timeout);
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
        void OnDragTargetDragEnter(CefDragDataWrapper^ dragData, MouseEvent^ mouseEvent, DragOperationsMask allowedOperations);
        void OnDragTargetDragOver(MouseEvent^ mouseEvent, DragOperationsMask allowedOperations);
        void OnDragTargetDragLeave();
        void OnDragTargetDragDrop(MouseEvent^ mouseEvent);

        ///
        // Returns the main (top-level) frame for the browser window.
        ///
        CefFrameWrapper^ GetMainFrame();

        ///
        // Returns the focused frame for the browser window.
        ///
        /*--cef()--*/
        CefFrameWrapper^ GetFocusedFrame();

        ///
        // Returns the frame with the specified identifier, or NULL if not found.
        ///
        /*--cef(capi_name=get_frame_byident)--*/
        CefFrameWrapper^ GetFrame(System::Int64 identifier);

        ///
        // Returns the frame with the specified name, or NULL if not found.
        ///
        /*--cef(optional_param=name)--*/
        CefFrameWrapper^ GetFrame(String^ name);

        ///
        // Returns the number of frames that currently exist.
        ///
        /*--cef()--*/
        int GetFrameCount();

        ///
        // Returns the identifiers of all existing frames.
        ///
        /*--cef(count_func=identifiers:GetFrameCount)--*/
        List<System::Int64>^ GetFrameIdentifiers();

        ///
        // Returns the names of all existing frames.
        ///
        /*--cef()--*/
        List<String^>^ GetFrameNames();
    };
}
