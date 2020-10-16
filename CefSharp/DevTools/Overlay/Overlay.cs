// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Overlay
{
    using System.Linq;

    /// <summary>
    /// This domain provides various functionality related to drawing atop the inspected page.
    /// </summary>
    public partial class Overlay : DevToolsDomainBase
    {
        private CefSharp.DevTools.IDevToolsClient _client;
        public Overlay(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        /// <summary>
        /// Disables domain notifications.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> DisableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.disable", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables domain notifications.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> EnableAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.enable", dict);
            return methodResult;
        }

        partial void ValidateGetHighlightObjectForTest(int nodeId, bool? includeDistance = null, bool? includeStyle = null, CefSharp.DevTools.Overlay.ColorFormat? colorFormat = null, bool? showAccessibilityInfo = null);
        /// <summary>
        /// For testing.
        /// </summary>
        /// <param name = "nodeId">Id of the node to get highlight object for.</param>
        /// <param name = "includeDistance">Whether to include distance info.</param>
        /// <param name = "includeStyle">Whether to include style info.</param>
        /// <param name = "colorFormat">The color format to get config with (default: hex).</param>
        /// <param name = "showAccessibilityInfo">Whether to show accessibility info (default: true).</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;GetHighlightObjectForTestResponse&gt;</returns>
        public async System.Threading.Tasks.Task<GetHighlightObjectForTestResponse> GetHighlightObjectForTestAsync(int nodeId, bool? includeDistance = null, bool? includeStyle = null, CefSharp.DevTools.Overlay.ColorFormat? colorFormat = null, bool? showAccessibilityInfo = null)
        {
            ValidateGetHighlightObjectForTest(nodeId, includeDistance, includeStyle, colorFormat, showAccessibilityInfo);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("nodeId", nodeId);
            if (includeDistance.HasValue)
            {
                dict.Add("includeDistance", includeDistance.Value);
            }

            if (includeStyle.HasValue)
            {
                dict.Add("includeStyle", includeStyle.Value);
            }

            if (colorFormat.HasValue)
            {
                dict.Add("colorFormat", this.EnumToString(colorFormat));
            }

            if (showAccessibilityInfo.HasValue)
            {
                dict.Add("showAccessibilityInfo", showAccessibilityInfo.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.getHighlightObjectForTest", dict);
            return methodResult.DeserializeJson<GetHighlightObjectForTestResponse>();
        }

        /// <summary>
        /// Hides any highlight.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> HideHighlightAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.hideHighlight", dict);
            return methodResult;
        }

        partial void ValidateHighlightFrame(string frameId, CefSharp.DevTools.DOM.RGBA contentColor = null, CefSharp.DevTools.DOM.RGBA contentOutlineColor = null);
        /// <summary>
        /// Highlights owner element of the frame with given id.
        /// </summary>
        /// <param name = "frameId">Identifier of the frame to highlight.</param>
        /// <param name = "contentColor">The content box highlight fill color (default: transparent).</param>
        /// <param name = "contentOutlineColor">The content box highlight outline color (default: transparent).</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> HighlightFrameAsync(string frameId, CefSharp.DevTools.DOM.RGBA contentColor = null, CefSharp.DevTools.DOM.RGBA contentOutlineColor = null)
        {
            ValidateHighlightFrame(frameId, contentColor, contentOutlineColor);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("frameId", frameId);
            if ((contentColor) != (null))
            {
                dict.Add("contentColor", contentColor.ToDictionary());
            }

            if ((contentOutlineColor) != (null))
            {
                dict.Add("contentOutlineColor", contentOutlineColor.ToDictionary());
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.highlightFrame", dict);
            return methodResult;
        }

        partial void ValidateHighlightNode(CefSharp.DevTools.Overlay.HighlightConfig highlightConfig, int? nodeId = null, int? backendNodeId = null, string objectId = null, string selector = null);
        /// <summary>
        /// Highlights DOM node with given id or with the given JavaScript object wrapper. Either nodeId or
        /// objectId must be specified.
        /// </summary>
        /// <param name = "highlightConfig">A descriptor for the highlight appearance.</param>
        /// <param name = "nodeId">Identifier of the node to highlight.</param>
        /// <param name = "backendNodeId">Identifier of the backend node to highlight.</param>
        /// <param name = "objectId">JavaScript object id of the node to be highlighted.</param>
        /// <param name = "selector">Selectors to highlight relevant nodes.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> HighlightNodeAsync(CefSharp.DevTools.Overlay.HighlightConfig highlightConfig, int? nodeId = null, int? backendNodeId = null, string objectId = null, string selector = null)
        {
            ValidateHighlightNode(highlightConfig, nodeId, backendNodeId, objectId, selector);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("highlightConfig", highlightConfig.ToDictionary());
            if (nodeId.HasValue)
            {
                dict.Add("nodeId", nodeId.Value);
            }

            if (backendNodeId.HasValue)
            {
                dict.Add("backendNodeId", backendNodeId.Value);
            }

            if (!(string.IsNullOrEmpty(objectId)))
            {
                dict.Add("objectId", objectId);
            }

            if (!(string.IsNullOrEmpty(selector)))
            {
                dict.Add("selector", selector);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.highlightNode", dict);
            return methodResult;
        }

        partial void ValidateHighlightQuad(long[] quad, CefSharp.DevTools.DOM.RGBA color = null, CefSharp.DevTools.DOM.RGBA outlineColor = null);
        /// <summary>
        /// Highlights given quad. Coordinates are absolute with respect to the main frame viewport.
        /// </summary>
        /// <param name = "quad">Quad to highlight</param>
        /// <param name = "color">The highlight fill color (default: transparent).</param>
        /// <param name = "outlineColor">The highlight outline color (default: transparent).</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> HighlightQuadAsync(long[] quad, CefSharp.DevTools.DOM.RGBA color = null, CefSharp.DevTools.DOM.RGBA outlineColor = null)
        {
            ValidateHighlightQuad(quad, color, outlineColor);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("quad", quad);
            if ((color) != (null))
            {
                dict.Add("color", color.ToDictionary());
            }

            if ((outlineColor) != (null))
            {
                dict.Add("outlineColor", outlineColor.ToDictionary());
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.highlightQuad", dict);
            return methodResult;
        }

        partial void ValidateHighlightRect(int x, int y, int width, int height, CefSharp.DevTools.DOM.RGBA color = null, CefSharp.DevTools.DOM.RGBA outlineColor = null);
        /// <summary>
        /// Highlights given rectangle. Coordinates are absolute with respect to the main frame viewport.
        /// </summary>
        /// <param name = "x">X coordinate</param>
        /// <param name = "y">Y coordinate</param>
        /// <param name = "width">Rectangle width</param>
        /// <param name = "height">Rectangle height</param>
        /// <param name = "color">The highlight fill color (default: transparent).</param>
        /// <param name = "outlineColor">The highlight outline color (default: transparent).</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> HighlightRectAsync(int x, int y, int width, int height, CefSharp.DevTools.DOM.RGBA color = null, CefSharp.DevTools.DOM.RGBA outlineColor = null)
        {
            ValidateHighlightRect(x, y, width, height, color, outlineColor);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("x", x);
            dict.Add("y", y);
            dict.Add("width", width);
            dict.Add("height", height);
            if ((color) != (null))
            {
                dict.Add("color", color.ToDictionary());
            }

            if ((outlineColor) != (null))
            {
                dict.Add("outlineColor", outlineColor.ToDictionary());
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.highlightRect", dict);
            return methodResult;
        }

        partial void ValidateSetInspectMode(CefSharp.DevTools.Overlay.InspectMode mode, CefSharp.DevTools.Overlay.HighlightConfig highlightConfig = null);
        /// <summary>
        /// Enters the 'inspect' mode. In this mode, elements that user is hovering over are highlighted.
        /// Backend then generates 'inspectNodeRequested' event upon element selection.
        /// </summary>
        /// <param name = "mode">Set an inspection mode.</param>
        /// <param name = "highlightConfig">A descriptor for the highlight appearance of hovered-over nodes. May be omitted if `enabled== false`.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetInspectModeAsync(CefSharp.DevTools.Overlay.InspectMode mode, CefSharp.DevTools.Overlay.HighlightConfig highlightConfig = null)
        {
            ValidateSetInspectMode(mode, highlightConfig);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("mode", this.EnumToString(mode));
            if ((highlightConfig) != (null))
            {
                dict.Add("highlightConfig", highlightConfig.ToDictionary());
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setInspectMode", dict);
            return methodResult;
        }

        partial void ValidateSetShowAdHighlights(bool show);
        /// <summary>
        /// Highlights owner element of all frames detected to be ads.
        /// </summary>
        /// <param name = "show">True for showing ad highlights</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetShowAdHighlightsAsync(bool show)
        {
            ValidateSetShowAdHighlights(show);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("show", show);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setShowAdHighlights", dict);
            return methodResult;
        }

        partial void ValidateSetPausedInDebuggerMessage(string message = null);
        /// <summary>
        /// SetPausedInDebuggerMessage
        /// </summary>
        /// <param name = "message">The message to display, also triggers resume and step over controls.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetPausedInDebuggerMessageAsync(string message = null)
        {
            ValidateSetPausedInDebuggerMessage(message);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(message)))
            {
                dict.Add("message", message);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setPausedInDebuggerMessage", dict);
            return methodResult;
        }

        partial void ValidateSetShowDebugBorders(bool show);
        /// <summary>
        /// Requests that backend shows debug borders on layers
        /// </summary>
        /// <param name = "show">True for showing debug borders</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetShowDebugBordersAsync(bool show)
        {
            ValidateSetShowDebugBorders(show);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("show", show);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setShowDebugBorders", dict);
            return methodResult;
        }

        partial void ValidateSetShowFPSCounter(bool show);
        /// <summary>
        /// Requests that backend shows the FPS counter
        /// </summary>
        /// <param name = "show">True for showing the FPS counter</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetShowFPSCounterAsync(bool show)
        {
            ValidateSetShowFPSCounter(show);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("show", show);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setShowFPSCounter", dict);
            return methodResult;
        }

        partial void ValidateSetShowPaintRects(bool result);
        /// <summary>
        /// Requests that backend shows paint rectangles
        /// </summary>
        /// <param name = "result">True for showing paint rectangles</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetShowPaintRectsAsync(bool result)
        {
            ValidateSetShowPaintRects(result);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("result", result);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setShowPaintRects", dict);
            return methodResult;
        }

        partial void ValidateSetShowLayoutShiftRegions(bool result);
        /// <summary>
        /// Requests that backend shows layout shift regions
        /// </summary>
        /// <param name = "result">True for showing layout shift regions</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetShowLayoutShiftRegionsAsync(bool result)
        {
            ValidateSetShowLayoutShiftRegions(result);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("result", result);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setShowLayoutShiftRegions", dict);
            return methodResult;
        }

        partial void ValidateSetShowScrollBottleneckRects(bool show);
        /// <summary>
        /// Requests that backend shows scroll bottleneck rects
        /// </summary>
        /// <param name = "show">True for showing scroll bottleneck rects</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetShowScrollBottleneckRectsAsync(bool show)
        {
            ValidateSetShowScrollBottleneckRects(show);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("show", show);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setShowScrollBottleneckRects", dict);
            return methodResult;
        }

        partial void ValidateSetShowHitTestBorders(bool show);
        /// <summary>
        /// Requests that backend shows hit-test borders on layers
        /// </summary>
        /// <param name = "show">True for showing hit-test borders</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetShowHitTestBordersAsync(bool show)
        {
            ValidateSetShowHitTestBorders(show);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("show", show);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setShowHitTestBorders", dict);
            return methodResult;
        }

        partial void ValidateSetShowViewportSizeOnResize(bool show);
        /// <summary>
        /// Paints viewport size upon main frame resize.
        /// </summary>
        /// <param name = "show">Whether to paint size or not.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetShowViewportSizeOnResizeAsync(bool show)
        {
            ValidateSetShowViewportSizeOnResize(show);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("show", show);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setShowViewportSizeOnResize", dict);
            return methodResult;
        }

        partial void ValidateSetShowHinge(CefSharp.DevTools.Overlay.HingeConfig hingeConfig = null);
        /// <summary>
        /// Add a dual screen device hinge
        /// </summary>
        /// <param name = "hingeConfig">hinge data, null means hideHinge</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetShowHingeAsync(CefSharp.DevTools.Overlay.HingeConfig hingeConfig = null)
        {
            ValidateSetShowHinge(hingeConfig);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if ((hingeConfig) != (null))
            {
                dict.Add("hingeConfig", hingeConfig.ToDictionary());
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Overlay.setShowHinge", dict);
            return methodResult;
        }
    }
}