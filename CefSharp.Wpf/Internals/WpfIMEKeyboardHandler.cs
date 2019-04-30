// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;
using Point = System.Windows.Point;
using CefSharp;
using CefSharp.Structs;
using CefSharp.Wpf;
using CefSharp.Wpf.Internals;
using Rect = CefSharp.Structs.Rect;

namespace CefSharp.Wpf.Internals
{
    public class WpfIMEKeyboardHandler : WpfKeyboardHandler
    {
        private int languageCodeId;
        private bool systemCaret;
        private bool isDisposed;
        private List<Rect> compositionBounds = new List<Rect>();
        private HwndSource source;
        private HwndSourceHook sourceHook;
        private bool hasIMEComposition;

        internal bool IsActive { get; set; }

        public WpfIMEKeyboardHandler(ChromiumWebBrowser owner): base(owner)
        {
        }

        public void OnImeCompositionRangeChanged(Control browser, ScreenInfo? screenInfo, Range selectedRange, Rect[] characterBounds)
        {
            var scaleFactor = screenInfo.HasValue ? screenInfo.Value.DeviceScaleFactor : 1.0f;

            var browserOffsetWPFWindow = browser.TransformToAncestor(Window.GetWindow(browser)).Transform(new Point());

            var rects = new List<Structs.Rect>();

            foreach (var item in characterBounds)
            {
                rects.Add(new Structs.Rect(
                    (int)((browserOffsetWPFWindow.X + item.X) * scaleFactor),
                    (int)((browserOffsetWPFWindow.Y + item.Y) * scaleFactor),
                    (int)(item.Width * scaleFactor),
                    (int)(item.Height * scaleFactor)));
            }

            compositionBounds = rects;
            MoveImeWindow(source.Handle);
        }

        private void OwnerLostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            IsActive = false;

            // These calls are needed in order for IME to function correctly.
            InputMethod.SetIsInputMethodEnabled(owner, false);
            InputMethod.SetIsInputMethodSuspended(owner, false);
        }

        private void OwnerGotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            // These calls are needed in order for IME to function correctly.
            InputMethod.SetIsInputMethodEnabled(owner, true);
            InputMethod.SetIsInputMethodSuspended(owner, true);

