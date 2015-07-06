// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "Internals/StringUtils.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    public ref class BrowserSettings
    {
    internal:
        CefBrowserSettings* _browserSettings;

    public:
        BrowserSettings() : _browserSettings(new CefBrowserSettings()) { }

        !BrowserSettings()
        {
            delete _browserSettings;
        }

        ~BrowserSettings()
        {
            this->!BrowserSettings();
        }

        property String^ StandardFontFamily
        {
            String^ get() { return StringUtils::ToClr(_browserSettings->standard_font_family); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_browserSettings->standard_font_family, value); }
        }

        property String^ FixedFontFamily
        {
            String^ get() { return StringUtils::ToClr(_browserSettings->fixed_font_family); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_browserSettings->fixed_font_family, value); }
        }

        property String^ SerifFontFamily
        {
            String^ get() { return StringUtils::ToClr(_browserSettings->serif_font_family); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_browserSettings->serif_font_family, value); }
        }

        property String^ SansSerifFontFamily
        {
            String^ get() { return StringUtils::ToClr(_browserSettings->sans_serif_font_family); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_browserSettings->sans_serif_font_family, value); }
        }

        property String^ CursiveFontFamily
        {
            String^ get() { return StringUtils::ToClr(_browserSettings->cursive_font_family); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_browserSettings->cursive_font_family, value); }
        }

        property String^ FantasyFontFamily
        {
            String^ get() { return StringUtils::ToClr(_browserSettings->fantasy_font_family); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_browserSettings->fantasy_font_family, value); }
        }

        property int DefaultFontSize
        {
            int get() { return _browserSettings->default_font_size; }
            void set(int value) { _browserSettings->default_font_size = value; }
        }

        property int DefaultFixedFontSize
        {
            int get() { return _browserSettings->default_fixed_font_size; }
            void set(int value) { _browserSettings->default_fixed_font_size = value; }
        }

        property int MinimumFontSize
        {
            int get() { return _browserSettings->minimum_font_size; }
            void set(int value) { _browserSettings->minimum_font_size = value; }
        }

        property int MinimumLogicalFontSize
        {
            int get() { return _browserSettings->minimum_logical_font_size; }
            void set(int value) { _browserSettings->minimum_logical_font_size = value; }
        }

        property CefState RemoteFonts
        {
            CefState get() { return (CefState)_browserSettings->remote_fonts; }
            void set(CefState value) { _browserSettings->remote_fonts = (cef_state_t)value; }
        }

        property String^ DefaultEncoding
        {
            String^ get() { return StringUtils::ToClr(_browserSettings->default_encoding); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_browserSettings->default_encoding, value); }
        }

        property CefState Javascript
        {
            CefState get() { return (CefState)_browserSettings->javascript; }
            void set(CefState value) { _browserSettings->javascript = (cef_state_t)value; }
        }

        property CefState JavaScriptOpenWindows
        {
            CefState get() { return (CefState)_browserSettings->javascript_open_windows; }
            void set(CefState value) { _browserSettings->javascript_open_windows = (cef_state_t)value; }
        }

        property CefState JavaScriptCloseWindows
        {
            CefState get() { return (CefState)_browserSettings->javascript_close_windows; }
            void set(CefState value) { _browserSettings->javascript_close_windows = (cef_state_t)value; }
        }

        property CefState JavaScriptAccessClipboard
        {
            CefState get() { return (CefState)_browserSettings->javascript_access_clipboard; }
            void set(CefState value) { _browserSettings->javascript_access_clipboard = (cef_state_t)value; }
        }

        property CefState JavascriptDomPaste
        {
            CefState get() { return (CefState)_browserSettings->javascript_dom_paste; }
            void set(CefState value) { _browserSettings->javascript_dom_paste = (cef_state_t)value; }
        }

        property CefState CaretBrowsing
        {
            CefState get() { return (CefState)_browserSettings->caret_browsing; }
            void set(CefState value) { _browserSettings->caret_browsing = (cef_state_t)value; }
        }

        property CefState Java
        {
            CefState get() { return (CefState)_browserSettings->java; }
            void set(CefState value) { _browserSettings->java = (cef_state_t)value; }
        } 

        property CefState Plugins
        {
            CefState get() { return (CefState)_browserSettings->plugins; }
            void set(CefState value) { _browserSettings->plugins = (cef_state_t)value; }
        }

        property CefState UniversalAccessFromFileUrls
        {
            CefState get() { return (CefState)_browserSettings->universal_access_from_file_urls; }
            void set(CefState value) { _browserSettings->universal_access_from_file_urls = (cef_state_t)value; }
        }

        property CefState FileAccessFromFileUrls
        {
            CefState get() { return (CefState)_browserSettings->file_access_from_file_urls; }
            void set(CefState value) { _browserSettings->file_access_from_file_urls = (cef_state_t)value; }
        }

        property CefState WebSecurity
        {
            CefState get() { return (CefState)_browserSettings->web_security; }
            void set(CefState value) { _browserSettings->web_security = (cef_state_t)value; }
        }

        property CefState ImageLoading
        {
            CefState get() { return (CefState)_browserSettings->image_loading; }
            void set(CefState value) { _browserSettings->image_loading = (cef_state_t)value; }
        }

        property CefState ImageShrinkStandaloneToFit
        {
            CefState get() { return (CefState)_browserSettings->image_shrink_standalone_to_fit; }
            void set(CefState value) { _browserSettings->image_shrink_standalone_to_fit = (cef_state_t)value; }
        }

        property CefState TextAreaResize
        {
            CefState get() { return (CefState)_browserSettings->text_area_resize; }
            void set(CefState value) { _browserSettings->text_area_resize = (cef_state_t)value; }
        }

        property CefState TabToLinks
        {
            CefState get() { return (CefState)_browserSettings->tab_to_links; }
            void set(CefState value) { _browserSettings->tab_to_links = (cef_state_t)value; }
        }

        property CefState LocalStorage
        {
            CefState get() { return (CefState)_browserSettings->local_storage; }
            void set(CefState value) { _browserSettings->local_storage = (cef_state_t)value; }
        }

        property CefState Databases
        {
            CefState get() { return (CefState)_browserSettings->databases; }
            void set(CefState value) { _browserSettings->databases = (cef_state_t)value; }
        }

        property CefState ApplicationCache
        {
            CefState get() { return (CefState)_browserSettings->application_cache; }
            void set(CefState value) { _browserSettings->application_cache = (cef_state_t)value; }
        }

        property CefState WebGlD
        {
            CefState get() { return (CefState)_browserSettings->webgl; }
            void set(CefState value) { _browserSettings->webgl = (cef_state_t)value; }
        }

        property Nullable<bool> OffScreenTransparentBackground;
    };
}