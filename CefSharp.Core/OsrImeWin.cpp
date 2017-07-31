#include "Stdafx.h"
#include "OsrImeWin.h"
#include "Internals\CefBrowserHostWrapper.h"

namespace CefSharp {
	OsrImeWin::OsrImeWin(IntPtr ownerHWnd, IBrowser^ browser) {
		_ownerHWnd = ownerHWnd;
		_browser = browser;
		_wndProcHandler = gcnew WndProcDelegate(this, &OsrImeWin::WndProc);

		_imeHandler = new OsrImeHandler(static_cast<HWND>(_ownerHWnd.ToPointer()));
	}

	OsrImeWin::~OsrImeWin() {
		_wndProcHandler = nullptr;
		_browser = nullptr;
		if (_imeHandler) {
			delete _imeHandler;
		}
	}

	void OsrImeWin::OnIMESetContext(UINT message, WPARAM wParam, LPARAM lParam) {
		// We handle the IME Composition Window ourselves (but let the IME Candidates
		// Window be handled by IME through DefWindowProc()), so clear the
		// ISC_SHOWUICOMPOSITIONWINDOW flag:
		lParam &= ~ISC_SHOWUICOMPOSITIONWINDOW;
		::DefWindowProc(static_cast<HWND>(_ownerHWnd.ToPointer()), message, wParam, lParam);

		// Create Caret Window if required
		if (_imeHandler) {
			_imeHandler->CreateImeWindow();
			_imeHandler->MoveImeWindow();
		}
	}

	void OsrImeWin::OnIMEStartComposition() {
		if (_imeHandler) {
			_imeHandler->CreateImeWindow();
			_imeHandler->MoveImeWindow();
			_imeHandler->ResetComposition();
		}
	}

	void OsrImeWin::OnIMEComposition(UINT message, WPARAM wParam, LPARAM lParam) {
		if (_browser && _imeHandler) {
			CefString cTextStr;
			if (_imeHandler->GetResult(lParam, cTextStr)) {
				// Send the text to the browser. The |replacement_range| and
				// |relative_cursor_pos| params are not used on Windows, so provide
				// default invalid values.

				auto host = safe_cast<CefBrowserHostWrapper^>(_browser->GetHost());
				//host->ImeCommitText(cTextStr)
				host->ImeCommitText(cTextStr,
					CefRange(UINT32_MAX, UINT32_MAX), 0);
				_imeHandler->ResetComposition();
				// Continue reading the composition string - Japanese IMEs send both
				// GCS_RESULTSTR and GCS_COMPSTR.
			}

			std::vector<CefCompositionUnderline> underlines;
			int composition_start = 0;

			if (_imeHandler->GetComposition(lParam, cTextStr, underlines,
				composition_start)) {
				// Send the composition string to the browser. The |replacement_range|
				// param is not used on Windows, so provide a default invalid value.

				auto host = safe_cast<CefBrowserHostWrapper^>(_browser->GetHost());
				host->ImeSetComposition(cTextStr, underlines,
					CefRange(UINT32_MAX, UINT32_MAX),
					CefRange(composition_start,
					static_cast<int>(composition_start + cTextStr.length())));

				// Update the Candidate Window position. The cursor is at the end so
				// subtract 1. This is safe because IMM32 does not support non-zero-width
				// in a composition. Also,  negative values are safely ignored in
				// MoveImeWindow
				_imeHandler->UpdateCaretPosition(composition_start - 1);
			} else {
				OnIMECancelCompositionEvent();
			}
		}
	}

	void OsrImeWin::OnIMECancelCompositionEvent() {
		if (_browser && _imeHandler) {
			_browser->GetHost()->ImeCancelComposition();
			_imeHandler->ResetComposition();
			_imeHandler->DestroyImeWindow();
		}
	}

	void OsrImeWin::OnImeCompositionRangeChanged(Range selectedRange, array<Rect>^ characterBounds) {
		if (_imeHandler) {
			// Convert from view coordinates to device coordinates.
			/*RectList device_bounds;
			CefRenderHandler::RectList::const_iterator it = character_bounds.begin();
			for (; it != character_bounds.end(); ++it) {
				device_bounds.push_back(LogicalToDevice(*it, device_scale_factor_));
			}*/

			std::vector<CefRect> device_bounds;
			for each(Rect rect in characterBounds) {
				CefRect rc(rect.X, rect.Y, rect.Width, rect.Height);
				device_bounds.push_back(rc);
			}

			CefRange selection_range(selectedRange.From, selectedRange.To);
			_imeHandler->ChangeCompositionRange(selection_range, device_bounds);
		}
	}

	void OsrImeWin::OnKeyEvent(int message, int wParam, int lParam) {
		if (!_browser)
			return;

		auto host = safe_cast<CefBrowserHostWrapper^>(_browser->GetHost());
		host->SendKeyEvent(message, wParam, lParam);
	}

	IntPtr OsrImeWin::WndProc(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam) {
		WPARAM pwParam;
		switch (message) {
			case WM_IME_SETCONTEXT:
				OnIMESetContext((UINT)message,
					reinterpret_cast<WPARAM>(wParam.ToPointer()),
					reinterpret_cast<LPARAM>(lParam.ToPointer()));
				return IntPtr::Zero;
			case WM_IME_STARTCOMPOSITION:
				OnIMEStartComposition();
				return IntPtr::Zero;
			case WM_IME_COMPOSITION:
				OnIMEComposition((UINT)message,
					reinterpret_cast<WPARAM>(wParam.ToPointer()),
					reinterpret_cast<LPARAM>(lParam.ToPointer()));
				return IntPtr::Zero;
			case WM_IME_ENDCOMPOSITION:
				OnIMECancelCompositionEvent();
				// Let WTL call::DefWindowProc() and release its resources.
				break;
			case WM_SYSCHAR:
			case WM_SYSKEYDOWN:
			case WM_SYSKEYUP:
			case WM_KEYDOWN:
			case WM_KEYUP:
			case WM_CHAR:
				OnKeyEvent(message, reinterpret_cast<int>(wParam.ToPointer()), reinterpret_cast<int>(lParam.ToPointer()));
				break;
		}

		return IntPtr(1);
	}
}