            IsActive = true;
        }

        public override void Setup(HwndSource source)
        {
            this.source = source;
            sourceHook = SourceHook;
            source.AddHook(SourceHook);

            owner.GotFocus += OwnerGotFocus;
            owner.LostFocus += OwnerLostFocus;

            owner.Focus();
        }

        public override void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            isDisposed = true;

            owner.GotFocus -= OwnerGotFocus;
            owner.LostFocus -= OwnerLostFocus;

            if (source != null && sourceHook != null)
            {
                source.RemoveHook(sourceHook);
                source = null;
            }
        }

        private IntPtr SourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = false;

            if (owner == null || owner.GetBrowserHost() == null || owner.IsDisposed || !IsActive || isDisposed)
            {
                return IntPtr.Zero;
            }

            switch ((WM)msg)
            {
                case WM.LBUTTONDOWN:
                {
                    if (hasIMEComposition)
                    {
                        // Set focus to 0, which destroys IME suggestions window.
                        NativeIME.SetFocus(IntPtr.Zero);
                        // Restore focus.
                        NativeIME.SetFocus(source.Handle);
                    }
                }
                break;

                case WM.IME_SETCONTEXT:
                {
                    OnIMESetContext(hwnd, (uint)msg, wParam, lParam);
                    handled = true;
                }
                break;

                case WM.IME_STARTCOMPOSITION:
                {
                    OnIMEStartComposition(hwnd);
                    hasIMEComposition = true;
                    handled = true;
                }
                break;

                case WM.IME_COMPOSITION:
                {
                    OnIMEComposition(hwnd, lParam.ToInt32());
                    handled = true;
                }
                break;

                case WM.IME_ENDCOMPOSITION:
                {
                    OnIMEEndComposition(hwnd);
                    hasIMEComposition = false;
                    handled = true;
                }
                break;
            }

            return handled ? IntPtr.Zero : new IntPtr(1);
        }

        private void OnIMEComposition(IntPtr hwnd, int lParam)
        {
            string text = string.Empty;

            var handler = new IMEHandler(hwnd);

            if (handler.GetResult((uint)lParam, out text))
            {
                owner.GetBrowserHost().ImeCommitText(text, new Range(Int32.MaxValue, Int32.MaxValue), 0);
            }
            else
            {
                var underlines = new List<CompositionUnderline>();
                int compositionStart = 0;

                if (handler.GetComposition((uint)lParam, underlines, ref compositionStart, out text))
                {
                    owner.GetBrowserHost().ImeSetComposition(text, underlines.ToArray(), new Range(Int32.MaxValue, Int32.MaxValue), new Range(compositionStart, compositionStart));

                    UpdateCaretPosition(compositionStart - 1);
                }
                else
                {
                    CancelComposition(hwnd);
                }
            }
        }

        public void CancelComposition(IntPtr hwnd)
        {
            owner.GetBrowserHost().ImeCancelComposition();
            DestroyImeWindow(hwnd);
        }

        private void OnIMEEndComposition(IntPtr hwnd)
        {
            owner.GetBrowserHost().ImeFinishComposingText(false);
            DestroyImeWindow(hwnd);
        }

        private void OnIMESetContext(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            // We handle the IME Composition Window ourselves (but let the IME Candidates
            // Window be handled by IME through DefWindowProc()), so clear the
            // ISC_SHOWUICOMPOSITIONWINDOW flag:
            NativeIME.DefWindowProc(hwnd, msg, wParam, (IntPtr)(lParam.ToInt64() & ~NativeIME.ISC_SHOWUICOMPOSITIONWINDOW));
            // TODO: should we call ImmNotifyIME?

            CreateImeWindow(hwnd);
            MoveImeWindow(hwnd);
        }

        private void OnIMEStartComposition(IntPtr hwnd)
        {
            CreateImeWindow(hwnd);
            MoveImeWindow(hwnd);
        }

        private void CreateImeWindow(IntPtr hwnd)
        {
            // Chinese/Japanese IMEs somehow ignore function calls to
            // ::ImmSetCandidateWindow(), and use the position of the current system
            // caret instead -::GetCaretPos().
            // Therefore, we create a temporary system caret for Chinese IMEs and use
            // it during this input context.
            // Since some third-party Japanese IME also uses ::GetCaretPos() to determine
            // their window position, we also create a caret for Japanese IMEs.
            languageCodeId = PrimaryLangId(InputLanguageManager.Current.CurrentInputLanguage.KeyboardLayoutId);

            if (languageCodeId == NativeIME.LANG_JAPANESE || languageCodeId == NativeIME.LANG_CHINESE)
            {
                if (!systemCaret)
                {
                    if (NativeIME.CreateCaret(hwnd, IntPtr.Zero, 1, 1))
                    {
                        systemCaret = true;
                    }
                }
            }
        }

        private int PrimaryLangId(int lgid)
        {
            return (lgid & 0x3ff);
        }

        private void MoveImeWindow(IntPtr hwnd)
        {
            if (0 == compositionBounds.Count)
            {
                return;
            }

            IntPtr hIMC = NativeIME.ImmGetContext(hwnd);

            Rect rc = compositionBounds[0];

            int x = rc.X + rc.Width;
            int y = rc.Y + rc.Height;

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
            var candidatePosition = new NativeIME.CANDIDATEFORM
            {
                dwIndex = 0,
                dwStyle = (int)NativeIME.CFS_CANDIDATEPOS,
                ptCurrentPos = new NativeIME.POINT(x, y),
                rcArea = new NativeIME.RECT(0, 0, 0, 0)
            };
            NativeIME.ImmSetCandidateWindow(hIMC, ref candidatePosition);

            if (systemCaret)
            {
                NativeIME.SetCaretPos(x, y);
            }

            if (languageCodeId == NativeIME.LANG_KOREAN)
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
            var excludeRectangle = new NativeIME.CANDIDATEFORM
            {
                dwIndex = 0,
                dwStyle = (int)NativeIME.CFS_EXCLUDE,
                ptCurrentPos = new NativeIME.POINT(x, y),
                rcArea = new NativeIME.RECT(rc.X, rc.Y, x, y + kCaretMargin)
            };
            NativeIME.ImmSetCandidateWindow(hIMC, ref excludeRectangle);

            NativeIME.ImmReleaseContext(hwnd, hIMC);
        }

        private void DestroyImeWindow(IntPtr hwnd)
        {
            if (systemCaret)
            {
                NativeIME.DestroyCaret();
                systemCaret = false;
            }
        }

        private void UpdateCaretPosition(int index)
        {
            MoveImeWindow(source.Handle);
        }
    }
}
