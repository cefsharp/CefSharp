#include "stdafx.h"

#pragma once

using namespace System;

namespace CefSharp
{

    public interface class IRequest
    {
        property String^ Url { String^ get(); void set(String^ url); }
    };

    public ref class CefRequestWrapper : public IRequest
    {
        MCefRefPtr<CefRequest> _wrappedRequest;
    internal:
        CefRequestWrapper(CefRefPtr<CefRequest> cefRequest) : _wrappedRequest(cefRequest)
        {
        }

    public:
        virtual property String^ Url { String^ get(); void set(String^ url); }

    };

}