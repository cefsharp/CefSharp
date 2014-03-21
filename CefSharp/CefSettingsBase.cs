using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefSharp
{
    public abstract class CefSettingsBase
    {
        protected CefSettingsBase()
        {
            _cefCustomSchemes = new List<CefCustomScheme>();
            BrowserSubprocessPath = "CefSharp.BrowserSubprocess.exe";
        }

        public abstract bool MultiThreadedMessageLoop { get; }

        public abstract string BrowserSubprocessPath { get; set; }

        public abstract string CachePath { get; set; }

        public abstract string Locale { get; set; }

        public abstract string LocalesDirPath { get; set; }

        public abstract string LogFile { get; set; }

        public abstract LogSeverity LogSeverity { get; set; }

        public abstract bool PackLoadingDisabled { get; set; }

        public abstract string ProductVersion { get; set; }

        public abstract int RemoteDebuggingPort { get; set; }

        public abstract string UserAgent { get; set; }

        public IEnumerable<CefCustomScheme> CefCustomSchemes { get { return _cefCustomSchemes; } }

        private List<CefCustomScheme> _cefCustomSchemes;

        /// <summary>
        /// Registers a custom scheme using the provided settings.
        /// </summary>
        /// <param name="cefCustomScheme">The CefCustomScheme which provides the details about the scheme.</param>
        public void RegisterScheme(CefCustomScheme cefCustomScheme)
        {
            _cefCustomSchemes.Add(cefCustomScheme);
        }
    }
}
