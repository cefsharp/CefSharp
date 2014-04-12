using System.Collections.Generic;

namespace CefSharp
{
    // TODO: I'd like us to get rid of this. It adds absolutely no value, it's just an extra class which we might as well do away
    // with to reduce the level of complexity.
    public abstract class CefSettingsBase
    {
        protected CefSettingsBase()
        {
            cefCustomSchemes = new List<CefCustomScheme>();
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
        public IEnumerable<CefCustomScheme> CefCustomSchemes { get { return cefCustomSchemes; } }
        private readonly List<CefCustomScheme> cefCustomSchemes;

        /// <summary>
        /// Registers a custom scheme using the provided settings.
        /// </summary>
        /// <param name="cefCustomScheme">The CefCustomScheme which provides the details about the scheme.</param>
        public void RegisterScheme(CefCustomScheme cefCustomScheme)
        {
            cefCustomSchemes.Add(cefCustomScheme);
        }
    }
}
