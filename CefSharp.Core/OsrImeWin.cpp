#include "Stdafx.h"
#include "OsrImeWin.h"
#include "Internals\CefBrowserHostWrapper.h"
#include "Internals\CefFrameWrapper.h"

namespace CefSharp
{
    static const auto s_propComposition = L"FuzeIME";

    static void EndIMEComposition(HWND hWnd)
    {
        if (GetProp(hWnd, s_propComposition))
        {
            HIMC hImc = ::ImmGetContext(hWnd);

            ImmNotifyIME(hImc, NI_COMPOSITIONSTR, CPS_COMPLETE, 0);
            ImmNotifyIME(hImc, NI_CLOSECANDIDATE, 0, 0);

            ImmReleaseContext(hWnd, hImc);
        }
    }

    static LRESULT CALLBACK KeyboardProc(int code, WPARAM wParam, LPARAM lParam)
    {
        if (HC_ACTION == code)
        {
            auto hWndFocus = GetActiveWindow();

            if (GetProp(hWndFocus, s_propComposition))
            {
                for (auto key: {VK_LEFT, VK_RIGHT, VK_HOME, VK_END, VK_ESCAPE, VK_DELETE})
                {
                    if (wParam == key)
                    {
                        EndIMEComposition(hWndFocus);

                        // After 'EndIMEComposition', key is lost, enter it again.
                        SendMessage(hWndFocus, WM_KEYUP, key, 1);
                    }
                }
            }
        }

        return CallNextHookEx(0, code, wParam, lParam);
    }

