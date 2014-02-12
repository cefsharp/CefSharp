#pragma once

#include "Stdafx.h"
#include "PostDataElement.h"

using namespace System;
using namespace System::Collections::Generic;

namespace CefSharp
{
    public ref class CefPostDataWrapper
    {
        MCefRefPtr<CefPostData> _wrappedData;

    internal:
        CefPostDataWrapper(CefRefPtr<CefPostData> cefPostData) : _wrappedData(cefPostData) {}

    public:
        property Int32^ ElementCount { Int32^ get(); }

        IList<CefPostDataElementWrapper^>^ GetElements();
    };
}