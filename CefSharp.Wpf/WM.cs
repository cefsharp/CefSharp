namespace CefSharp.Wpf
{
    // Gratiously based on http://www.pinvoke.net/default.aspx/Enums/WindowsMessages.html
    public enum WM : uint
    {
        /// <summary>
        /// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem key is pressed. A nonsystem
        /// key is a key that is pressed when the ALT key is not pressed.
        /// </summary>
        KEYDOWN = 0x0100,
    
        /// <summary>
        /// The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem key is released. A nonsystem
        /// key is a key that is pressed when the ALT key is not pressed, or a keyboard key that is pressed when a window has the
        /// keyboard focus.
        /// </summary>
        KEYUP = 0x0101,

        /// <summary>
        /// The WM_CHAR message is posted to the window with the keyboard focus when a WM_KEYDOWN message is translated by the
        /// TranslateMessage function. The WM_CHAR message contains the character code of the key that was pressed.
        /// </summary>
        CHAR = 0x0102,

        /// <summary>
        /// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user presses the F10 key (which
        /// activates the menu bar) or holds down the ALT key and then presses another key. It also occurs when no window
        /// currently has the keyboard focus; in this case, the WM_SYSKEYDOWN message is sent to the active window. The window
        /// that receives the message can distinguish between these two contexts by checking the context code in the lParam
        /// parameter.
        /// </summary>
        SYSKEYDOWN = 0x0104,

        /// <summary>
        /// The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user releases a key that was pressed
        /// while the ALT key was held down. It also occurs when no window currently has the keyboard focus; in this case, the
        /// WM_SYSKEYUP message is sent to the active window. The window that receives the message can distinguish between these
        /// two contexts by checking the context code in the lParam parameter.
        /// </summary>
        SYSKEYUP = 0x0105,

        /// <summary>
        /// The WM_SYSCHAR message is posted to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated by
        /// the TranslateMessage function. It specifies the character code of a system character key — that is, a character key
        /// that is pressed while the ALT key is down.
        /// </summary>
        SYSCHAR = 0x0106,

        /// <summary>
        /// Sent to an application when the IME gets a character of the conversion result. A window receives this message through
        /// its WindowProc function. 
        /// </summary>
        IME_CHAR = 0x0286,
    }
}
