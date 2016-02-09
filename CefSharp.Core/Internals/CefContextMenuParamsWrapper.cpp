// Copyright © 2010-2015 The CefSharp Project. All rights reserved.
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
            ThrowIfDisposed();

            return _wrappedInfo->GetYCoord();
        }

        int CefContextMenuParamsWrapper::XCoord::get()
        {
            ThrowIfDisposed();

            return _wrappedInfo->GetXCoord();
        }

        //// TODO: Implement:
        ////virtual TypeFlags GetTypeFlags() OVERRIDE;

        String^ CefContextMenuParamsWrapper::LinkUrl::get()
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_wrappedInfo->GetLinkUrl());
        }

        String^ CefContextMenuParamsWrapper::UnfilteredLinkUrl::get()
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_wrappedInfo->GetUnfilteredLinkUrl());
        }

        String^ CefContextMenuParamsWrapper::SourceUrl::get()
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_wrappedInfo->GetSourceUrl());
        }

        bool CefContextMenuParamsWrapper::HasImageContents::get()
        {
            ThrowIfDisposed();

            return _wrappedInfo->HasImageContents();
        }

        String^ CefContextMenuParamsWrapper::PageUrl::get()
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_wrappedInfo->GetPageUrl());
        }

        String^ CefContextMenuParamsWrapper::FrameUrl::get()
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_wrappedInfo->GetFrameUrl());
        }

        String^ CefContextMenuParamsWrapper::FrameCharset::get()
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_wrappedInfo->GetFrameCharset());
        }

        //// TODO: Implement:
        ////virtual MediaType GetMediaType() OVERRIDE;
        ////virtual MediaStateFlags GetMediaStateFlags() OVERRIDE;

        String^ CefContextMenuParamsWrapper::SelectionText::get()
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_wrappedInfo->GetSelectionText());
        }

        String^ CefContextMenuParamsWrapper::MisspelledWord::get()
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_wrappedInfo->GetMisspelledWord());
        }

        List<String^>^ CefContextMenuParamsWrapper::DictionarySuggestions::get()
        {
            ThrowIfDisposed();

            std::vector<CefString>& dictionarySuggestions = std::vector<CefString>();
            bool result = _wrappedInfo->GetDictionarySuggestions(dictionarySuggestions);

            return StringUtils::ToClr(dictionarySuggestions);
        }

        bool CefContextMenuParamsWrapper::IsEditable::get()
        {
            ThrowIfDisposed();

            return _wrappedInfo->IsEditable();
        }

        bool CefContextMenuParamsWrapper::IsSpellCheckEnabled::get()
        {
            ThrowIfDisposed();

            return _wrappedInfo->IsSpellCheckEnabled();
        }

        //// TODO: Implement:
        ////virtual EditStateFlags GetEditStateFlags() OVERRIDE;
    }
}
