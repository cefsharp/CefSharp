#include "stdafx.h"
#pragma once

using namespace System;
using namespace System::Collections::Generic;

namespace CefSharp
{
    public interface class IRequest
    {
        property String^ Url { String^ get(); void set(String^ url); }
        property String^ Method { String^ get(); }
        IDictionary<String^, String^>^ GetHeaders();
        void SetHeaders(IDictionary<String^, String^>^ headers);
    };

    ref class CefRequestWrapper : public IRequest
    {
        MCefRefPtr<CefRequest> _wrappedRequest;
    internal:
        CefRequestWrapper(CefRefPtr<CefRequest> cefRequest) : _wrappedRequest(cefRequest) {}

    public:
        virtual property String^ Url { String^ get(); void set(String^ url); }
        virtual property String^ Method { String^ get(); }
        virtual IDictionary<String^, String^>^ GetHeaders();
        virtual void SetHeaders(IDictionary<String^, String^>^ headers);

    };
}