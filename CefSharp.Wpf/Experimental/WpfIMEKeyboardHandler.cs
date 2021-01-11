// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using CefSharp.Internals;
using CefSharp.Structs;
using CefSharp.Wpf.Internals;
using Point = System.Windows.Point;
using Range = CefSharp.Structs.Range;
using Rect = CefSharp.Structs.Rect;

namespace CefSharp.Wpf.Experimental
{
    /// <summary>
    /// A WPF Keyboard handler implementation that supports IME
    /// </summary>
    /// <seealso cref="T:CefSharp.Wpf.Internals.WpfKeyboardHandler"/>
    public class WpfImeKeyboardHandler : WpfKeyboardHandler
    {
        private int languageCodeId;
        private bool systemCaret;
        private bool isSetup;
        private List<Rect> compositionBounds = new List<Rect>();
        private HwndSource source;
        private HwndSourceHook sourceHook;
        private bool hasImeComposition;
        private MouseButtonEventHandler mouseDownEventHandler;
        private bool isActive;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public WpfImeKeyboardHandler(ChromiumWebBrowser owner) : base(owner)
        {
        }

        /// <summary>
        /// Change composition range.
        /// </summary>
        /// <param name="selectionRange">The selection range.</param>
        /// <param name="characterBounds">The character bounds.</param>
        public void ChangeCompositionRange(Range selectionRange, Rect[] characterBounds)
        {
            if (!isActive)
            {
                return;
            }

            var screenInfo = ((IRenderWebBrowser)owner).GetScreenInfo();
            var scaleFactor = screenInfo.HasValue ? screenInfo.Value.DeviceScaleFactor : 1.0f;

            //This is called on the CEF UI thread, we need to invoke back onte main UI thread to
            //access the UI controls
            owner.UiThreadRunAsync(() =>
            {
                //TODO: Getting the root window for every composition range change seems expensive,
                //we should cache the position and update it on window move.
                var parentWindow = Window.GetWindow(owner);
                if (parentWindow != null)
                {
                    //TODO: What are we calculating here exactly???
                    var point = owner.TransformToAncestor(parentWindow).Transform(new Point(0, 0));

                    var rects = new List<Rect>();

                    foreach (var item in characterBounds)
                    {
                        rects.Add(new Rect(
                            (int)((point.X + item.X) * scaleFactor),
                            (int)((point.Y + item.Y) * scaleFactor),
                            (int)(item.Width * scaleFactor),
                            (int)(item.Height * scaleFactor)));
                    }

                    compositionBounds = rects;
                    MoveImeWindow(source.Handle);
                }
            });
        }

