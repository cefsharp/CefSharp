// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Windows.Input;
using System.Windows.Interop;

namespace CefSharp.Wpf.Internals 
{
    public class WpfKeyboardHandler : IWpfKeyboardHandler 
    {
        /// <summary>
        /// The owner browser instance
        /// </summary>
        private readonly ChromiumWebBrowser owner;

        public WpfKeyboardHandler(ChromiumWebBrowser owner) 
        {
            this.owner = owner;
        }

        public void Setup(HwndSource source) 
        {
            // nothing to do here
        }

        public void Dispose()
        {
            // nothing to do here
        }

        public virtual void HandleKeyPress(KeyEventArgs e) 
        {
            var browser = owner.GetBrowser();
            var key = e.SystemKey == Key.None ? e.Key : e.SystemKey;
            if (browser != null) 
            {
                int message;
                int virtualKey = 0;
                var modifiers = e.GetModifiers();

                switch (key) 
                {
                    case Key.LeftAlt:
                    case Key.RightAlt:
                    { 
                        virtualKey = (int) VirtualKeys.Menu;
                        break;
                    }

                    case Key.LeftCtrl:
                    case Key.RightCtrl:
                    { 
                        virtualKey = (int) VirtualKeys.Control;
                        break;
                    }

                    case Key.LeftShift:
                    case Key.RightShift:
                    { 
                        virtualKey = (int) VirtualKeys.Shift;
                        break;
                    }

                    default:
                        virtualKey = KeyInterop.VirtualKeyFromKey(key);
                        break;
                }

                if (e.IsDown)
                {
                    message = (int)(e.SystemKey != Key.None ? WM.SYSKEYDOWN : WM.KEYDOWN);
                }
                else
                {
                    message = (int)(e.SystemKey != Key.None ? WM.SYSKEYUP : WM.KEYUP);
                }

                browser.GetHost().SendKeyEvent(message, virtualKey, (int)modifiers);
            }

            // Hooking the Tab key like this makes the tab focusing in essence work like
            // KeyboardNavigation.TabNavigation="Cycle"; you will never be able to Tab out of the web browser control.
            // We also add the condition to allow ctrl+a to work when the web browser control is put inside listbox.
            // Prevent keyboard navigation using arrows and home and end keys
            if (key == Key.Tab || key == Key.Home || key == Key.End || key == Key.Up
                               || key == Key.Down || key == Key.Left || key == Key.Right
                               || (key == Key.A && Keyboard.Modifiers == ModifierKeys.Control)) 
            {
                e.Handled = true;
            }
        }

        public virtual void HandleTextInput(TextCompositionEventArgs e) 
        {
            var browser = owner.GetBrowser();
            if (browser != null) 
            {
                var browserHost = browser.GetHost();
                for (int i = 0; i < e.Text.Length; i++) 
                {
                    browserHost.SendKeyEvent((int)WM.CHAR, e.Text[i], 0);
                }
                e.Handled = true;
            }
        }   
    }
}