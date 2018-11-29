// Copyright 2016 The Chromium Embedded Framework Authors. Portions copyright
// 2013 The Chromium Authors. All rights reserved. Use of this source code is
// governed by a BSD-style license that can be found in the LICENSE file.

// Implementation based on ui/base/ime/win/imm32_manager.cc from Chromium.

#include "Stdafx.h"

#include "include/base/cef_build.h"
#include "OsrImeHandler.h"
#include <Imm.h>

#pragma comment(lib, "imm32.lib")

static const auto ColorUNDERLINE = 0x00000000;    // Black SkColor value for underline,
static const auto ColorBKCOLOR = 0x00000000;    // White SkColor value for background,

namespace CefSharp
{
    namespace
    {
        // Determines whether or not the given attribute represents a selection
        bool IsSelectionAttribute(char attribute)
        {
            return (attribute == ATTR_TARGET_CONVERTED || attribute == ATTR_TARGET_NOTCONVERTED);
        }

        // Helper function for OsrImeHandler::GetCompositionInfo() method,
        // to get the target range that's selected by the user in the current
        // composition string.
        void GetCompositionSelectionRange(HIMC imc, int* target_start, int* target_end)
        {
            int attribute_size = ::ImmGetCompositionString(imc, GCS_COMPATTR, NULL, 0);
            if (attribute_size > 0)
            {
                int start = 0;
                int end = 0;
                std::vector<char> attribute_data(attribute_size);

                ::ImmGetCompositionString(imc, GCS_COMPATTR, &attribute_data[0], attribute_size);
                for (start = 0; start < attribute_size; ++start)
                {
                    if (IsSelectionAttribute(attribute_data[start]))
                        break;
                }

                for (end = start; end < attribute_size; ++end)
                {
                    if (!IsSelectionAttribute(attribute_data[end]))
                        break;
                }

                *target_start = start;
                *target_end = end;
            }
        }

        // Helper function for OsrImeHandler::GetCompositionInfo() method, to get underlines information of the current composition string.
        void GetCompositionUnderlines(HIMC imc, int target_start, int target_end, std::vector<CefCompositionUnderline> &underlines)
        {
            int clause_size = ::ImmGetCompositionString(imc, GCS_COMPCLAUSE, NULL, 0);
            int clause_length = clause_size / sizeof(uint32);
            if (clause_length)
            {
                std::vector<uint32> clause_data(clause_length);

                ::ImmGetCompositionString(imc, GCS_COMPCLAUSE, &clause_data[0], clause_size);
                for (int i = 0; i < clause_length - 1; ++i)
                {
                    cef_composition_underline_t underline;
                    underline.range.from = clause_data[i];
                    underline.range.to = clause_data[i + 1];
                    underline.color = ColorUNDERLINE;
                    underline.background_color = ColorBKCOLOR;
                    underline.thick = 0;

                    // Use thick underline for the target clause.
                    if (underline.range.from >= target_start && underline.range.to <= target_end)
                    {
                        underline.thick = 1;
                    }

                    underlines.push_back(underline);
                }
            }
        }
    }  // namespace


    OsrImeHandler::OsrImeHandler(HWND hwnd)
        : ime_status_(false),
        hwnd_(hwnd),
        input_language_id_(LANG_USER_DEFAULT),
        is_composing_(false),
        cursor_index_(-1),
        system_caret_(false),
        ime_rect_{ -1, -1, 0, 0 }
    {}

    OsrImeHandler::~OsrImeHandler()
    {
        DestroyImeWindow();
    }

    void OsrImeHandler::SetInputLanguage()
    {
        // Retrieve the current input language from the system's keyboard layout.
        // Using GetKeyboardLayoutName instead of GetKeyboardLayout, because
        // the language from GetKeyboardLayout is the language under where the
        // keyboard layout is installed. And the language from GetKeyboardLayoutName
        // indicates the language of the keyboard layout itself.
        // See crbug.com/344834.
        WCHAR keyboard_layout[KL_NAMELENGTH];
        if (::GetKeyboardLayoutNameW(keyboard_layout))
        {
            input_language_id_ = static_cast<LANGID>(_wtoi(&keyboard_layout[KL_NAMELENGTH >> 1]));
        }
        else
        {
            input_language_id_ = 0x0409;  // Fallback to en-US.
        }
    }

