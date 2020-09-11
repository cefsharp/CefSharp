// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Browser
{
    using System.Linq;

    /// <summary>
    /// The Browser domain defines methods and events for browser managing.
    /// </summary>
    public partial class Browser : DevToolsDomainBase
    {
        public Browser(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.IDevToolsClient _client;
        /// <summary>
        /// Set permission settings for given origin.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetPermissionAsync(CefSharp.DevTools.Browser.PermissionDescriptor permission, CefSharp.DevTools.Browser.PermissionSetting setting, string origin = null, string browserContextId = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("permission", permission.ToDictionary());
            dict.Add("setting", this.EnumToString(setting));
            if (!(string.IsNullOrEmpty(origin)))
            {
                dict.Add("origin", origin);
            }

            if (!(string.IsNullOrEmpty(browserContextId)))
            {
                dict.Add("browserContextId", browserContextId);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.setPermission", dict);
            return methodResult;
        }

        /// <summary>
        /// Grant specific permissions to the given origin and reject all others.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> GrantPermissionsAsync(CefSharp.DevTools.Browser.PermissionType[] permissions, string origin = null, string browserContextId = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("permissions", this.EnumToString(permissions));
            if (!(string.IsNullOrEmpty(origin)))
            {
                dict.Add("origin", origin);
            }

            if (!(string.IsNullOrEmpty(browserContextId)))
            {
                dict.Add("browserContextId", browserContextId);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.grantPermissions", dict);
            return methodResult;
        }

        /// <summary>
        /// Reset all permission management for all origins.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ResetPermissionsAsync(string browserContextId = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(browserContextId)))
            {
                dict.Add("browserContextId", browserContextId);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.resetPermissions", dict);
            return methodResult;
        }

        /// <summary>
        /// Set the behavior when downloading a file.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDownloadBehaviorAsync(string behavior, string browserContextId = null, string downloadPath = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("behavior", behavior);
            if (!(string.IsNullOrEmpty(browserContextId)))
            {
                dict.Add("browserContextId", browserContextId);
            }

            if (!(string.IsNullOrEmpty(downloadPath)))
            {
                dict.Add("downloadPath", downloadPath);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.setDownloadBehavior", dict);
            return methodResult;
        }

        /// <summary>
        /// Close browser gracefully.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> CloseAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.close", dict);
            return methodResult;
        }

        /// <summary>
        /// Crashes browser on the main thread.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> CrashAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.crash", dict);
            return methodResult;
        }

        /// <summary>
        /// Crashes GPU process.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> CrashGpuProcessAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.crashGpuProcess", dict);
            return methodResult;
        }

        /// <summary>
        /// Returns version information.
        /// </summary>
        public async System.Threading.Tasks.Task<GetVersionResponse> GetVersionAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.getVersion", dict);
            return methodResult.DeserializeJson<GetVersionResponse>();
        }

        /// <summary>
        /// Returns the command line switches for the browser process if, and only if
        /// --enable-automation is on the commandline.
        /// </summary>
        public async System.Threading.Tasks.Task<GetBrowserCommandLineResponse> GetBrowserCommandLineAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.getBrowserCommandLine", dict);
            return methodResult.DeserializeJson<GetBrowserCommandLineResponse>();
        }

        /// <summary>
        /// Get Chrome histograms.
        /// </summary>
        public async System.Threading.Tasks.Task<GetHistogramsResponse> GetHistogramsAsync(string query = null, bool? delta = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(query)))
            {
                dict.Add("query", query);
            }

            if (delta.HasValue)
            {
                dict.Add("delta", delta.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.getHistograms", dict);
            return methodResult.DeserializeJson<GetHistogramsResponse>();
        }

        /// <summary>
        /// Get a Chrome histogram by name.
        /// </summary>
        public async System.Threading.Tasks.Task<GetHistogramResponse> GetHistogramAsync(string name, bool? delta = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("name", name);
            if (delta.HasValue)
            {
                dict.Add("delta", delta.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.getHistogram", dict);
            return methodResult.DeserializeJson<GetHistogramResponse>();
        }

        /// <summary>
        /// Get position and size of the browser window.
        /// </summary>
        public async System.Threading.Tasks.Task<GetWindowBoundsResponse> GetWindowBoundsAsync(int windowId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("windowId", windowId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.getWindowBounds", dict);
            return methodResult.DeserializeJson<GetWindowBoundsResponse>();
        }

        /// <summary>
        /// Get the browser window that contains the devtools target.
        /// </summary>
        public async System.Threading.Tasks.Task<GetWindowForTargetResponse> GetWindowForTargetAsync(string targetId = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(targetId)))
            {
                dict.Add("targetId", targetId);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.getWindowForTarget", dict);
            return methodResult.DeserializeJson<GetWindowForTargetResponse>();
        }

        /// <summary>
        /// Set position and/or size of the browser window.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetWindowBoundsAsync(int windowId, CefSharp.DevTools.Browser.Bounds bounds)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("windowId", windowId);
            dict.Add("bounds", bounds.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.setWindowBounds", dict);
            return methodResult;
        }

        /// <summary>
        /// Set dock tile details, platform-specific.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDockTileAsync(string badgeLabel = null, byte[] image = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(badgeLabel)))
            {
                dict.Add("badgeLabel", badgeLabel);
            }

            if ((image) != (null))
            {
                dict.Add("image", ToBase64String(image));
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.setDockTile", dict);
            return methodResult;
        }
    }
}