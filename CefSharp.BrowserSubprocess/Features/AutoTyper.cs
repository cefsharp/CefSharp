using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace CefSharp.BrowserSubprocess.Features
{
    /// <summary>
    /// AutoTyper v2.0 — types text character-by-character using SendInput with KEYEVENTF_UNICODE.
    ///
    /// v2.0 changes:
    /// - Interlocked for thread-safe position tracking
    /// - Adaptive typing speed (auto-slows on failures)
    /// - Speed preset support (Slow/Normal/Fast/Instant)
    /// - Better resume accuracy after pause
    ///
    /// Why this bypasses SEB:
    /// KEYEVENTF_UNICODE sends characters as WM_CHAR messages with scan code = Unicode value.
    /// No virtual key code is generated, so WH_KEYBOARD_LL hooks (which filter by VK code)
    /// never see these keystrokes. SEB's KeyboardInterceptor can't block what it can't see.
    /// </summary>
    public class AutoTyper : IDisposable
    {
        #region P/Invoke

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public uint type;
            public INPUTUNION u;
        }

        [StructLayout(LayoutKind.Explicit, Size = 32)]
        private struct INPUTUNION
        {
            [FieldOffset(0)] public KEYBDINPUT ki;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        private const uint INPUT_KEYBOARD = 1;
        private const uint KEYEVENTF_UNICODE = 0x0004;
        private const uint KEYEVENTF_KEYUP = 0x0002;
        private const ushort VK_RETURN = 0x0D;
        private const ushort VK_BACK = 0x08;
        private const ushort VK_TAB = 0x09;

        #endregion

        // State
        private string textToType = "";
        private int currentPosition = 0;  // Accessed via Interlocked for thread safety
        private Thread typingThread;
        private volatile bool isTyping = false;
        private volatile bool stopRequested = false;
        private readonly object lockObj = new object();
        private readonly Random rng = new Random();
        private int consecutiveFailures = 0;  // For adaptive speed

        // Speed presets
        public enum SpeedPreset { Slow, Normal, Fast, Instant }
        private SpeedPreset currentPreset = SpeedPreset.Normal;

        // Settings — managed via preset
        public int BaseDelayMs { get; set; } = 70;
        public int JitterMs { get; set; } = 20;
        public int PunctuationPauseMs { get; set; } = 100;
        public int NewlinePauseMs { get; set; } = 180;
        public int WordPauseMs { get; set; } = 25;
        private const int KEYDOWN_UP_GAP_MS = 15;
        private const int MAX_DELAY_MS = 250;
        private const int MIN_DELAY_MS = 5;

        /// <summary>Sets typing speed from a preset. Also resets adaptive failure counter.</summary>
        public void SetSpeedPreset(SpeedPreset preset)
        {
            currentPreset = preset;
            consecutiveFailures = 0;
            switch (preset)
            {
                case SpeedPreset.Slow:
                    BaseDelayMs = 120; JitterMs = 25; PunctuationPauseMs = 150; NewlinePauseMs = 220; WordPauseMs = 40;
                    break;
                case SpeedPreset.Normal:
                    BaseDelayMs = 70; JitterMs = 20; PunctuationPauseMs = 100; NewlinePauseMs = 180; WordPauseMs = 25;
                    break;
                case SpeedPreset.Fast:
                    BaseDelayMs = 40; JitterMs = 10; PunctuationPauseMs = 60; NewlinePauseMs = 100; WordPauseMs = 15;
                    break;
                case SpeedPreset.Instant:
                    BaseDelayMs = 10; JitterMs = 3; PunctuationPauseMs = 20; NewlinePauseMs = 40; WordPauseMs = 8;
                    break;
            }
            Log("Speed preset: " + preset + " (base=" + BaseDelayMs + "ms)");
        }

        public SpeedPreset CurrentPreset { get { return currentPreset; } }

        // Status
        public bool IsTyping { get { return isTyping; } }
        public bool HasText { get { return !string.IsNullOrEmpty(textToType); } }
        public int Position { get { return Thread.VolatileRead(ref currentPosition); } }
        public int TotalLength { get { return textToType.Length; } }
        public float Progress { get { return textToType.Length > 0 ? (float)Thread.VolatileRead(ref currentPosition) / textToType.Length * 100f : 0; } }
        public string StatusText
        {
            get
            {
                int pos = Thread.VolatileRead(ref currentPosition);
                int total = textToType.Length;
                if (!HasText) return "No text loaded";
                if (isTyping) return $"TYPING {pos}/{total} ({(total > 0 ? (float)pos / total * 100f : 0):F0}%)";
                if (pos > 0 && pos < total) return $"PAUSED at {pos}/{total} ({(total > 0 ? (float)pos / total * 100f : 0):F0}%)";
                if (pos >= total) return "DONE ✓";
                return "READY";
            }
        }

        // Events
        public event Action<string> LogEvent;
        public event Action<int, int> ProgressChanged;  // (position, total)
        public event Action TypingStarted;
        public event Action TypingStopped;
        public event Action TypingCompleted;

        /// <summary>
        /// Loads text into the auto-typer buffer. Resets position to 0.
        /// </summary>
        public void LoadText(string text)
        {
            lock (lockObj)
            {
                if (isTyping) Stop();
                textToType = text ?? "";
                currentPosition = 0;
                Log($"Loaded {textToType.Length} chars into auto-typer");
            }
        }

        /// <summary>
        /// Loads text and preserves position if the text starts with the same content.
        /// Useful when re-loading the same text.
        /// </summary>
        public void LoadTextPreservePosition(string text)
        {
            lock (lockObj)
            {
                if (isTyping) Stop();
                if (text == null) text = "";

                // If new text starts with what we've already typed, keep position
                if (text.Length >= currentPosition && textToType.Length >= currentPosition
                    && text.Substring(0, Math.Min(currentPosition, text.Length))
                       == textToType.Substring(0, Math.Min(currentPosition, textToType.Length)))
                {
                    textToType = text;
                    Log($"Reloaded text ({text.Length} chars), position preserved at {currentPosition}");
                }
                else
                {
                    textToType = text;
                    currentPosition = 0;
                    Log($"Loaded new text ({text.Length} chars), position reset");
                }
            }
        }

        /// <summary>
        /// Toggles typing on/off. If typing, stops. If stopped, starts from current position.
        /// Returns true if typing started, false if stopped.
        /// </summary>
        public bool Toggle()
        {
            if (isTyping)
            {
                Stop();
                return false;
            }
            else
            {
                Start();
                return true;
            }
        }

        /// <summary>
        /// Starts typing from the current position.
        /// </summary>
        public void Start()
        {
            lock (lockObj)
            {
                if (isTyping) return;
                if (string.IsNullOrEmpty(textToType))
                {
                    Log("Cannot start — no text loaded");
                    return;
                }
                if (currentPosition >= textToType.Length)
                {
                    Log("Text already fully typed. Reset position first.");
                    return;
                }

                stopRequested = false;
                isTyping = true;

                typingThread = new Thread(TypingLoop)
                {
                    IsBackground = true,
                    Name = "AutoTyper"
                };
                typingThread.Start();

                Log($"Auto-typer STARTED at position {currentPosition}/{textToType.Length}");
                TypingStarted?.Invoke();
            }
        }

        /// <summary>
        /// Stops typing. Position is preserved for resume.
        /// </summary>
        public void Stop()
        {
            stopRequested = true;
            Log($"Auto-typer STOPPED at position {currentPosition}/{textToType.Length}");
        }

        /// <summary>
        /// Resets position to 0 without clearing the text.
        /// </summary>
        public void ResetPosition()
        {
            lock (lockObj)
            {
                if (isTyping) Stop();
                currentPosition = 0;
                Log("Position reset to 0");
            }
        }

        /// <summary>
        /// Clears all text and resets position.
        /// </summary>
        public void Clear()
        {
            lock (lockObj)
            {
                if (isTyping) Stop();
                textToType = "";
                currentPosition = 0;
                Log("Auto-typer cleared");
            }
        }

        /// <summary>
        /// Gets a preview of what's been typed and what's remaining.
        /// </summary>
        public string GetContextPreview(int contextChars = 30)
        {
            if (string.IsNullOrEmpty(textToType)) return "[empty]";

            var start = Math.Max(0, currentPosition - contextChars);
            var end = Math.Min(textToType.Length, currentPosition + contextChars);

            var before = textToType.Substring(start, currentPosition - start);
            var after = textToType.Substring(currentPosition, end - currentPosition);

            return $"...{before}|CURSOR|{after}...";
        }

        private void TypingLoop()
        {
            try
            {
                int pos = Thread.VolatileRead(ref currentPosition);
                Log($"TypingLoop started — {textToType.Length - pos} chars remaining");

                while (pos < textToType.Length && !stopRequested)
                {
                    char c = textToType[pos];

                    // Type the character
                    bool success = TypeCharacter(c);

                    if (success)
                    {
                        Interlocked.Increment(ref currentPosition);
                        consecutiveFailures = 0;
                    }
                    else
                    {
                        // Retry once after a longer pause
                        Thread.Sleep(80);
                        TypeCharacter(c);
                        Interlocked.Increment(ref currentPosition);
                        consecutiveFailures++;

                        // Adaptive slow-down: if we keep failing, increase delay
                        if (consecutiveFailures > 3 && BaseDelayMs < MAX_DELAY_MS)
                        {
                            int newDelay = Math.Min(BaseDelayMs + 20, MAX_DELAY_MS);
                            Log($"Adaptive slow-down: {BaseDelayMs}ms → {newDelay}ms (failures: {consecutiveFailures})");
                            BaseDelayMs = newDelay;
                        }
                    }

                    pos = Thread.VolatileRead(ref currentPosition);

                    // Notify progress (throttled — every 5 chars)
                    if (pos % 5 == 0)
                        ProgressChanged?.Invoke(pos, textToType.Length);

                    // Calculate delay — human-like variability
                    int delay = BaseDelayMs + rng.Next(-JitterMs, JitterMs + 1);

                    // Extra pauses to look human
                    if (c == '.' || c == ',' || c == ';' || c == ':' || c == '!' || c == '?')
                        delay += PunctuationPauseMs + rng.Next(0, 40);
                    else if (c == '\n')
                        delay += NewlinePauseMs + rng.Next(0, 60);
                    else if (c == ' ')
                        delay += WordPauseMs + rng.Next(0, 15);

                    // Clamp
                    if (delay < MIN_DELAY_MS) delay = MIN_DELAY_MS;

                    Thread.Sleep(delay);
                }

                pos = Thread.VolatileRead(ref currentPosition);
                if (pos >= textToType.Length)
                {
                    ProgressChanged?.Invoke(pos, textToType.Length);
                    Log("Auto-typer COMPLETED — all text typed");
                    TypingCompleted?.Invoke();
                }
            }
            catch (Exception ex)
            {
                Log("Auto-typer error: " + ex.Message);
            }
            finally
            {
                isTyping = false;
                TypingStopped?.Invoke();
            }
        }

        /// <summary>
        /// Types a single character via SEPARATE SendInput calls for keydown and keyup.
        /// 
        /// WHY SEPARATE CALLS:
        /// Chromium's input pipeline processes events from the OS message queue.
        /// When keydown+keyup arrive in the same SendInput batch, Chromium sometimes
        /// processes them as a single event and drops the character. By separating
        /// them with a 15ms gap, each event gets its own processing cycle.
        /// 
        /// ALL characters go through KEYEVENTF_UNICODE except newlines, which use
        /// VK_RETURN so browsers insert proper line breaks in textareas.
        /// </summary>
        private bool TypeCharacter(char c)
        {
            // Skip \r — we handle \n for line breaks
            if (c == '\r') return true;

            // Newline → send VK_RETURN (Enter key) so the browser inserts a proper line break.
            // KEYEVENTF_UNICODE with \n (U+000A) just sends a character — browsers need an actual
            // Enter keystroke to insert \n into a <textarea>. Without this, multi-line text
            // gets typed as a single continuous line.
            if (c == '\n')
            {
                SendVirtualKey(VK_RETURN);
                return true;
            }

            // Tab → type 4 spaces instead (VK_TAB changes focus!)
            if (c == '\t')
            {
                for (int i = 0; i < 4; i++)
                {
                    TypeCharacter(' ');
                    Thread.Sleep(5);
                }
                return true;
            }

            // --- KEYDOWN ---
            var downInput = new INPUT[1];
            downInput[0].type = INPUT_KEYBOARD;
            downInput[0].u.ki.wVk = 0;
            downInput[0].u.ki.wScan = c;
            downInput[0].u.ki.dwFlags = KEYEVENTF_UNICODE;

            uint sentDown = SendInput(1, downInput, Marshal.SizeOf(typeof(INPUT)));

            // Gap — let Chromium process the keydown
            Thread.Sleep(KEYDOWN_UP_GAP_MS);

            // --- KEYUP ---
            var upInput = new INPUT[1];
            upInput[0].type = INPUT_KEYBOARD;
            upInput[0].u.ki.wVk = 0;
            upInput[0].u.ki.wScan = c;
            upInput[0].u.ki.dwFlags = KEYEVENTF_UNICODE | KEYEVENTF_KEYUP;

            uint sentUp = SendInput(1, upInput, Marshal.SizeOf(typeof(INPUT)));

            if (sentDown != 1 || sentUp != 1)
            {
                Log($"SendInput issue: char='{c}' (0x{((int)c):X4}) down={sentDown} up={sentUp} err={Marshal.GetLastWin32Error()}");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Sends a virtual key as separate down/up with gap.
        /// </summary>
        private void SendVirtualKey(ushort vk)
        {
            var down = new INPUT[1];
            down[0].type = INPUT_KEYBOARD;
            down[0].u.ki.wVk = vk;
            SendInput(1, down, Marshal.SizeOf(typeof(INPUT)));

            Thread.Sleep(KEYDOWN_UP_GAP_MS);

            var up = new INPUT[1];
            up[0].type = INPUT_KEYBOARD;
            up[0].u.ki.wVk = vk;
            up[0].u.ki.dwFlags = KEYEVENTF_KEYUP;
            SendInput(1, up, Marshal.SizeOf(typeof(INPUT)));
        }

        /// <summary>
        /// Sends a backspace keystroke.
        /// </summary>
        public void SendBackspace()
        {
            SendVirtualKey(VK_BACK);
        }

        private void Log(string msg)
        {
            LogEvent?.Invoke(msg);
            try
            {
                File.AppendAllText(
                    Path.Combine(Path.GetTempPath(), "seb_v8_debug.log"),
                    "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "] [AutoTyper] " + msg + Environment.NewLine);
            }
            catch { }
        }

        public void Dispose()
        {
            stopRequested = true;
            try { typingThread?.Join(1000); } catch { }
        }
    }
}
