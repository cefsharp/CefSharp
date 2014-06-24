// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;

namespace CefSharp
{
    public abstract class CefSettingsBase
    {
        protected CefSettingsBase()
        {
            cefCustomSchemes = new List<CefCustomScheme>();
            cefCommandLineArgs = new Dictionary<string, string>();
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

        public IDictionary<string, string> CefCommandLineArgs { get { return cefCommandLineArgs; } }

        private readonly List<CefCustomScheme> cefCustomSchemes;

        private readonly IDictionary<string, string> cefCommandLineArgs;

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
