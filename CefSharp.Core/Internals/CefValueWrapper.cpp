// Copyright © 2010-2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"

#include "CefValueWrapper.h"

Enums::CefValueType CefValueWrapper::GetCefValueType()
{
    ThrowIfDisposed();

    return (Enums::CefValueType)_cefValue->GetType();
}

bool CefValueWrapper::GetBool()
{
    ThrowIfDisposed();

    return _cefValue->GetBool();
}

double CefValueWrapper::GetDouble()
{
    ThrowIfDisposed();

    return _cefValue->GetDouble();
}

int CefValueWrapper::GetInt()
{
    ThrowIfDisposed();

    return _cefValue->GetInt();
}

String^ CefValueWrapper::GetString()
{
    ThrowIfDisposed();

    return StringUtils::ToClr(_cefValue->GetString());
}

Dictionary<String^, ICefValue^>^ CefValueWrapper::GetDictionary()
{
    ThrowIfDisposed();

    auto dictionary = _cefValue->GetDictionary();

    if (!dictionary.get() || dictionary->GetSize() == 0)
    {
        return nullptr;
    }

    auto result = gcnew Dictionary<String^, ICefValue^>();

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

List<ICefValue^>^ CefValueWrapper::GetList()
{
    ThrowIfDisposed();

    auto list = _cefValue->GetList();
    auto result = gcnew List<ICefValue^>(list->GetSize());
    
    for (auto i = 0; i < list->GetSize(); i++)
    {
        result->Add(gcnew CefValueWrapper(list->GetValue(i)));
    }

    return result;
}