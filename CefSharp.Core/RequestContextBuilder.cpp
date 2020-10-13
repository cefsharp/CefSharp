// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Stdafx.h"
#include "RequestContextBuilder.h"
#include "RequestContext.h"

namespace CefSharp
{
    IRequestContext^ RequestContextBuilder::Create()
    {
        if (_otherContext != nullptr)
        {
            return gcnew RequestContext(_otherContext, _handler);
        }

        if(_settings != nullptr)
        {
            return gcnew RequestContext(_settings, _handler);
        }

        return gcnew RequestContext(_handler);
    }

    RequestContextBuilder^ RequestContextBuilder::OnInitialize(Action<IRequestContext^>^ action)
    {
        if (_handler == nullptr)
        {
            _handler = gcnew RequestContextHandler();
        }

        _handler->OnInitialize(action);

        return this;
    }

    RequestContextBuilder^ RequestContextBuilder::WithPreference(String^ name, Object^ value)
    {
        if (_handler == nullptr)
        {
            _handler = gcnew RequestContextHandler();
        }

        _handler->SetPreferenceOnContextInitialized(name, value);

        return this;
    }

    RequestContextBuilder^ RequestContextBuilder::WithProxyServer(String^ host)
    {
        if (_handler == nullptr)
        {
            _handler = gcnew RequestContextHandler();
        }

        _handler->SetProxyOnContextInitialized(host, Nullable<int>());

        return this;
    }

    RequestContextBuilder^ RequestContextBuilder::WithProxyServer(String^ host, Nullable<int> port)
    {
        if (_handler == nullptr)
        {
            _handler = gcnew RequestContextHandler();
        }

        _handler->SetProxyOnContextInitialized(host, port);

        return this;
    }

    RequestContextBuilder^ RequestContextBuilder::WithProxyServer(String^ scheme, String^ host, Nullable<int> port)
    {
        if (_handler == nullptr)
        {
            _handler = gcnew RequestContextHandler();
        }

        _handler->SetProxyOnContextInitialized(scheme, host, port);

        return this;
    }

    RequestContextBuilder^ RequestContextBuilder::PersistUserPreferences()
    {
        ThrowExceptionIfContextAlreadySet();

        if (_settings == nullptr)
        {
            _settings = gcnew RequestContextSettings();
        }

        _settings->PersistUserPreferences = true;

        return this;
    }

    RequestContextBuilder^ RequestContextBuilder::WithCachePath(String^ cachePath)
    {
        ThrowExceptionIfContextAlreadySet();

        if (_settings == nullptr)
        {
            _settings = gcnew RequestContextSettings();
        }

        _settings->CachePath = cachePath;

        return this;
    }

    RequestContextBuilder^ RequestContextBuilder::WithSharedSettings(IRequestContext^ other)
    {
        if (other == nullptr)
        {
            throw gcnew ArgumentNullException("other");
        }

        ThrowExceptionIfCustomSettingSpecified();

        _otherContext = other;

        return this;
    }
}
