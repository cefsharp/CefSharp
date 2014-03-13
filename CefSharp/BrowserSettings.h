// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "Internals/StringUtils.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    namespace
    {
        Nullable<bool>^ CefStateToBoolean(cef_state_t state)
        {
            if (state == STATE_DEFAULT)
            {
                return nullptr;
            }
            else if (state == STATE_ENABLED)
            {
                return gcnew Nullable<bool>(true);
            }
            else if (state == STATE_DISABLED)
            {
                return gcnew Nullable<bool>(false);
            }

            return nullptr;
        }

        cef_state_t CefStateFromBoolean(Nullable<bool>^ value)
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
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->remote_fonts); }
            void set(Nullable<bool>^ value) { _browserSettings->remote_fonts = CefStateFromBoolean(value); }
        }

        property String^ DefaultEncoding
        {
            String^ get() { return StringUtils::ToClr(_browserSettings->default_encoding); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_browserSettings->default_encoding, value); }
        }

        property Nullable<bool>^ JavaScript
        {
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->javascript); }
            void set(Nullable<bool>^ value) { _browserSettings->javascript = CefStateFromBoolean(value); }
        }

        property Nullable<bool>^ JavaScriptOpenWindows
        {
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->javascript_open_windows); }
            void set(Nullable<bool>^ value) { _browserSettings->javascript_open_windows = CefStateFromBoolean(value); }
        }

        property Nullable<bool>^ JavaScriptCloseWindows
        {
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->javascript_close_windows); }
            void set(Nullable<bool>^ value) { _browserSettings->javascript_close_windows = CefStateFromBoolean(value); }
        }

        property Nullable<bool>^ JavaScriptAccessClipboard
        {
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->javascript_access_clipboard); }
            void set(Nullable<bool>^ value) { _browserSettings->javascript_access_clipboard = CefStateFromBoolean(value); }
        }

        property Nullable<bool>^ DomPaste
        {
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->javascript_dom_paste); }
            void set(Nullable<bool>^ value) { _browserSettings->javascript_dom_paste = CefStateFromBoolean(value); }
        }

        property Nullable<bool>^ CaretBrowsing
        {
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->caret_browsing); }
            void set(Nullable<bool>^ value) { _browserSettings->caret_browsing = CefStateFromBoolean(value); }
        }

        property Nullable<bool>^ Java
        {
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->java); }
            void set(Nullable<bool>^ value) { _browserSettings->java = CefStateFromBoolean(value); }
        }

        property Nullable<bool>^ Plugins
        {
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->plugins); }
            void set(Nullable<bool>^ value) { _browserSettings->plugins = CefStateFromBoolean(value); }
        }

        property Nullable<bool>^ UniversalAccessFromFileUrls
        {
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->universal_access_from_file_urls); }
            void set(Nullable<bool>^ value) { _browserSettings->universal_access_from_file_urls = CefStateFromBoolean(value); }
        }

        property Nullable<bool>^ FileAccessFromFileUrls
        {
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->file_access_from_file_urls); }
            void set(Nullable<bool>^ value) { _browserSettings->file_access_from_file_urls = CefStateFromBoolean(value); }
        }

        property Nullable<bool>^ WebSecurity
        {
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->web_security); }
            void set(Nullable<bool>^ value) { _browserSettings->web_security = CefStateFromBoolean(value); }
        }

        property Nullable<bool>^ ImageLoading
        {
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->image_loading); }
            void set(Nullable<bool>^ value) { _browserSettings->image_loading = CefStateFromBoolean(value); }
        }

        property Nullable<bool>^ ShrinkStandaloneImagesToFit
        {
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->image_shrink_standalone_to_fit); }
            void set(Nullable<bool>^ value) { _browserSettings->image_shrink_standalone_to_fit = CefStateFromBoolean(value); }
        }
		
		property Nullable<bool>^ TextAreaResize
        {
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->text_area_resize); }
            void set(Nullable<bool>^ value) { _browserSettings->text_area_resize = CefStateFromBoolean(value); }
        }

        property Nullable<bool>^ TabToLinks
        {
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->tab_to_links); }
            void set(Nullable<bool>^ value) { _browserSettings->tab_to_links = CefStateFromBoolean(value); }
        }

        property String^ UserStyleSheetLocation
        {
            String^ get() { return StringUtils::ToClr(_browserSettings->user_style_sheet_location); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_browserSettings->user_style_sheet_location, value); }
        }

        property Nullable<bool>^ AuthorAndUserStyles
        {
			Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->author_and_user_styles); }
			void set(Nullable<bool>^ value) { _browserSettings->author_and_user_styles = CefStateFromBoolean(value); }
        }

        property Nullable<bool>^ LocalStorage
        {
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->local_storage); }
            void set(Nullable<bool>^ value) { _browserSettings->local_storage = CefStateFromBoolean(value); }
        }

        property Nullable<bool>^ Databases
        {
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->databases); }
            void set(Nullable<bool>^ value) { _browserSettings->databases = CefStateFromBoolean(value); }
        }

        property Nullable<bool>^ ApplicationCache
        {
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->application_cache); }
            void set(Nullable<bool>^ value) { _browserSettings->application_cache = CefStateFromBoolean(value); }
        }

        property Nullable<bool>^ WebGl
        {
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->webgl); }
            void set(Nullable<bool>^ value) { _browserSettings->webgl = CefStateFromBoolean(value); }
        }

        property Nullable<bool>^ AcceleratedCompositing
        {
            Nullable<bool>^ get() { return CefStateToBoolean(_browserSettings->accelerated_compositing); }
            void set(Nullable<bool>^ value) { _browserSettings->accelerated_compositing = CefStateFromBoolean(value); }
        }

        // I cannot find these settings anywhere
        /*
        property bool EncodingDetectorEnabled
        {
            bool get() { return _browserSettings->encoding_detector_enabled; }
            void set(bool value) { _browserSettings->encoding_detector_enabled = value; }
        }

        property bool XssAuditorEnabled
        {
            bool get() { return _browserSettings->xss_auditor_enabled; }
            void set(bool value) { _browserSettings->xss_auditor_enabled = value; }
        }

        property bool SiteSpecificQuirksDisabled
        {
            bool get() { return _browserSettings->site_specific_quirks_disabled; }
            void set(bool value) { _browserSettings->site_specific_quirks_disabled = value; }
        }
		
		property bool PageCacheDisabled
        {
            bool get() { return _browserSettings->page_cache_disabled; }
            void set(bool value) { _browserSettings->page_cache_disabled = value; }
        }
       
        property bool HyperlinkAuditingDisabled
        {
            bool get() { return _browserSettings->hyperlink_auditing_disabled; }
            void set(bool value) { _browserSettings->hyperlink_auditing_disabled = value; }
        }
        
		property bool AcceleratedLayersDisabled
        {
            bool get() { return _browserSettings->accelerated_layers_disabled; }
            void set(bool value) { _browserSettings->accelerated_layers_disabled = value; }
        }

        property bool Accelerated2dCanvasDisabled
        {
            bool get() { return _browserSettings->accelerated_2d_canvas_disabled; }
            void set(bool value) { _browserSettings->accelerated_2d_canvas_disabled = value; }
        }

        property bool AcceleratedPaintingDisabled
        {
            bool get() { return _browserSettings->accelerated_painting_disabled; }
            void set(bool value) { _browserSettings->accelerated_painting_disabled = value; }
        }

        property bool AcceleratedFiltersDisabled
        {
            bool get() { return _browserSettings->accelerated_filters_disabled; }
            void set(bool value) { _browserSettings->accelerated_filters_disabled = value; }
        }

        property bool AcceleratedPluginsDisabled
        {
            bool get() { return _browserSettings->accelerated_plugins_disabled; }
            void set(bool value) { _browserSettings->accelerated_plugins_disabled = value; }
        }

        property bool DeveloperToolsDisabled
        {
            bool get() { return _browserSettings->developer_tools_disabled; }
            void set(bool value) { _browserSettings->developer_tools_disabled = value; }
        }

        property bool FullscreenEnabled
        {
            bool get() { return _browserSettings->fullscreen_enabled; }
            void set(bool value) { _browserSettings->fullscreen_enabled = value; }
        }
        */
    };
}