// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    /// <summary>
    /// Actions and events related to the inspected page belong to the page domain.
    /// </summary>
    public partial class Page
    {
        public Page(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Deprecated, please use addScriptToEvaluateOnNewDocument instead.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> AddScriptToEvaluateOnLoad(string scriptSource)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"scriptSource", scriptSource}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.AddScriptToEvaluateOnLoad", dict);
            return result;
        }

        /// <summary>
        /// Evaluates given script in every frame upon creation (before loading frame's scripts).
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> AddScriptToEvaluateOnNewDocument(string source, string worldName)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"source", source}, {"worldName", worldName}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.AddScriptToEvaluateOnNewDocument", dict);
            return result;
        }

        /// <summary>
        /// Brings page to front (activates tab).
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> BringToFront()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.BringToFront", dict);
            return result;
        }

        /// <summary>
        /// Capture page screenshot.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> CaptureScreenshot(string format, int quality, Viewport clip, bool fromSurface)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"format", format}, {"quality", quality}, {"clip", clip}, {"fromSurface", fromSurface}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.CaptureScreenshot", dict);
            return result;
        }

        /// <summary>
        /// Returns a snapshot of the page as a string. For MHTML format, the serialization includes
        public async System.Threading.Tasks.Task<DevToolsMethodResult> CaptureSnapshot(string format)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"format", format}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.CaptureSnapshot", dict);
            return result;
        }

        /// <summary>
        /// Clears the overriden device metrics.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ClearDeviceMetricsOverride()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.ClearDeviceMetricsOverride", dict);
            return result;
        }

        /// <summary>
        /// Clears the overridden Device Orientation.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ClearDeviceOrientationOverride()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.ClearDeviceOrientationOverride", dict);
            return result;
        }

        /// <summary>
        /// Clears the overriden Geolocation Position and Error.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ClearGeolocationOverride()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.ClearGeolocationOverride", dict);
            return result;
        }

        /// <summary>
        /// Creates an isolated world for the given frame.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> CreateIsolatedWorld(string frameId, string worldName, bool grantUniveralAccess)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"frameId", frameId}, {"worldName", worldName}, {"grantUniveralAccess", grantUniveralAccess}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.CreateIsolatedWorld", dict);
            return result;
        }

        /// <summary>
        /// Deletes browser cookie with given name, domain and path.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DeleteCookie(string cookieName, string url)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"cookieName", cookieName}, {"url", url}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.DeleteCookie", dict);
            return result;
        }

        /// <summary>
        /// Disables page domain notifications.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Disable()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.Disable", dict);
            return result;
        }

        /// <summary>
        /// Enables page domain notifications.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Enable()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.Enable", dict);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetAppManifest()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.GetAppManifest", dict);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetInstallabilityErrors()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.GetInstallabilityErrors", dict);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetManifestIcons()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.GetManifestIcons", dict);
            return result;
        }

        /// <summary>
        /// Returns all browser cookies. Depending on the backend support, will return detailed cookie
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetCookies()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.GetCookies", dict);
            return result;
        }

        /// <summary>
        /// Returns present frame tree structure.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetFrameTree()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.GetFrameTree", dict);
            return result;
        }

        /// <summary>
        /// Returns metrics relating to the layouting of the page, such as viewport bounds/scale.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetLayoutMetrics()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.GetLayoutMetrics", dict);
            return result;
        }

        /// <summary>
        /// Returns navigation history for the current page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetNavigationHistory()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.GetNavigationHistory", dict);
            return result;
        }

        /// <summary>
        /// Resets navigation history for the current page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ResetNavigationHistory()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.ResetNavigationHistory", dict);
            return result;
        }

        /// <summary>
        /// Returns content of the given resource.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetResourceContent(string frameId, string url)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"frameId", frameId}, {"url", url}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.GetResourceContent", dict);
            return result;
        }

        /// <summary>
        /// Returns present frame / resource tree structure.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GetResourceTree()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.GetResourceTree", dict);
            return result;
        }

        /// <summary>
        /// Accepts or dismisses a JavaScript initiated dialog (alert, confirm, prompt, or onbeforeunload).
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> HandleJavaScriptDialog(bool accept, string promptText)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"accept", accept}, {"promptText", promptText}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.HandleJavaScriptDialog", dict);
            return result;
        }

        /// <summary>
        /// Navigates current page to the given URL.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Navigate(string url, string referrer, string transitionType, string frameId, string referrerPolicy)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"url", url}, {"referrer", referrer}, {"transitionType", transitionType}, {"frameId", frameId}, {"referrerPolicy", referrerPolicy}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.Navigate", dict);
            return result;
        }

        /// <summary>
        /// Navigates current page to the given history entry.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> NavigateToHistoryEntry(int entryId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"entryId", entryId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.NavigateToHistoryEntry", dict);
            return result;
        }

        /// <summary>
        /// Print page as PDF.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> PrintToPDF(bool landscape, bool displayHeaderFooter, bool printBackground, long scale, long paperWidth, long paperHeight, long marginTop, long marginBottom, long marginLeft, long marginRight, string pageRanges, bool ignoreInvalidPageRanges, string headerTemplate, string footerTemplate, bool preferCSSPageSize, string transferMode)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"landscape", landscape}, {"displayHeaderFooter", displayHeaderFooter}, {"printBackground", printBackground}, {"scale", scale}, {"paperWidth", paperWidth}, {"paperHeight", paperHeight}, {"marginTop", marginTop}, {"marginBottom", marginBottom}, {"marginLeft", marginLeft}, {"marginRight", marginRight}, {"pageRanges", pageRanges}, {"ignoreInvalidPageRanges", ignoreInvalidPageRanges}, {"headerTemplate", headerTemplate}, {"footerTemplate", footerTemplate}, {"preferCSSPageSize", preferCSSPageSize}, {"transferMode", transferMode}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.PrintToPDF", dict);
            return result;
        }

        /// <summary>
        /// Reloads given page optionally ignoring the cache.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Reload(bool ignoreCache, string scriptToEvaluateOnLoad)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"ignoreCache", ignoreCache}, {"scriptToEvaluateOnLoad", scriptToEvaluateOnLoad}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.Reload", dict);
            return result;
        }

        /// <summary>
        /// Deprecated, please use removeScriptToEvaluateOnNewDocument instead.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RemoveScriptToEvaluateOnLoad(string identifier)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"identifier", identifier}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.RemoveScriptToEvaluateOnLoad", dict);
            return result;
        }

        /// <summary>
        /// Removes given script from the list.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RemoveScriptToEvaluateOnNewDocument(string identifier)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"identifier", identifier}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.RemoveScriptToEvaluateOnNewDocument", dict);
            return result;
        }

        /// <summary>
        /// Acknowledges that a screencast frame has been received by the frontend.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ScreencastFrameAck(int sessionId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"sessionId", sessionId}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.ScreencastFrameAck", dict);
            return result;
        }

        /// <summary>
        /// Searches for given string in resource content.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SearchInResource(string frameId, string url, string query, bool caseSensitive, bool isRegex)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"frameId", frameId}, {"url", url}, {"query", query}, {"caseSensitive", caseSensitive}, {"isRegex", isRegex}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.SearchInResource", dict);
            return result;
        }

        /// <summary>
        /// Enable Chrome's experimental ad filter on all sites.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetAdBlockingEnabled(bool enabled)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"enabled", enabled}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.SetAdBlockingEnabled", dict);
            return result;
        }

        /// <summary>
        /// Enable page Content Security Policy by-passing.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetBypassCSP(bool enabled)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"enabled", enabled}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.SetBypassCSP", dict);
            return result;
        }

        /// <summary>
        /// Overrides the values of device screen dimensions (window.screen.width, window.screen.height,
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetDeviceMetricsOverride(int width, int height, long deviceScaleFactor, bool mobile, long scale, int screenWidth, int screenHeight, int positionX, int positionY, bool dontSetVisibleSize, Emulation.ScreenOrientation screenOrientation, Viewport viewport)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"width", width}, {"height", height}, {"deviceScaleFactor", deviceScaleFactor}, {"mobile", mobile}, {"scale", scale}, {"screenWidth", screenWidth}, {"screenHeight", screenHeight}, {"positionX", positionX}, {"positionY", positionY}, {"dontSetVisibleSize", dontSetVisibleSize}, {"screenOrientation", screenOrientation}, {"viewport", viewport}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.SetDeviceMetricsOverride", dict);
            return result;
        }

        /// <summary>
        /// Overrides the Device Orientation.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetDeviceOrientationOverride(long alpha, long beta, long gamma)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"alpha", alpha}, {"beta", beta}, {"gamma", gamma}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.SetDeviceOrientationOverride", dict);
            return result;
        }

        /// <summary>
        /// Set generic font families.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetFontFamilies(FontFamilies fontFamilies)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"fontFamilies", fontFamilies}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.SetFontFamilies", dict);
            return result;
        }

        /// <summary>
        /// Set default font sizes.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetFontSizes(FontSizes fontSizes)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"fontSizes", fontSizes}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.SetFontSizes", dict);
            return result;
        }

        /// <summary>
        /// Sets given markup as the document's HTML.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetDocumentContent(string frameId, string html)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"frameId", frameId}, {"html", html}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.SetDocumentContent", dict);
            return result;
        }

        /// <summary>
        /// Set the behavior when downloading a file.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetDownloadBehavior(string behavior, string downloadPath)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"behavior", behavior}, {"downloadPath", downloadPath}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.SetDownloadBehavior", dict);
            return result;
        }

        /// <summary>
        /// Overrides the Geolocation Position or Error. Omitting any of the parameters emulates position
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetGeolocationOverride(long latitude, long longitude, long accuracy)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"latitude", latitude}, {"longitude", longitude}, {"accuracy", accuracy}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.SetGeolocationOverride", dict);
            return result;
        }

        /// <summary>
        /// Controls whether page will emit lifecycle events.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetLifecycleEventsEnabled(bool enabled)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"enabled", enabled}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.SetLifecycleEventsEnabled", dict);
            return result;
        }

        /// <summary>
        /// Toggles mouse event-based touch event emulation.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetTouchEmulationEnabled(bool enabled, string configuration)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"enabled", enabled}, {"configuration", configuration}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.SetTouchEmulationEnabled", dict);
            return result;
        }

        /// <summary>
        /// Starts sending each frame using the `screencastFrame` event.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> StartScreencast(string format, int quality, int maxWidth, int maxHeight, int everyNthFrame)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"format", format}, {"quality", quality}, {"maxWidth", maxWidth}, {"maxHeight", maxHeight}, {"everyNthFrame", everyNthFrame}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.StartScreencast", dict);
            return result;
        }

        /// <summary>
        /// Force the page stop all navigations and pending resource fetches.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> StopLoading()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.StopLoading", dict);
            return result;
        }

        /// <summary>
        /// Crashes renderer on the IO thread, generates minidumps.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Crash()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.Crash", dict);
            return result;
        }

        /// <summary>
        /// Tries to close page, running its beforeunload hooks, if any.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> Close()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.Close", dict);
            return result;
        }

        /// <summary>
        /// Tries to update the web lifecycle state of the page.
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetWebLifecycleState(string state)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"state", state}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.SetWebLifecycleState", dict);
            return result;
        }

        /// <summary>
        /// Stops sending each frame in the `screencastFrame`.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> StopScreencast()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.StopScreencast", dict);
            return result;
        }

        /// <summary>
        /// Forces compilation cache to be generated for every subresource script.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetProduceCompilationCache(bool enabled)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"enabled", enabled}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.SetProduceCompilationCache", dict);
            return result;
        }

        /// <summary>
        /// Seeds compilation cache for given url. Compilation cache does not survive
        public async System.Threading.Tasks.Task<DevToolsMethodResult> AddCompilationCache(string url, string data)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"url", url}, {"data", data}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.AddCompilationCache", dict);
            return result;
        }

        /// <summary>
        /// Clears seeded compilation cache.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ClearCompilationCache()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.ClearCompilationCache", dict);
            return result;
        }

        /// <summary>
        /// Generates a report for testing.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> GenerateTestReport(string message, string group)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"message", message}, {"group", group}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.GenerateTestReport", dict);
            return result;
        }

        /// <summary>
        /// Pauses page execution. Can be resumed using generic Runtime.runIfWaitingForDebugger.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> WaitForDebugger()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.WaitForDebugger", dict);
            return result;
        }

        /// <summary>
        /// Intercept file chooser requests and transfer control to protocol clients.
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetInterceptFileChooserDialog(bool enabled)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>{{"enabled", enabled}, };
            var result = await _client.ExecuteDevToolsMethodAsync("Page.SetInterceptFileChooserDialog", dict);
            return result;
        }
    }
}