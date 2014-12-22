// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"
#include "MCefRefPtr.h"

using namespace System;
using namespace System::Collections::Generic;

namespace CefSharp
{
    namespace Internals
    {
        ref class CefContextMenuParamsWrapper : public IContextMenuParams
        {
            MCefRefPtr<CefContextMenuParams> _wrappedInfo;

        internal:
            CefContextMenuParamsWrapper(CefRefPtr<CefContextMenuParams> cefParams) : _wrappedInfo(cefParams) {}

        public:
            virtual property int YCoord { int get(); }
            virtual property int XCoord { int get(); }

            // TODO: Implement:
            //virtual TypeFlags GetTypeFlags() OVERRIDE;
            virtual property String^ LinkUrl { String^ get(); }
            virtual property String^ UnfilteredLinkUrl { String^ get(); }
            virtual property String^ SourceUrl { String^ get(); }
            virtual property bool HasImageContents { bool get(); }
            virtual property String^ PageUrl { String^ get(); }
            virtual property String^ FrameUrl { String^ get(); }
            virtual property String^ FrameCharset { String^ get(); }

            // TODO: Implement:
            //virtual MediaType GetMediaType() OVERRIDE;
            //virtual MediaStateFlags GetMediaStateFlags() OVERRIDE;

            virtual property String^ SelectionText { String^ get(); }
            virtual property String^ MisspelledWord { String^ get(); }
            virtual property int MisspellingHash { int get(); }

            virtual property List<String^>^ DictionarySuggestions { List<String^>^ get(); }
            
            // TODO: Implement:
            //virtual bool GetDictionarySuggestions(std::vector<CefString>& suggestions) OVERRIDE;

            virtual property bool IsEditable { bool get(); }
            virtual property bool IsSpellCheckEnabled { bool get(); }
          
            // TODO: Implement:
            //virtual EditStateFlags GetEditStateFlags() OVERRIDE;
        };
    }
}
