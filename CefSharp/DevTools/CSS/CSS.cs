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
        private CefSharp.DevTools.IDevToolsClient _client;
        public CSS(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        partial void ValidateAddRule(string styleSheetId, string ruleText, CefSharp.DevTools.CSS.SourceRange location);
        /// <summary>
        /// Inserts a new rule with the given `ruleText` in a stylesheet with given `styleSheetId`, at the
        /// position specified by `location`.
        /// </summary>
        /// <param name = "styleSheetId">The css style sheet identifier where a new rule should be inserted.</param>
        /// <param name = "ruleText">The text of a new rule.</param>
        /// <param name = "location">Text position of a new rule in the target style sheet.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;AddRuleResponse&gt;</returns>
        public async System.Threading.Tasks.Task<AddRuleResponse> AddRuleAsync(string styleSheetId, string ruleText, CefSharp.DevTools.CSS.SourceRange location)
        {
            ValidateAddRule(styleSheetId, ruleText, location);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("styleSheetId", styleSheetId);
            dict.Add("ruleText", ruleText);
            dict.Add("location", location.ToDictionary());
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.addRule", dict);
            return methodResult.DeserializeJson<AddRuleResponse>();
        }

        partial void ValidateCollectClassNames(string styleSheetId);
        /// <summary>
        /// Returns all class names from specified stylesheet.
        /// </summary>
        /// <param name = "styleSheetId">styleSheetId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;CollectClassNamesResponse&gt;</returns>
        public async System.Threading.Tasks.Task<CollectClassNamesResponse> CollectClassNamesAsync(string styleSheetId)
        {
            ValidateCollectClassNames(styleSheetId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("styleSheetId", styleSheetId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.collectClassNames", dict);
            return methodResult.DeserializeJson<CollectClassNamesResponse>();
        }

        partial void ValidateCreateStyleSheet(string frameId);
        /// <summary>
        /// Creates a new special "via-inspector" stylesheet in the frame with given `frameId`.
        /// </summary>
        /// <param name = "frameId">Identifier of the frame where "via-inspector" stylesheet should be created.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;CreateStyleSheetResponse&gt;</returns>
        public async System.Threading.Tasks.Task<CreateStyleSheetResponse> CreateStyleSheetAsync(string frameId)
        {
            ValidateCreateStyleSheet(frameId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("frameId", frameId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.createStyleSheet", dict);
            return methodResult.DeserializeJson<CreateStyleSheetResponse>();
        }

        /// <summary>
        /// Disables the CSS agent for the given page.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
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
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.enable", dict);
            return methodResult;
        }

        partial void ValidateForcePseudoState(int nodeId, string[] forcedPseudoClasses);
        /// <summary>
        /// Ensures that the given node will have specified pseudo-classes whenever its style is computed by
        /// the browser.
        /// </summary>
        /// <param name = "nodeId">The element id for which to force the pseudo state.</param>
        /// <param name = "forcedPseudoClasses">Element pseudo classes to force when computing the element's style.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ForcePseudoStateAsync(int nodeId, string[] forcedPseudoClasses)
        {
            ValidateForcePseudoState(nodeId, forcedPseudoClasses);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("forcedPseudoClasses", forcedPseudoClasses);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.forcePseudoState", dict);
            return methodResult;
        }

        partial void ValidateGetBackgroundColors(int nodeId);
        /// <summary>
        /// GetBackgroundColors
        /// </summary>
        /// <param name = "nodeId">Id of the node to get background colors for.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetBackgroundColorsResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetBackgroundColorsResponse> GetBackgroundColorsAsync(int nodeId)
        {
            ValidateGetBackgroundColors(nodeId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.getBackgroundColors", dict);
            return methodResult.DeserializeJson<GetBackgroundColorsResponse>();
        }

        partial void ValidateGetComputedStyleForNode(int nodeId);
        /// <summary>
        /// Returns the computed style for a DOM node identified by `nodeId`.
        /// </summary>
        /// <param name = "nodeId">nodeId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetComputedStyleForNodeResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetComputedStyleForNodeResponse> GetComputedStyleForNodeAsync(int nodeId)
        {
            ValidateGetComputedStyleForNode(nodeId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.getComputedStyleForNode", dict);
            return methodResult.DeserializeJson<GetComputedStyleForNodeResponse>();
        }

        partial void ValidateGetInlineStylesForNode(int nodeId);
        /// <summary>
        /// Returns the styles defined inline (explicitly in the "style" attribute and implicitly, using DOM
        /// attributes) for a DOM node identified by `nodeId`.
        /// </summary>
        /// <param name = "nodeId">nodeId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetInlineStylesForNodeResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetInlineStylesForNodeResponse> GetInlineStylesForNodeAsync(int nodeId)
        {
            ValidateGetInlineStylesForNode(nodeId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.getInlineStylesForNode", dict);
            return methodResult.DeserializeJson<GetInlineStylesForNodeResponse>();
        }

        partial void ValidateGetMatchedStylesForNode(int nodeId);
        /// <summary>
        /// Returns requested styles for a DOM node identified by `nodeId`.
        /// </summary>
        /// <param name = "nodeId">nodeId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetMatchedStylesForNodeResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetMatchedStylesForNodeResponse> GetMatchedStylesForNodeAsync(int nodeId)
        {
            ValidateGetMatchedStylesForNode(nodeId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.getMatchedStylesForNode", dict);
            return methodResult.DeserializeJson<GetMatchedStylesForNodeResponse>();
        }

        /// <summary>
        /// Returns all media queries parsed by the rendering engine.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetMediaQueriesResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetMediaQueriesResponse> GetMediaQueriesAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.getMediaQueries", dict);
            return methodResult.DeserializeJson<GetMediaQueriesResponse>();
        }

        partial void ValidateGetPlatformFontsForNode(int nodeId);
        /// <summary>
        /// Requests information about platform fonts which we used to render child TextNodes in the given
        /// node.
        /// </summary>
        /// <param name = "nodeId">nodeId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetPlatformFontsForNodeResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetPlatformFontsForNodeResponse> GetPlatformFontsForNodeAsync(int nodeId)
        {
            ValidateGetPlatformFontsForNode(nodeId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.getPlatformFontsForNode", dict);
            return methodResult.DeserializeJson<GetPlatformFontsForNodeResponse>();
        }

        partial void ValidateGetStyleSheetText(string styleSheetId);
        /// <summary>
        /// Returns the current textual content for a stylesheet.
        /// </summary>
        /// <param name = "styleSheetId">styleSheetId</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetStyleSheetTextResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetStyleSheetTextResponse> GetStyleSheetTextAsync(string styleSheetId)
        {
            ValidateGetStyleSheetText(styleSheetId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("styleSheetId", styleSheetId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.getStyleSheetText", dict);
            return methodResult.DeserializeJson<GetStyleSheetTextResponse>();
        }

        partial void ValidateSetEffectivePropertyValueForNode(int nodeId, string propertyName, string value);
        /// <summary>
        /// Find a rule with the given active property for the given node and set the new value for this
        /// property
        /// </summary>
        /// <param name = "nodeId">The element id for which to set property.</param>
        /// <param name = "propertyName">propertyName</param>
        /// <param name = "value">value</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetEffectivePropertyValueForNodeAsync(int nodeId, string propertyName, string value)
        {
            ValidateSetEffectivePropertyValueForNode(nodeId, propertyName, value);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            dict.Add("propertyName", propertyName);
            dict.Add("value", value);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.setEffectivePropertyValueForNode", dict);
            return methodResult;
        }

        partial void ValidateSetKeyframeKey(string styleSheetId, CefSharp.DevTools.CSS.SourceRange range, string keyText);
        /// <summary>
        /// Modifies the keyframe rule key text.
        /// </summary>
        /// <param name = "styleSheetId">styleSheetId</param>
        /// <param name = "range">range</param>
        /// <param name = "keyText">keyText</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;SetKeyframeKeyResponse&gt;</returns>
        public async System.Threading.Tasks.Task<SetKeyframeKeyResponse> SetKeyframeKeyAsync(string styleSheetId, CefSharp.DevTools.CSS.SourceRange range, string keyText)
        {
            ValidateSetKeyframeKey(styleSheetId, range, keyText);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("styleSheetId", styleSheetId);
            dict.Add("range", range.ToDictionary());
            dict.Add("keyText", keyText);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.setKeyframeKey", dict);
            return methodResult.DeserializeJson<SetKeyframeKeyResponse>();
        }

        partial void ValidateSetMediaText(string styleSheetId, CefSharp.DevTools.CSS.SourceRange range, string text);
        /// <summary>
        /// Modifies the rule selector.
        /// </summary>
        /// <param name = "styleSheetId">styleSheetId</param>
        /// <param name = "range">range</param>
        /// <param name = "text">text</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;SetMediaTextResponse&gt;</returns>
        public async System.Threading.Tasks.Task<SetMediaTextResponse> SetMediaTextAsync(string styleSheetId, CefSharp.DevTools.CSS.SourceRange range, string text)
        {
            ValidateSetMediaText(styleSheetId, range, text);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("styleSheetId", styleSheetId);
            dict.Add("range", range.ToDictionary());
            dict.Add("text", text);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.setMediaText", dict);
            return methodResult.DeserializeJson<SetMediaTextResponse>();
        }

        partial void ValidateSetRuleSelector(string styleSheetId, CefSharp.DevTools.CSS.SourceRange range, string selector);
        /// <summary>
        /// Modifies the rule selector.
        /// </summary>
        /// <param name = "styleSheetId">styleSheetId</param>
        /// <param name = "range">range</param>
        /// <param name = "selector">selector</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;SetRuleSelectorResponse&gt;</returns>
        public async System.Threading.Tasks.Task<SetRuleSelectorResponse> SetRuleSelectorAsync(string styleSheetId, CefSharp.DevTools.CSS.SourceRange range, string selector)
        {
            ValidateSetRuleSelector(styleSheetId, range, selector);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("styleSheetId", styleSheetId);
            dict.Add("range", range.ToDictionary());
            dict.Add("selector", selector);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.setRuleSelector", dict);
            return methodResult.DeserializeJson<SetRuleSelectorResponse>();
        }

        partial void ValidateSetStyleSheetText(string styleSheetId, string text);
        /// <summary>
        /// Sets the new stylesheet text.
        /// </summary>
        /// <param name = "styleSheetId">styleSheetId</param>
        /// <param name = "text">text</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;SetStyleSheetTextResponse&gt;</returns>
        public async System.Threading.Tasks.Task<SetStyleSheetTextResponse> SetStyleSheetTextAsync(string styleSheetId, string text)
        {
            ValidateSetStyleSheetText(styleSheetId, text);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("styleSheetId", styleSheetId);
            dict.Add("text", text);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.setStyleSheetText", dict);
            return methodResult.DeserializeJson<SetStyleSheetTextResponse>();
        }

        partial void ValidateSetStyleTexts(System.Collections.Generic.IList<CefSharp.DevTools.CSS.StyleDeclarationEdit> edits);
        /// <summary>
        /// Applies specified style edits one after another in the given order.
        /// </summary>
        /// <param name = "edits">edits</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;SetStyleTextsResponse&gt;</returns>
        public async System.Threading.Tasks.Task<SetStyleTextsResponse> SetStyleTextsAsync(System.Collections.Generic.IList<CefSharp.DevTools.CSS.StyleDeclarationEdit> edits)
        {
            ValidateSetStyleTexts(edits);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("edits", edits.Select(x => x.ToDictionary()));
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.setStyleTexts", dict);
            return methodResult.DeserializeJson<SetStyleTextsResponse>();
        }

        /// <summary>
        /// Enables the selector recording.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
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
        /// <returns>returns System.Threading.Tasks.Task&lt;StopRuleUsageTrackingResponse&gt;</returns>
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
        /// <returns>returns System.Threading.Tasks.Task&lt;TakeCoverageDeltaResponse&gt;</returns>
        public async System.Threading.Tasks.Task<TakeCoverageDeltaResponse> TakeCoverageDeltaAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("CSS.takeCoverageDelta", dict);
            return methodResult.DeserializeJson<TakeCoverageDeltaResponse>();
        }
    }
}