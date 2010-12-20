// Copyright (c) 2010 Marshall A. Greenblatt. All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
//    * Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following disclaimer
// in the documentation and/or other materials provided with the
// distribution.
//    * Neither the name of Google Inc. nor the name Chromium Embedded
// Framework nor the names of its contributors may be used to endorse
// or promote products derived from this software without specific prior
// written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.


#ifndef _CEF_TYPES_H
#define _CEF_TYPES_H

#include "cef_string_list.h"

// Bring in platform-specific definitions.
#if defined(_WIN32)
#include "cef_types_win.h"
#elif defined(__APPLE__)
#include "cef_types_mac.h"
#elif defined(__linux__)
#include "cef_types_linux.h"
#endif

// The NSPR system headers define 64-bit as |long| when possible.  In order to
// not have typedef mismatches, we do the same on LP64.
#if __LP64__
typedef long                int64;
#else
typedef long long           int64;
#endif

#ifdef __cplusplus
extern "C" {
#endif

// Initialization settings. Specify NULL or 0 to get the recommended default
// values.
typedef struct _cef_settings_t
{
  // Size of this structure.
  size_t size;

  // Set to true (1) to have the message loop run in a separate thread. If
  // false (0) than the CefDoMessageLoopWork() function must be called from
  // your application message loop.
  bool multi_threaded_message_loop;
  
  // The location where cache data will be stored on disk. If empty an
  // in-memory cache will be used. HTML5 databases such as localStorage will
  // only persist across sessions if a cache path is specified.
  cef_string_t cache_path;
  
  // Value that will be returned as the User-Agent HTTP header. If empty the
  // default User-Agent string will be used.
  cef_string_t user_agent;

  // Value that will be inserted as the product portion of the default
  // User-Agent string. If empty the Chromium product version will be used. If
  // |userAgent| is specified this value will be ignored.
  cef_string_t product_version;

  // The locale string that will be passed to WebKit. If empty the default
  // locale of "en-US" will be used.
  cef_string_t locale;

  // List of file system paths that will be searched by the browser to locate
  // plugins. This is in addition to the default search paths.
  cef_string_list_t extra_plugin_paths;
} cef_settings_t;

// Browser initialization settings. Specify NULL or 0 to get the recommended
// default values. The consequences of using custom values may not be well
// tested.
typedef struct _cef_browser_settings_t
{
  // Size of this structure.
  size_t size;

  // Disable drag & drop of URLs from other windows.
  bool drag_drop_disabled;

  // The below values map to WebPreferences settings.

  // Font settings.
  cef_string_t standard_font_family;
  cef_string_t fixed_font_family;
  cef_string_t serif_font_family;
  cef_string_t sans_serif_font_family;
  cef_string_t cursive_font_family;
  cef_string_t fantasy_font_family;
  int default_font_size;
  int default_fixed_font_size;
  int minimum_font_size;
  int minimum_logical_font_size;

  // Set to true (1) to disable loading of fonts from remote sources.
  bool remote_fonts_disabled;

  // Default encoding for Web content. If empty "ISO-8859-1" will be used.
  cef_string_t default_encoding;

  // Set to true (1) to attempt automatic detection of content encoding.
  bool encoding_detector_enabled;

  // Set to true (1) to disable JavaScript.
  bool javascript_disabled;

  // Set to true (1) to disallow JavaScript from opening windows.
  bool javascript_open_windows_disallowed;

  // Set to true (1) to disallow JavaScript from closing windows.
  bool javascript_close_windows_disallowed;

  // Set to true (1) to disallow JavaScript from accessing the clipboard.
  bool javascript_access_clipboard_disallowed;

  // Set to true (1) to disable DOM pasting in the editor. DOM pasting also
  // depends on |javascript_cannot_access_clipboard| being false (0).
  bool dom_paste_disabled;

  // Set to true (1) to enable drawing of the caret position.
  bool caret_browsing_enabled;

  // Set to true (1) to disable Java.
  bool java_disabled;

  // Set to true (1) to disable plugins.
  bool plugins_disabled;

  // Set to true (1) to allow access to all URLs from file URLs.
  bool universal_access_from_file_urls_allowed;

  // Set to true (1) to allow access to file URLs from other file URLs.
  bool file_access_from_file_urls_allowed;

  // Set to true (1) to allow risky security behavior such as cross-site
  // scripting (XSS). Use with extreme care.
  bool web_security_disabled;

  // Set to true (1) to enable console warnings about XSS attempts.
  bool xss_auditor_enabled;

  // Set to true (1) to suppress the network load of image URLs.  A cached
  // image will still be rendered if requested.
  bool image_load_disabled;

  // Set to true (1) to shrink standalone images to fit the page.
  bool shrink_standalone_images_to_fit;

  // Set to true (1) to disable browser backwards compatibility features.
  bool site_specific_quirks_disabled;

  // Set to true (1) to disable resize of text areas.
  bool text_area_resize_disabled;

  // Set to true (1) to disable use of the page cache.
  bool page_cache_disabled;

  // Set to true (1) to not have the tab key advance focus to links.
  bool tab_to_links_disabled;

  // Set to true (1) to disable hyperlink pings (<a ping> and window.sendPing).
  bool hyperlink_auditing_disabled;

  // Set to true (1) to enable the user style sheet for all pages.
  // |user_style_sheet_location| must be set to the style sheet URL.
  bool user_style_sheet_enabled;
  cef_string_t user_style_sheet_location;

  // Set to true (1) to disable style sheets.
  bool author_and_user_styles_disabled;

  // Set to true (1) to disable local storage.
  bool local_storage_disabled;

  // Set to true (1) to disable databases.
  bool databases_disabled;

  // Set to true (1) to disable application cache.
  bool application_cache_disabled;

  // Set to true (1) to disable WebGL.
  bool webgl_disabled;

  // Set to true (1) to disable accelerated compositing.
  bool accelerated_compositing_disabled;

  // Set to true (1) to disable accelerated layers. This affects features like
  // 3D CSS transforms.
  bool accelerated_layers_disabled;

  // Set to true (1) to disable accelerated 2d canvas.
  bool accelerated_2d_canvas_disabled;
} cef_browser_settings_t;

// Define handler return value types. Returning RV_HANDLED indicates
// that the implementation completely handled the method and that no further
// processing is required.  Returning RV_CONTINUE indicates that the
// implementation did not handle the method and that the default handler
// should be called.
enum cef_retval_t
{
  RV_HANDLED   = 0,
  RV_CONTINUE  = 1,
};

// Various browser navigation types supported by chrome.
enum cef_handler_navtype_t
{
  NAVTYPE_LINKCLICKED = 0,
  NAVTYPE_FORMSUBMITTED,
  NAVTYPE_BACKFORWARD,
  NAVTYPE_RELOAD,
  NAVTYPE_FORMRESUBMITTED,
  NAVTYPE_OTHER,
};

// Supported error code values. See net\base\net_error_list.h for complete
// descriptions of the error codes.
enum cef_handler_errorcode_t
{
  ERR_FAILED = -2,
  ERR_ABORTED = -3,
  ERR_INVALID_ARGUMENT = -4,
  ERR_INVALID_HANDLE = -5,
  ERR_FILE_NOT_FOUND = -6,
  ERR_TIMED_OUT = -7,
  ERR_FILE_TOO_BIG = -8,
  ERR_UNEXPECTED = -9,
  ERR_ACCESS_DENIED = -10,
  ERR_NOT_IMPLEMENTED = -11,
  ERR_CONNECTION_CLOSED = -100,
  ERR_CONNECTION_RESET = -101,
  ERR_CONNECTION_REFUSED = -102,
  ERR_CONNECTION_ABORTED = -103,
  ERR_CONNECTION_FAILED = -104,
  ERR_NAME_NOT_RESOLVED = -105,
  ERR_INTERNET_DISCONNECTED = -106,
  ERR_SSL_PROTOCOL_ERROR = -107,
  ERR_ADDRESS_INVALID = -108,
  ERR_ADDRESS_UNREACHABLE = -109,
  ERR_SSL_CLIENT_AUTH_CERT_NEEDED = -110,
  ERR_TUNNEL_CONNECTION_FAILED = -111,
  ERR_NO_SSL_VERSIONS_ENABLED = -112,
  ERR_SSL_VERSION_OR_CIPHER_MISMATCH = -113,
  ERR_SSL_RENEGOTIATION_REQUESTED = -114,
  ERR_CERT_COMMON_NAME_INVALID = -200,
  ERR_CERT_DATE_INVALID = -201,
  ERR_CERT_AUTHORITY_INVALID = -202,
  ERR_CERT_CONTAINS_ERRORS = -203,
  ERR_CERT_NO_REVOCATION_MECHANISM = -204,
  ERR_CERT_UNABLE_TO_CHECK_REVOCATION = -205,
  ERR_CERT_REVOKED = -206,
  ERR_CERT_INVALID = -207,
  ERR_CERT_END = -208,
  ERR_INVALID_URL = -300,
  ERR_DISALLOWED_URL_SCHEME = -301,
  ERR_UNKNOWN_URL_SCHEME = -302,
  ERR_TOO_MANY_REDIRECTS = -310,
  ERR_UNSAFE_REDIRECT = -311,
  ERR_UNSAFE_PORT = -312,
  ERR_INVALID_RESPONSE = -320,
  ERR_INVALID_CHUNKED_ENCODING = -321,
  ERR_METHOD_NOT_SUPPORTED = -322,
  ERR_UNEXPECTED_PROXY_AUTH = -323,
  ERR_EMPTY_RESPONSE = -324,
  ERR_RESPONSE_HEADERS_TOO_BIG = -325,
  ERR_CACHE_MISS = -400,
  ERR_INSECURE_RESPONSE = -501,
};

// Structure representing menu information.
typedef struct _cef_handler_menuinfo_t
{
  int typeFlags;
  int x;
  int y;
  cef_string_t linkUrl;
  cef_string_t imageUrl;
  cef_string_t pageUrl;
  cef_string_t frameUrl;
  cef_string_t selectionText;
  cef_string_t misspelledWord;
  int editFlags;
  cef_string_t securityInfo;
} cef_handler_menuinfo_t;

// The cef_handler_menuinfo_t typeFlags value will be a combination of the
// following values.
enum cef_handler_menutypebits_t
{
  // No node is selected
  MENUTYPE_NONE = 0x0,
  // The top page is selected
  MENUTYPE_PAGE = 0x1,
  // A subframe page is selected
  MENUTYPE_FRAME = 0x2,
  // A link is selected
  MENUTYPE_LINK = 0x4,
  // An image is selected
  MENUTYPE_IMAGE = 0x8,
  // There is a textual or mixed selection that is selected
  MENUTYPE_SELECTION = 0x10,
  // An editable element is selected
  MENUTYPE_EDITABLE = 0x20,
  // A misspelled word is selected
  MENUTYPE_MISSPELLED_WORD = 0x40,
  // A video node is selected
  MENUTYPE_VIDEO = 0x80,
  // A video node is selected
  MENUTYPE_AUDIO = 0x100,
};

// The cef_handler_menuinfo_t editFlags value will be a combination of the
// following values.
enum cef_handler_menucapabilitybits_t
{
  // Values from WebContextMenuData::EditFlags in WebContextMenuData.h
  MENU_CAN_DO_NONE = 0x0,
  MENU_CAN_UNDO = 0x1,
  MENU_CAN_REDO = 0x2,
  MENU_CAN_CUT = 0x4,
  MENU_CAN_COPY = 0x8,
  MENU_CAN_PASTE = 0x10,
  MENU_CAN_DELETE = 0x20,
  MENU_CAN_SELECT_ALL = 0x40,
  MENU_CAN_TRANSLATE = 0x80,
  // Values unique to CEF
  MENU_CAN_GO_FORWARD = 0x10000000,
  MENU_CAN_GO_BACK = 0x20000000,
};

// Supported menu ID values.
enum cef_handler_menuid_t
{
  MENU_ID_NAV_BACK = 10,
  MENU_ID_NAV_FORWARD = 11,
  MENU_ID_NAV_RELOAD = 12,
  MENU_ID_NAV_RELOAD_NOCACHE = 13,
  MENU_ID_NAV_STOP = 14,
  MENU_ID_UNDO = 20,
  MENU_ID_REDO = 21,
  MENU_ID_CUT = 22,
  MENU_ID_COPY = 23,
  MENU_ID_PASTE = 24,
  MENU_ID_DELETE = 25,
  MENU_ID_SELECTALL = 26,
  MENU_ID_PRINT = 30,
  MENU_ID_VIEWSOURCE = 31,
};

// Post data elements may represent either bytes or files.
enum cef_postdataelement_type_t
{
  PDE_TYPE_EMPTY  = 0,
  PDE_TYPE_BYTES,
  PDE_TYPE_FILE,
};

// Key event types.
enum cef_handler_keyevent_type_t
{
  KEYEVENT_RAWKEYDOWN = 0,
  KEYEVENT_KEYDOWN,
  KEYEVENT_KEYUP,
  KEYEVENT_CHAR
};

// Key event modifiers.
enum cef_handler_keyevent_modifiers_t
{
  KEY_SHIFT = 1 << 0,
  KEY_CTRL  = 1 << 1,
  KEY_ALT   = 1 << 2,
  KEY_META  = 1 << 3
};

// Structure representing a rectangle.
typedef struct _cef_rect_t
{
  int x;
  int y;
  int width;
  int height;
} cef_rect_t;

// Existing thread IDs.
enum cef_thread_id_t
{
  TID_UI      = 0,
  TID_IO      = 1,
  TID_FILE    = 2,
};

// Paper type for printing.
enum cef_paper_type_t
{
  PT_LETTER = 0,
  PT_LEGAL,
  PT_EXECUTIVE,
  PT_A3,
  PT_A4,
  PT_CUSTOM
};

// Paper metric information for printing.
struct cef_paper_metrics
{
  enum cef_paper_type_t paper_type;
  //Length and width needed if paper_type is custom_size
  //Units are in inches.
  double length;
  double width;
};

// Paper print margins.
struct cef_print_margins
{
  //Margin size in inches for left/right/top/bottom (this is content margins).
  double left;
  double right;
  double top;
  double bottom;
  //Margin size (top/bottom) in inches for header/footer.
  double header;
  double footer;
};

// Page orientation for printing.
enum cef_page_orientation
{
  PORTRAIT = 0,
  LANDSCAPE
};

// Printing options.
typedef struct _cef_print_options_t
{
  enum cef_page_orientation page_orientation;
  struct cef_paper_metrics paper_metrics;
  struct cef_print_margins paper_margins;
} cef_print_options_t;

// Supported XML encoding types. The parser supports ASCII, ISO-8859-1, and
// UTF16 (LE and BE) by default. All other types must be translated to UTF8
// before being passed to the parser. If a BOM is detected and the correct
// decoder is available then that decoder will be used automatically.
enum cef_xml_encoding_type_t
{
  XML_ENCODING_NONE = 0,
  XML_ENCODING_UTF8,
  XML_ENCODING_UTF16LE,
  XML_ENCODING_UTF16BE,
  XML_ENCODING_ASCII,
};

// XML node types.
enum cef_xml_node_type_t
{
  XML_NODE_UNSUPPORTED = 0,
  XML_NODE_PROCESSING_INSTRUCTION,
  XML_NODE_DOCUMENT_TYPE,
  XML_NODE_ELEMENT_START,
  XML_NODE_ELEMENT_END,
  XML_NODE_ATTRIBUTE,
  XML_NODE_TEXT,
  XML_NODE_CDATA,
  XML_NODE_ENTITY_REFERENCE,
  XML_NODE_WHITESPACE,
  XML_NODE_COMMENT,
};

// Popup window features.
typedef struct _cef_popup_features_t
{
  int x;
  bool xSet;
  int y;
  bool ySet;
  int width;
  bool widthSet;
  int height;
  bool heightSet;

  bool menuBarVisible;
  bool statusBarVisible;
  bool toolBarVisible;
  bool locationBarVisible;
  bool scrollbarsVisible;
  bool resizable;

  bool fullscreen;
  bool dialog;
  cef_string_list_t additionalFeatures;
} cef_popup_features_t;

#ifdef __cplusplus
}
#endif

#endif // _CEF_TYPES_H
