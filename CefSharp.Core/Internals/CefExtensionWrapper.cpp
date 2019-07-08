// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "CefExtensionWrapper.h"
#include "CefValueWrapper.h"
#include "RequestContext.h"

namespace CefSharp
{
    namespace Internals
    {
        String^ CefExtensionWrapper::Identifier::get()
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_extension->GetIdentifier());
        }

        String^ CefExtensionWrapper::Path::get()
        {
            ThrowIfDisposed();

            return StringUtils::ToClr(_extension->GetPath());
        }

        IDictionary<String^, IValue^>^ CefExtensionWrapper::Manifest::get()
        {
            ThrowIfDisposed();

            auto dictionary = _extension->GetManifest();;

            if (!dictionary.get() || (int)dictionary->GetSize() == 0)
            {
                return nullptr;
            }

            auto result = gcnew Dictionary<String^, IValue^>();

            CefDictionaryValue::KeyList keys;
            dictionary->GetKeys(keys);

            for (auto i = 0; i < keys.size(); i++)
            {
                auto key = keys[i];
                auto keyValue = StringUtils::ToClr(key);
                auto valueWrapper = gcnew CefValueWrapper(dictionary->GetValue(keys[i]));

                result->Add(keyValue, valueWrapper);
            }

            return result;
        }

        bool CefExtensionWrapper::IsSame(IExtension^ that)
        {
            ThrowIfDisposed();

            return _extension->IsSame(((CefExtensionWrapper^)that)->_extension.get());
        }

        IRequestContext^ CefExtensionWrapper::LoaderContext::get()
        {
            ThrowIfDisposed();

            return gcnew RequestContext(_extension->GetLoaderContext());
        }

        bool CefExtensionWrapper::IsLoaded::get()
        {
            ThrowIfDisposed();

            return _extension->IsLoaded();
        }

        void CefExtensionWrapper::Unload()
        {
            ThrowIfDisposed();

            _extension->Unload();
        }
    }
}
