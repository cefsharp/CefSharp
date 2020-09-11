// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.
namespace CefSharp.DevTools.Emulation
{
    using System.Linq;

    /// <summary>
    /// This domain emulates different environments for the page.
    /// </summary>
    public partial class Emulation : DevToolsDomainBase
    {
        public Emulation(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        private CefSharp.DevTools.IDevToolsClient _client;
        /// <summary>
        /// Tells whether emulation is supported.
        /// </summary>
        public async System.Threading.Tasks.Task<CanEmulateResponse> CanEmulateAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.canEmulate", dict);
            return methodResult.DeserializeJson<CanEmulateResponse>();
        }

        /// <summary>
        /// Clears the overriden device metrics.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearDeviceMetricsOverrideAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.clearDeviceMetricsOverride", dict);
            return methodResult;
        }

        /// <summary>
        /// Clears the overriden Geolocation Position and Error.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearGeolocationOverrideAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.clearGeolocationOverride", dict);
            return methodResult;
        }

        /// <summary>
        /// Requests that page scale factor is reset to initial values.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ResetPageScaleFactorAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.resetPageScaleFactor", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables or disables simulating a focused and active page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetFocusEmulationEnabledAsync(bool enabled)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enabled", enabled);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setFocusEmulationEnabled", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables CPU throttling to emulate slow CPUs.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetCPUThrottlingRateAsync(long rate)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("rate", rate);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setCPUThrottlingRate", dict);
            return methodResult;
        }

        /// <summary>
        /// Sets or clears an override of the default background color of the frame. This override is used
        /// if the content does not specify one.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDefaultBackgroundColorOverrideAsync(CefSharp.DevTools.DOM.RGBA color = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if ((color) != (null))
            {
                dict.Add("color", color.ToDictionary());
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setDefaultBackgroundColorOverride", dict);
            return methodResult;
        }

        /// <summary>
        /// Overrides the values of device screen dimensions (window.screen.width, window.screen.height,
        /// window.innerWidth, window.innerHeight, and "device-width"/"device-height"-related CSS media
        /// query results).
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDeviceMetricsOverrideAsync(int width, int height, long deviceScaleFactor, bool mobile, long? scale = null, int? screenWidth = null, int? screenHeight = null, int? positionX = null, int? positionY = null, bool? dontSetVisibleSize = null, CefSharp.DevTools.Emulation.ScreenOrientation screenOrientation = null, CefSharp.DevTools.Page.Viewport viewport = null)
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
                dict.Add("screenOrientation", screenOrientation.ToDictionary());
            }

            if ((viewport) != (null))
            {
                dict.Add("viewport", viewport.ToDictionary());
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setDeviceMetricsOverride", dict);
            return methodResult;
        }

        /// <summary>
        /// SetScrollbarsHidden
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetScrollbarsHiddenAsync(bool hidden)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("hidden", hidden);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setScrollbarsHidden", dict);
            return methodResult;
        }

        /// <summary>
        /// SetDocumentCookieDisabled
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDocumentCookieDisabledAsync(bool disabled)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("disabled", disabled);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setDocumentCookieDisabled", dict);
            return methodResult;
        }

        /// <summary>
        /// SetEmitTouchEventsForMouse
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetEmitTouchEventsForMouseAsync(bool enabled, string configuration = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enabled", enabled);
            if (!(string.IsNullOrEmpty(configuration)))
            {
                dict.Add("configuration", configuration);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setEmitTouchEventsForMouse", dict);
            return methodResult;
        }

        /// <summary>
        /// Emulates the given media type or media feature for CSS media queries.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetEmulatedMediaAsync(string media = null, System.Collections.Generic.IList<CefSharp.DevTools.Emulation.MediaFeature> features = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(media)))
            {
                dict.Add("media", media);
            }

            if ((features) != (null))
            {
                dict.Add("features", features.Select(x => x.ToDictionary()));
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setEmulatedMedia", dict);
            return methodResult;
        }

        /// <summary>
        /// Emulates the given vision deficiency.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetEmulatedVisionDeficiencyAsync(string type)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("type", type);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setEmulatedVisionDeficiency", dict);
            return methodResult;
        }

        /// <summary>
        /// Overrides the Geolocation Position or Error. Omitting any of the parameters emulates position
        /// unavailable.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetGeolocationOverrideAsync(long? latitude = null, long? longitude = null, long? accuracy = null)
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

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setGeolocationOverride", dict);
            return methodResult;
        }

        /// <summary>
        /// Sets a specified page scale factor.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetPageScaleFactorAsync(long pageScaleFactor)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("pageScaleFactor", pageScaleFactor);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setPageScaleFactor", dict);
            return methodResult;
        }

        /// <summary>
        /// Switches script execution in the page.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetScriptExecutionDisabledAsync(bool value)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("value", value);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setScriptExecutionDisabled", dict);
            return methodResult;
        }

        /// <summary>
        /// Enables touch on platforms which do not support them.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetTouchEmulationEnabledAsync(bool enabled, int? maxTouchPoints = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enabled", enabled);
            if (maxTouchPoints.HasValue)
            {
                dict.Add("maxTouchPoints", maxTouchPoints.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setTouchEmulationEnabled", dict);
            return methodResult;
        }

        /// <summary>
        /// Turns on virtual time for all frames (replacing real-time with a synthetic time source) and sets
        /// the current virtual time policy.  Note this supersedes any previous time budget.
        /// </summary>
        public async System.Threading.Tasks.Task<SetVirtualTimePolicyResponse> SetVirtualTimePolicyAsync(CefSharp.DevTools.Emulation.VirtualTimePolicy policy, long? budget = null, int? maxVirtualTimeTaskStarvationCount = null, bool? waitForNavigation = null, long? initialVirtualTime = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("policy", this.EnumToString(policy));
            if (budget.HasValue)
            {
                dict.Add("budget", budget.Value);
            }

            if (maxVirtualTimeTaskStarvationCount.HasValue)
            {
                dict.Add("maxVirtualTimeTaskStarvationCount", maxVirtualTimeTaskStarvationCount.Value);
            }

            if (waitForNavigation.HasValue)
            {
                dict.Add("waitForNavigation", waitForNavigation.Value);
            }

            if (initialVirtualTime.HasValue)
            {
                dict.Add("initialVirtualTime", initialVirtualTime.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setVirtualTimePolicy", dict);
            return methodResult.DeserializeJson<SetVirtualTimePolicyResponse>();
        }

        /// <summary>
        /// Overrides default host system locale with the specified one.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetLocaleOverrideAsync(string locale = null)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(locale)))
            {
                dict.Add("locale", locale);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setLocaleOverride", dict);
            return methodResult;
        }

        /// <summary>
        /// Overrides default host system timezone with the specified one.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetTimezoneOverrideAsync(string timezoneId)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("timezoneId", timezoneId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setTimezoneOverride", dict);
            return methodResult;
        }

        /// <summary>
        /// Allows overriding user agent with the given string.
        /// </summary>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetUserAgentOverrideAsync(string userAgent, string acceptLanguage = null, string platform = null, CefSharp.DevTools.Emulation.UserAgentMetadata userAgentMetadata = null)
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
                dict.Add("userAgentMetadata", userAgentMetadata.ToDictionary());
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setUserAgentOverride", dict);
            return methodResult;
        }
    }
}