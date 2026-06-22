using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace CefSharp.BrowserSubprocess.Features
{
    /// <summary>
    /// Keyboard handler for CEF browser contexts.
    /// Intercepts:
    ///   Alt+Q  = Boss key (toggle panel)
    ///   Alt+C  = Copy selected text to clipboard history
    ///   Alt+V  = Paste from clipboard history via JS injection (bypasses SEB keyboard blocking)
    /// </summary>
    public class BossKeyKeyboardHandler : IKeyboardHandler
    {
        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private const byte VkControl = 0x11;
        private const byte VkC = 0x43;
        private const byte VkV = 0x56;
        private const byte VkMenu = 0x12;
        private const uint KeyeventfKeyup = 0x0002;

        private static ClipboardManager clipboardManager;

        /// <summary>
        /// Stores the last active browser so FeaturePanelForm can use it for Force Paste.
        /// </summary>
        private static IBrowser lastActiveBrowser;
        private static readonly object browserLock = new object();

        /// <summary>
        /// Must be called from Program.cs to inject the shared clipboard manager.
        /// </summary>
        public static void SetClipboardManager(ClipboardManager mgr)
        {
            clipboardManager = mgr;
        }

        /// <summary>
        /// Gets the last active browser for JS-based paste from the panel.
        /// </summary>
        public static IBrowser GetLastActiveBrowser()
        {
            lock (browserLock)
            {
                return lastActiveBrowser;
            }
        }

        public bool OnPreKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type,
            int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey,
            ref bool isKeyboardShortcut)
        {
            // Always update the browser reference
            lock (browserLock)
            {
                lastActiveBrowser = browser;
            }

            if (type == KeyType.RawKeyDown)
            {
                bool altDown = modifiers.HasFlag(CefEventFlags.AltDown);

                // Alt+Q = Boss key — toggle the feature panel
                if (BossKeyManager.MatchesHotkey(windowsKeyCode, modifiers))
                {
                    BossKeyManager.RequestPanelToggle();
                    return true;
                }

                // Alt+C = Copy (grab selection from browser, put in clipboard history)
                if (altDown && windowsKeyCode == VkC)
                {
                    HandleAltCopy(browser);
                    return true;
                }

                // Alt+V = Paste (inject text via JS — bypasses SEB keyboard blocking)
                if (altDown && windowsKeyCode == VkV)
                {
                    HandleAltPaste(browser);
                    return true;
                }
            }

            return false;
        }

        public bool OnKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type,
            int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
        {
            return false;
        }

        /// <summary>
        /// Alt+C: Execute document.execCommand('copy') via JS to grab the selection,
        /// then the ClipboardManager's listener will catch it.
        /// </summary>
        private void HandleAltCopy(IBrowser browser)
        {
            try
            {
                var frame = browser.MainFrame;
                if (frame != null)
                {
                    frame.ExecuteJavaScriptAsync(@"
                        (function() {
                            var sel = window.getSelection();
                            if (sel && sel.toString().length > 0) {
                                document.execCommand('copy');
                            }
                        })();
                    ");
                }

                // Also simulate Ctrl+C as backup (release Alt first)
                ThreadPool.QueueUserWorkItem(delegate
                {
                    keybd_event(VkMenu, 0, KeyeventfKeyup, UIntPtr.Zero);
                    Thread.Sleep(50);
                    keybd_event(VkControl, 0, 0, UIntPtr.Zero);
                    keybd_event(VkC, 0, 0, UIntPtr.Zero);
                    keybd_event(VkC, 0, KeyeventfKeyup, UIntPtr.Zero);
                    keybd_event(VkControl, 0, KeyeventfKeyup, UIntPtr.Zero);
                });
            }
            catch { }
        }

        /// <summary>
        /// Alt+V: Paste text by injecting JavaScript into the browser.
        /// This COMPLETELY bypasses SEB's keyboard interception.
        /// Instead of simulating Ctrl+V (which SEB blocks), we directly
        /// insert text into the focused DOM element.
        /// </summary>
        private void HandleAltPaste(IBrowser browser)
        {
            try
            {
                if (clipboardManager == null || clipboardManager.Count == 0) return;

                var entry = clipboardManager.History[0];
                if (entry == null || string.IsNullOrEmpty(entry.Text)) return;

                PasteTextViaBrowser(browser, entry.Text);
            }
            catch { }
        }

        /// <summary>
        /// Injects text into the active element of the browser via JavaScript.
        /// Works for: input, textarea, contentEditable, Quill, CKEditor, etc.
        /// </summary>
        public static void PasteTextViaBrowser(IBrowser browser, string text)
        {
            if (browser == null || string.IsNullOrEmpty(text)) return;

            try
            {
                var frame = browser.MainFrame;
                if (frame == null) return;

                // Escape the text for JS string literal
                string escaped = text
                    .Replace("\\", "\\\\")
                    .Replace("'", "\\'")
                    .Replace("\r\n", "\\n")
                    .Replace("\r", "\\n")
                    .Replace("\n", "\\n")
                    .Replace("\t", "\\t");

                string js = @"
                    (function() {
                        var text = '" + escaped + @"';
                        var el = document.activeElement;
                        if (!el) return;
                        
                        // For input/textarea elements
                        if (el.tagName === 'INPUT' || el.tagName === 'TEXTAREA') {
                            var start = el.selectionStart || 0;
                            var end = el.selectionEnd || 0;
                            var before = el.value.substring(0, start);
                            var after = el.value.substring(end);
                            el.value = before + text + after;
                            el.selectionStart = el.selectionEnd = start + text.length;
                            // Trigger input events so frameworks detect the change
                            el.dispatchEvent(new Event('input', { bubbles: true }));
                            el.dispatchEvent(new Event('change', { bubbles: true }));
                            return;
                        }
                        
                        // For contentEditable elements (rich text editors, Moodle, etc.)
                        if (el.isContentEditable || document.designMode === 'on') {
                            // Try insertText first (preserves undo)
                            var ok = document.execCommand('insertText', false, text);
                            if (!ok) {
                                // Fallback: insert at selection
                                var sel = window.getSelection();
                                if (sel && sel.rangeCount > 0) {
                                    var range = sel.getRangeAt(0);
                                    range.deleteContents();
                                    var node = document.createTextNode(text);
                                    range.insertNode(node);
                                    range.setStartAfter(node);
                                    range.collapse(true);
                                    sel.removeAllRanges();
                                    sel.addRange(range);
                                }
                            }
                            return;
                        }

                        // Last resort: try execCommand on document
                        document.execCommand('insertText', false, text);
                    })();
                ";

                frame.ExecuteJavaScriptAsync(js);
            }
            catch { }
        }
    }
}
