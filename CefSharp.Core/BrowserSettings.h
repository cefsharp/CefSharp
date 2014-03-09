// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "Internals/StringUtils.h"

using namespace CefSharp::Internals;

namespace CefSharp
{
    Nullable<bool> CefStateToBoolean(cef_state_t state)
    {
        if (state == STATE_DEFAULT)
        {
            return Nullable<bool>();
        }
        else if (state == STATE_ENABLED)
        {
            return Nullable<bool>(true);
        }
        else if (state == STATE_DISABLED)
        {
            return Nullable<bool>(false);
        }

        return Nullable<bool>();
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

    public ref class BrowserSettings : public BrowserSettingsBase
    {
    internal:
        CefBrowserSettings* _browserSettings;

    public:
        BrowserSettings() :
            _browserSettings(new CefBrowserSettings())
        {
        }

        virtual void DoDispose(bool disposing) override
        {
            delete _browserSettings;
            _browserSettings = nullptr;

            BrowserSettingsBase::DoDispose(disposing);
        }

        virtual property String^ StandardFontFamily
        {
            String^ get() override { return StringUtils::ToClr(_browserSettings->standard_font_family); }
            void set(String^ value) override { StringUtils::AssignNativeFromClr(_browserSettings->standard_font_family, value); }
        }

        virtual property String^ FixedFontFamily
        {
            String^ get() override { return StringUtils::ToClr(_browserSettings->fixed_font_family); }
            void set(String^ value) override { StringUtils::AssignNativeFromClr(_browserSettings->fixed_font_family, value); }
        }

        virtual property String^ SerifFontFamily
        {
            String^ get() override { return StringUtils::ToClr(_browserSettings->serif_font_family); }
            void set(String^ value) override { StringUtils::AssignNativeFromClr(_browserSettings->serif_font_family, value); }
        }

        virtual property String^ SansSerifFontFamily
        {
            String^ get() override { return StringUtils::ToClr(_browserSettings->sans_serif_font_family); }
            void set(String^ value) override { StringUtils::AssignNativeFromClr(_browserSettings->sans_serif_font_family, value); }
        }

        virtual property String^ CursiveFontFamily
        {
            String^ get() override { return StringUtils::ToClr(_browserSettings->cursive_font_family); }
            void set(String^ value) override { StringUtils::AssignNativeFromClr(_browserSettings->cursive_font_family, value); }
        }

        virtual property String^ FantasyFontFamily
        {
            String^ get() override { return StringUtils::ToClr(_browserSettings->fantasy_font_family); }
            void set(String^ value) override { StringUtils::AssignNativeFromClr(_browserSettings->fantasy_font_family, value); }
        }

        virtual property int DefaultFontSize
        {
            int get() override { return _browserSettings->default_font_size; }
            void set(int value) override { _browserSettings->default_font_size = value; }
        }

        virtual property int DefaultFixedFontSize
        {
            int get() override { return _browserSettings->default_fixed_font_size; }
            void set(int value) override { _browserSettings->default_fixed_font_size = value; }
        }

        virtual property int MinimumFontSize
        {
            int get() override { return _browserSettings->minimum_font_size; }
            void set(int value) override { _browserSettings->minimum_font_size = value; }
        }

        virtual property int MinimumLogicalFontSize
        {
            int get() override { return _browserSettings->minimum_logical_font_size; }
            void set(int value) override { _browserSettings->minimum_logical_font_size = value; }
        }

        virtual property Nullable<bool> RemoteFontsDisabled
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->remote_fonts); }
            void set(Nullable<bool> value) override { _browserSettings->remote_fonts = CefStateFromBoolean(value); }
        }

        virtual property String^ DefaultEncoding
        {
            String^ get() override { return StringUtils::ToClr(_browserSettings->default_encoding); }
            void set(String^ value) override { StringUtils::AssignNativeFromClr(_browserSettings->default_encoding, value); }
        }

        /* virtual property Nullable<bool> EncodingDetectorEnabled
         {
         Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->encoding_detector_enabled ); }
         void set(Nullable<bool> value) override { _browserSettings->encoding_detector_enabled = CefStateFromBoolean(value); }
         }*/

        virtual property Nullable<bool> JavaScriptDisabled
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->javascript); }
            void set(Nullable<bool> value) override { _browserSettings->javascript = CefStateFromBoolean(value); }
        }

        virtual property Nullable<bool> JavaScriptOpenWindowsDisallowed
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->javascript_open_windows); }
            void set(Nullable<bool> value) override { _browserSettings->javascript_open_windows = CefStateFromBoolean(value); }
        }

        virtual property Nullable<bool> JavaScriptCloseWindowsDisallowed
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->javascript_close_windows); }
            void set(Nullable<bool> value) override { _browserSettings->javascript_close_windows = CefStateFromBoolean(value); }
        }

        virtual property Nullable<bool> JavaScriptAccessClipboardDisallowed
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->javascript_access_clipboard); }
            void set(Nullable<bool> value) override { _browserSettings->javascript_access_clipboard = CefStateFromBoolean(value); }
        }

        virtual property Nullable<bool> DomPasteDisabled
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->javascript_dom_paste); }
            void set(Nullable<bool> value) override { _browserSettings->javascript_dom_paste = CefStateFromBoolean(value); }
        }

        virtual property Nullable<bool> CaretBrowsingEnabled
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->caret_browsing); }
            void set(Nullable<bool> value) override { _browserSettings->caret_browsing = CefStateFromBoolean(value); }
        }

        virtual property Nullable<bool> JavaDisabled
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->java); }
            void set(Nullable<bool> value) override { _browserSettings->java = CefStateFromBoolean(value); }
        }

        virtual property Nullable<bool> PluginsDisabled
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->plugins); }
            void set(Nullable<bool> value) override { _browserSettings->plugins = CefStateFromBoolean(value); }
        }

        virtual property Nullable<bool> UniversalAccessFromFileUrlsAllowed
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->universal_access_from_file_urls); }
            void set(Nullable<bool> value) override { _browserSettings->universal_access_from_file_urls = CefStateFromBoolean(value); }
        }

        virtual property Nullable<bool> FileAccessFromFileUrlsAllowed
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->file_access_from_file_urls); }
            void set(Nullable<bool> value) override { _browserSettings->file_access_from_file_urls = CefStateFromBoolean(value); }
        }

        virtual property Nullable<bool> WebSecurityDisabled
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->web_security); }
            void set(Nullable<bool> value) override { _browserSettings->web_security = CefStateFromBoolean(value); }
        }

        /*virtual property Nullable<bool> XssAuditorEnabled
        {
        Nullable<bool>^ get() override { return CefStateToBoolean(_browserSettings->xss_auditor_enabled; }
        void set(Nullable<bool>^ value) override { _browserSettings->xss_auditor_enabled = CefStateFromBoolean(value); }
        }*/

        virtual property Nullable<bool> ImageLoadDisabled
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->image_loading); }
            void set(Nullable<bool> value) override { _browserSettings->image_loading = CefStateFromBoolean(value); }
        }

        virtual property Nullable<bool> ShrinkStandaloneImagesToFit
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->image_shrink_standalone_to_fit); }
            void set(Nullable<bool> value) override { _browserSettings->image_shrink_standalone_to_fit = CefStateFromBoolean(value); }
        }

        //virtual property Nullable<bool> SiteSpecificQuirksDisabled
        //{
        //    Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->site_specific_quirks_disabled; }
        //    void set(Nullable<bool> value) override { _browserSettings->site_specific_quirks_disabled = CefStateFromBoolean(value); }
        //}

        virtual property Nullable<bool> TextAreaResizeDisabled
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->text_area_resize); }
            void set(Nullable<bool> value) override { _browserSettings->text_area_resize = CefStateFromBoolean(value); }
        }

        /*virtual property Nullable<bool> PageCacheDisabled
        {
        Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->application_cache); }
        void set(Nullable<bool> value) override { _browserSettings->application_cache xxx = CefStateFromBoolean(value); }
        }*/

        virtual property Nullable<bool> TabToLinksDisabled
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->tab_to_links); }
            void set(Nullable<bool> value) override { _browserSettings->tab_to_links = CefStateFromBoolean(value); }
        }

        /* virtual property Nullable<bool> HyperlinkAuditingDisabled
         {
         Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->hyperlink_auditing_disabled; }
         void set(Nullable<bool> value) override { _browserSettings->hyperlink_auditing_disabled = CefStateFromBoolean(value); }
         }

         virtual property Nullable<bool> UserStyleSheetEnabled
         {
         Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->user_style_sheet_enabled; }
         void set(Nullable<bool> value) override { _browserSettings->user_style_sheet_enabled = CefStateFromBoolean(value); }
         }*/

        virtual property String^ UserStyleSheetLocation
        {
            String^ get() override { return StringUtils::ToClr(_browserSettings->user_style_sheet_location); }
            void set(String^ value) override { StringUtils::AssignNativeFromClr(_browserSettings->user_style_sheet_location, value); }
        }

        virtual property Nullable<bool> AuthorAndUserStylesDisabled
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->author_and_user_styles); }
            void set(Nullable<bool> value) override { _browserSettings->author_and_user_styles = CefStateFromBoolean(value); }
        }

        virtual property Nullable<bool> LocalStorageDisabled
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->local_storage); }
            void set(Nullable<bool> value) override { _browserSettings->local_storage = CefStateFromBoolean(value); }
        }

        virtual property Nullable<bool> DatabasesDisabled
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->databases); }
            void set(Nullable<bool> value) override { _browserSettings->databases = CefStateFromBoolean(value); }
        }

        virtual property Nullable<bool> ApplicationCacheDisabled
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->application_cache); }
            void set(Nullable<bool> value) override { _browserSettings->application_cache = CefStateFromBoolean(value); }
        }

        virtual property Nullable<bool> WebGlDisabled
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->webgl); }
            void set(Nullable<bool> value) override { _browserSettings->webgl = CefStateFromBoolean(value); }
        }

        virtual property Nullable<bool> AcceleratedCompositingEnabled
        {
            Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->accelerated_compositing); }
            void set(Nullable<bool> value) override { _browserSettings->accelerated_compositing = CefStateFromBoolean(value); }
        }

        /* virtual property Nullable<bool> AcceleratedLayersDisabled
         {
         Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->accelerated_layers_disabled; }
         void set(Nullable<bool>^ value) override { _browserSettings->accelerated_layers_disabled = CefStateFromBoolean(value); }
         }

         virtual property Nullable<bool> Accelerated2dCanvasDisabled
         {
         Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->accelerated_2d_canvas_disabled; }
         void set(Nullable<bool> value) override { _browserSettings->accelerated_2d_canvas_disabled = CefStateFromBoolean(value); }
         }

         virtual property Nullable<bool> AcceleratedPaintingDisabled
         {
         Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->accelerated_painting_disabled; }
         void set(Nullable<bool> value) override { _browserSettings->accelerated_painting_disabled = CefStateFromBoolean(value); }
         }

         virtual property Nullable<bool> AcceleratedFiltersDisabled
         {
         Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->accelerated_filters_disabled; }
         void set(Nullable<bool> value) override { _browserSettings->accelerated_filters_disabled = CefStateFromBoolean(value); }
         }

         virtual property Nullable<bool> AcceleratedPluginsDisabled
         {
         Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->accelerated_plugins_disabled; }
         void set(Nullable<bool> value) override { _browserSettings->accelerated_plugins_disabled = CefStateFromBoolean(value); }
         }

         virtual property Nullable<bool> DeveloperToolsDisabled
         {
         Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->developer_tools_disabled; }
         void set(Nullable<bool>^ value) override { _browserSettings->developer_tools_disabled = CefStateFromBoolean(value); }
         }

         virtual property Nullable<bool> FullscreenEnabled
         {
         Nullable<bool> get() override { return CefStateToBoolean(_browserSettings->fullscreen_enabled; }
         void set(Nullable<bool> value) override { _browserSettings->fullscreen_enabled = CefStateFromBoolean(value); }
         }*/
    };
}