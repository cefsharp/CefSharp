// Copyright © 2010-2014 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "CefContextMenuParamsWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        int CefContextMenuParamsWrapper::YCoord::get()
        {
            return _wrappedInfo->GetYCoord();
        }

        int CefContextMenuParamsWrapper::XCoord::get()
        {
            return _wrappedInfo->GetXCoord();
        }

        //// TODO: Implement:
        ////virtual TypeFlags GetTypeFlags() OVERRIDE;

        String^ CefContextMenuParamsWrapper::LinkUrl::get()
        {
            return StringUtils::ToClr(_wrappedInfo->GetLinkUrl());
        }

        String^ CefContextMenuParamsWrapper::UnfilteredLinkUrl::get()
        {
            return StringUtils::ToClr(_wrappedInfo->GetUnfilteredLinkUrl());
        }

        String^ CefContextMenuParamsWrapper::SourceUrl::get()
        {
            return StringUtils::ToClr(_wrappedInfo->GetSourceUrl());
        }

        bool CefContextMenuParamsWrapper::HasImageContents::get()
        {
            return _wrappedInfo->HasImageContents();
        }

        String^ CefContextMenuParamsWrapper::PageUrl::get()
        {
            return StringUtils::ToClr(_wrappedInfo->GetPageUrl());
        }

        String^ CefContextMenuParamsWrapper::FrameUrl::get()
        {
            return StringUtils::ToClr(_wrappedInfo->GetFrameUrl());
        }

        String^ CefContextMenuParamsWrapper::FrameCharset::get()
        {
            return StringUtils::ToClr(_wrappedInfo->GetFrameCharset());
        }

        //// TODO: Implement:
        ////virtual MediaType GetMediaType() OVERRIDE;
        ////virtual MediaStateFlags GetMediaStateFlags() OVERRIDE;

        String^ CefContextMenuParamsWrapper::SelectionText::get()
        {
            return StringUtils::ToClr(_wrappedInfo->GetSelectionText());
        }

        String^ CefContextMenuParamsWrapper::MisspelledWord::get()
        {
            return StringUtils::ToClr(_wrappedInfo->GetMisspelledWord());
        }

        int CefContextMenuParamsWrapper::MisspellingHash::get()
        {
            return _wrappedInfo->GetMisspellingHash();
        }

        //// TODO: Implement:
        ////virtual bool GetDictionarySuggestions(std::vector<CefString>& suggestions) OVERRIDE;

        List<String^>^ CefContextMenuParamsWrapper::DictionarySuggestions::get()
        {
            std::vector<CefString>& dictionarySuggestions = std::vector<CefString>();
            bool result = _wrappedInfo->GetDictionarySuggestions(dictionarySuggestions);

            return StringUtils::ToClr(dictionarySuggestions);
        }

        bool CefContextMenuParamsWrapper::IsEditable::get()
        {
            return _wrappedInfo->IsEditable();
        }

        bool CefContextMenuParamsWrapper::IsSpellCheckEnabled::get()
        {
            return _wrappedInfo->IsSpellCheckEnabled();
        }

        //// TODO: Implement:
        ////virtual EditStateFlags GetEditStateFlags() OVERRIDE;
    }
}
