// Copyright 2016 The Chromium Embedded Framework Authors. Portions copyright
// 2013 The Chromium Authors. All rights reserved. Use of this source code is
// governed by a BSD-style license that can be found in the LICENSE file.

#ifndef CEF_TESTS_CEFCLIENT_BROWSER_OSR_IME_HANDLER_WIN_H_
#define CEF_TESTS_CEFCLIENT_BROWSER_OSR_IME_HANDLER_WIN_H_
#pragma once

#include "Stdafx.h"
#include <windows.h>

namespace CefSharp 
{

	// Handles IME for the native parent window that hosts an off-screen browser.
	// This object is only accessed on the CEF UI thread.
	class OsrImeHandler 
  {
	public:
		explicit OsrImeHandler(HWND hwnd);
		virtual ~OsrImeHandler();

		// Retrieves whether or not there is an ongoing composition.
		bool is_composing() const { return is_composing_; }

		// Retrieves the input language from Windows and update it.
		void SetInputLanguage();

		// Creates the IME caret windows if required.
		void CreateImeWindow();

		// Destroys the IME caret windows.
		void DestroyImeWindow();

		// Cleans up the all resources attached to the given IMM32Manager object, and
		// reset its composition status.
		void CleanupComposition();

		// Resets the composition status and cancels the ongoing composition.
		void ResetComposition();

		// Retrieves a composition result of the ongoing composition if it exists.
		bool GetResult(LPARAM lparam, CefString& result);

		// Retrieves the current composition status of the ongoing composition.
		// Includes composition text, underline information and selection range in the
		// composition text. IMM32 does not support char selection.
		bool GetComposition(LPARAM lparam, CefString &composition_text, std::vector<CefCompositionUnderline> &underlines, int& composition_start);

		// Enables the IME attached to the given window.
		virtual void EnableIME();

		// Disables the IME attached to the given window.
		virtual void DisableIME();

		// Cancels an ongoing composition of the IME.
		virtual void CancelIME();

		// Updates the IME caret position of the given window.
		void UpdateCaretPosition(int index);

		// Updates the composition range. |selected_range| is the range of characters
		// that have been selected. |character_bounds| is the bounds of each character
		// in view device coordinates.
		void ChangeCompositionRange(const CefRange& selection_range, const std::vector<CefRect>& character_bounds);

		// Updates the position of the IME windows.
		void MoveImeWindow();

        void MoveImeWindow(int x, int y);

	private:
		// Retrieves the composition information.
		void GetCompositionInfo(HIMC imm_context, LPARAM lparam,
			CefString &composition_text,
			std::vector<CefCompositionUnderline>& underlines,
			int& composition_start);

		// Retrieves a string from the IMM.
		bool GetString(HIMC imm_context, WPARAM lparam, int type, CefString& result);

		// Represents whether or not there is an ongoing composition.
		bool is_composing_;

		// The current composition character range and its bounds.
		std::vector<CefRect> composition_bounds_;

		// This value represents whether or not the current input context has IMEs.
		bool ime_status_;

		// The current input Language ID retrieved from Windows -
		// used for processing language-specific operations in IME.
		LANGID input_language_id_;

		// Represents whether or not the current input context has created a system
		// caret to set the position of its IME candidate window.
		bool system_caret_;

		// The rectangle of the input caret retrieved from a renderer process.
		CefRect ime_rect_;

		// The current cursor index in composition string.
		int cursor_index_;

		// The composition range in the string. This may be used to determine the
		// offset in composition bounds.
		CefRange composition_range_;

		// Hwnd associated with this instance.
		HWND hwnd_;

        int caretX_ = 0;
        int caretY_ = 0;
	};

}  // namespace client

#endif  // CEF_TESTS_CEFCLIENT_BROWSER_OSR_IME_HANDLER_WIN_H_
