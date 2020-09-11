// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.CSS
{
    using System.Linq;

    /// <summary>
    /// This domain exposes CSS read/write operations. All CSS objects (stylesheets, rules, and styles)
    /// have an associated `id` used in subsequent operations on the related object. Each object type has
    /// a specific `id` structure, and those are not interchangeable between objects of different kinds.
    /// CSS objects can be loaded using the `get*ForNode()` calls (which accept a DOM node id). A client
    /// can also keep track of stylesheets via the `styleSheetAdded`/`styleSheetRemoved` events and
    /// subsequently load the required stylesheet contents using the `getStyleSheet[Text]()` methods.
    /// </summary>
    public partial class CSS : DevToolsDomainBase
    {
        public CSS(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.IDevToolsClient _client;
        /// <summary>
        /// Inserts a new rule with the given `ruleText` in a stylesheet with given `styleSheetId`, at the
        /// position specified by `location`.
        /// </summary>
        public async System.Threading.Tasks.Task<AddRuleResponse> AddRuleAsync(string styleSheetId, string ruleText, CefSharp.DevTools.CSS.SourceRange location)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("styleSheetId", styleSheetId);
            dict.Add("ruleText", ruleText);
            dict.Add("location", location.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.addRule", dict);
            return methodResult.DeserializeJson<AddRuleResponse>();
        }

        /// <summary>
        /// Returns all class names from specified stylesheet.
        /// </summary>
        public async System.Threading.Tasks.Task<CollectClassNamesResponse> CollectClassNamesAsync(string styleSheetId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("styleSheetId", styleSheetId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.collectClassNames", dict);
            return methodResult.DeserializeJson<CollectClassNamesResponse>();
        }

        /// <summary>
        /// Creates a new special "via-inspector" stylesheet in the frame with given `frameId`.
        /// </summary>
        public async System.Threading.Tasks.Task<CreateStyleSheetResponse> CreateStyleSheetAsync(string frameId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("frameId", frameId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.createStyleSheet", dict);
            return methodResult.DeserializeJson<CreateStyleSheetResponse>();
        }

        /// <summary>
        /// Disables the CSS agent for the given page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables the CSS agent for the given page. Clients should not assume that the CSS agent has been
        /// enabled until the result of this command is received.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.enable", dict);
            return methodResult;
        }

        /// <summary>
        /// Ensures that the given node will have specified pseudo-classes whenever its style is computed by
        /// the browser.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ForcePseudoStateAsync(int nodeId, string[] forcedPseudoClasses)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("forcedPseudoClasses", forcedPseudoClasses);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.forcePseudoState", dict);
            return methodResult;
        }

        /// <summary>
        /// GetBackgroundColors
        /// </summary>
        public async System.Threading.Tasks.Task<GetBackgroundColorsResponse> GetBackgroundColorsAsync(int nodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.getBackgroundColors", dict);
            return methodResult.DeserializeJson<GetBackgroundColorsResponse>();
        }

        /// <summary>
        /// Returns the computed style for a DOM node identified by `nodeId`.
        /// </summary>
        public async System.Threading.Tasks.Task<GetComputedStyleForNodeResponse> GetComputedStyleForNodeAsync(int nodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.getComputedStyleForNode", dict);
            return methodResult.DeserializeJson<GetComputedStyleForNodeResponse>();
        }

        /// <summary>
        /// Returns the styles defined inline (explicitly in the "style" attribute and implicitly, using DOM
        /// attributes) for a DOM node identified by `nodeId`.
        /// </summary>
        public async System.Threading.Tasks.Task<GetInlineStylesForNodeResponse> GetInlineStylesForNodeAsync(int nodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.getInlineStylesForNode", dict);
            return methodResult.DeserializeJson<GetInlineStylesForNodeResponse>();
        }

        /// <summary>
        /// Returns requested styles for a DOM node identified by `nodeId`.
        /// </summary>
        public async System.Threading.Tasks.Task<GetMatchedStylesForNodeResponse> GetMatchedStylesForNodeAsync(int nodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.getMatchedStylesForNode", dict);
            return methodResult.DeserializeJson<GetMatchedStylesForNodeResponse>();
        }

        /// <summary>
        /// Returns all media queries parsed by the rendering engine.
        /// </summary>
        public async System.Threading.Tasks.Task<GetMediaQueriesResponse> GetMediaQueriesAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.getMediaQueries", dict);
            return methodResult.DeserializeJson<GetMediaQueriesResponse>();
        }

        /// <summary>
        /// Requests information about platform fonts which we used to render child TextNodes in the given
        /// node.
        /// </summary>
        public async System.Threading.Tasks.Task<GetPlatformFontsForNodeResponse> GetPlatformFontsForNodeAsync(int nodeId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.getPlatformFontsForNode", dict);
            return methodResult.DeserializeJson<GetPlatformFontsForNodeResponse>();
        }

        /// <summary>
        /// Returns the current textual content for a stylesheet.
        /// </summary>
        public async System.Threading.Tasks.Task<GetStyleSheetTextResponse> GetStyleSheetTextAsync(string styleSheetId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("styleSheetId", styleSheetId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.getStyleSheetText", dict);
            return methodResult.DeserializeJson<GetStyleSheetTextResponse>();
        }

        /// <summary>
        /// Find a rule with the given active property for the given node and set the new value for this
        /// property
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetEffectivePropertyValueForNodeAsync(int nodeId, string propertyName, string value)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("propertyName", propertyName);
            dict.Add("value", value);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.setEffectivePropertyValueForNode", dict);
            return methodResult;
        }

        /// <summary>
        /// Modifies the keyframe rule key text.
        /// </summary>
        public async System.Threading.Tasks.Task<SetKeyframeKeyResponse> SetKeyframeKeyAsync(string styleSheetId, CefSharp.DevTools.CSS.SourceRange range, string keyText)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("styleSheetId", styleSheetId);
            dict.Add("range", range.ToDictionary());
            dict.Add("keyText", keyText);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.setKeyframeKey", dict);
            return methodResult.DeserializeJson<SetKeyframeKeyResponse>();
        }

        /// <summary>
        /// Modifies the rule selector.
        /// </summary>
        public async System.Threading.Tasks.Task<SetMediaTextResponse> SetMediaTextAsync(string styleSheetId, CefSharp.DevTools.CSS.SourceRange range, string text)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("styleSheetId", styleSheetId);
            dict.Add("range", range.ToDictionary());
            dict.Add("text", text);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.setMediaText", dict);
            return methodResult.DeserializeJson<SetMediaTextResponse>();
        }

        /// <summary>
        /// Modifies the rule selector.
        /// </summary>
        public async System.Threading.Tasks.Task<SetRuleSelectorResponse> SetRuleSelectorAsync(string styleSheetId, CefSharp.DevTools.CSS.SourceRange range, string selector)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("styleSheetId", styleSheetId);
            dict.Add("range", range.ToDictionary());
            dict.Add("selector", selector);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.setRuleSelector", dict);
            return methodResult.DeserializeJson<SetRuleSelectorResponse>();
        }

        /// <summary>
        /// Sets the new stylesheet text.
        /// </summary>
        public async System.Threading.Tasks.Task<SetStyleSheetTextResponse> SetStyleSheetTextAsync(string styleSheetId, string text)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("styleSheetId", styleSheetId);
            dict.Add("text", text);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.setStyleSheetText", dict);
            return methodResult.DeserializeJson<SetStyleSheetTextResponse>();
        }

        /// <summary>
        /// Applies specified style edits one after another in the given order.
        /// </summary>
        public async System.Threading.Tasks.Task<SetStyleTextsResponse> SetStyleTextsAsync(System.Collections.Generic.IList<CefSharp.DevTools.CSS.StyleDeclarationEdit> edits)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("edits", edits.Select(x => x.ToDictionary()));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.setStyleTexts", dict);
            return methodResult.DeserializeJson<SetStyleTextsResponse>();
        }

        /// <summary>
        /// Enables the selector recording.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> StartRuleUsageTrackingAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.startRuleUsageTracking", dict);
            return methodResult;
        }

        /// <summary>
        /// Stop tracking rule usage and return the list of rules that were used since last call to
        /// `takeCoverageDelta` (or since start of coverage instrumentation)
        /// </summary>
        public async System.Threading.Tasks.Task<StopRuleUsageTrackingResponse> StopRuleUsageTrackingAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.stopRuleUsageTracking", dict);
            return methodResult.DeserializeJson<StopRuleUsageTrackingResponse>();
        }

        /// <summary>
        /// Obtain list of rules that became used since last call to this method (or since start of coverage
        /// instrumentation)
        /// </summary>
        public async System.Threading.Tasks.Task<TakeCoverageDeltaResponse> TakeCoverageDeltaAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.takeCoverageDelta", dict);
            return methodResult.DeserializeJson<TakeCoverageDeltaResponse>();
        }
    }
}