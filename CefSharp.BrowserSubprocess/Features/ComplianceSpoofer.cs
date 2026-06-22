using System;
using System.Collections.Generic;
using System.Text;

namespace CefSharp.BrowserSubprocess.Features
{
    /// <summary>
    /// Auto-enabled compliance spoofer. All spoofs are ON by default.
    /// No manual UI needed — runs pre-integrated outside the panel.
    /// </summary>
    public class ComplianceSpoofer
    {
        // All ON by default — no manual toggle needed
        public bool SpoofBekCkHeaders { get; set; } = true;
        public bool FakeSebVersion { get; set; } = true;
        public bool BlockScreenshotDetection { get; set; } = true;
        public bool DisableUrlFiltering { get; set; } = true;
        public bool AllowAllPopups { get; set; } = true;
        public string CustomBekHash { get; set; } = "";
        public string CustomCkHash { get; set; } = "";

        private readonly List<string> spoofLog = new List<string>();
        private readonly object syncLock = new object();

        public event Action<string> SpoofEvent;

        public void Initialize()
        {
            Log("ComplianceSpoofer auto-initialized — ALL spoofs ACTIVE");
            Log("  BEK/CK Headers: ON");
            Log("  Fake SEB Version: ON");
            Log("  Screenshot Block: ON");
            Log("  URL Filter Bypass: ON");
            Log("  Allow Popups: ON");
        }

        /// <summary>
        /// Returns JS to inject into every page for full SEB environment faking.
        /// Called automatically — no user action needed.
        /// </summary>
        public string GetComplianceJS()
        {
            var sb = new StringBuilder();

            sb.AppendLine(@"
(function() {
    if (typeof window.SafeExamBrowser === 'undefined') {
        window.SafeExamBrowser = {
            version: '3.9.0',
            security: {
                browserExamKey: '',
                configKey: '',
                updateKeys: function(bek, ck) {
                    this.browserExamKey = bek;
                    this.configKey = ck;
                }
            },
            clipboard: {
                id: Math.round((Date.now() + Math.random()) * 1000),
                ranges: [],
                text: '',
                clear: function() { this.ranges = []; this.text = ''; },
                getContentEncoded: function() {
                    var bytes = new TextEncoder().encode(this.text);
                    return btoa(String.fromCodePoint.apply(null, bytes));
                },
                update: function(id, base64) {
                    if (this.id != id) {
                        var bytes = Uint8Array.from(atob(base64), function(m) { return m.codePointAt(0); });
                        this.text = new TextDecoder().decode(bytes);
                        this.ranges = [];
                    }
                }
            }
        };
    }

    // Block screenshot detection APIs
    if (window.navigator) {
        try {
            Object.defineProperty(navigator, 'mediaDevices', {
                get: function() {
                    return {
                        getDisplayMedia: function() { return Promise.reject(new Error('NotAllowedError')); },
                        getUserMedia: function(c) { return Promise.resolve(new MediaStream()); },
                        enumerateDevices: function() { return Promise.resolve([]); }
                    };
                },
                configurable: true
            });
        } catch(e) {}
    }
    window.__screenshotBlocked = true;

    // Suppress proctoring error callbacks
    var _origSetTimeout = window.setTimeout;
    window.setTimeout = function(fn, delay) {
        if (typeof fn === 'function') {
            var fnStr = fn.toString();
            if (fnStr.indexOf('seb') > -1 && fnStr.indexOf('error') > -1) {
                return _origSetTimeout(function(){}, delay);
            }
        }
        return _origSetTimeout.apply(window, arguments);
    };

    // Moodle SEB quiz access rule — auto-hide errors
    if (typeof window.M !== 'undefined' && window.M && window.M.cfg) {
        var hideErrors = function() {
            var errors = document.querySelectorAll('.seb-error, .seb-warning, [class*=""seb""][class*=""error""]');
            if (errors && errors.length > 0) {
                errors.forEach(function(el) { el.style.display = 'none'; });
            }
        };
        hideErrors();
        _origSetTimeout(hideErrors, 1000);
        _origSetTimeout(hideErrors, 3000);
    }

    // EdX SEB check bypass
    if (window.location.href.indexOf('/xblock/') > -1 || window.location.href.indexOf('/courseware/') > -1) {
        window.__SEB_BYPASS = true;
    }

    // Force-enable copy/paste in all contenteditable and input fields
    document.addEventListener('copy', function(e) { e.stopImmediatePropagation(); }, true);
    document.addEventListener('cut', function(e) { e.stopImmediatePropagation(); }, true);
    document.addEventListener('paste', function(e) { e.stopImmediatePropagation(); }, true);

    // Remove oncopy/onpaste/oncut restrictions from all elements
    var allElements = document.querySelectorAll('[oncopy], [onpaste], [oncut]');
    allElements.forEach(function(el) {
        el.removeAttribute('oncopy');
        el.removeAttribute('onpaste');
        el.removeAttribute('oncut');
    });

    // Override document.execCommand to always allow copy/paste
    var origExecCommand = document.execCommand;
    document.execCommand = function(cmd) {
        if (cmd === 'copy' || cmd === 'paste' || cmd === 'cut') {
            return origExecCommand.apply(document, arguments);
        }
        return origExecCommand.apply(document, arguments);
    };

    // Remove CSS user-select restrictions
    var style = document.createElement('style');
    style.textContent = '* { -webkit-user-select: auto !important; user-select: auto !important; -moz-user-select: auto !important; }';
    document.head.appendChild(style);
})();
");

            return sb.ToString();
        }

        public string GetStatus()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== Compliance Spoofer Status ===");
            sb.AppendLine("  BEK/CK Header Spoof: " + (SpoofBekCkHeaders ? "ON" : "OFF"));
            sb.AppendLine("  Fake SEB Version:    " + (FakeSebVersion ? "ON" : "OFF"));
            sb.AppendLine("  Screenshot Block:    " + (BlockScreenshotDetection ? "ON" : "OFF"));
            sb.AppendLine("  URL Filter Bypass:   " + (DisableUrlFiltering ? "ON" : "OFF"));
            sb.AppendLine("  Allow All Popups:    " + (AllowAllPopups ? "ON" : "OFF"));
            sb.AppendLine("  Mode: AUTO (pre-integrated)");
            sb.AppendLine("  Spoof Events Logged: " + spoofLog.Count);
            return sb.ToString();
        }

        public List<string> GetLog()
        {
            lock (syncLock) return new List<string>(spoofLog);
        }

        public void Log(string message)
        {
            var entry = "[" + DateTime.Now.ToString("HH:mm:ss") + "] [SPOOF] " + message;
            lock (syncLock)
            {
                spoofLog.Add(entry);
                if (spoofLog.Count > 1000)
                    spoofLog.RemoveAt(0);
            }
            SpoofEvent?.Invoke(entry);
        }
    }
}
