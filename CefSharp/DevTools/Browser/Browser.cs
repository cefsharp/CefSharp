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
        private CefSharp.DevTools.IDevToolsClient _client;
        public Browser(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        partial void ValidateSetPermission(CefSharp.DevTools.Browser.PermissionDescriptor permission, CefSharp.DevTools.Browser.PermissionSetting setting, string origin = null, string browserContextId = null);
        /// <summary>
        /// Set permission settings for given origin.
        /// </summary>
        /// <param name = "permission">Descriptor of permission to override.</param>
        /// <param name = "setting">Setting of the permission.</param>
        /// <param name = "origin">Origin the permission applies to, all origins if not specified.</param>
        /// <param name = "browserContextId">Context to override. When omitted, default browser context is used.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetPermissionAsync(CefSharp.DevTools.Browser.PermissionDescriptor permission, CefSharp.DevTools.Browser.PermissionSetting setting, string origin = null, string browserContextId = null)
        {
            ValidateSetPermission(permission, setting, origin, browserContextId);
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

        partial void ValidateGrantPermissions(CefSharp.DevTools.Browser.PermissionType[] permissions, string origin = null, string browserContextId = null);
        /// <summary>
        /// Grant specific permissions to the given origin and reject all others.
        /// </summary>
        /// <param name = "permissions">permissions</param>
        /// <param name = "origin">Origin the permission applies to, all origins if not specified.</param>
        /// <param name = "browserContextId">BrowserContext to override permissions. When omitted, default browser context is used.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> GrantPermissionsAsync(CefSharp.DevTools.Browser.PermissionType[] permissions, string origin = null, string browserContextId = null)
        {
            ValidateGrantPermissions(permissions, origin, browserContextId);
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

        partial void ValidateResetPermissions(string browserContextId = null);
        /// <summary>
        /// Reset all permission management for all origins.
        /// </summary>
        /// <param name = "browserContextId">BrowserContext to reset permissions. When omitted, default browser context is used.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ResetPermissionsAsync(string browserContextId = null)
        {
            ValidateResetPermissions(browserContextId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(browserContextId)))
            {
                dict.Add("browserContextId", browserContextId);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.resetPermissions", dict);
            return methodResult;
        }

        partial void ValidateSetDownloadBehavior(string behavior, string browserContextId = null, string downloadPath = null);
        /// <summary>
        /// Set the behavior when downloading a file.
        /// </summary>
        /// <param name = "behavior">Whether to allow all or deny all download requests, or use default Chrome behavior if
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDownloadBehaviorAsync(string behavior, string browserContextId = null, string downloadPath = null)
        {
            ValidateSetDownloadBehavior(behavior, browserContextId, downloadPath);
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
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> CloseAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.close", dict);
            return methodResult;
        }

        /// <summary>
        /// Crashes browser on the main thread.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> CrashAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.crash", dict);
            return methodResult;
        }

        /// <summary>
        /// Crashes GPU process.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> CrashGpuProcessAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.crashGpuProcess", dict);
            return methodResult;
        }

        /// <summary>
        /// Returns version information.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetVersionResponse&gt;</returns>
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
        /// <returns>returns System.Threading.Tasks.Task&lt;GetBrowserCommandLineResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetBrowserCommandLineResponse> GetBrowserCommandLineAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.getBrowserCommandLine", dict);
            return methodResult.DeserializeJson<GetBrowserCommandLineResponse>();
        }

        partial void ValidateGetHistograms(string query = null, bool? delta = null);
        /// <summary>
        /// Get Chrome histograms.
        /// </summary>
        /// <param name = "query">Requested substring in name. Only histograms which have query as a
        public async System.Threading.Tasks.Task<GetHistogramsResponse> GetHistogramsAsync(string query = null, bool? delta = null)
        {
            ValidateGetHistograms(query, delta);
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

        partial void ValidateGetHistogram(string name, bool? delta = null);
        /// <summary>
        /// Get a Chrome histogram by name.
        /// </summary>
        /// <param name = "name">Requested histogram name.</param>
        /// <param name = "delta">If true, retrieve delta since last call.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetHistogramResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetHistogramResponse> GetHistogramAsync(string name, bool? delta = null)
        {
            ValidateGetHistogram(name, delta);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("name", name);
            if (delta.HasValue)
            {
                dict.Add("delta", delta.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.getHistogram", dict);
            return methodResult.DeserializeJson<GetHistogramResponse>();
        }

        partial void ValidateGetWindowBounds(int windowId);
        /// <summary>
        /// Get position and size of the browser window.
        /// </summary>
        /// <param name = "windowId">Browser window id.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetWindowBoundsResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetWindowBoundsResponse> GetWindowBoundsAsync(int windowId)
        {
            ValidateGetWindowBounds(windowId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("windowId", windowId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.getWindowBounds", dict);
            return methodResult.DeserializeJson<GetWindowBoundsResponse>();
        }

        partial void ValidateGetWindowForTarget(string targetId = null);
        /// <summary>
        /// Get the browser window that contains the devtools target.
        /// </summary>
        /// <param name = "targetId">Devtools agent host id. If called as a part of the session, associated targetId is used.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetWindowForTargetResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetWindowForTargetResponse> GetWindowForTargetAsync(string targetId = null)
        {
            ValidateGetWindowForTarget(targetId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(targetId)))
            {
                dict.Add("targetId", targetId);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.getWindowForTarget", dict);
            return methodResult.DeserializeJson<GetWindowForTargetResponse>();
        }

        partial void ValidateSetWindowBounds(int windowId, CefSharp.DevTools.Browser.Bounds bounds);
        /// <summary>
        /// Set position and/or size of the browser window.
        /// </summary>
        /// <param name = "windowId">Browser window id.</param>
        /// <param name = "bounds">New window bounds. The 'minimized', 'maximized' and 'fullscreen' states cannot be combined
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetWindowBoundsAsync(int windowId, CefSharp.DevTools.Browser.Bounds bounds)
        {
            ValidateSetWindowBounds(windowId, bounds);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("windowId", windowId);
            dict.Add("bounds", bounds.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Browser.setWindowBounds", dict);
            return methodResult;
        }

        partial void ValidateSetDockTile(string badgeLabel = null, byte[] image = null);
        /// <summary>
        /// Set dock tile details, platform-specific.
        /// </summary>
        /// <param name = "badgeLabel">badgeLabel</param>
        /// <param name = "image">Png encoded image.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDockTileAsync(string badgeLabel = null, byte[] image = null)
        {
            ValidateSetDockTile(badgeLabel, image);
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