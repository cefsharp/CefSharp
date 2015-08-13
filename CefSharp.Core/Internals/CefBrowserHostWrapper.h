// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

using namespace System;

namespace CefSharp
{
    namespace Internals
    {
        public ref class CefBrowserHostWrapper : IBrowserHost
        {
        private:
            MCefRefPtr<CefBrowserHost> _browserHost;
            bool _disposed;
            
            double GetZoomLevelOnUI();

        internal:
            CefBrowserHostWrapper(CefRefPtr<CefBrowserHost> &browserHost) : _browserHost(browserHost), _disposed(false)
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

        private:
            void ThrowIfDisposed();

        public:
            virtual void StartDownload(String^ url);
            virtual void Print();
            virtual void SetZoomLevel(double zoomLevel);
            virtual Task<double>^ GetZoomLevelAsync();
            virtual IntPtr GetWindowHandle();
            virtual void CloseBrowser(bool forceClose);
        
            virtual void ShowDevTools(IWindowInfo^ windowInfo, int inspectElementAtX, int inspectElementAtY);
            virtual void CloseDevTools();

            virtual void AddWordToDictionary(String^ word);
            virtual void ReplaceMisspelling(String^ word);

            virtual void Find(int identifier, String^ searchText, bool forward, bool matchCase, bool findNext);
            virtual void StopFinding(bool clearSelection);

            virtual void SetFocus(bool focus);
            virtual void SendFocusEvent(bool setFocus);
            virtual void SendKeyEvent(KeyEvent keyEvent);

            virtual void SendMouseWheelEvent(int x, int y, int deltaX, int deltaY, CefEventFlags modifiers);

            virtual void Invalidate(PaintElementType type);

            virtual void SendMouseClickEvent(int x, int y, MouseButtonType mouseButtonType, bool mouseUp, int clickCount, CefEventFlags modifiers);

            virtual void SendMouseMoveEvent(int x, int y, bool mouseLeave, CefEventFlags modifiers);
        };
    }
}