        /// <summary>
        /// Setup the Ime Keyboard Handler specific hooks and events
        /// </summary>
        /// <param name="source">HwndSource.</param>
        public override void Setup(HwndSource source)
        {
            if (isSetup)
            {
                return;
            }

            isSetup = true;

            this.source = source;
            sourceHook = SourceHook;
            source.AddHook(SourceHook);

            owner.GotFocus += OwnerGotFocus;
            owner.LostFocus += OwnerLostFocus;

            mouseDownEventHandler = new MouseButtonEventHandler(OwnerMouseDown);

            owner.AddHandler(UIElement.MouseDownEvent, mouseDownEventHandler, true);

            // If the owner had focus before adding the handler then we have to run the "got focus" code here
            // or it won't set up IME properly in all cases
            if (owner.IsFocused)
            {
                SetActive();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            // Note Setup can be run after disposing, to "reset" this instance
            // due to the code in ChromiumWebBrowser.PresentationSourceChangedHandler
            if (!isSetup)
            {
                return;
            }

            isSetup = false;

            owner.GotFocus -= OwnerGotFocus;
            owner.LostFocus -= OwnerLostFocus;

            owner.RemoveHandler(UIElement.MouseDownEvent, mouseDownEventHandler);

            if (source != null && sourceHook != null)
            {
                source.RemoveHook(sourceHook);
                source = null;
            }
        }

        private void OwnerMouseDown(object sender, MouseButtonEventArgs e)
        {
            CloseImeComposition();
        }

        private void OwnerGotFocus(object sender, RoutedEventArgs e)
        {
            SetActive();
        }

        private void OwnerLostFocus(object sender, RoutedEventArgs e)
        {
            SetInactive();
        }

        private void SetActive()
        {
            // Set to false first if not already, because the value change (and raising of changes)
            // between false and true is necessary for IME to work in all circumstances
            if (InputMethod.GetIsInputMethodEnabled(owner))
            {
                InputMethod.SetIsInputMethodEnabled(owner, false);
            }
            if (InputMethod.GetIsInputMethodSuspended(owner))
            {
                InputMethod.SetIsInputMethodSuspended(owner, false);
            }

            // These calls are needed in order for IME to function correctly.
            InputMethod.SetIsInputMethodEnabled(owner, true);
            InputMethod.SetIsInputMethodSuspended(owner, true);

            isActive = true;
        }

        private void SetInactive()
        {
            isActive = false;

            // These calls are needed in order for IME to function correctly.
            InputMethod.SetIsInputMethodEnabled(owner, false);
            InputMethod.SetIsInputMethodSuspended(owner, false);
        }

        private IntPtr SourceHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = false;

            if (!isActive || !isSetup || owner == null || owner.IsDisposed || owner.GetBrowserHost() == null)
            {
                return IntPtr.Zero;
            }

            switch ((WM)msg)
            {
                case WM.IME_SETCONTEXT:
                {
                    OnImeSetContext(hwnd, (uint)msg, wParam, lParam);
                    handled = true;
                    break;
                }
                case WM.IME_STARTCOMPOSITION:
                {
                    OnIMEStartComposition(hwnd);
                    hasImeComposition = true;
                    handled = true;
                    break;
                }
                case WM.IME_COMPOSITION:
                {
                    OnImeComposition(hwnd, lParam.ToInt32());
                    handled = true;
                    break;
                }
                case WM.IME_ENDCOMPOSITION:
                {
                    OnImeEndComposition(hwnd);
                    hasImeComposition = false;
                    handled = true;
                    break;
                }
            }

            return handled ? IntPtr.Zero : new IntPtr(1);
        }

        private void CloseImeComposition()
        {
            if (hasImeComposition)
            {
                // Set focus to 0, which destroys IME suggestions window.
                ImeNative.SetFocus(IntPtr.Zero);
                // Restore focus.
                ImeNative.SetFocus(source.Handle);
            }
        }

        private void OnImeComposition(IntPtr hwnd, int lParam)
        {
            string text = string.Empty;

            if (ImeHandler.GetResult(hwnd, (uint)lParam, out text))
            {
                owner.GetBrowserHost().ImeCommitText(text, new Range(int.MaxValue, int.MaxValue), 0);
                if (languageCodeId == ImeNative.LANG_KOREAN || languageCodeId == ImeNative.LANG_CHINESE)
                {
                    owner.GetBrowserHost().ImeSetComposition(text, new CompositionUnderline[0], new Range(int.MaxValue, int.MaxValue), new Range(0, 0));
                    owner.GetBrowserHost().ImeFinishComposingText(false);
                }
            }
            else
            {
                var underlines = new List<CompositionUnderline>();
                int compositionStart = 0;

                if (ImeHandler.GetComposition(hwnd, (uint)lParam, underlines, ref compositionStart, out text))
                {
                    owner.GetBrowserHost().ImeSetComposition(text, underlines.ToArray(),
                        new Range(int.MaxValue, int.MaxValue), new Range(compositionStart, compositionStart));

                    UpdateCaretPosition(compositionStart - 1);
                }
                else
                {
                    CancelComposition(hwnd);
                }
            }
        }