    OsrImeWin::OsrImeWin(IntPtr ownerHWnd, IBrowser^ browser)
        :_ownerHWnd(ownerHWnd), _browser(browser)
    {
        _wndProcHandler = gcnew WndProcDelegate(this, &OsrImeWin::WndProc);

        _imeHandler = new OsrImeHandler(static_cast<HWND>(_ownerHWnd.ToPointer()));

        // When composition is started, some keyboard input like 'VK_LEFT, VK_RIGHT, VK_HOME, VK_END, VK_ESCAPE, VK_DELETE' don't work correctly.
        // As a workaround, when one of the above keys are pressed, composition is ended. It is only possible to detect these key via keyboard hook. 
        _hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD, KeyboardProc, 0, GetCurrentThreadId());
    }

    OsrImeWin::~OsrImeWin() 
    {
        _wndProcHandler = nullptr;
        _browser = nullptr;

        if (_imeHandler) 
        {
            delete _imeHandler;
        }

        if (_hKeyboardHook)
        {
            UnhookWindowsHookEx(_hKeyboardHook);
        }
    }
  
    void OsrImeWin::OnIMESetContext(UINT message, WPARAM wParam, LPARAM lParam) 
    {
        // We handle the IME Composition Window ourselves (but let the IME Candidates.
        // Window be handled by IME through DefWindowProc()), so clear the ISC_SHOWUICOMPOSITIONWINDOW flag.
        ::DefWindowProc(static_cast<HWND>(_ownerHWnd.ToPointer()), message, wParam, lParam & ~ISC_SHOWUICOMPOSITIONWINDOW);

        // Create Caret Window if required
        if (_imeHandler) 
        {
            _imeHandler->CreateImeWindow();
            _imeHandler->MoveImeWindow();
        }
    }

    void OsrImeWin::OnIMEStartComposition() 
    {
        if (_imeHandler) 
        {
            _imeHandler->CreateImeWindow();
            _imeHandler->MoveImeWindow();
            _imeHandler->ResetComposition();
        }
    }

    void OsrImeWin::OnIMEComposition(UINT message, WPARAM wParam, LPARAM lParam) 
    {
        if (_browser && _imeHandler) 
        {
            CefString cTextStr;
            if (_imeHandler->GetResult(lParam, cTextStr)) 
            {
                // Send the text to the browser. The |replacement_range| and |relative_cursor_pos| params are not used on Windows, so provide default invalid values.

                auto host = safe_cast<CefBrowserHostWrapper^>(_browser->GetHost());

                host->ImeCommitText(cTextStr, CefRange(UINT32_MAX, UINT32_MAX), 0);

                _imeHandler->ResetComposition();

                // Continue reading the composition string - Japanese IMEs send both GCS_RESULTSTR and GCS_COMPSTR.
            }

            std::vector<CefCompositionUnderline> underlines;
            int composition_start = 0;

            if (_imeHandler->GetComposition(lParam, cTextStr, underlines, composition_start)) 
            {
                // Send the composition string to the browser. The |replacement_range|
                // param is not used on Windows, so provide a default invalid value.

                auto host = safe_cast<CefBrowserHostWrapper^>(_browser->GetHost());

                host->ImeSetComposition(cTextStr,
                                        underlines,
                                        CefRange(UINT32_MAX, UINT32_MAX),
                                        CefRange(composition_start, static_cast<int>(composition_start + cTextStr.length())));

                // Update the Candidate Window position. The cursor is at the end so
                // subtract 1. This is safe because IMM32 does not support non-zero-width
                // in a composition. Also,  negative values are safely ignored in
                // MoveImeWindow
                _imeHandler->UpdateCaretPosition(composition_start - 1);
            }
            else 
            {
                OnIMECancelCompositionEvent();
            }
        }
    }

    void OsrImeWin::OnIMECancelCompositionEvent() 
    {
        if (_browser && _imeHandler) 
        {
            _browser->GetHost()->ImeCancelComposition();
            _imeHandler->ResetComposition();
            _imeHandler->DestroyImeWindow();
        }
    }

    void OsrImeWin::OnImeCompositionRangeChanged(CefSharp::Structs::Range selectedRange, array<CefSharp::Structs::Rect>^ characterBounds)
    {
        if (_imeHandler) 
        {
            std::vector<CefRect> device_bounds;
            for each(Rect rect in characterBounds) 
            {
                device_bounds.push_back({rect.X, rect.Y, rect.Width, rect.Height});
            }

            _imeHandler->ChangeCompositionRange({selectedRange.From, selectedRange.To}, device_bounds);
        }
    }

    void OsrImeWin::MoveWindow(int x, int y)
    {
        if (_imeHandler)
        {
            _imeHandler->MoveImeWindow(x, y);
        }
    }

    void OsrImeWin::HideWindow()
    {
        EndIMEComposition((HWND)_ownerHWnd.ToInt32());
    }

    void OsrImeWin::KillFocus()
    {
        ::SetFocus(0);
    }

    void OsrImeWin::SetFocus()
    {
        ::SetFocus((HWND)_ownerHWnd.ToInt32());
    }

    IntPtr OsrImeWin::WndProc(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam)
    {
        switch (message)
        {
        case WM_IME_SETCONTEXT:
            OnIMESetContext((UINT)message, reinterpret_cast<WPARAM>(wParam.ToPointer()), reinterpret_cast<LPARAM>(lParam.ToPointer()));

            return IntPtr::Zero;

        case WM_IME_STARTCOMPOSITION:
            SetProp((HWND)hWnd.ToInt32(), s_propComposition, (HANDLE)1);

            OnIMEStartComposition();

            return IntPtr::Zero;

        case WM_IME_COMPOSITION:
            OnIMEComposition((UINT)message, reinterpret_cast<WPARAM>(wParam.ToPointer()), reinterpret_cast<LPARAM>(lParam.ToPointer()));

            return IntPtr::Zero;

        case WM_IME_ENDCOMPOSITION:
            SetProp((HWND)hWnd.ToInt32(), s_propComposition, (HANDLE)0);

            OnIMECancelCompositionEvent();

            // Let WTL call::DefWindowProc() and release its resources.
            break;
        }

        return IntPtr(1);
    }
}
