// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "Internals/StringUtils.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    Nullable<bool> CefStateToDisabledSetting(cef_state_t state)
    {
        if (state == STATE_ENABLED)
        {
            return Nullable<bool>(false);
        }
        else if (state == STATE_DISABLED)
        {
            return Nullable<bool>(true);
        }
        return Nullable<bool>();
    }

    cef_state_t CefStateFromDisabledSetting(Nullable<bool>^ value)
    {
        if (value == nullptr)
        {
            return STATE_DEFAULT;
        }
        else if (value->Value)
        {
            return STATE_DISABLED;
        }
        else // !value->Value
        {
            return STATE_ENABLED;
        }
        return STATE_DEFAULT;
    }

    Nullable<bool> CefStateToEnabledSetting(cef_state_t state)
    {
        if (state == STATE_ENABLED)
        {
            return Nullable<bool>(true);
        }
        else if (state == STATE_DISABLED)
        {
            return Nullable<bool>(false);
        }
        return Nullable<bool>();
    }

    cef_state_t CefStateFromEnabledSetting(Nullable<bool>^ value)
    {
        if (value == nullptr)
        {
            return STATE_DEFAULT;
        }
        else if (value->Value)
        {
            return STATE_ENABLED;
        }
        else // !value->Value
        {
            return STATE_DISABLED;
        }
        return STATE_DEFAULT;
    }

    public ref class BrowserSettings
    {
    internal:
        CefBrowserSettings* _browserSettings;

    public:
        BrowserSettings() : _browserSettings(new CefBrowserSettings()) { }
        !BrowserSettings() { delete _browserSettings; }
        ~BrowserSettings() { delete _browserSettings; }

        // CefBrowserSettings is private causing whole field to be private
        // exposing void* as a workaround
        property void* _internalBrowserSettings
        {
            void* get() { return _browserSettings; }
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

        property Nullable<bool>^ RemoteFontsDisabled
        {
            Nullable<bool>^ get() { return CefStateToDisabledSetting(_browserSettings->remote_fonts); }
            void set(Nullable<bool>^ value) { _browserSettings->remote_fonts = CefStateFromDisabledSetting(value); }
        }

        property String^ DefaultEncoding
        {
            String^ get() { return StringUtils::ToClr(_browserSettings->default_encoding); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_browserSettings->default_encoding, value); }
        }

        property Nullable<bool>^ JavascriptDisabled
        {
            Nullable<bool>^ get() { return CefStateToDisabledSetting(_browserSettings->javascript); }
            void set(Nullable<bool>^ value) { _browserSettings->javascript = CefStateFromDisabledSetting(value); }
        }

        property Nullable<bool>^ JavaScriptOpenWindowsDisabled
        {
            Nullable<bool>^ get() { return CefStateToDisabledSetting(_browserSettings->javascript_open_windows); }
            void set(Nullable<bool>^ value) { _browserSettings->javascript_open_windows = CefStateFromDisabledSetting(value); }
        }

        property Nullable<bool>^ JavaScriptCloseWindowsDisabled
        {
            Nullable<bool>^ get() { return CefStateToDisabledSetting(_browserSettings->javascript_close_windows); }
            void set(Nullable<bool>^ value) { _browserSettings->javascript_close_windows = CefStateFromDisabledSetting(value); }
        }

        property Nullable<bool>^ JavaScriptAccessClipboardDisabled
        {
            Nullable<bool>^ get() { return CefStateToDisabledSetting(_browserSettings->javascript_access_clipboard); }
            void set(Nullable<bool>^ value) { _browserSettings->javascript_access_clipboard = CefStateFromDisabledSetting(value); }
        }

        property Nullable<bool>^ JavascriptDomPasteDisabled
        {
            Nullable<bool>^ get() { return CefStateToDisabledSetting(_browserSettings->javascript_dom_paste); }
            void set(Nullable<bool>^ value) { _browserSettings->javascript_dom_paste = CefStateFromDisabledSetting(value); }
        }

        property Nullable<bool>^ CaretBrowsingEnabled
        {
            Nullable<bool>^ get() { return CefStateToEnabledSetting(_browserSettings->caret_browsing); }
            void set(Nullable<bool>^ value) { _browserSettings->caret_browsing = CefStateFromEnabledSetting(value); }
        }

        property Nullable<bool>^ JavaDisabled
        {
            Nullable<bool>^ get() { return CefStateToDisabledSetting(_browserSettings->java); }
            void set(Nullable<bool>^ value) { _browserSettings->java = CefStateFromDisabledSetting(value); }
        } 

        property Nullable<bool>^ PluginsDisabled
        {
            Nullable<bool>^ get() { return CefStateToDisabledSetting(_browserSettings->plugins); }
            void set(Nullable<bool>^ value) { _browserSettings->plugins = CefStateFromDisabledSetting(value); }
        }

        property Nullable<bool>^ UniversalAccessFromFileUrlsAllowed
        {
            Nullable<bool>^ get() { return CefStateToEnabledSetting(_browserSettings->universal_access_from_file_urls); }
            void set(Nullable<bool>^ value) { _browserSettings->universal_access_from_file_urls = CefStateFromEnabledSetting(value); }
        }

        property Nullable<bool>^ FileAccessFromFileUrlsAllowed
        {
            Nullable<bool>^ get() { return CefStateToEnabledSetting(_browserSettings->file_access_from_file_urls); }
            void set(Nullable<bool>^ value) { _browserSettings->file_access_from_file_urls = CefStateFromEnabledSetting(value); }
        }

        property Nullable<bool>^ WebSecurityDisabled
        {
            Nullable<bool>^ get() { return CefStateToDisabledSetting(_browserSettings->web_security); }
            void set(Nullable<bool>^ value) { _browserSettings->web_security = CefStateFromDisabledSetting(value); }
        }

        property Nullable<bool>^ ImageLoadingDisabled
        {
            Nullable<bool>^ get() { return CefStateToDisabledSetting(_browserSettings->image_loading); }
            void set(Nullable<bool>^ value) { _browserSettings->image_loading = CefStateFromDisabledSetting(value); }
        }

        property Nullable<bool>^ ImageShrinkStandaloneToFitEnabled
        {
            Nullable<bool>^ get() { return CefStateToEnabledSetting(_browserSettings->image_shrink_standalone_to_fit); }
            void set(Nullable<bool>^ value) { _browserSettings->image_shrink_standalone_to_fit= CefStateFromEnabledSetting(value); }
        }

        property Nullable<bool>^ TextAreaResizeDisabled
        {
            Nullable<bool>^ get() { return CefStateToDisabledSetting(_browserSettings->text_area_resize); }
            void set(Nullable<bool>^ value) { _browserSettings->text_area_resize = CefStateFromDisabledSetting(value); }
        }

        property Nullable<bool>^ TabToLinksDisabled
        {
            Nullable<bool>^ get() { return CefStateToDisabledSetting(_browserSettings->tab_to_links); }
            void set(Nullable<bool>^ value) { _browserSettings->tab_to_links = CefStateFromDisabledSetting(value); }
        }

        property Nullable<bool>^ LocalStorageDisabled
        {
            Nullable<bool>^ get() { return CefStateToDisabledSetting(_browserSettings->local_storage); }
            void set(Nullable<bool>^ value) { _browserSettings->local_storage = CefStateFromDisabledSetting(value); }
        }

        property Nullable<bool>^ DatabasesDisabled
        {
            Nullable<bool>^ get() { return CefStateToDisabledSetting(_browserSettings->databases); }
            void set(Nullable<bool>^ value) { _browserSettings->databases = CefStateFromDisabledSetting(value); }
        }

        property Nullable<bool>^ ApplicationCacheDisabled
        {
            Nullable<bool>^ get() { return CefStateToDisabledSetting(_browserSettings->application_cache); }
            void set(Nullable<bool>^ value) { _browserSettings->application_cache = CefStateFromDisabledSetting(value); }
        }

        property Nullable<bool>^ WebGlDisabled
        {
            Nullable<bool>^ get() { return CefStateToDisabledSetting(_browserSettings->webgl); }
            void set(Nullable<bool>^ value) { _browserSettings->webgl = CefStateFromDisabledSetting(value); }
        }

    };
}