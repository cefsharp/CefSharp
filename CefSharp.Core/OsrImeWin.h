#pragma once

#include "OsrImeHandler.h"

namespace CefSharp 
{
    public ref class OsrImeWin 
    {
    public:
		delegate IntPtr WndProcDelegate(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam);
		OsrImeWin(IntPtr ownerHWnd, IBrowser^ browser);
		~OsrImeWin();
		property WndProcDelegate^ WndProcHandler 
        {
			WndProcDelegate^ get() { return _wndProcHandler; }
		}

		void OnImeCompositionRangeChanged(CefSharp::Structs::Range selectedRange, array<CefSharp::Structs::Rect>^ characterBounds);
        void MoveWindow(int x, int y);
        void HideWindow();
        void KillFocus();
        void SetFocus();

      protected:
		void OnIMESetContext(UINT message, WPARAM wParam, LPARAM lParam);
		void OnIMEStartComposition();
		void OnIMEComposition(UINT message, WPARAM wParam, LPARAM lParam);
		void OnIMECancelCompositionEvent();

		IntPtr WndProc(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam);

		IntPtr _ownerHWnd;
		WndProcDelegate^ _wndProcHandler;
		IBrowser^ _browser;
		OsrImeHandler* _imeHandler;
        HHOOK _hKeyboardHook = 0;
    };
}
