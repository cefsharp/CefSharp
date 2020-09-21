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
        private CefSharp.DevTools.IDevToolsClient _client;
        public Page(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        partial void ValidateAddScriptToEvaluateOnNewDocument(string source, string worldName = null);
        /// <summary>
        /// Evaluates given script in every frame upon creation (before loading frame's scripts).
        /// </summary>
        /// <param name = "source">source</param>
        /// <param name = "worldName">If specified, creates an isolated world with the given name and evaluates given script in it.
        public async System.Threading.Tasks.Task<AddScriptToEvaluateOnNewDocumentResponse> AddScriptToEvaluateOnNewDocumentAsync(string source, string worldName = null)
        {
            ValidateAddScriptToEvaluateOnNewDocument(source, worldName);
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
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> BringToFrontAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.bringToFront", dict);
            return methodResult;
        }

        partial void ValidateCaptureScreenshot(string format = null, int? quality = null, CefSharp.DevTools.Page.Viewport clip = null, bool? fromSurface = null);
        /// <summary>
        /// Capture page screenshot.
        /// </summary>
        /// <param name = "format">Image compression format (defaults to png).</param>
        /// <param name = "quality">Compression quality from range [0..100] (jpeg only).</param>
        /// <param name = "clip">Capture the screenshot of a given region only.</param>
        /// <param name = "fromSurface">Capture the screenshot from the surface, rather than the view. Defaults to true.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;CaptureScreenshotResponse&gt;</returns>
        public async System.Threading.Tasks.Task<CaptureScreenshotResponse> CaptureScreenshotAsync(string format = null, int? quality = null, CefSharp.DevTools.Page.Viewport clip = null, bool? fromSurface = null)
        {
            ValidateCaptureScreenshot(format, quality, clip, fromSurface);
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

        partial void ValidateCaptureSnapshot(string format = null);
        /// <summary>
        /// Returns a snapshot of the page as a string. For MHTML format, the serialization includes
        /// iframes, shadow DOM, external resources, and element-inline styles.
        /// </summary>
        /// <param name = "format">Format (defaults to mhtml).</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;CaptureSnapshotResponse&gt;</returns>
        public async System.Threading.Tasks.Task<CaptureSnapshotResponse> CaptureSnapshotAsync(string format = null)
        {
            ValidateCaptureSnapshot(format);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(format)))
            {
                dict.Add("format", format);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.captureSnapshot", dict);
            return methodResult.DeserializeJson<CaptureSnapshotResponse>();
        }

        partial void ValidateCreateIsolatedWorld(string frameId, string worldName = null, bool? grantUniveralAccess = null);
        /// <summary>
        /// Creates an isolated world for the given frame.
        /// </summary>
        /// <param name = "frameId">Id of the frame in which the isolated world should be created.</param>
        /// <param name = "worldName">An optional name which is reported in the Execution Context.</param>
        /// <param name = "grantUniveralAccess">Whether or not universal access should be granted to the isolated world. This is a powerful
        public async System.Threading.Tasks.Task<CreateIsolatedWorldResponse> CreateIsolatedWorldAsync(string frameId, string worldName = null, bool? grantUniveralAccess = null)
        {
            ValidateCreateIsolatedWorld(frameId, worldName, grantUniveralAccess);
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
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables page domain notifications.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// GetAppManifest
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetAppManifestResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetAppManifestResponse> GetAppManifestAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.getAppManifest", dict);
            return methodResult.DeserializeJson<GetAppManifestResponse>();
        }

        /// <summary>
        /// GetInstallabilityErrors
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetInstallabilityErrorsResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetInstallabilityErrorsResponse> GetInstallabilityErrorsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.getInstallabilityErrors", dict);
            return methodResult.DeserializeJson<GetInstallabilityErrorsResponse>();
        }

        /// <summary>
        /// GetManifestIcons
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetManifestIconsResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetManifestIconsResponse> GetManifestIconsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.getManifestIcons", dict);
            return methodResult.DeserializeJson<GetManifestIconsResponse>();
        }

        /// <summary>
        /// Returns present frame tree structure.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetFrameTreeResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetFrameTreeResponse> GetFrameTreeAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.getFrameTree", dict);
            return methodResult.DeserializeJson<GetFrameTreeResponse>();
        }

        /// <summary>
        /// Returns metrics relating to the layouting of the page, such as viewport bounds/scale.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetLayoutMetricsResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetLayoutMetricsResponse> GetLayoutMetricsAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.getLayoutMetrics", dict);
            return methodResult.DeserializeJson<GetLayoutMetricsResponse>();
        }

        /// <summary>
        /// Returns navigation history for the current page.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetNavigationHistoryResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetNavigationHistoryResponse> GetNavigationHistoryAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.getNavigationHistory", dict);
            return methodResult.DeserializeJson<GetNavigationHistoryResponse>();
        }

        /// <summary>
        /// Resets navigation history for the current page.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ResetNavigationHistoryAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.resetNavigationHistory", dict);
            return methodResult;
        }

        partial void ValidateGetResourceContent(string frameId, string url);
        /// <summary>
        /// Returns content of the given resource.
        /// </summary>
        /// <param name = "frameId">Frame id to get resource for.</param>
        /// <param name = "url">URL of the resource to get content for.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetResourceContentResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetResourceContentResponse> GetResourceContentAsync(string frameId, string url)
        {
            ValidateGetResourceContent(frameId, url);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("frameId", frameId);
            dict.Add("url", url);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.getResourceContent", dict);
            return methodResult.DeserializeJson<GetResourceContentResponse>();
        }

        /// <summary>
        /// Returns present frame / resource tree structure.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetResourceTreeResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetResourceTreeResponse> GetResourceTreeAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.getResourceTree", dict);
            return methodResult.DeserializeJson<GetResourceTreeResponse>();
        }

        partial void ValidateHandleJavaScriptDialog(bool accept, string promptText = null);
        /// <summary>
        /// Accepts or dismisses a JavaScript initiated dialog (alert, confirm, prompt, or onbeforeunload).
        /// </summary>
        /// <param name = "accept">Whether to accept or dismiss the dialog.</param>
        /// <param name = "promptText">The text to enter into the dialog prompt before accepting. Used only if this is a prompt
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> HandleJavaScriptDialogAsync(bool accept, string promptText = null)
        {
            ValidateHandleJavaScriptDialog(accept, promptText);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("accept", accept);
            if (!(string.IsNullOrEmpty(promptText)))
            {
                dict.Add("promptText", promptText);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.handleJavaScriptDialog", dict);
            return methodResult;
        }

        partial void ValidateNavigate(string url, string referrer = null, CefSharp.DevTools.Page.TransitionType? transitionType = null, string frameId = null, CefSharp.DevTools.Page.ReferrerPolicy? referrerPolicy = null);
        /// <summary>
        /// Navigates current page to the given URL.
        /// </summary>
        /// <param name = "url">URL to navigate the page to.</param>
        /// <param name = "referrer">Referrer URL.</param>
        /// <param name = "transitionType">Intended transition type.</param>
        /// <param name = "frameId">Frame id to navigate, if not specified navigates the top frame.</param>
        /// <param name = "referrerPolicy">Referrer-policy used for the navigation.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;NavigateResponse&gt;</returns>
        public async System.Threading.Tasks.Task<NavigateResponse> NavigateAsync(string url, string referrer = null, CefSharp.DevTools.Page.TransitionType? transitionType = null, string frameId = null, CefSharp.DevTools.Page.ReferrerPolicy? referrerPolicy = null)
        {
            ValidateNavigate(url, referrer, transitionType, frameId, referrerPolicy);
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

        partial void ValidateNavigateToHistoryEntry(int entryId);
        /// <summary>
        /// Navigates current page to the given history entry.
        /// </summary>
        /// <param name = "entryId">Unique id of the entry to navigate to.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> NavigateToHistoryEntryAsync(int entryId)
        {
            ValidateNavigateToHistoryEntry(entryId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("entryId", entryId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.navigateToHistoryEntry", dict);
            return methodResult;
        }

        partial void ValidatePrintToPDF(bool? landscape = null, bool? displayHeaderFooter = null, bool? printBackground = null, long? scale = null, long? paperWidth = null, long? paperHeight = null, long? marginTop = null, long? marginBottom = null, long? marginLeft = null, long? marginRight = null, string pageRanges = null, bool? ignoreInvalidPageRanges = null, string headerTemplate = null, string footerTemplate = null, bool? preferCSSPageSize = null, string transferMode = null);
        /// <summary>
        /// Print page as PDF.
        /// </summary>
        /// <param name = "landscape">Paper orientation. Defaults to false.</param>
        /// <param name = "displayHeaderFooter">Display header and footer. Defaults to false.</param>
        /// <param name = "printBackground">Print background graphics. Defaults to false.</param>
        /// <param name = "scale">Scale of the webpage rendering. Defaults to 1.</param>
        /// <param name = "paperWidth">Paper width in inches. Defaults to 8.5 inches.</param>
        /// <param name = "paperHeight">Paper height in inches. Defaults to 11 inches.</param>
        /// <param name = "marginTop">Top margin in inches. Defaults to 1cm (~0.4 inches).</param>
        /// <param name = "marginBottom">Bottom margin in inches. Defaults to 1cm (~0.4 inches).</param>
        /// <param name = "marginLeft">Left margin in inches. Defaults to 1cm (~0.4 inches).</param>
        /// <param name = "marginRight">Right margin in inches. Defaults to 1cm (~0.4 inches).</param>
        /// <param name = "pageRanges">Paper ranges to print, e.g., '1-5, 8, 11-13'. Defaults to the empty string, which means
        public async System.Threading.Tasks.Task<PrintToPDFResponse> PrintToPDFAsync(bool? landscape = null, bool? displayHeaderFooter = null, bool? printBackground = null, long? scale = null, long? paperWidth = null, long? paperHeight = null, long? marginTop = null, long? marginBottom = null, long? marginLeft = null, long? marginRight = null, string pageRanges = null, bool? ignoreInvalidPageRanges = null, string headerTemplate = null, string footerTemplate = null, bool? preferCSSPageSize = null, string transferMode = null)
        {
            ValidatePrintToPDF(landscape, displayHeaderFooter, printBackground, scale, paperWidth, paperHeight, marginTop, marginBottom, marginLeft, marginRight, pageRanges, ignoreInvalidPageRanges, headerTemplate, footerTemplate, preferCSSPageSize, transferMode);
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

        partial void ValidateReload(bool? ignoreCache = null, string scriptToEvaluateOnLoad = null);
        /// <summary>
        /// Reloads given page optionally ignoring the cache.
        /// </summary>
        /// <param name = "ignoreCache">If true, browser cache is ignored (as if the user pressed Shift+refresh).</param>
        /// <param name = "scriptToEvaluateOnLoad">If set, the script will be injected into all frames of the inspected page after reload.
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ReloadAsync(bool? ignoreCache = null, string scriptToEvaluateOnLoad = null)
        {
            ValidateReload(ignoreCache, scriptToEvaluateOnLoad);
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

        partial void ValidateRemoveScriptToEvaluateOnNewDocument(string identifier);
        /// <summary>
        /// Removes given script from the list.
        /// </summary>
        /// <param name = "identifier">identifier</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> RemoveScriptToEvaluateOnNewDocumentAsync(string identifier)
        {
            ValidateRemoveScriptToEvaluateOnNewDocument(identifier);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("identifier", identifier);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.removeScriptToEvaluateOnNewDocument", dict);
            return methodResult;
        }

        partial void ValidateScreencastFrameAck(int sessionId);
        /// <summary>
        /// Acknowledges that a screencast frame has been received by the frontend.
        /// </summary>
        /// <param name = "sessionId">Frame number.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ScreencastFrameAckAsync(int sessionId)
        {
            ValidateScreencastFrameAck(sessionId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("sessionId", sessionId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.screencastFrameAck", dict);
            return methodResult;
        }

        partial void ValidateSearchInResource(string frameId, string url, string query, bool? caseSensitive = null, bool? isRegex = null);
        /// <summary>
        /// Searches for given string in resource content.
        /// </summary>
        /// <param name = "frameId">Frame id for resource to search in.</param>
        /// <param name = "url">URL of the resource to search in.</param>
        /// <param name = "query">String to search for.</param>
        /// <param name = "caseSensitive">If true, search is case sensitive.</param>
        /// <param name = "isRegex">If true, treats string parameter as regex.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;SearchInResourceResponse&gt;</returns>
        public async System.Threading.Tasks.Task<SearchInResourceResponse> SearchInResourceAsync(string frameId, string url, string query, bool? caseSensitive = null, bool? isRegex = null)
        {
            ValidateSearchInResource(frameId, url, query, caseSensitive, isRegex);
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

        partial void ValidateSetAdBlockingEnabled(bool enabled);
        /// <summary>
        /// Enable Chrome's experimental ad filter on all sites.
        /// </summary>
        /// <param name = "enabled">Whether to block ads.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetAdBlockingEnabledAsync(bool enabled)
        {
            ValidateSetAdBlockingEnabled(enabled);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enabled", enabled);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.setAdBlockingEnabled", dict);
            return methodResult;
        }

        partial void ValidateSetBypassCSP(bool enabled);
        /// <summary>
        /// Enable page Content Security Policy by-passing.
        /// </summary>
        /// <param name = "enabled">Whether to bypass page CSP.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetBypassCSPAsync(bool enabled)
        {
            ValidateSetBypassCSP(enabled);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enabled", enabled);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.setBypassCSP", dict);
            return methodResult;
        }

        partial void ValidateSetFontFamilies(CefSharp.DevTools.Page.FontFamilies fontFamilies);
        /// <summary>
        /// Set generic font families.
        /// </summary>
        /// <param name = "fontFamilies">Specifies font families to set. If a font family is not specified, it won't be changed.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetFontFamiliesAsync(CefSharp.DevTools.Page.FontFamilies fontFamilies)
        {
            ValidateSetFontFamilies(fontFamilies);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("fontFamilies", fontFamilies.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.setFontFamilies", dict);
            return methodResult;
        }

        partial void ValidateSetFontSizes(CefSharp.DevTools.Page.FontSizes fontSizes);
        /// <summary>
        /// Set default font sizes.
        /// </summary>
        /// <param name = "fontSizes">Specifies font sizes to set. If a font size is not specified, it won't be changed.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetFontSizesAsync(CefSharp.DevTools.Page.FontSizes fontSizes)
        {
            ValidateSetFontSizes(fontSizes);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("fontSizes", fontSizes.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.setFontSizes", dict);
            return methodResult;
        }

        partial void ValidateSetDocumentContent(string frameId, string html);
        /// <summary>
        /// Sets given markup as the document's HTML.
        /// </summary>
        /// <param name = "frameId">Frame id to set HTML for.</param>
        /// <param name = "html">HTML content to set.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDocumentContentAsync(string frameId, string html)
        {
            ValidateSetDocumentContent(frameId, html);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("frameId", frameId);
            dict.Add("html", html);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.setDocumentContent", dict);
            return methodResult;
        }

        partial void ValidateSetLifecycleEventsEnabled(bool enabled);
        /// <summary>
        /// Controls whether page will emit lifecycle events.
        /// </summary>
        /// <param name = "enabled">If true, starts emitting lifecycle events.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetLifecycleEventsEnabledAsync(bool enabled)
        {
            ValidateSetLifecycleEventsEnabled(enabled);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enabled", enabled);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.setLifecycleEventsEnabled", dict);
            return methodResult;
        }

        partial void ValidateStartScreencast(string format = null, int? quality = null, int? maxWidth = null, int? maxHeight = null, int? everyNthFrame = null);
        /// <summary>
        /// Starts sending each frame using the `screencastFrame` event.
        /// </summary>
        /// <param name = "format">Image compression format.</param>
        /// <param name = "quality">Compression quality from range [0..100].</param>
        /// <param name = "maxWidth">Maximum screenshot width.</param>
        /// <param name = "maxHeight">Maximum screenshot height.</param>
        /// <param name = "everyNthFrame">Send every n-th frame.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StartScreencastAsync(string format = null, int? quality = null, int? maxWidth = null, int? maxHeight = null, int? everyNthFrame = null)
        {
            ValidateStartScreencast(format, quality, maxWidth, maxHeight, everyNthFrame);
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
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StopLoadingAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.stopLoading", dict);
            return methodResult;
        }

        /// <summary>
        /// Crashes renderer on the IO thread, generates minidumps.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> CrashAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.crash", dict);
            return methodResult;
        }

        /// <summary>
        /// Tries to close page, running its beforeunload hooks, if any.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> CloseAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.close", dict);
            return methodResult;
        }

        partial void ValidateSetWebLifecycleState(string state);
        /// <summary>
        /// Tries to update the web lifecycle state of the page.
        /// It will transition the page to the given state according to:
        /// https://github.com/WICG/web-lifecycle/
        /// </summary>
        /// <param name = "state">Target lifecycle state</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetWebLifecycleStateAsync(string state)
        {
            ValidateSetWebLifecycleState(state);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("state", state);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.setWebLifecycleState", dict);
            return methodResult;
        }

        /// <summary>
        /// Stops sending each frame in the `screencastFrame`.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StopScreencastAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.stopScreencast", dict);
            return methodResult;
        }

        partial void ValidateSetProduceCompilationCache(bool enabled);
        /// <summary>
        /// Forces compilation cache to be generated for every subresource script.
        /// </summary>
        /// <param name = "enabled">enabled</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetProduceCompilationCacheAsync(bool enabled)
        {
            ValidateSetProduceCompilationCache(enabled);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enabled", enabled);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.setProduceCompilationCache", dict);
            return methodResult;
        }

        partial void ValidateAddCompilationCache(string url, byte[] data);
        /// <summary>
        /// Seeds compilation cache for given url. Compilation cache does not survive
        /// cross-process navigation.
        /// </summary>
        /// <param name = "url">url</param>
        /// <param name = "data">Base64-encoded data</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> AddCompilationCacheAsync(string url, byte[] data)
        {
            ValidateAddCompilationCache(url, data);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("url", url);
            dict.Add("data", ToBase64String(data));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.addCompilationCache", dict);
            return methodResult;
        }

        /// <summary>
        /// Clears seeded compilation cache.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearCompilationCacheAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.clearCompilationCache", dict);
            return methodResult;
        }

        partial void ValidateGenerateTestReport(string message, string group = null);
        /// <summary>
        /// Generates a report for testing.
        /// </summary>
        /// <param name = "message">Message to be displayed in the report.</param>
        /// <param name = "group">Specifies the endpoint group to deliver the report to.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> GenerateTestReportAsync(string message, string group = null)
        {
            ValidateGenerateTestReport(message, group);
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
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> WaitForDebuggerAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.waitForDebugger", dict);
            return methodResult;
        }

        partial void ValidateSetInterceptFileChooserDialog(bool enabled);
        /// <summary>
        /// Intercept file chooser requests and transfer control to protocol clients.
        /// When file chooser interception is enabled, native file chooser dialog is not shown.
        /// Instead, a protocol event `Page.fileChooserOpened` is emitted.
        /// </summary>
        /// <param name = "enabled">enabled</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetInterceptFileChooserDialogAsync(bool enabled)
        {
            ValidateSetInterceptFileChooserDialog(enabled);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enabled", enabled);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Page.setInterceptFileChooserDialog", dict);
            return methodResult;
        }
    }
}