    void OsrImeHandler::CreateImeWindow()
    {
        // Chinese/Japanese IMEs somehow ignore function calls to
        // ::ImmSetCandidateWindow(), and use the position of the current system
        // caret instead -::GetCaretPos().
        // Therefore, we create a temporary system caret for Chinese IMEs and use
        // it during this input context.
        // Since some third-party Japanese IME also uses ::GetCaretPos() to determine
        // their window position, we also create a caret for Japanese IMEs.
        if (PRIMARYLANGID(input_language_id_) == LANG_CHINESE || PRIMARYLANGID(input_language_id_) == LANG_JAPANESE)
        {
            if (!system_caret_)
            {
                if (::CreateCaret(hwnd_, NULL, 1, 1))
                {
                    system_caret_ = true;
                }
            }
        }
    }

    void OsrImeHandler::DestroyImeWindow()
    {
        // Destroy the system caret if we have created for this IME input context.
        if (system_caret_)
        {
            ::DestroyCaret();
            system_caret_ = false;
        }
    }

    void OsrImeHandler::MoveImeWindow()
    {
        HWND window_handle = hwnd_;
        HIMC imm_context = ::ImmGetContext(hwnd_);

        RECT rectWnd;
        GetWindowRect(hwnd_, &rectWnd);

        int x = caretX_;
        int y = caretY_ + 100; // !!! Should be fixed. Hadcoded just for WPF.Example.

        int width = 0;
        int height = 0;
        const int kCaretMargin = 1;
        // As written in a comment in ImeInput::CreateImeWindow(),
        // Chinese IMEs ignore function calls to ::ImmSetCandidateWindow()
        // when a user disables TSF (Text Service Framework) and CUAS (Cicero
        // Unaware Application Support).
        // On the other hand, when a user enables TSF and CUAS, Chinese IMEs
        // ignore the position of the current system caret and uses the
        // parameters given to ::ImmSetCandidateWindow() with its 'dwStyle'
        // parameter CFS_CANDIDATEPOS.
        // Therefore, we do not only call ::ImmSetCandidateWindow() but also
        // set the positions of the temporary system caret if it exists.
        CANDIDATEFORM candidate_position = { 0, CFS_CANDIDATEPOS, { x, y }, { 0, 0, 0, 0 } };
        ::ImmSetCandidateWindow(imm_context, &candidate_position);

        if (system_caret_)
        {
            switch (PRIMARYLANGID(input_language_id_))
            {
            case LANG_JAPANESE:
                ::SetCaretPos(x, y + height);
                break;
            default:
                ::SetCaretPos(x, y);
                break;
            }
        }

        if (PRIMARYLANGID(input_language_id_) == LANG_KOREAN)
        {
            // Chinese IMEs and Japanese IMEs require the upper-left corner of
            // the caret to move the position of their candidate windows.
            // On the other hand, Korean IMEs require the lower-left corner of the
            // caret to move their candidate windows.
            y += kCaretMargin;
        }

        // Japanese IMEs and Korean IMEs also use the rectangle given to
        // ::ImmSetCandidateWindow() with its 'dwStyle' parameter CFS_EXCLUDE
        // to move their candidate windows when a user disables TSF and CUAS.
        // Therefore, we also set this parameter here.
        CANDIDATEFORM exclude_rectangle = { 0, CFS_EXCLUDE, { x, y }, { x, y, x + width, y + height } };
        ::ImmSetCandidateWindow(imm_context, &exclude_rectangle);

        ::ImmReleaseContext(hwnd_, imm_context);
    }

    void OsrImeHandler::MoveImeWindow(int x, int y)
    {
        caretX_ = x;
        caretY_ = y;

        MoveImeWindow();
    }


    void OsrImeHandler::CleanupComposition()
    {
        // Notify the IMM attached to the given window to complete the ongoing
        // composition (when given window is de-activated while composing and
        // re-activated) and reset the composition status.
        if (is_composing_)
        {
            HIMC imc = ::ImmGetContext(hwnd_);
            if (imc)
            {
                ::ImmNotifyIME(imc, NI_COMPOSITIONSTR, CPS_COMPLETE, 0);
                ::ImmReleaseContext(hwnd_, imc);
            }
            ResetComposition();
        }
    }

    void OsrImeHandler::ResetComposition()
    {
        // Reset the composition status.
        is_composing_ = false;
        cursor_index_ = -1;
    }

