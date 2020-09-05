// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Input
{
    using System.Linq;

    /// <summary>
    /// Input
    /// </summary>
    public partial class Input : DevToolsDomainBase
    {
        public Input(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Dispatches a key event to the page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DispatchKeyEventAsync(string type, int? modifiers = null, long? timestamp = null, string text = null, string unmodifiedText = null, string keyIdentifier = null, string code = null, string key = null, int? windowsVirtualKeyCode = null, int? nativeVirtualKeyCode = null, bool? autoRepeat = null, bool? isKeypad = null, bool? isSystemKey = null, int? location = null, string commands = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("type", type);
            if (modifiers.HasValue)
            {
                dict.Add("modifiers", modifiers.Value);
            }

            if (timestamp.HasValue)
            {
                dict.Add("timestamp", timestamp.Value);
            }

            if (!(string.IsNullOrEmpty(text)))
            {
                dict.Add("text", text);
            }

            if (!(string.IsNullOrEmpty(unmodifiedText)))
            {
                dict.Add("unmodifiedText", unmodifiedText);
            }

            if (!(string.IsNullOrEmpty(keyIdentifier)))
            {
                dict.Add("keyIdentifier", keyIdentifier);
            }

            if (!(string.IsNullOrEmpty(code)))
            {
                dict.Add("code", code);
            }

            if (!(string.IsNullOrEmpty(key)))
            {
                dict.Add("key", key);
            }

            if (windowsVirtualKeyCode.HasValue)
            {
                dict.Add("windowsVirtualKeyCode", windowsVirtualKeyCode.Value);
            }

            if (nativeVirtualKeyCode.HasValue)
            {
                dict.Add("nativeVirtualKeyCode", nativeVirtualKeyCode.Value);
            }

            if (autoRepeat.HasValue)
            {
                dict.Add("autoRepeat", autoRepeat.Value);
            }

            if (isKeypad.HasValue)
            {
                dict.Add("isKeypad", isKeypad.Value);
            }

            if (isSystemKey.HasValue)
            {
                dict.Add("isSystemKey", isSystemKey.Value);
            }

            if (location.HasValue)
            {
                dict.Add("location", location.Value);
            }

            if (!(string.IsNullOrEmpty(commands)))
            {
                dict.Add("commands", commands);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Input.dispatchKeyEvent", dict);
            return result;
        }

        /// <summary>
        /// Dispatches a mouse event to the page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DispatchMouseEventAsync(string type, long x, long y, int? modifiers = null, long? timestamp = null, string button = null, int? buttons = null, int? clickCount = null, long? deltaX = null, long? deltaY = null, string pointerType = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("type", type);
            dict.Add("x", x);
            dict.Add("y", y);
            if (modifiers.HasValue)
            {
                dict.Add("modifiers", modifiers.Value);
            }

            if (timestamp.HasValue)
            {
                dict.Add("timestamp", timestamp.Value);
            }

            if (!(string.IsNullOrEmpty(button)))
            {
                dict.Add("button", button);
            }

            if (buttons.HasValue)
            {
                dict.Add("buttons", buttons.Value);
            }

            if (clickCount.HasValue)
            {
                dict.Add("clickCount", clickCount.Value);
            }

            if (deltaX.HasValue)
            {
                dict.Add("deltaX", deltaX.Value);
            }

            if (deltaY.HasValue)
            {
                dict.Add("deltaY", deltaY.Value);
            }

            if (!(string.IsNullOrEmpty(pointerType)))
            {
                dict.Add("pointerType", pointerType);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Input.dispatchMouseEvent", dict);
            return result;
        }

        /// <summary>
        /// Dispatches a touch event to the page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> DispatchTouchEventAsync(string type, System.Collections.Generic.IList<TouchPoint> touchPoints, int? modifiers = null, long? timestamp = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("type", type);
            dict.Add("touchPoints", touchPoints.Select(x => x.ToDictionary()));
            if (modifiers.HasValue)
            {
                dict.Add("modifiers", modifiers.Value);
            }

            if (timestamp.HasValue)
            {
                dict.Add("timestamp", timestamp.Value);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Input.dispatchTouchEvent", dict);
            return result;
        }

        /// <summary>
        /// Ignores input events (useful while auditing page).
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetIgnoreInputEventsAsync(bool ignore)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("ignore", ignore);
            var result = await _client.ExecuteDevToolsMethodAsync("Input.setIgnoreInputEvents", dict);
            return result;
        }
    }
}