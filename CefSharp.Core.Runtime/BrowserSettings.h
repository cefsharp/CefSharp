// Copyright © 2010 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\internal\cef_types_wrappers.h"

namespace CefSharp
{
    namespace Core
    {
        /// <summary>
        /// Browser initialization settings. Specify NULL or 0 to get the recommended
        /// default values. The consequences of using custom values may not be well
        /// tested. Many of these and other settings can also configured using command-
        /// line switches.
        /// </summary>
        [System::ComponentModel::EditorBrowsableAttribute(System::ComponentModel::EditorBrowsableState::Never)]
        public ref class BrowserSettings : IBrowserSettings
        {
        private:
            bool _isDisposed = false;
            bool _ownsPointer = false;
            bool _autoDispose = false;
        internal:
            CefBrowserSettings* _browserSettings;

            /// <summary>
            /// Internal Constructor
            /// </summary>
            BrowserSettings(CefBrowserSettings* browserSettings)
            {
                _browserSettings = browserSettings;
            }

        public:
            /// <summary>
            /// Default Constructor
            /// </summary>
            BrowserSettings() : _browserSettings(new CefBrowserSettings())
            {
                _ownsPointer = true;
            }

            BrowserSettings(bool autoDispose) : _browserSettings(new CefBrowserSettings())
            {
                _ownsPointer = true;
                _autoDispose = autoDispose;
            }

            /// <summary>
            /// Finalizer.
            /// </summary>
            !BrowserSettings()
            {
                if (_ownsPointer)
                {
                    delete _browserSettings;
                }

                _browserSettings = NULL;
                _isDisposed = true;
            }

            /// <summary>
            /// Destructor.
            /// </summary>
            ~BrowserSettings()
            {
                this->!BrowserSettings();
            }

            /// <summary>
            /// StandardFontFamily
            /// </summary>
            virtual property String^ StandardFontFamily
            {
                String^ get() { return StringUtils::ToClr(_browserSettings->standard_font_family); }
                void set(String^ value) { StringUtils::AssignNativeFromClr(_browserSettings->standard_font_family, value); }
            }

            /// <summary>
            /// FixedFontFamily
            /// </summary>
            virtual property String^ FixedFontFamily
            {
                String^ get() { return StringUtils::ToClr(_browserSettings->fixed_font_family); }
                void set(String^ value) { StringUtils::AssignNativeFromClr(_browserSettings->fixed_font_family, value); }
            }

            /// <summary>
            /// SerifFontFamily
            /// </summary>
            virtual property String^ SerifFontFamily
            {
                String^ get() { return StringUtils::ToClr(_browserSettings->serif_font_family); }
                void set(String^ value) { StringUtils::AssignNativeFromClr(_browserSettings->serif_font_family, value); }
            }

            /// <summary>
            /// SansSerifFontFamily
            /// </summary>
            virtual property String^ SansSerifFontFamily
            {
                String^ get() { return StringUtils::ToClr(_browserSettings->sans_serif_font_family); }
                void set(String^ value) { StringUtils::AssignNativeFromClr(_browserSettings->sans_serif_font_family, value); }
            }

            /// <summary>
            /// CursiveFontFamily
            /// </summary>
            virtual property String^ CursiveFontFamily
            {
                String^ get() { return StringUtils::ToClr(_browserSettings->cursive_font_family); }
                void set(String^ value) { StringUtils::AssignNativeFromClr(_browserSettings->cursive_font_family, value); }
            }

            /// <summary>
            /// FantasyFontFamily
            /// </summary>
            virtual property String^ FantasyFontFamily
            {
                String^ get() { return StringUtils::ToClr(_browserSettings->fantasy_font_family); }
                void set(String^ value) { StringUtils::AssignNativeFromClr(_browserSettings->fantasy_font_family, value); }
            }

            /// <summary>
            /// DefaultFontSize
            /// </summary>
            virtual property int DefaultFontSize
            {
                int get() { return _browserSettings->default_font_size; }
                void set(int value) { _browserSettings->default_font_size = value; }
            }

            /// <summary>
            /// DefaultFixedFontSize
            /// </summary>
            virtual property int DefaultFixedFontSize
            {
                int get() { return _browserSettings->default_fixed_font_size; }
                void set(int value) { _browserSettings->default_fixed_font_size = value; }
            }

            /// <summary>
            /// MinimumFontSize
            /// </summary>
            virtual property int MinimumFontSize
            {
                int get() { return _browserSettings->minimum_font_size; }
                void set(int value) { _browserSettings->minimum_font_size = value; }
            }

            /// <summary>
            /// MinimumLogicalFontSize
            /// </summary>
            virtual property int MinimumLogicalFontSize
            {
                int get() { return _browserSettings->minimum_logical_font_size; }
                void set(int value) { _browserSettings->minimum_logical_font_size = value; }
            }

            /// <summary>
            /// Default encoding for Web content. If empty "ISO-8859-1" will be used. Also
            /// configurable using the "default-encoding" command-line switch.
            /// </summary>
            virtual property String^ DefaultEncoding
            {
                String^ get() { return StringUtils::ToClr(_browserSettings->default_encoding); }
                void set(String^ value) { StringUtils::AssignNativeFromClr(_browserSettings->default_encoding, value); }
            }

            /// <summary>
            /// Controls the loading of fonts from remote sources. Also configurable using
            /// the "disable-remote-fonts" command-line switch.
            /// </summary>
            virtual property CefState RemoteFonts
            {
                CefState get() { return (CefState)_browserSettings->remote_fonts; }
                void set(CefState value) { _browserSettings->remote_fonts = (cef_state_t)value; }
            }

            /// <summary>
            /// Controls whether JavaScript can be executed. (Used to Enable/Disable javascript)
            /// Also configurable using the "disable-javascript" command-line switch.
            /// </summary>
            virtual property CefState Javascript
            {
                CefState get() { return (CefState)_browserSettings->javascript; }
                void set(CefState value) { _browserSettings->javascript = (cef_state_t)value; }
            }

            /// <summary>
            /// Controls whether JavaScript can be used to close windows that were not
            /// opened via JavaScript. JavaScript can still be used to close windows that
            /// were opened via JavaScript. Also configurable using the
            /// "disable-javascript-close-windows" command-line switch.
            /// </summary>
            virtual property CefState JavascriptCloseWindows
            {
                CefState get() { return (CefState)_browserSettings->javascript_close_windows; }
                void set(CefState value) { _browserSettings->javascript_close_windows = (cef_state_t)value; }
            }

            /// <summary>
            /// Controls whether JavaScript can access the clipboard. Also configurable
            /// using the "disable-javascript-access-clipboard" command-line switch.
            /// </summary>
            virtual property CefState JavascriptAccessClipboard
            {
                CefState get() { return (CefState)_browserSettings->javascript_access_clipboard; }
                void set(CefState value) { _browserSettings->javascript_access_clipboard = (cef_state_t)value; }
            }

            /// <summary>
            /// Controls whether DOM pasting is supported in the editor via
            /// execCommand("paste"). The |javascript_access_clipboard| setting must also
            /// be enabled. Also configurable using the "disable-javascript-dom-paste"
            /// command-line switch.
            /// </summary>
            virtual property CefState JavascriptDomPaste
            {
                CefState get() { return (CefState)_browserSettings->javascript_dom_paste; }
                void set(CefState value) { _browserSettings->javascript_dom_paste = (cef_state_t)value; }
            }

            /// <summary>
            /// Controls whether any plugins will be loaded. Also configurable using the
            /// "disable-plugins" command-line switch.
            /// </summary>
            virtual property CefState Plugins
            {
                CefState get() { return (CefState)_browserSettings->plugins; }
                void set(CefState value) { _browserSettings->plugins = (cef_state_t)value; }
            }

            /// <summary>
            /// Controls whether file URLs will have access to all URLs. Also configurable
            /// using the "allow-universal-access-from-files" command-line switch.
            /// </summary>
            virtual property CefState UniversalAccessFromFileUrls
            {
                CefState get() { return (CefState)_browserSettings->universal_access_from_file_urls; }
                void set(CefState value) { _browserSettings->universal_access_from_file_urls = (cef_state_t)value; }
            }

            /// <summary>
            /// Controls whether file URLs will have access to other file URLs. Also
            /// configurable using the "allow-access-from-files" command-line switch.
            /// </summary>
            virtual property CefState FileAccessFromFileUrls
            {
                CefState get() { return (CefState)_browserSettings->file_access_from_file_urls; }
                void set(CefState value) { _browserSettings->file_access_from_file_urls = (cef_state_t)value; }
            }

            /// <summary>
            /// Controls whether web security restrictions (same-origin policy) will be
            /// enforced. Disabling this setting is not recommend as it will allow risky
            /// security behavior such as cross-site scripting (XSS). Also configurable
            /// using the "disable-web-security" command-line switch.
            /// </summary>
            virtual property CefState WebSecurity
            {
                CefState get() { return (CefState)_browserSettings->web_security; }
                void set(CefState value) { _browserSettings->web_security = (cef_state_t)value; }
            }

            /// <summary>
            /// Controls whether image URLs will be loaded from the network. A cached image
            /// will still be rendered if requested. Also configurable using the
            /// "disable-image-loading" command-line switch.
            /// </summary>
            virtual property CefState ImageLoading
            {
                CefState get() { return (CefState)_browserSettings->image_loading; }
                void set(CefState value) { _browserSettings->image_loading = (cef_state_t)value; }
            }

            /// <summary>
            /// Controls whether standalone images will be shrunk to fit the page. Also
            /// configurable using the "image-shrink-standalone-to-fit" command-line
            /// switch.
            /// </summary>
            virtual property CefState ImageShrinkStandaloneToFit
            {
                CefState get() { return (CefState)_browserSettings->image_shrink_standalone_to_fit; }
                void set(CefState value) { _browserSettings->image_shrink_standalone_to_fit = (cef_state_t)value; }
            }

            /// <summary>
            /// Controls whether text areas can be resized. Also configurable using the
            /// "disable-text-area-resize" command-line switch.
            /// </summary>
            virtual property CefState TextAreaResize
            {
                CefState get() { return (CefState)_browserSettings->text_area_resize; }
                void set(CefState value) { _browserSettings->text_area_resize = (cef_state_t)value; }
            }

            /// <summary>
            /// Controls whether the tab key can advance focus to links. Also configurable
            /// using the "disable-tab-to-links" command-line switch.
            /// </summary>
            virtual property CefState TabToLinks
            {
                CefState get() { return (CefState)_browserSettings->tab_to_links; }
                void set(CefState value) { _browserSettings->tab_to_links = (cef_state_t)value; }
            }

            /// <summary>
            /// Controls whether local storage can be used. Also configurable using the
            /// "disable-local-storage" command-line switch.
            /// </summary>
            virtual property CefState LocalStorage
            {
                CefState get() { return (CefState)_browserSettings->local_storage; }
                void set(CefState value) { _browserSettings->local_storage = (cef_state_t)value; }
            }

            /// <summary>
            /// Controls whether databases can be used. Also configurable using the
            /// "disable-databases" command-line switch.
            /// </summary>
            virtual property CefState Databases
            {
                CefState get() { return (CefState)_browserSettings->databases; }
                void set(CefState value) { _browserSettings->databases = (cef_state_t)value; }
            }

            /// <summary>
            /// Controls whether the application cache can be used. Also configurable using
            /// the "disable-application-cache" command-line switch.
            /// </summary>
            virtual property CefState ApplicationCache
            {
                CefState get() { return (CefState)_browserSettings->application_cache; }
                void set(CefState value) { _browserSettings->application_cache = (cef_state_t)value; }
            }

            /// <summary>
            /// Controls whether WebGL can be used. Note that WebGL requires hardware
            /// support and may not work on all systems even when enabled. Also
            /// configurable using the "disable-webgl" command-line switch.
            /// </summary>
            virtual property CefState WebGl
            {
                CefState get() { return (CefState)_browserSettings->webgl; }
                void set(CefState value) { _browserSettings->webgl = (cef_state_t)value; }
            }

            /// <summary>
            /// Background color used for the browser before a document is loaded and when no document color
            /// is specified. The alpha component must be either fully opaque (0xFF) or fully transparent (0x00).
            /// If the alpha component is fully opaque then the RGB components will be used as the background
            /// color. If the alpha component is fully transparent for a WinForms browser then the
            /// CefSettings.BackgroundColor value will be used. If the alpha component is fully transparent
            /// for a windowless (WPF/OffScreen) browser then transparent painting will be enabled.
            /// </summary>
            virtual property uint32 BackgroundColor
            {
                uint32 get() { return _browserSettings->background_color; }
                void set(uint32 value) { _browserSettings->background_color = value; }
            }

            /// <summary>
            /// Comma delimited ordered list of language codes without any whitespace that
            /// will be used in the "Accept-Language" HTTP header. May be overridden on a
            /// per-browser basis using the CefBrowserSettings.AcceptLanguageList value.
            /// If both values are empty then "en-US,en" will be used. Can be overridden
            /// for individual RequestContext instances via the
            /// RequestContextSettings.AcceptLanguageList value.
            /// </summary>
            virtual property String^ AcceptLanguageList
            {
                String^ get() { return StringUtils::ToClr(_browserSettings->accept_language_list); }
                void set(String^ value) { StringUtils::AssignNativeFromClr(_browserSettings->accept_language_list, value); }
            }

            /// <summary>
            /// The maximum rate in frames per second (fps) that CefRenderHandler::OnPaint
            /// will be called for a windowless browser. The actual fps may be lower if
            /// the browser cannot generate frames at the requested rate. The minimum
            /// value is 1 and the maximum value is 60 (default 30). This value can also be
            /// changed dynamically via IBrowserHost.SetWindowlessFrameRate.
            /// </summary>
            virtual property int WindowlessFrameRate
            {
                int get() { return _browserSettings->windowless_frame_rate; }
                void set(int value) { _browserSettings->windowless_frame_rate = value; }
            }

            /// <summary>
            /// Gets a value indicating if the browser settings has been disposed.
            /// </summary>
            virtual property bool IsDisposed
            {
                bool get() { return _isDisposed; }
            }

            /// <summary>
            /// True if dispose should be called after this object is used
            /// </summary>
            virtual property bool AutoDispose
            {
                bool get() { return _autoDispose; }
            }
        };
    }
}
