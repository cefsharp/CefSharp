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
        private CefSharp.DevTools.IDevToolsClient _client;
        public Emulation(CefSharp.DevTools.IDevToolsClient client)
        {
            _client = (client);
        }

        /// <summary>
        /// Tells whether emulation is supported.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;CanEmulateResponse&gt;</returns>
        public async System.Threading.Tasks.Task<CanEmulateResponse> CanEmulateAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.canEmulate", dict);
            return methodResult.DeserializeJson<CanEmulateResponse>();
        }

        /// <summary>
        /// Clears the overriden device metrics.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearDeviceMetricsOverrideAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.clearDeviceMetricsOverride", dict);
            return methodResult;
        }

        /// <summary>
        /// Clears the overriden Geolocation Position and Error.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ClearGeolocationOverrideAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.clearGeolocationOverride", dict);
            return methodResult;
        }

        /// <summary>
        /// Requests that page scale factor is reset to initial values.
        /// </summary>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> ResetPageScaleFactorAsync()
        {
            System.Collections.Generic.Dictionary<string, object> dict = null;
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.resetPageScaleFactor", dict);
            return methodResult;
        }

        partial void ValidateSetFocusEmulationEnabled(bool enabled);
        /// <summary>
        /// Enables or disables simulating a focused and active page.
        /// </summary>
        /// <param name = "enabled">Whether to enable to disable focus emulation.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetFocusEmulationEnabledAsync(bool enabled)
        {
            ValidateSetFocusEmulationEnabled(enabled);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enabled", enabled);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setFocusEmulationEnabled", dict);
            return methodResult;
        }

        partial void ValidateSetCPUThrottlingRate(long rate);
        /// <summary>
        /// Enables CPU throttling to emulate slow CPUs.
        /// </summary>
        /// <param name = "rate">Throttling rate as a slowdown factor (1 is no throttle, 2 is 2x slowdown, etc).</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetCPUThrottlingRateAsync(long rate)
        {
            ValidateSetCPUThrottlingRate(rate);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("rate", rate);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setCPUThrottlingRate", dict);
            return methodResult;
        }

        partial void ValidateSetDefaultBackgroundColorOverride(CefSharp.DevTools.DOM.RGBA color = null);
        /// <summary>
        /// Sets or clears an override of the default background color of the frame. This override is used
        /// if the content does not specify one.
        /// </summary>
        /// <param name = "color">RGBA of the default background color. If not specified, any existing override will becleared.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDefaultBackgroundColorOverrideAsync(CefSharp.DevTools.DOM.RGBA color = null)
        {
            ValidateSetDefaultBackgroundColorOverride(color);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if ((color) != (null))
            {
                dict.Add("color", color.ToDictionary());
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setDefaultBackgroundColorOverride", dict);
            return methodResult;
        }

        partial void ValidateSetDeviceMetricsOverride(int width, int height, long deviceScaleFactor, bool mobile, long? scale = null, int? screenWidth = null, int? screenHeight = null, int? positionX = null, int? positionY = null, bool? dontSetVisibleSize = null, CefSharp.DevTools.Emulation.ScreenOrientation screenOrientation = null, CefSharp.DevTools.Page.Viewport viewport = null);
        /// <summary>
        /// Overrides the values of device screen dimensions (window.screen.width, window.screen.height,
        /// window.innerWidth, window.innerHeight, and "device-width"/"device-height"-related CSS media
        /// query results).
        /// </summary>
        /// <param name = "width">Overriding width value in pixels (minimum 0, maximum 10000000). 0 disables the override.</param>
        /// <param name = "height">Overriding height value in pixels (minimum 0, maximum 10000000). 0 disables the override.</param>
        /// <param name = "deviceScaleFactor">Overriding device scale factor value. 0 disables the override.</param>
        /// <param name = "mobile">Whether to emulate mobile device. This includes viewport meta tag, overlay scrollbars, textautosizing and more.</param>
        /// <param name = "scale">Scale to apply to resulting view image.</param>
        /// <param name = "screenWidth">Overriding screen width value in pixels (minimum 0, maximum 10000000).</param>
        /// <param name = "screenHeight">Overriding screen height value in pixels (minimum 0, maximum 10000000).</param>
        /// <param name = "positionX">Overriding view X position on screen in pixels (minimum 0, maximum 10000000).</param>
        /// <param name = "positionY">Overriding view Y position on screen in pixels (minimum 0, maximum 10000000).</param>
        /// <param name = "dontSetVisibleSize">Do not set visible view size, rely upon explicit setVisibleSize call.</param>
        /// <param name = "screenOrientation">Screen orientation override.</param>
        /// <param name = "viewport">If set, the visible area of the page will be overridden to this viewport. This viewportchange is not observed by the page, e.g. viewport-relative elements do not change positions.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDeviceMetricsOverrideAsync(int width, int height, long deviceScaleFactor, bool mobile, long? scale = null, int? screenWidth = null, int? screenHeight = null, int? positionX = null, int? positionY = null, bool? dontSetVisibleSize = null, CefSharp.DevTools.Emulation.ScreenOrientation screenOrientation = null, CefSharp.DevTools.Page.Viewport viewport = null)
        {
            ValidateSetDeviceMetricsOverride(width, height, deviceScaleFactor, mobile, scale, screenWidth, screenHeight, positionX, positionY, dontSetVisibleSize, screenOrientation, viewport);
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

        partial void ValidateSetScrollbarsHidden(bool hidden);
        /// <summary>
        /// SetScrollbarsHidden
        /// </summary>
        /// <param name = "hidden">Whether scrollbars should be always hidden.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetScrollbarsHiddenAsync(bool hidden)
        {
            ValidateSetScrollbarsHidden(hidden);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("hidden", hidden);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setScrollbarsHidden", dict);
            return methodResult;
        }

        partial void ValidateSetDocumentCookieDisabled(bool disabled);
        /// <summary>
        /// SetDocumentCookieDisabled
        /// </summary>
        /// <param name = "disabled">Whether document.coookie API should be disabled.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetDocumentCookieDisabledAsync(bool disabled)
        {
            ValidateSetDocumentCookieDisabled(disabled);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("disabled", disabled);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setDocumentCookieDisabled", dict);
            return methodResult;
        }

        partial void ValidateSetEmitTouchEventsForMouse(bool enabled, string configuration = null);
        /// <summary>
        /// SetEmitTouchEventsForMouse
        /// </summary>
        /// <param name = "enabled">Whether touch emulation based on mouse input should be enabled.</param>
        /// <param name = "configuration">Touch/gesture events configuration. Default: current platform.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetEmitTouchEventsForMouseAsync(bool enabled, string configuration = null)
        {
            ValidateSetEmitTouchEventsForMouse(enabled, configuration);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enabled", enabled);
            if (!(string.IsNullOrEmpty(configuration)))
            {
                dict.Add("configuration", configuration);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setEmitTouchEventsForMouse", dict);
            return methodResult;
        }

        partial void ValidateSetEmulatedMedia(string media = null, System.Collections.Generic.IList<CefSharp.DevTools.Emulation.MediaFeature> features = null);
        /// <summary>
        /// Emulates the given media type or media feature for CSS media queries.
        /// </summary>
        /// <param name = "media">Media type to emulate. Empty string disables the override.</param>
        /// <param name = "features">Media features to emulate.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetEmulatedMediaAsync(string media = null, System.Collections.Generic.IList<CefSharp.DevTools.Emulation.MediaFeature> features = null)
        {
            ValidateSetEmulatedMedia(media, features);
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

        partial void ValidateSetEmulatedVisionDeficiency(string type);
        /// <summary>
        /// Emulates the given vision deficiency.
        /// </summary>
        /// <param name = "type">Vision deficiency to emulate.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetEmulatedVisionDeficiencyAsync(string type)
        {
            ValidateSetEmulatedVisionDeficiency(type);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("type", type);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setEmulatedVisionDeficiency", dict);
            return methodResult;
        }

        partial void ValidateSetGeolocationOverride(long? latitude = null, long? longitude = null, long? accuracy = null);
        /// <summary>
        /// Overrides the Geolocation Position or Error. Omitting any of the parameters emulates position
        /// unavailable.
        /// </summary>
        /// <param name = "latitude">Mock latitude</param>
        /// <param name = "longitude">Mock longitude</param>
        /// <param name = "accuracy">Mock accuracy</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetGeolocationOverrideAsync(long? latitude = null, long? longitude = null, long? accuracy = null)
        {
            ValidateSetGeolocationOverride(latitude, longitude, accuracy);
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

        partial void ValidateSetPageScaleFactor(long pageScaleFactor);
        /// <summary>
        /// Sets a specified page scale factor.
        /// </summary>
        /// <param name = "pageScaleFactor">Page scale factor.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetPageScaleFactorAsync(long pageScaleFactor)
        {
            ValidateSetPageScaleFactor(pageScaleFactor);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("pageScaleFactor", pageScaleFactor);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setPageScaleFactor", dict);
            return methodResult;
        }

        partial void ValidateSetScriptExecutionDisabled(bool value);
        /// <summary>
        /// Switches script execution in the page.
        /// </summary>
        /// <param name = "value">Whether script execution should be disabled in the page.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetScriptExecutionDisabledAsync(bool value)
        {
            ValidateSetScriptExecutionDisabled(value);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("value", value);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setScriptExecutionDisabled", dict);
            return methodResult;
        }

        partial void ValidateSetTouchEmulationEnabled(bool enabled, int? maxTouchPoints = null);
        /// <summary>
        /// Enables touch on platforms which do not support them.
        /// </summary>
        /// <param name = "enabled">Whether the touch event emulation should be enabled.</param>
        /// <param name = "maxTouchPoints">Maximum touch points supported. Defaults to one.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetTouchEmulationEnabledAsync(bool enabled, int? maxTouchPoints = null)
        {
            ValidateSetTouchEmulationEnabled(enabled, maxTouchPoints);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("enabled", enabled);
            if (maxTouchPoints.HasValue)
            {
                dict.Add("maxTouchPoints", maxTouchPoints.Value);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setTouchEmulationEnabled", dict);
            return methodResult;
        }

        partial void ValidateSetVirtualTimePolicy(CefSharp.DevTools.Emulation.VirtualTimePolicy policy, long? budget = null, int? maxVirtualTimeTaskStarvationCount = null, bool? waitForNavigation = null, long? initialVirtualTime = null);
        /// <summary>
        /// Turns on virtual time for all frames (replacing real-time with a synthetic time source) and sets
        /// the current virtual time policy.  Note this supersedes any previous time budget.
        /// </summary>
        /// <param name = "policy">policy</param>
        /// <param name = "budget">If set, after this many virtual milliseconds have elapsed virtual time will be paused and avirtualTimeBudgetExpired event is sent.</param>
        /// <param name = "maxVirtualTimeTaskStarvationCount">If set this specifies the maximum number of tasks that can be run before virtual is forcedforwards to prevent deadlock.</param>
        /// <param name = "waitForNavigation">If set the virtual time policy change should be deferred until any frame starts navigating.Note any previous deferred policy change is superseded.</param>
        /// <param name = "initialVirtualTime">If set, base::Time::Now will be overriden to initially return this value.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;SetVirtualTimePolicyResponse&gt;</returns>
        public async System.Threading.Tasks.Task<SetVirtualTimePolicyResponse> SetVirtualTimePolicyAsync(CefSharp.DevTools.Emulation.VirtualTimePolicy policy, long? budget = null, int? maxVirtualTimeTaskStarvationCount = null, bool? waitForNavigation = null, long? initialVirtualTime = null)
        {
            ValidateSetVirtualTimePolicy(policy, budget, maxVirtualTimeTaskStarvationCount, waitForNavigation, initialVirtualTime);
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

        partial void ValidateSetLocaleOverride(string locale = null);
        /// <summary>
        /// Overrides default host system locale with the specified one.
        /// </summary>
        /// <param name = "locale">ICU style C locale (e.g. "en_US"). If not specified or empty, disables the override andrestores default host system locale.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetLocaleOverrideAsync(string locale = null)
        {
            ValidateSetLocaleOverride(locale);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(locale)))
            {
                dict.Add("locale", locale);
            }

            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setLocaleOverride", dict);
            return methodResult;
        }

        partial void ValidateSetTimezoneOverride(string timezoneId);
        /// <summary>
        /// Overrides default host system timezone with the specified one.
        /// </summary>
        /// <param name = "timezoneId">The timezone identifier. If empty, disables the override andrestores default host system timezone.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetTimezoneOverrideAsync(string timezoneId)
        {
            ValidateSetTimezoneOverride(timezoneId);
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            dict.Add("timezoneId", timezoneId);
            var methodResult = await _client.ExecuteDevToolsMethodAsync("Emulation.setTimezoneOverride", dict);
            return methodResult;
        }

        partial void ValidateSetUserAgentOverride(string userAgent, string acceptLanguage = null, string platform = null, CefSharp.DevTools.Emulation.UserAgentMetadata userAgentMetadata = null);
        /// <summary>
        /// Allows overriding user agent with the given string.
        /// </summary>
        /// <param name = "userAgent">User agent to use.</param>
        /// <param name = "acceptLanguage">Browser langugage to emulate.</param>
        /// <param name = "platform">The platform navigator.platform should return.</param>
        /// <param name = "userAgentMetadata">To be sent in Sec-CH-UA-* headers and returned in navigator.userAgentData</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;DevToolsMethodResponse&gt;</returns>
        public async System.Threading.Tasks.Task<DevToolsMethodResponse> SetUserAgentOverrideAsync(string userAgent, string acceptLanguage = null, string platform = null, CefSharp.DevTools.Emulation.UserAgentMetadata userAgentMetadata = null)
        {
            ValidateSetUserAgentOverride(userAgent, acceptLanguage, platform, userAgentMetadata);
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