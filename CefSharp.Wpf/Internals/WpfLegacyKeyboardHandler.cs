// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Input;
using System.Windows.Interop;
using CefSharp.Internals;

namespace CefSharp.Wpf.Internals 
{
    public class WpfLegacyKeyboardHandler : IWpfKeyboardHandler
    {
        /// <summary>
        /// The source hook		
        /// </summary>		
        private HwndSourceHook sourceHook;

        /// <summary>
        /// The source		
        /// </summary>		
        private HwndSource source;

        /// <summary>
        /// The owner browser instance
        /// </summary>
        private readonly ChromiumWebBrowser owner;

        public WpfLegacyKeyboardHandler(ChromiumWebBrowser owner) 
        {
            this.owner = owner;
        }
    
        public void Setup(HwndSource source) 
        {
            this.source = source;
            if (source != null) 
            {
                sourceHook = SourceHook;
                source.AddHook(SourceHook);
            }
        }

        public void Dispose()
        {		
            if (source != null && sourceHook != null)		
            {		
                source.RemoveHook(sourceHook);
            }
            source = null;
        }

        /// <summary>		
        /// WindowProc callback interceptor. Handles Windows messages intended for the source hWnd, and passes them to the		
        /// contained browser as needed.		
        /// </summary>		
        /// <param name="hWnd">The source handle.</param>		
        /// <param name="message">The message.</param>		
        /// <param name="wParam">Additional message info.</param>		
        /// <param name="lParam">Even more message info.</param>		
        /// <param name="handled">if set to <c>true</c>, the event has already been handled by someone else.</param>		
        /// <returns>IntPtr.</returns>		
        protected virtual IntPtr SourceHook(IntPtr hWnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
        {		
            if (handled)		
            {		
                return IntPtr.Zero;		
            }		
            
            switch ((WM)message)		
            {		
                case WM.SYSCHAR:		
                case WM.SYSKEYDOWN:		
                case WM.SYSKEYUP:		
                case WM.KEYDOWN:		
                case WM.KEYUP:		
                case WM.CHAR:		
                case WM.IME_CHAR:		
                {		
                    if (!owner.IsKeyboardFocused)		
                    {		
                        break;		
                    }		
            
                    if (message == (int)WM.SYSKEYDOWN &&		
                        wParam.ToInt32() == KeyInterop.VirtualKeyFromKey(Key.F4))		
                    {		
                        // We don't want CEF to receive this event (and mark it as handled), since that makes it impossible to		
                        // shut down a CefSharp-based app by pressing Alt-F4, which is kind of bad.		
                        return IntPtr.Zero;		
                    }

                    var browser = owner.GetBrowser();
                    if (browser != null)		
                    {		
                        browser.GetHost().SendKeyEvent(message, wParam.CastToInt32(), lParam.CastToInt32());		
                        handled = true;		
                    }		
            
                    break;		
                }		
            }		
            
            return IntPtr.Zero;		
        }

        public virtual void HandleKeyPress(KeyEventArgs e) 
        {
            // As KeyDown and KeyUp bubble, it appears they're being handled before they get a chance to
            // trigger the appropriate WM_ messages handled by our SourceHook, so we have to handle these extra keys here.
            // Hooking the Tab key like this makes the tab focusing in essence work like
            // KeyboardNavigation.TabNavigation="Cycle"; you will never be able to Tab out of the web browser control.
            // We also add the condition to allow ctrl+a to work when the web browser control is put inside listbox.
            if (e.Key == Key.Tab || e.Key == Key.Home || e.Key == Key.End || e.Key == Key.Up
                                 || e.Key == Key.Down || e.Key == Key.Left || e.Key == Key.Right
                                 || (e.Key == Key.A && Keyboard.Modifiers == ModifierKeys.Control))
            {
                var modifiers = e.GetModifiers();
                var message = (int)(e.IsDown ? WM.KEYDOWN : WM.KEYUP);
                var virtualKey = KeyInterop.VirtualKeyFromKey(e.Key);
                var browser = owner.GetBrowser();

                if (browser != null)
                {
                    browser.GetHost().SendKeyEvent(message, virtualKey, (int)modifiers);
                    e.Handled = true;
                }
            }
        }

        public virtual void HandleTextInput(TextCompositionEventArgs e) 
        {
            // nothing to do here
        }
    }
}
