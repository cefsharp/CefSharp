// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "Internals/StringUtils.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
#pragma region enum BrowserSettingsState
    
    public enum class BrowserSettingsState
    {
        DEFAULT = cef_state_t::STATE_DEFAULT,
        ENABLED = cef_state_t::STATE_ENABLED,
        DISABLED = cef_state_t::STATE_DISABLED
    };

    BrowserSettingsState CefStateToSettingsState(cef_state_t state)
    {
        return (BrowserSettingsState)BrowserSettingsState::DEFAULT;
    }

    cef_state_t CefStateFromSettingsState(BrowserSettingsState value)
    {
        return (cef_state_t)value;
    } 

#pragma endregion


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

        property BrowserSettingsState RemoteFonts
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->remote_fonts); }
            void set(BrowserSettingsState value) { _browserSettings->remote_fonts = CefStateFromSettingsState(value); }
        }

        property String^ DefaultEncoding
        {
            String^ get() { return StringUtils::ToClr(_browserSettings->default_encoding); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_browserSettings->default_encoding, value); }
        }

        /* TODO: Nothing similar found
        property BrowserSettingsState EncodingDetectorEnabled
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->encoding_detector _enabled); }
            void set(BrowserSettingsState value) { _browserSettings->encoding_detector_enabled = CefStateFromSettingState(value); }
        }*/

        property BrowserSettingsState JavaScript
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->javascript); }
            void set(BrowserSettingsState value) { _browserSettings->javascript = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState JavaScriptOpenWindows
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->javascript_open_windows); }
            void set(BrowserSettingsState value) { _browserSettings->javascript_open_windows = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState JavaScriptCloseWindows
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->javascript_close_windows); }
            void set(BrowserSettingsState value) { _browserSettings->javascript_close_windows = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState JavaScriptAccessClipboard
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->javascript_access_clipboard); }
            void set(BrowserSettingsState value) { _browserSettings->javascript_access_clipboard = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState JavascriptDomPaste
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->javascript_dom_paste); }
            void set(BrowserSettingsState value) { _browserSettings->javascript_dom_paste = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState CaretBrowsing
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->caret_browsing); }
            void set(BrowserSettingsState value) { _browserSettings->caret_browsing = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState Java
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->java); }
            void set(BrowserSettingsState value) { _browserSettings->java = CefStateFromSettingsState(value); }
        } 

        property BrowserSettingsState Plugins
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->plugins); }
            void set(BrowserSettingsState value) { _browserSettings->plugins = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState UniversalAccessFromFileUrls
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->universal_access_from_file_urls); }
            void set(BrowserSettingsState value) { _browserSettings->universal_access_from_file_urls = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState FileAccessFromFileUrls
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->file_access_from_file_urls); }
            void set(BrowserSettingsState value) { _browserSettings->file_access_from_file_urls = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState WebSecurity
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->web_security); }
            void set(BrowserSettingsState value) { _browserSettings->web_security = CefStateFromSettingsState(value); }
        }

        /* TODO: No corresponding CEF3 setting found
        property BrowserSettingsState XssAuditor
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->xss_auditor); }
            void set(BrowserSettingsState value) { _browserSettings->xss_auditor = CefStateFromSettingsState(value); }
        } */

        property BrowserSettingsState ImageLoading
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->image_loading); }
            void set(BrowserSettingsState value) { _browserSettings->image_loading = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState ImageShrinkStandaloneToFit
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->image_shrink_standalone_to_fit); }
            void set(BrowserSettingsState value) { _browserSettings->image_shrink_standalone_to_fit= CefStateFromSettingsState(value); }
        }

        /* TODO: No corresponding CEF3 setting found
        property BrowserSettingsState SiteSpecificQuirksDisabled
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->site_specific_quirks); }
            void set(BrowserSettingsState value) { _browserSettings->site_specific_quirks_disabled = CefStateFromSettingsState(value); }
        } */

        property BrowserSettingsState TextAreaResize
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->text_area_resize); }
            void set(BrowserSettingsState value) { _browserSettings->text_area_resize = CefStateFromSettingsState(value); }
        }

        /* TODO: No corresponding CEF3 setting found
        property BrowserSettingsState PageCacheDisabled
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->page_cache; }
            void set(BrowserSettingsState value) { _browserSettings->page_cache_disab = CefStateFromSettingsState(value); }
        } */

        property BrowserSettingsState TabToLinks
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->tab_to_links); }
            void set(BrowserSettingsState value) { _browserSettings->tab_to_links = CefStateFromSettingsState(value); }
        }

        /* TODO: No corresponding CEF3 setting found
        property BrowserSettingsState HyperlinkAuditing
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->hyperlink_auditing); }
            void set(BrowserSettingsState value) { _browserSettings->hyperlink_auditing_disabled = CefStateFromSettingsState(value); }
        } */

        /* TODO: No corresponding CEF3 setting found
        property BrowserSettingsState UserStyleSheetEnabled
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->user_style_sheet_enabled; }
            void set(BrowserSettingsState value) { _browserSettings->user_style_sheet_enabled = CefStateFromSettingsState(value); }
        } */

        property String^ UserStyleSheetLocation
        {
            String^ get() { return StringUtils::ToClr(_browserSettings->user_style_sheet_location); }
            void set(String^ value) { StringUtils::AssignNativeFromClr(_browserSettings->user_style_sheet_location, value); }
        }

        property BrowserSettingsState AuthorAndUserStyles
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->author_and_user_styles); }
            void set(BrowserSettingsState value) { _browserSettings->author_and_user_styles = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState LocalStorage
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->local_storage); }
            void set(BrowserSettingsState value) { _browserSettings->local_storage = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState Databases
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->databases); }
            void set(BrowserSettingsState value) { _browserSettings->databases = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState ApplicationCache
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->application_cache); }
            void set(BrowserSettingsState value) { _browserSettings->application_cache = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState WebGl
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->webgl); }
            void set(BrowserSettingsState value) { _browserSettings->webgl = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState AcceleratedCompositing
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->accelerated_compositing); }
            void set(BrowserSettingsState value) { _browserSettings->accelerated_compositing = CefStateFromSettingsState(value); }
        }

        /* TODO: No corresponding CEF3 setting found
        property BrowserSettingsState AcceleratedLayers
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->accelerated_layers); }
            void set(BrowserSettingsState value) { _browserSettings->accelerated_layers = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState Accelerated2dCanvasDisabled
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->accelerated_2d_canvas_disabled; }
            void set(BrowserSettingsState value) { _browserSettings->accelerated_2d_canvas_disabled = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState AcceleratedPaintingDisabled
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->accelerated_painting_disabled; }
            void set(BrowserSettingsState value) { _browserSettings->accelerated_painting_disabled = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState AcceleratedFiltersDisabled
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->accelerated_filters_disabled; }
            void set(BrowserSettingsState value) { _browserSettings->accelerated_filters_disabled = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState AcceleratedPluginsDisabled
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->accelerated_plugins_disabled; }
            void set(BrowserSettingsState value) { _browserSettings->accelerated_plugins_disabled = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState DeveloperTools
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->developer_tools); }
            void set(BrowserSettingsState value) { _browserSettings->developer_tools_disabled = CefStateFromSettingsState(value); }
        }

        property BrowserSettingsState FullscreenEnabled
        {
            BrowserSettingsState get() { return CefStateToSettingsState(_browserSettings->fullscreen_enabled); }
            void set(BrowserSettingsState value) { _browserSettings->fullscreen_enabled = CefStateFromSettingsState(value); }
        } */
    };
}