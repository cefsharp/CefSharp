// Copyright © 2021 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.Handler
{
    /// <summary>
    /// Inherit from this class to handle events related to keyboard input.
    /// </summary>
    public class KeyboardHandler : IKeyboardHandler
    {
        /// <summary>
        /// Called before a keyboard event is sent to the renderer.
        /// Return true if the event was handled or false
        /// otherwise. If the event will be handled in <see cref="OnKeyEvent"/> as a keyboard
        /// shortcut set isKeyboardShortcut to true and return false.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">The browser instance.</param>
        /// <param name="type">Whether this was a key up/down/raw/etc...</param>
        /// <param name="windowsKeyCode">
        /// The Windows key code for the key event. This value is used by the DOM
        /// specification. Sometimes it comes directly from the event (i.e. on
        /// Windows) and sometimes it's determined using a mapping function. See
        /// WebCore/platform/chromium/KeyboardCodes.h for the list of values.
        /// </param>
        /// <param name="nativeKeyCode">The native key code. On Windows this appears to be in the format of WM_KEYDOWN/WM_KEYUP/etc... lParam data.</param>
        /// <param name="modifiers">What other modifier keys are currently down: Shift/Control/Alt/OS X Command/etc...</param>
        /// <param name="isSystemKey">
        /// Indicates whether the event is considered a "system key" event (see
        /// http://msdn.microsoft.com/en-us/library/ms646286(VS.85).aspx for details).
        /// </param>
        /// <param name="isKeyboardShortcut">See the summary for an explanation of when to set this to true.</param>
        /// <returns>Returns true if the event was handled or false otherwise.</returns>
        bool IKeyboardHandler.OnPreKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
        {
            return OnPreKeyEvent(chromiumWebBrowser, browser, type, windowsKeyCode, nativeKeyCode, modifiers, isSystemKey, ref isKeyboardShortcut);
        }

        /// <summary>
        /// Called before a keyboard event is sent to the renderer.
        /// Return true if the event was handled or false
        /// otherwise. If the event will be handled in <see cref="OnKeyEvent"/> as a keyboard
        /// shortcut set isKeyboardShortcut to true and return false.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">The browser instance.</param>
        /// <param name="type">Whether this was a key up/down/raw/etc...</param>
        /// <param name="windowsKeyCode">
        /// The Windows key code for the key event. This value is used by the DOM
        /// specification. Sometimes it comes directly from the event (i.e. on
        /// Windows) and sometimes it's determined using a mapping function. See
        /// WebCore/platform/chromium/KeyboardCodes.h for the list of values.
        /// </param>
        /// <param name="nativeKeyCode">The native key code. On Windows this appears to be in the format of WM_KEYDOWN/WM_KEYUP/etc... lParam data.</param>
        /// <param name="modifiers">What other modifier keys are currently down: Shift/Control/Alt/OS X Command/etc...</param>
        /// <param name="isSystemKey">
        /// Indicates whether the event is considered a "system key" event (see
        /// http://msdn.microsoft.com/en-us/library/ms646286(VS.85).aspx for details).
        /// </param>
        /// <param name="isKeyboardShortcut">See the summary for an explanation of when to set this to true.</param>
        /// <returns>Returns true if the event was handled or false otherwise.</returns>
        protected virtual bool OnPreKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
        {
            return false;
        }

        /// <summary>
        /// Called after the renderer and JavaScript in the page has had a chance to
        /// handle the event. Return true if the keyboard event was handled or false otherwise.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">The browser instance.</param>
        /// <param name="type">Whether this was a key up/down/raw/etc...</param>
        /// <param name="windowsKeyCode">
        /// The Windows key code for the key event. This value is used by the DOM
        /// specification. Sometimes it comes directly from the event (i.e. on
        /// Windows) and sometimes it's determined using a mapping function. See
        /// WebCore/platform/chromium/KeyboardCodes.h for the list of values.
        /// </param>
        /// <param name="nativeKeyCode">The native key code. On Windows this appears to be in the format of WM_KEYDOWN/WM_KEYUP/etc... lParam data.</param>
        /// <param name="modifiers">What other modifier keys are currently down: Shift/Control/Alt/OS X Command/etc...</param>
        /// <param name="isSystemKey">
        /// Indicates whether the event is considered a "system key" event (see
        /// http://msdn.microsoft.com/en-us/library/ms646286(VS.85).aspx for details).
        /// </param>
        /// <returns>Return true if the keyboard event was handled or false otherwise.</returns>
        bool IKeyboardHandler.OnKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
        {
            return OnKeyEvent(chromiumWebBrowser, browser, type, windowsKeyCode, nativeKeyCode, modifiers, isSystemKey);
        }

        /// <summary>
        /// Called after the renderer and JavaScript in the page has had a chance to
        /// handle the event. Return true if the keyboard event was handled or false otherwise.
        /// </summary>
        /// <param name="chromiumWebBrowser">the ChromiumWebBrowser control</param>
        /// <param name="browser">The browser instance.</param>
        /// <param name="type">Whether this was a key up/down/raw/etc...</param>
        /// <param name="windowsKeyCode">
        /// The Windows key code for the key event. This value is used by the DOM
        /// specification. Sometimes it comes directly from the event (i.e. on
        /// Windows) and sometimes it's determined using a mapping function. See
        /// WebCore/platform/chromium/KeyboardCodes.h for the list of values.
        /// </param>
        /// <param name="nativeKeyCode">The native key code. On Windows this appears to be in the format of WM_KEYDOWN/WM_KEYUP/etc... lParam data.</param>
        /// <param name="modifiers">What other modifier keys are currently down: Shift/Control/Alt/OS X Command/etc...</param>
        /// <param name="isSystemKey">
        /// Indicates whether the event is considered a "system key" event (see
        /// http://msdn.microsoft.com/en-us/library/ms646286(VS.85).aspx for details).
        /// </param>
        /// <returns>Return true if the keyboard event was handled or false otherwise.</returns>
        protected virtual bool OnKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
        {
            return false;
        }
    }
}
