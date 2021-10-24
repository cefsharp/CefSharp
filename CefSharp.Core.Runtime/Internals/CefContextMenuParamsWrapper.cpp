// Copyright © 2014 The CefSharp Authors. All rights reserved.
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

        ContextMenuType CefContextMenuParamsWrapper::TypeFlags::get()
        {
            ThrowIfDisposed();

            return (ContextMenuType)_wrappedInfo->GetTypeFlags();
        }

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

        ContextMenuMediaType CefContextMenuParamsWrapper::MediaType::get()
        {
            ThrowIfDisposed();

            return (ContextMenuMediaType)_wrappedInfo->GetMediaType();
        }

        ContextMenuMediaState CefContextMenuParamsWrapper::MediaStateFlags::get()
        {
            ThrowIfDisposed();

            return (ContextMenuMediaState)_wrappedInfo->GetMediaStateFlags();
        }

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

        ContextMenuEditState CefContextMenuParamsWrapper::EditStateFlags::get()
        {
            ThrowIfDisposed();

            return (ContextMenuEditState)_wrappedInfo->GetEditStateFlags();
        }

        bool CefContextMenuParamsWrapper::IsCustomMenu::get()
        {
            ThrowIfDisposed();

            return _wrappedInfo->IsCustomMenu();
        }
    }
}
