// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Emulation
{
    /// <summary>
    /// This domain emulates different environments for the page.
    /// </summary>
    public partial class Emulation : DevToolsDomainBase
    {
        public Emulation(CefSharp.DevTools.DevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.DevToolsClient _client;
        /// <summary>
        /// Tells whether emulation is supported.
        /// </summary>
        public async System.Threading.Tasks.Task<CanEmulateResponse> CanEmulateAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.canEmulate", dict);
            return result.DeserializeJson<CanEmulateResponse>();
        }

        /// <summary>
        /// Clears the overriden device metrics.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ClearDeviceMetricsOverrideAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.clearDeviceMetricsOverride", dict);
            return result;
        }

        /// <summary>
        /// Clears the overriden Geolocation Position and Error.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> ClearGeolocationOverrideAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.clearGeolocationOverride", dict);
            return result;
        }

        /// <summary>
        /// Sets or clears an override of the default background color of the frame. This override is used
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetDefaultBackgroundColorOverrideAsync(DOM.RGBA color = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if ((color) != (null))
            {
                dict.Add("color", color);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.setDefaultBackgroundColorOverride", dict);
            return result;
        }

        /// <summary>
        /// Overrides the values of device screen dimensions (window.screen.width, window.screen.height,
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetDeviceMetricsOverrideAsync(int width, int height, long deviceScaleFactor, bool mobile, long? scale = null, int? screenWidth = null, int? screenHeight = null, int? positionX = null, int? positionY = null, bool? dontSetVisibleSize = null, ScreenOrientation screenOrientation = null, Page.Viewport viewport = null, DisplayFeature displayFeature = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("width", width);
            dict.Add("height", height);
            dict.Add("deviceScaleFactor", deviceScaleFactor);
            dict.Add("mobile", mobile);
            if (scale.HasValue)
            {
                dict.Add("scale", scale.Value);
            }

            if (screenWidth.HasValue)
            {
                dict.Add("screenWidth", screenWidth.Value);
            }

            if (screenHeight.HasValue)
            {
                dict.Add("screenHeight", screenHeight.Value);
            }

            if (positionX.HasValue)
            {
                dict.Add("positionX", positionX.Value);
            }

            if (positionY.HasValue)
            {
                dict.Add("positionY", positionY.Value);
            }

            if (dontSetVisibleSize.HasValue)
            {
                dict.Add("dontSetVisibleSize", dontSetVisibleSize.Value);
            }

            if ((screenOrientation) != (null))
            {
                dict.Add("screenOrientation", screenOrientation);
            }

            if ((viewport) != (null))
            {
                dict.Add("viewport", viewport);
            }

            if ((displayFeature) != (null))
            {
                dict.Add("displayFeature", displayFeature);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.setDeviceMetricsOverride", dict);
            return result;
        }

        /// <summary>
        /// Emulates the given media type or media feature for CSS media queries.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetEmulatedMediaAsync(string media = null, System.Collections.Generic.IList<MediaFeature> features = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(media)))
            {
                dict.Add("media", media);
            }

            if ((features) != (null))
            {
                dict.Add("features", features);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.setEmulatedMedia", dict);
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

            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.setGeolocationOverride", dict);
            return result;
        }

        /// <summary>
        /// Switches script execution in the page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetScriptExecutionDisabledAsync(bool value)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("value", value);
            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.setScriptExecutionDisabled", dict);
            return result;
        }

        /// <summary>
        /// Enables touch on platforms which do not support them.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetTouchEmulationEnabledAsync(bool enabled, int? maxTouchPoints = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enabled", enabled);
            if (maxTouchPoints.HasValue)
            {
                dict.Add("maxTouchPoints", maxTouchPoints.Value);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.setTouchEmulationEnabled", dict);
            return result;
        }

        /// <summary>
        /// Allows overriding user agent with the given string.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResult> SetUserAgentOverrideAsync(string userAgent, string acceptLanguage = null, string platform = null, UserAgentMetadata userAgentMetadata = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("userAgent", userAgent);
            if (!(string.IsNullOrEmpty(acceptLanguage)))
            {
                dict.Add("acceptLanguage", acceptLanguage);
            }

            if (!(string.IsNullOrEmpty(platform)))
            {
                dict.Add("platform", platform);
            }

            if ((userAgentMetadata) != (null))
            {
                dict.Add("userAgentMetadata", userAgentMetadata);
            }

            var result = await _client.ExecuteDevToolsMethodAsync("Emulation.setUserAgentOverride", dict);
            return result;
        }
    }
}