        /// <summary>
        /// Cancel composition.
        /// </summary>
        /// <param name="hwnd">The hwnd.</param>
        public void CancelComposition(IntPtr hwnd)
        {
            owner.GetBrowserHost().ImeCancelComposition();
            DestroyImeWindow(hwnd);
        }

        private void OnImeEndComposition(IntPtr hwnd)
        {
            // Korean IMEs somehow ignore function calls to ::ImeFinishComposingText()
            // The same letter is commited in ::OnImeComposition()
            if (languageCodeId != ImeNative.LANG_KOREAN)
            {
                owner.GetBrowserHost().ImeFinishComposingText(false);
            }
            DestroyImeWindow(hwnd);
        }

        private void OnImeSetContext(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            // We handle the IME Composition Window ourselves (but let the IME Candidates
            // Window be handled by IME through DefWindowProc()), so clear the
            // ISC_SHOWUICOMPOSITIONWINDOW flag:
            ImeNative.DefWindowProc(hwnd, msg, wParam, (IntPtr)(lParam.ToInt64() & ~ImeNative.ISC_SHOWUICOMPOSITIONWINDOW));
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

            if (languageCodeId == ImeNative.LANG_JAPANESE || languageCodeId == ImeNative.LANG_CHINESE)
            {
                if (!systemCaret)
                {
                    if (ImeNative.CreateCaret(hwnd, IntPtr.Zero, 1, 1))
                    {
                        systemCaret = true;
                    }
                }
            }
        }

        private int PrimaryLangId(int lgid)
        {
            return lgid & 0x3ff;
        }

        private void MoveImeWindow(IntPtr hwnd)
        {
            if (compositionBounds.Count == 0)
            {
                return;
            }

            var hIMC = ImeNative.ImmGetContext(hwnd);

            var rc = compositionBounds[0];

            var x = rc.X + rc.Width;
            var y = rc.Y + rc.Height;

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
            var candidatePosition = new ImeNative.CANDIDATEFORM
            {
                dwIndex = 0,
                dwStyle = (int)ImeNative.CFS_CANDIDATEPOS,
                ptCurrentPos = new ImeNative.POINT(x, y),
                rcArea = new ImeNative.RECT(0, 0, 0, 0)
            };
            ImeNative.ImmSetCandidateWindow(hIMC, ref candidatePosition);

            if (systemCaret)
            {
                ImeNative.SetCaretPos(x, y);
            }

            if (languageCodeId == ImeNative.LANG_CHINESE)
            {
                // Chinese IMEs need set composition window 
                var compositionPotision = new ImeNative.COMPOSITIONFORM
                {
                    dwStyle = (int)ImeNative.CFS_POINT,
                    ptCurrentPos = new ImeNative.POINT(x, y),
                    rcArea = new ImeNative.RECT(0, 0, 0, 0)
                };
                ImeNative.ImmSetCompositionWindow(hIMC, ref compositionPotision);
            }

            if (languageCodeId == ImeNative.LANG_KOREAN)
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
            var excludeRectangle = new ImeNative.CANDIDATEFORM
            {
                dwIndex = 0,
                dwStyle = (int)ImeNative.CFS_EXCLUDE,
                ptCurrentPos = new ImeNative.POINT(x, y),
                rcArea = new ImeNative.RECT(rc.X, rc.Y, x, y + kCaretMargin)
            };
            ImeNative.ImmSetCandidateWindow(hIMC, ref excludeRectangle);

            ImeNative.ImmReleaseContext(hwnd, hIMC);
        }

        private void DestroyImeWindow(IntPtr hwnd)
        {
            if (systemCaret)
            {
                ImeNative.DestroyCaret();
                systemCaret = false;
            }
        }

        //TODO: Should we remove this, it's only a single method
        private void UpdateCaretPosition(int index)
        {
            MoveImeWindow(source.Handle);
        }
    }
}
