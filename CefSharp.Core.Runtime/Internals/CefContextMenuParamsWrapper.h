// Copyright © 2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "CefWrapper.h"

#include "include\cef_context_menu_handler.h"

using namespace System::Collections::Generic;

namespace CefSharp
{
    namespace Internals
    {
        private ref class CefContextMenuParamsWrapper : public IContextMenuParams, public CefWrapper
        {
            MCefRefPtr<CefContextMenuParams> _wrappedInfo;

        internal:
            CefContextMenuParamsWrapper(CefRefPtr<CefContextMenuParams> &cefParams) :
                _wrappedInfo(cefParams)
            {
            }

            !CefContextMenuParamsWrapper()
            {
                _wrappedInfo = NULL;
            }

            ~CefContextMenuParamsWrapper()
            {
                this->!CefContextMenuParamsWrapper();

                _disposed = true;
            }

        public:
            virtual property int YCoord { int get(); }
            virtual property int XCoord { int get(); }
            virtual property ContextMenuType TypeFlags { ContextMenuType get(); }
            virtual property String^ LinkUrl { String^ get(); }
            virtual property String^ UnfilteredLinkUrl { String^ get(); }
            virtual property String^ SourceUrl { String^ get(); }
            virtual property bool HasImageContents { bool get(); }
            virtual property String^ PageUrl { String^ get(); }
            virtual property String^ FrameUrl { String^ get(); }
            virtual property String^ FrameCharset { String^ get(); }
            virtual property ContextMenuMediaType MediaType { ContextMenuMediaType get(); }
            virtual property ContextMenuMediaState MediaStateFlags { ContextMenuMediaState get(); }
            virtual property String^ SelectionText { String^ get(); }
            virtual property String^ MisspelledWord { String^ get(); }
            virtual property List<String^>^ DictionarySuggestions { List<String^>^ get(); }
            virtual property bool IsEditable { bool get(); }
            virtual property bool IsSpellCheckEnabled { bool get(); }
            virtual property ContextMenuEditState EditStateFlags { ContextMenuEditState get(); }
            virtual property bool IsCustomMenu { bool get(); }
        };
    }
}
