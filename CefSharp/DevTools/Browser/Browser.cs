// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Browser
{
    /// <summary>
    /// The Browser domain defines methods and events for browser managing.
    /// </summary>
    public partial class Browser
    {
        public Browser(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Set permission settings for given origin.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetPermission(PermissionDescriptor permission, string setting, string origin, string browserContextId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"permission", permission}, {"setting", setting}, {"origin", origin}, {"browserContextId", browserContextId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Browser.SetPermission", dict);
            return result;
        }

        /// <summary>
        /// Grant specific permissions to the given origin and reject all others.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GrantPermissions(string permissions, string origin, string browserContextId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"permissions", permissions}, {"origin", origin}, {"browserContextId", browserContextId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Browser.GrantPermissions", dict);
            return result;
        }

        /// <summary>
        /// Reset all permission management for all origins.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ResetPermissions(string browserContextId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"browserContextId", browserContextId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Browser.ResetPermissions", dict);
            return result;
        }

        /// <summary>
        /// Set the behavior when downloading a file.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetDownloadBehavior(string behavior, string browserContextId, string downloadPath)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"behavior", behavior}, {"browserContextId", browserContextId}, {"downloadPath", downloadPath}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Browser.SetDownloadBehavior", dict);
            return result;
        }

        /// <summary>
        /// Close browser gracefully.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Close()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Browser.Close", dict);
            return result;
        }

        /// <summary>
        /// Crashes browser on the main thread.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Crash()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Browser.Crash", dict);
            return result;
        }

        /// <summary>
        /// Crashes GPU process.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> CrashGpuProcess()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Browser.CrashGpuProcess", dict);
            return result;
        }

        /// <summary>
        /// Returns version information.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetVersion()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Browser.GetVersion", dict);
            return result;
        }

        /// <summary>
        /// Returns the command line switches for the browser process if, and only if
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetBrowserCommandLine()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Browser.GetBrowserCommandLine", dict);
            return result;
        }

        /// <summary>
        /// Get Chrome histograms.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetHistograms(string query, bool delta)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"query", query}, {"delta", delta}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Browser.GetHistograms", dict);
            return result;
        }

        /// <summary>
        /// Get a Chrome histogram by name.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetHistogram(string name, bool delta)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"name", name}, {"delta", delta}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Browser.GetHistogram", dict);
            return result;
        }

        /// <summary>
        /// Get position and size of the browser window.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetWindowBounds(int windowId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"windowId", windowId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Browser.GetWindowBounds", dict);
            return result;
        }

        /// <summary>
        /// Get the browser window that contains the devtools target.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetWindowForTarget(string targetId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"targetId", targetId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Browser.GetWindowForTarget", dict);
            return result;
        }

        /// <summary>
        /// Set position and/or size of the browser window.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetWindowBounds(int windowId, Bounds bounds)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"windowId", windowId}, {"bounds", bounds}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Browser.SetWindowBounds", dict);
            return result;
        }

        /// <summary>
        /// Set dock tile details, platform-specific.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetDockTile(string badgeLabel, string image)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"badgeLabel", badgeLabel}, {"image", image}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Browser.SetDockTile", dict);
            return result;
        }
    }
}