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

            var result = await _client.ExecuteDevToolsMethodAsync("Page.addScriptToEvaluateOnNewDocument", dict);
            return result.DeserializeJson<AddScriptToEvaluateOnNewDocumentResponse>();
        }

        /// <summary>
        /// Brings page to front (activates tab).
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> BringToFrontAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.bringToFront", dict);
            return result;
        }

        /// <summary>
        /// Capture page screenshot.
        /// </summary>
        public async System.Threading.Tasks.Task<CaptureScreenshotResponse> CaptureScreenshotAsync(string format = null, int? quality = null, Viewport clip = null, bool? fromSurface = null)
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

            var result = await _client.ExecuteDevToolsMethodAsync("Page.captureScreenshot", dict);
            return result.DeserializeJson<CaptureScreenshotResponse>();
        }

        /// <summary>
        /// Clears the overriden Geolocation Position and Error.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ClearGeolocationOverrideAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.clearGeolocationOverride", dict);
            return result;
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

            var result = await _client.ExecuteDevToolsMethodAsync("Page.createIsolatedWorld", dict);
            return result.DeserializeJson<CreateIsolatedWorldResponse>();
        }

        /// <summary>
        /// Disables page domain notifications.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.disable", dict);
            return result;
        }

        /// <summary>
        /// Enables page domain notifications.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.enable", dict);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public async System.Threading.Tasks.Task<GetAppManifestResponse> GetAppManifestAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.getAppManifest", dict);
            return result.DeserializeJson<GetAppManifestResponse>();
        }

        /// <summary>
        /// Returns present frame tree structure.
        /// </summary>
        public async System.Threading.Tasks.Task<GetFrameTreeResponse> GetFrameTreeAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.getFrameTree", dict);
            return result.DeserializeJson<GetFrameTreeResponse>();
        }

        /// <summary>
        /// Returns metrics relating to the layouting of the page, such as viewport bounds/scale.
        /// </summary>
        public async System.Threading.Tasks.Task<GetLayoutMetricsResponse> GetLayoutMetricsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.getLayoutMetrics", dict);
            return result.DeserializeJson<GetLayoutMetricsResponse>();
        }

        /// <summary>
        /// Returns navigation history for the current page.
        /// </summary>
        public async System.Threading.Tasks.Task<GetNavigationHistoryResponse> GetNavigationHistoryAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.getNavigationHistory", dict);
            return result.DeserializeJson<GetNavigationHistoryResponse>();
        }

        /// <summary>
        /// Resets navigation history for the current page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ResetNavigationHistoryAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.resetNavigationHistory", dict);
            return result;
        }

        /// <summary>
        /// Accepts or dismisses a JavaScript initiated dialog (alert, confirm, prompt, or onbeforeunload).
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> HandleJavaScriptDialogAsync(bool accept, string promptText = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("accept", accept);
            if (!(string.IsNullOrEmpty(promptText)))
            {
                dict.Add("promptText", promptText);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Page.handleJavaScriptDialog", dict);
            return result;
        }

        /// <summary>
        /// Navigates current page to the given URL.
        /// </summary>
        public async System.Threading.Tasks.Task<NavigateResponse> NavigateAsync(string url, string referrer = null, string transitionType = null, string frameId = null, string referrerPolicy = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("url", url);
            if (!(string.IsNullOrEmpty(referrer)))
            {
                dict.Add("referrer", referrer);
            }

            if (!(string.IsNullOrEmpty(transitionType)))
            {
                dict.Add("transitionType", transitionType);
            }

            if (!(string.IsNullOrEmpty(frameId)))
            {
                dict.Add("frameId", frameId);
            }

            if (!(string.IsNullOrEmpty(referrerPolicy)))
            {
                dict.Add("referrerPolicy", referrerPolicy);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Page.navigate", dict);
            return result.DeserializeJson<NavigateResponse>();
        }

        /// <summary>
        /// Navigates current page to the given history entry.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> NavigateToHistoryEntryAsync(int entryId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("entryId", entryId);
            var result = await _client.ExecuteDevToolsMethodAsync("Page.navigateToHistoryEntry", dict);
            return result;
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

            var result = await _client.ExecuteDevToolsMethodAsync("Page.printToPDF", dict);
            return result.DeserializeJson<PrintToPDFResponse>();
        }

        /// <summary>
        /// Reloads given page optionally ignoring the cache.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ReloadAsync(bool? ignoreCache = null, string scriptToEvaluateOnLoad = null)
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

            var result = await _client.ExecuteDevToolsMethodAsync("Page.reload", dict);
            return result;
        }

        /// <summary>
        /// Removes given script from the list.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> RemoveScriptToEvaluateOnNewDocumentAsync(string identifier)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("identifier", identifier);
            var result = await _client.ExecuteDevToolsMethodAsync("Page.removeScriptToEvaluateOnNewDocument", dict);
            return result;
        }

        /// <summary>
        /// Sets given markup as the document's HTML.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetDocumentContentAsync(string frameId, string html)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("frameId", frameId);
            dict.Add("html", html);
            var result = await _client.ExecuteDevToolsMethodAsync("Page.setDocumentContent", dict);
            return result;
        }

        /// <summary>
        /// Overrides the Geolocation Position or Error. Omitting any of the parameters emulates position
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetGeolocationOverrideAsync(long? latitude = null, long? longitude = null, long? accuracy = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (latitude.HasValue)
            {
                dict.Add("latitude", latitude.Value);
            }

            if (longitude.HasValue)
            {
                dict.Add("longitude", longitude.Value);
            }

            if (accuracy.HasValue)
            {
                dict.Add("accuracy", accuracy.Value);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Page.setGeolocationOverride", dict);
            return result;
        }

        /// <summary>
        /// Force the page stop all navigations and pending resource fetches.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> StopLoadingAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Page.stopLoading", dict);
            return result;
        }
    }
}