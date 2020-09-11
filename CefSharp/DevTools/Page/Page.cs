// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Page
{
    using System.Linq;

    /// <summary>
    /// Actions and events related to the inspected page belong to the page domain.
    /// </summary>
    public partial class Page : DevToolsDomainBase
    {
        public Page(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Evaluates given script in every frame upon creation (before loading frame's scripts).
        /// </summary>
        public async System.Threading.Tasks.Task<AddScriptToEvaluateOnNewDocumentResponse> AddScriptToEvaluateOnNewDocumentAsync(string source, string worldName = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("source", source);
            if (!(string.IsNullOrEmpty(worldName)))
            {
                dict.Add("worldName", worldName);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.addScriptToEvaluateOnNewDocument", dict);
            return methodResult.DeserializeJson<AddScriptToEvaluateOnNewDocumentResponse>();
        }

        /// <summary>
        /// Brings page to front (activates tab).
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> BringToFrontAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.bringToFront", dict);
            return methodResult;
        }

        /// <summary>
        /// Capture page screenshot.
        /// </summary>
        public async System.Threading.Tasks.Task<CaptureScreenshotResponse> CaptureScreenshotAsync(string format = null, int? quality = null, CefSharp.DevTools.Page.Viewport clip = null, bool? fromSurface = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(format)))
            {
                dict.Add("format", format);
            }

            if (quality.HasValue)
            {
                dict.Add("quality", quality.Value);
            }

            if ((clip) != (null))
            {
                dict.Add("clip", clip.ToDictionary());
            }

            if (fromSurface.HasValue)
            {
                dict.Add("fromSurface", fromSurface.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.captureScreenshot", dict);
            return methodResult.DeserializeJson<CaptureScreenshotResponse>();
        }

        /// <summary>
        /// Returns a snapshot of the page as a string. For MHTML format, the serialization includes
        public async System.Threading.Tasks.Task<CaptureSnapshotResponse> CaptureSnapshotAsync(string format = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(format)))
            {
                dict.Add("format", format);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.captureSnapshot", dict);
            return methodResult.DeserializeJson<CaptureSnapshotResponse>();
        }

        /// <summary>
        /// Creates an isolated world for the given frame.
        /// </summary>
        public async System.Threading.Tasks.Task<CreateIsolatedWorldResponse> CreateIsolatedWorldAsync(string frameId, string worldName = null, bool? grantUniveralAccess = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("frameId", frameId);
            if (!(string.IsNullOrEmpty(worldName)))
            {
                dict.Add("worldName", worldName);
            }

            if (grantUniveralAccess.HasValue)
            {
                dict.Add("grantUniveralAccess", grantUniveralAccess.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.createIsolatedWorld", dict);
            return methodResult.DeserializeJson<CreateIsolatedWorldResponse>();
        }

        /// <summary>
        /// Disables page domain notifications.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables page domain notifications.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<GetAppManifestResponse> GetAppManifestAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.getAppManifest", dict);
            return methodResult.DeserializeJson<GetAppManifestResponse>();
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<GetInstallabilityErrorsResponse> GetInstallabilityErrorsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.getInstallabilityErrors", dict);
            return methodResult.DeserializeJson<GetInstallabilityErrorsResponse>();
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<GetManifestIconsResponse> GetManifestIconsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.getManifestIcons", dict);
            return methodResult.DeserializeJson<GetManifestIconsResponse>();
        }

        /// <summary>
        /// Returns present frame tree structure.
        /// </summary>
        public async System.Threading.Tasks.Task<GetFrameTreeResponse> GetFrameTreeAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.getFrameTree", dict);
            return methodResult.DeserializeJson<GetFrameTreeResponse>();
        }

        /// <summary>
        /// Returns metrics relating to the layouting of the page, such as viewport bounds/scale.
        /// </summary>
        public async System.Threading.Tasks.Task<GetLayoutMetricsResponse> GetLayoutMetricsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.getLayoutMetrics", dict);
            return methodResult.DeserializeJson<GetLayoutMetricsResponse>();
        }

        /// <summary>
        /// Returns navigation history for the current page.
        /// </summary>
        public async System.Threading.Tasks.Task<GetNavigationHistoryResponse> GetNavigationHistoryAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.getNavigationHistory", dict);
            return methodResult.DeserializeJson<GetNavigationHistoryResponse>();
        }

        /// <summary>
        /// Resets navigation history for the current page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ResetNavigationHistoryAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.resetNavigationHistory", dict);
            return methodResult;
        }

        /// <summary>
        /// Returns content of the given resource.
        /// </summary>
        public async System.Threading.Tasks.Task<GetResourceContentResponse> GetResourceContentAsync(string frameId, string url)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("frameId", frameId);
            dict.Add("url", url);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.getResourceContent", dict);
            return methodResult.DeserializeJson<GetResourceContentResponse>();
        }

        /// <summary>
        /// Returns present frame / resource tree structure.
        /// </summary>
        public async System.Threading.Tasks.Task<GetResourceTreeResponse> GetResourceTreeAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.getResourceTree", dict);
            return methodResult.DeserializeJson<GetResourceTreeResponse>();
        }

        /// <summary>
        /// Accepts or dismisses a JavaScript initiated dialog (alert, confirm, prompt, or onbeforeunload).
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> HandleJavaScriptDialogAsync(bool accept, string promptText = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("accept", accept);
            if (!(string.IsNullOrEmpty(promptText)))
            {
                dict.Add("promptText", promptText);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.handleJavaScriptDialog", dict);
            return methodResult;
        }

        /// <summary>
        /// Navigates current page to the given URL.
        /// </summary>
        public async System.Threading.Tasks.Task<NavigateResponse> NavigateAsync(string url, string referrer = null, CefSharp.DevTools.Page.TransitionType? transitionType = null, string frameId = null, CefSharp.DevTools.Page.ReferrerPolicy? referrerPolicy = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("url", url);
            if (!(string.IsNullOrEmpty(referrer)))
            {
                dict.Add("referrer", referrer);
            }

            if (transitionType.HasValue)
            {
                dict.Add("transitionType", this.EnumToString(transitionType));
            }

            if (!(string.IsNullOrEmpty(frameId)))
            {
                dict.Add("frameId", frameId);
            }

            if (referrerPolicy.HasValue)
            {
                dict.Add("referrerPolicy", this.EnumToString(referrerPolicy));
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.navigate", dict);
            return methodResult.DeserializeJson<NavigateResponse>();
        }

        /// <summary>
        /// Navigates current page to the given history entry.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> NavigateToHistoryEntryAsync(int entryId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("entryId", entryId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.navigateToHistoryEntry", dict);
            return methodResult;
        }

        /// <summary>
        /// Print page as PDF.
        /// </summary>
        public async System.Threading.Tasks.Task<PrintToPDFResponse> PrintToPDFAsync(bool? landscape = null, bool? displayHeaderFooter = null, bool? printBackground = null, long? scale = null, long? paperWidth = null, long? paperHeight = null, long? marginTop = null, long? marginBottom = null, long? marginLeft = null, long? marginRight = null, string pageRanges = null, bool? ignoreInvalidPageRanges = null, string headerTemplate = null, string footerTemplate = null, bool? preferCSSPageSize = null, string transferMode = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (landscape.HasValue)
            {
                dict.Add("landscape", landscape.Value);
            }

            if (displayHeaderFooter.HasValue)
            {
                dict.Add("displayHeaderFooter", displayHeaderFooter.Value);
            }

            if (printBackground.HasValue)
            {
                dict.Add("printBackground", printBackground.Value);
            }

            if (scale.HasValue)
            {
                dict.Add("scale", scale.Value);
            }

            if (paperWidth.HasValue)
            {
                dict.Add("paperWidth", paperWidth.Value);
            }

            if (paperHeight.HasValue)
            {
                dict.Add("paperHeight", paperHeight.Value);
            }

            if (marginTop.HasValue)
            {
                dict.Add("marginTop", marginTop.Value);
            }

            if (marginBottom.HasValue)
            {
                dict.Add("marginBottom", marginBottom.Value);
            }

            if (marginLeft.HasValue)
            {
                dict.Add("marginLeft", marginLeft.Value);
            }

            if (marginRight.HasValue)
            {
                dict.Add("marginRight", marginRight.Value);
            }

            if (!(string.IsNullOrEmpty(pageRanges)))
            {
                dict.Add("pageRanges", pageRanges);
            }

            if (ignoreInvalidPageRanges.HasValue)
            {
                dict.Add("ignoreInvalidPageRanges", ignoreInvalidPageRanges.Value);
            }

            if (!(string.IsNullOrEmpty(headerTemplate)))
            {
                dict.Add("headerTemplate", headerTemplate);
            }

            if (!(string.IsNullOrEmpty(footerTemplate)))
            {
                dict.Add("footerTemplate", footerTemplate);
            }

            if (preferCSSPageSize.HasValue)
            {
                dict.Add("preferCSSPageSize", preferCSSPageSize.Value);
            }

            if (!(string.IsNullOrEmpty(transferMode)))
            {
                dict.Add("transferMode", transferMode);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.printToPDF", dict);
            return methodResult.DeserializeJson<PrintToPDFResponse>();
        }

        /// <summary>
        /// Reloads given page optionally ignoring the cache.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ReloadAsync(bool? ignoreCache = null, string scriptToEvaluateOnLoad = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (ignoreCache.HasValue)
            {
                dict.Add("ignoreCache", ignoreCache.Value);
            }

            if (!(string.IsNullOrEmpty(scriptToEvaluateOnLoad)))
            {
                dict.Add("scriptToEvaluateOnLoad", scriptToEvaluateOnLoad);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.reload", dict);
            return methodResult;
        }

        /// <summary>
        /// Removes given script from the list.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveScriptToEvaluateOnNewDocumentAsync(string identifier)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("identifier", identifier);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.removeScriptToEvaluateOnNewDocument", dict);
            return methodResult;
        }

        /// <summary>
        /// Acknowledges that a screencast frame has been received by the frontend.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ScreencastFrameAckAsync(int sessionId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("sessionId", sessionId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.screencastFrameAck", dict);
            return methodResult;
        }

        /// <summary>
        /// Searches for given string in resource content.
        /// </summary>
        public async System.Threading.Tasks.Task<SearchInResourceResponse> SearchInResourceAsync(string frameId, string url, string query, bool? caseSensitive = null, bool? isRegex = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("frameId", frameId);
            dict.Add("url", url);
            dict.Add("query", query);
            if (caseSensitive.HasValue)
            {
                dict.Add("caseSensitive", caseSensitive.Value);
            }

            if (isRegex.HasValue)
            {
                dict.Add("isRegex", isRegex.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.searchInResource", dict);
            return methodResult.DeserializeJson<SearchInResourceResponse>();
        }

        /// <summary>
        /// Enable Chrome's experimental ad filter on all sites.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetAdBlockingEnabledAsync(bool enabled)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enabled", enabled);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.setAdBlockingEnabled", dict);
            return methodResult;
        }

        /// <summary>
        /// Enable page Content Security Policy by-passing.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetBypassCSPAsync(bool enabled)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enabled", enabled);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.setBypassCSP", dict);
            return methodResult;
        }

        /// <summary>
        /// Set generic font families.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetFontFamiliesAsync(CefSharp.DevTools.Page.FontFamilies fontFamilies)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("fontFamilies", fontFamilies.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.setFontFamilies", dict);
            return methodResult;
        }

        /// <summary>
        /// Set default font sizes.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetFontSizesAsync(CefSharp.DevTools.Page.FontSizes fontSizes)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("fontSizes", fontSizes.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.setFontSizes", dict);
            return methodResult;
        }

        /// <summary>
        /// Sets given markup as the document's HTML.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDocumentContentAsync(string frameId, string html)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("frameId", frameId);
            dict.Add("html", html);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.setDocumentContent", dict);
            return methodResult;
        }

        /// <summary>
        /// Controls whether page will emit lifecycle events.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetLifecycleEventsEnabledAsync(bool enabled)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enabled", enabled);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.setLifecycleEventsEnabled", dict);
            return methodResult;
        }

        /// <summary>
        /// Starts sending each frame using the `screencastFrame` event.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StartScreencastAsync(string format = null, int? quality = null, int? maxWidth = null, int? maxHeight = null, int? everyNthFrame = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(format)))
            {
                dict.Add("format", format);
            }

            if (quality.HasValue)
            {
                dict.Add("quality", quality.Value);
            }

            if (maxWidth.HasValue)
            {
                dict.Add("maxWidth", maxWidth.Value);
            }

            if (maxHeight.HasValue)
            {
                dict.Add("maxHeight", maxHeight.Value);
            }

            if (everyNthFrame.HasValue)
            {
                dict.Add("everyNthFrame", everyNthFrame.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.startScreencast", dict);
            return methodResult;
        }

        /// <summary>
        /// Force the page stop all navigations and pending resource fetches.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StopLoadingAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.stopLoading", dict);
            return methodResult;
        }

        /// <summary>
        /// Crashes renderer on the IO thread, generates minidumps.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> CrashAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.crash", dict);
            return methodResult;
        }

        /// <summary>
        /// Tries to close page, running its beforeunload hooks, if any.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> CloseAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.close", dict);
            return methodResult;
        }

        /// <summary>
        /// Tries to update the web lifecycle state of the page.
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetWebLifecycleStateAsync(string state)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("state", state);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.setWebLifecycleState", dict);
            return methodResult;
        }

        /// <summary>
        /// Stops sending each frame in the `screencastFrame`.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StopScreencastAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.stopScreencast", dict);
            return methodResult;
        }

        /// <summary>
        /// Forces compilation cache to be generated for every subresource script.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetProduceCompilationCacheAsync(bool enabled)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enabled", enabled);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.setProduceCompilationCache", dict);
            return methodResult;
        }

        /// <summary>
        /// Seeds compilation cache for given url. Compilation cache does not survive
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> AddCompilationCacheAsync(string url, byte[] data)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("url", url);
            dict.Add("data", ToBase64String(data));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.addCompilationCache", dict);
            return methodResult;
        }

        /// <summary>
        /// Clears seeded compilation cache.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearCompilationCacheAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.clearCompilationCache", dict);
            return methodResult;
        }

        /// <summary>
        /// Generates a report for testing.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> GenerateTestReportAsync(string message, string group = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("message", message);
            if (!(string.IsNullOrEmpty(group)))
            {
                dict.Add("group", group);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.generateTestReport", dict);
            return methodResult;
        }

        /// <summary>
        /// Pauses page execution. Can be resumed using generic Runtime.runIfWaitingForDebugger.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> WaitForDebuggerAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.waitForDebugger", dict);
            return methodResult;
        }

        /// <summary>
        /// Intercept file chooser requests and transfer control to protocol clients.
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetInterceptFileChooserDialogAsync(bool enabled)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enabled", enabled);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.setInterceptFileChooserDialog", dict);
            return methodResult;
        }
    }
}