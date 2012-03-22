#include "stdafx.h"
#pragma once

namespace CefSharp
{
    public ref class BrowserSettings
    {
    internal:
        CefBrowserSettings* _browserSettings;

    public:
        BrowserSettings() : _browserSettings(new CefBrowserSettings()) { }
        !BrowserSettings() { delete _browserSettings; }
        ~BrowserSettings() { delete _browserSettings; }

        property bool DragDropDisabled
        {
            bool get() { return _browserSettings->drag_drop_disabled; }
            void set(bool value) { _browserSettings->drag_drop_disabled = value; }
        }

        property bool LoadDropsDisabled
        {
            bool get() { return _browserSettings->load_drops_disabled; }
            void set(bool value) { _browserSettings->load_drops_disabled = value; }
        }

        property bool HistoryDisabled
        {
            bool get() { return _browserSettings->history_disabled; }
            void set(bool value) { _browserSettings->history_disabled = value; }
        }

        property String^ StandardFontFamily
        {
            String^ get() { return toClr(_browserSettings->standard_font_family); }
            void set(String^ value) { assignFromString(_browserSettings->standard_font_family, value); }
        }

        property String^ FixedFontFamily
        {
            String^ get() { return toClr(_browserSettings->fixed_font_family); }
            void set(String^ value) { assignFromString(_browserSettings->fixed_font_family, value); }
        }

        property String^ SerifFontFamily
        {
            String^ get() { return toClr(_browserSettings->serif_font_family); }
            void set(String^ value) { assignFromString(_browserSettings->serif_font_family, value); }
        }

        property String^ SansSerifFontFamily
        {
            String^ get() { return toClr(_browserSettings->sans_serif_font_family); }
            void set(String^ value) { assignFromString(_browserSettings->sans_serif_font_family, value); }
        }

        property String^ CursiveFontFamily
        {
            String^ get() { return toClr(_browserSettings->cursive_font_family); }
            void set(String^ value) { assignFromString(_browserSettings->cursive_font_family, value); }
        }

        property String^ FantasyFontFamily
        {
            String^ get() { return toClr(_browserSettings->fantasy_font_family); }
            void set(String^ value) { assignFromString(_browserSettings->fantasy_font_family, value); }
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

        property bool RemoteFontsDisabled
        {
            bool get() { return _browserSettings->remote_fonts_disabled; }
            void set(bool value) { _browserSettings->remote_fonts_disabled = value; }
        }

        property String^ DefaultEncoding
        {
            String^ get() { return toClr(_browserSettings->default_encoding); }
            void set(String^ value) { assignFromString(_browserSettings->default_encoding, value); }
        }

        property bool EncodingDetectorEnabled
        {
            bool get() { return _browserSettings->encoding_detector_enabled; }
            void set(bool value) { _browserSettings->encoding_detector_enabled = value; }
        }

        property bool JavaScriptDisabled
        {
            bool get() { return _browserSettings->javascript_disabled; }
            void set(bool value) { _browserSettings->javascript_disabled = value; }
        }

        property bool JavaScriptOpenWindowsDisallowed
        {
            bool get() { return _browserSettings->javascript_open_windows_disallowed; }
            void set(bool value) { _browserSettings->javascript_open_windows_disallowed = value; }
        }

        property bool JavaScriptCloseWindowsDisallowed
        {
            bool get() { return _browserSettings->javascript_close_windows_disallowed; }
            void set(bool value) { _browserSettings->javascript_close_windows_disallowed = value; }
        }

        property bool JavaScriptAccessClipboardDisallowed
        {
            bool get() { return _browserSettings->javascript_access_clipboard_disallowed; }
            void set(bool value) { _browserSettings->javascript_access_clipboard_disallowed = value; }
        }

        property bool DomPasteDisabled
        {
            bool get() { return _browserSettings->dom_paste_disabled; }
            void set(bool value) { _browserSettings->dom_paste_disabled = value; }
        }

        property bool CaretBrowsingEnabled
        {
            bool get() { return _browserSettings->caret_browsing_enabled; }
            void set(bool value) { _browserSettings->caret_browsing_enabled = value; }
        }

        property bool JavaDisabled
        {
            bool get() { return _browserSettings->java_disabled; }
            void set(bool value) { _browserSettings->java_disabled = value; }
        }

        property bool PluginsDisabled
        {
            bool get() { return _browserSettings->plugins_disabled; }
            void set(bool value) { _browserSettings->plugins_disabled = value; }
        }

        property bool UniversalAccessFromFileUrlsAllowed
        {
            bool get() { return _browserSettings->universal_access_from_file_urls_allowed; }
            void set(bool value) { _browserSettings->universal_access_from_file_urls_allowed = value; }
        }

        property bool FileAccessFromFileUrlsAllowed
        {
            bool get() { return _browserSettings->file_access_from_file_urls_allowed; }
            void set(bool value) { _browserSettings->file_access_from_file_urls_allowed = value; }
        }

        property bool WebSecurityDisabled
        {
            bool get() { return _browserSettings->web_security_disabled; }
            void set(bool value) { _browserSettings->web_security_disabled = value; }
        }

        property bool XssAuditorEnabled
        {
            bool get() { return _browserSettings->xss_auditor_enabled; }
            void set(bool value) { _browserSettings->xss_auditor_enabled = value; }
        }

        property bool ImageLoadDisabled
        {
            bool get() { return _browserSettings->image_load_disabled; }
            void set(bool value) { _browserSettings->image_load_disabled = value; }
        }

        property bool ShrinkStandaloneImagesToFit
        {
            bool get() { return _browserSettings->shrink_standalone_images_to_fit; }
            void set(bool value) { _browserSettings->shrink_standalone_images_to_fit = value; }
        }

        property bool SiteSpecificQuirksDisabled
        {
            bool get() { return _browserSettings->site_specific_quirks_disabled; }
            void set(bool value) { _browserSettings->site_specific_quirks_disabled = value; }
        }

        property bool TextAreaResizeDisabled
        {
            bool get() { return _browserSettings->text_area_resize_disabled; }
            void set(bool value) { _browserSettings->text_area_resize_disabled = value; }
        }

        property bool PageCacheDisabled
        {
            bool get() { return _browserSettings->page_cache_disabled; }
            void set(bool value) { _browserSettings->page_cache_disabled = value; }
        }

        property bool TabToLinksDisabled
        {
            bool get() { return _browserSettings->tab_to_links_disabled; }
            void set(bool value) { _browserSettings->tab_to_links_disabled = value; }
        }

        property bool HyperlinkAuditingDisabled
        {
            bool get() { return _browserSettings->hyperlink_auditing_disabled; }
            void set(bool value) { _browserSettings->hyperlink_auditing_disabled = value; }
        }

        property bool UserStyleSheetEnabled
        {
            bool get() { return _browserSettings->user_style_sheet_enabled; }
            void set(bool value) { _browserSettings->user_style_sheet_enabled = value; }
        }

        property String^ UserStyleSheetLocation
        {
            String^ get() { return toClr(_browserSettings->user_style_sheet_location); }
            void set(String^ value) { assignFromString(_browserSettings->user_style_sheet_location, value); }
        }

        property bool AuthorAndUserStylesDisabled
        {
            bool get() { return _browserSettings->author_and_user_styles_disabled; }
            void set(bool value) { _browserSettings->author_and_user_styles_disabled = value; }
        }

        property bool LocalStorageDisabled
        {
            bool get() { return _browserSettings->local_storage_disabled; }
            void set(bool value) { _browserSettings->local_storage_disabled = value; }
        }

        property bool DatabasesDisabled
        {
            bool get() { return _browserSettings->databases_disabled; }
            void set(bool value) { _browserSettings->databases_disabled = value; }
        }

        property bool ApplicationCacheDisabled
        {
            bool get() { return _browserSettings->application_cache_disabled; }
            void set(bool value) { _browserSettings->application_cache_disabled = value; }
        }

        property bool WebGlDisabled
        {
            bool get() { return _browserSettings->webgl_disabled; }
            void set(bool value) { _browserSettings->webgl_disabled = value; }
        }

        property bool AcceleratedCompositingEnabled
        {
            bool get() { return _browserSettings->accelerated_compositing_enabled; }
            void set(bool value) { _browserSettings->accelerated_compositing_enabled = value; }
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
    };
}