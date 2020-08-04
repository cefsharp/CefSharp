// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "CefWrapper.h"

using namespace System::Threading::Tasks;
using namespace CefSharp::Structs;
using namespace CefSharp::Enums;
using namespace CefSharp::Callback;

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
            virtual double GetZoomLevel();
            virtual Task<double>^ GetZoomLevelAsync();
            virtual IntPtr GetWindowHandle();
            virtual void CloseBrowser(bool forceClose);
            virtual bool TryCloseBrowser();

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

            virtual bool SendDevToolsMessage(String^ messageAsJson);
            virtual int ExecuteDevToolsMethod(int messageId, String^ method, String^ paramsAsJson);
            virtual int ExecuteDevToolsMethod(int messageId, String^ method, IDictionary<String^, Object^>^ paramaters);
            virtual IRegistration^ AddDevToolsMessageObserver(IDevToolsMessageObserver^ observer);

            virtual void AddWordToDictionary(String^ word);
            virtual void ReplaceMisspelling(String^ word);

            virtual property IExtension^ Extension
            {
                IExtension^ get();
            }

            virtual void RunFileDialog(CefFileDialogMode mode, String^ title, String^ defaultFilePath, IList<String^>^ acceptFilters, int selectedAcceptFilter, IRunFileDialogCallback^ callback);

            virtual void Find(int identifier, String^ searchText, bool forward, bool matchCase, bool findNext);
            virtual void StopFinding(bool clearSelection);

            virtual void SetFocus(bool focus);
            virtual void SendFocusEvent(bool setFocus);
            virtual void SendKeyEvent(KeyEvent keyEvent);
            virtual void SendKeyEvent(int message, int wParam, int lParam);

            virtual void SendMouseWheelEvent(MouseEvent mouseEvent, int deltaX, int deltaY);

            virtual void SendTouchEvent(TouchEvent evt);

            virtual void Invalidate(PaintElementType type);

            virtual property bool IsBackgroundHost
            {
                bool get();
            }

            virtual void ImeSetComposition(String^ text, cli::array<CompositionUnderline>^ underlines, Nullable<CefSharp::Structs::Range> replacementRange, Nullable<CefSharp::Structs::Range> selectionRange);
            virtual void ImeCommitText(String^ text, Nullable<CefSharp::Structs::Range> replacementRange, int relativeCursorPos);
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

            virtual property bool IsAudioMuted
            {
                bool get();
            }

            virtual void SetAudioMuted(bool mute);

            virtual IntPtr GetOpenerWindowHandle();

            virtual void SendExternalBeginFrame();

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

