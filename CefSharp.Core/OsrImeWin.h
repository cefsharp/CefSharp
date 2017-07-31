#pragma once

#include "OsrImeHandler.h"

namespace CefSharp {

	public ref class OsrImeWin {
	public:
		delegate IntPtr WndProcDelegate(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam);
		OsrImeWin(IntPtr ownerHWnd, IBrowser^ browser);
		~OsrImeWin();
		property WndProcDelegate^ WndProcHandler {
			WndProcDelegate^ get() {
				return _wndProcHandler;
			}
		}

		void OnImeCompositionRangeChanged(Range selectedRange, array<Rect>^ characterBounds);

	protected:
		void OnIMESetContext(UINT message, WPARAM wParam, LPARAM lParam);
		void OnIMEStartComposition();
		void OnIMEComposition(UINT message, WPARAM wParam, LPARAM lParam);
		void OnIMECancelCompositionEvent();
		void OnKeyEvent(int message, int wParam, int lParam);

		IntPtr WndProc(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam);

		IntPtr _ownerHWnd;
		WndProcDelegate^ _wndProcHandler;
		IBrowser^ _browser;
		OsrImeHandler* _imeHandler;
	};
}
