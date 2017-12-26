// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "CefWrapper.h"

using namespace System::Threading::Tasks;
using namespace CefSharp::Structs;

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefBrowserHostWrapper : public IBrowserHost, public CefWrapper
        {
        private:
            MCefRefPtr<CefBrowserHost> _browserHost;
            
            double GetZoomLevelOnUI();

        internal:
            CefBrowserHostWrapper(CefRefPtr<CefBrowserHost> &browserHost) : _browserHost(browserHost)
            {
            }
            
            !CefBrowserHostWrapper()
            {
                _browserHost = NULL;
            }

            ~CefBrowserHostWrapper()
            {
                this->!CefBrowserHostWrapper();

                _disposed = true;
            }

        public:
            virtual void StartDownload(String^ url);
            virtual void Print();
            virtual void PrintToPdf(String^ path, PdfPrintSettings^ settings, IPrintToPdfCallback^ callback);
            virtual void SetZoomLevel(double zoomLevel);
            virtual Task<double>^ GetZoomLevelAsync();
            virtual IntPtr GetWindowHandle();
            virtual void CloseBrowser(bool forceClose);

            virtual void DragTargetDragEnter(IDragData^ dragData, MouseEvent mouseEvent, DragOperationsMask allowedOperations);
            virtual void DragTargetDragOver(MouseEvent mouseEvent, DragOperationsMask allowedOperations);
            virtual void DragTargetDragDrop(MouseEvent mouseEvent);
            virtual void DragSourceEndedAt(int x, int y, DragOperationsMask op);
            virtual void DragTargetDragLeave();
            virtual void DragSourceSystemDragEnded();
        
            virtual void ShowDevTools(IWindowInfo^ windowInfo, int inspectElementAtX, int inspectElementAtY);
            virtual void CloseDevTools();
            ///
            // Returns true if this browser currently has an associated DevTools browser.
            // Must be called on the browser process UI thread.
            ///
            /*--cef()--*/
            virtual property bool HasDevTools
            {
                bool get();
            }

            virtual void AddWordToDictionary(String^ word);
            virtual void ReplaceMisspelling(String^ word);

            virtual void Find(int identifier, String^ searchText, bool forward, bool matchCase, bool findNext);
            virtual void StopFinding(bool clearSelection);

            virtual void SetFocus(bool focus);
            virtual void SendFocusEvent(bool setFocus);
            virtual void SendKeyEvent(KeyEvent keyEvent);
            virtual void SendKeyEvent(int message, int wParam, int lParam);

            virtual void SendMouseWheelEvent(MouseEvent mouseEvent, int deltaX, int deltaY);

            virtual void Invalidate(PaintElementType type);

            virtual void ImeSetComposition(String^ text, cli::array<CompositionUnderline>^ underlines, Nullable<Range> selectionRange);
            virtual void ImeCommitText(String^ text);
            virtual void ImeFinishComposingText(bool keepSelection);
            virtual void ImeCancelComposition();

            virtual void SendMouseClickEvent(MouseEvent mouseEvent, MouseButtonType mouseButtonType, bool mouseUp, int clickCount);

            virtual void SendMouseMoveEvent(MouseEvent mouseEvent, bool mouseLeave);

            virtual void SetAccessibilityState(CefState accessibilityState);

            virtual void SetAutoResizeEnabled(bool enabled, Size minSize, Size maxSize);

            virtual void NotifyMoveOrResizeStarted();

            virtual void NotifyScreenInfoChanged();

            virtual void WasResized();

            virtual void WasHidden(bool hidden);

            virtual void GetNavigationEntries(INavigationEntryVisitor^ visitor, bool currentOnly);

            virtual NavigationEntry^ GetVisibleNavigationEntry();

            virtual property int WindowlessFrameRate
            {
                int get();
                void set(int val);
            }

            virtual property bool MouseCursorChangeDisabled
            {
                bool get();
                void set(bool val);
            }

            virtual property bool WindowRenderingDisabled
            {
                bool get();
            }

            virtual IntPtr GetOpenerWindowHandle();

            virtual void SendCaptureLostEvent();

            virtual property IRequestContext^ RequestContext
            {
                IRequestContext^ get();
            }

            // Misc. private functions:
            CefMouseEvent GetCefMouseEvent(MouseEvent mouseEvent);
            int GetCefKeyboardModifiers(WPARAM wparam, LPARAM lparam);

            // Private keyboard functions:
            bool IsKeyDown(WPARAM wparam)
            {
                return (GetKeyState(wparam) & 0x8000) != 0;
            }
        };
    }
}