    void OsrImeHandler::GetCompositionInfo(HIMC imc, LPARAM lparam, CefString &composition_text, std::vector<CefCompositionUnderline> &underlines, int& composition_start)
    {
        // We only care about GCS_COMPATTR, GCS_COMPCLAUSE and GCS_CURSORPOS, and
        // convert them into underlines and selection range respectively.
        underlines.clear();

        int length = static_cast<int>(composition_text.length());

        // Find out the range selected by the user.
        int target_start = length;
        int target_end = length;
        if (lparam & GCS_COMPATTR)
        {
            GetCompositionSelectionRange(imc, &target_start, &target_end);
        }

        // Retrieve the selection range information. If CS_NOMOVECARET is specified
        // it means the cursor should not be moved and we therefore place the caret at
        // the beginning of the composition string. Otherwise we should honour the
        // GCS_CURSORPOS value if it's available.
        // TODO(suzhe): Due to a bug in WebKit we currently can't use selection range
        // with composition string.
        // See: https://bugs.webkit.org/show_bug.cgi?id=40805
        if (!(lparam & CS_NOMOVECARET) && (lparam & GCS_CURSORPOS))
        {
            // IMM32 does not support non-zero-width selection in a composition. So
            // always use the caret position as selection range.
            int cursor = ::ImmGetCompositionString(imc, GCS_CURSORPOS, NULL, 0);
            composition_start = cursor;
        }
        else
        {
            composition_start = 0;
        }

        // Retrieve the clause segmentations and convert them to underlines.
        if (lparam & GCS_COMPCLAUSE)
        {
            GetCompositionUnderlines(imc, target_start, target_end, underlines);
        }

        // Set default underlines in case there is no clause information.
        if (!underlines.size())
        {
            CefCompositionUnderline underline;
            underline.color = ColorUNDERLINE;
            underline.background_color = ColorBKCOLOR;
            if (target_start > 0)
            {
                underline.range.from = 0;
                underline.range.to = target_start;
                underline.thick = 0;
                underlines.push_back(underline);
            }
            if (target_end > target_start)
            {
                underline.range.from = target_start;
                underline.range.to = target_end;
                underline.thick = 1;
                underlines.push_back(underline);
            }
            if (target_end < length)
            {
                underline.range.from = target_end;
                underline.range.to = length;
                underline.thick = 0;
                underlines.push_back(underline);
            }
        }
    }

    bool OsrImeHandler::GetString(HIMC imc, WPARAM lparam, int type, CefString& result)
    {
        if (!(lparam & type))
            return false;

        LONG string_size = ::ImmGetCompositionString(imc, type, NULL, 0);
        if (string_size <= 0)
            return false;

        // For trailing NULL - ImmGetCompositionString excludes that.
        string_size += sizeof(WCHAR);

        std::vector<wchar_t> buffer(string_size);
        ::ImmGetCompositionString(imc, type, &buffer[0], string_size);
        result.FromWString(&buffer[0]);

        return true;
    }

    bool OsrImeHandler::GetResult(LPARAM lparam, CefString& result)
    {
        bool ret = false;
        HIMC imc = ::ImmGetContext(hwnd_);
        if (imc)
        {
            ret = GetString(imc, lparam, GCS_RESULTSTR, result);
            ::ImmReleaseContext(hwnd_, imc);
        }
        return ret;
    }

    bool OsrImeHandler::GetComposition(LPARAM lparam, CefString &composition_text, std::vector<CefCompositionUnderline> &underlines, int& composition_start)
    {
        bool ret = false;
        HIMC imc = ::ImmGetContext(hwnd_);
        if (imc)
        {
            // Copy the composition string to the CompositionText object.
            ret = GetString(imc, lparam, GCS_COMPSTR, composition_text);

            if (ret)
            {
                // Retrieve the composition underlines and selection range information.
                GetCompositionInfo(imc, lparam, composition_text, underlines, composition_start);

                // Mark that there is an ongoing composition.
                is_composing_ = true;
            }

            ::ImmReleaseContext(hwnd_, imc);
        }
        return ret;
    }

    void OsrImeHandler::DisableIME()
    {
        CleanupComposition();
        ::ImmAssociateContextEx(hwnd_, NULL, 0);
    }

    void OsrImeHandler::CancelIME()
    {
        if (is_composing_)
        {
            HIMC imc = ::ImmGetContext(hwnd_);
            if (imc)
            {
                ::ImmNotifyIME(imc, NI_COMPOSITIONSTR, CPS_CANCEL, 0);
                ::ImmReleaseContext(hwnd_, imc);
            }
            ResetComposition();
        }
    }

    void OsrImeHandler::EnableIME()
    {
        // Load the default IME context.
        ::ImmAssociateContextEx(hwnd_, NULL, IACE_DEFAULT);
    }

    void OsrImeHandler::UpdateCaretPosition(int index)
    {
        // Save the caret position.
        cursor_index_ = index;
        // Move the IME window.
        MoveImeWindow();
    }

    void OsrImeHandler::ChangeCompositionRange(const CefRange& selection_range, const std::vector<CefRect>& bounds)
    {
        composition_range_ = selection_range;
        composition_bounds_ = bounds;
        MoveImeWindow();
    }

}  // namespace CefSharp
