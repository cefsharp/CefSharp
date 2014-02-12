#pragma once

#include "Stdafx.h"

using namespace System;
using namespace System::Collections::Generic;

namespace CefSharp
{
    public enum class ElementType
    {
        Empty = PDE_TYPE_EMPTY,
        Bytes = PDE_TYPE_BYTES,
        File = PDE_TYPE_FILE
    };

    public ref class CefPostDataElementWrapper
    {
        MCefRefPtr<CefPostDataElement> _wrappedPostDataElement;

    internal:
        CefPostDataElementWrapper(CefRefPtr<CefPostDataElement> cefPostDataElement) : _wrappedPostDataElement(cefPostDataElement) {}

    public:
        property ElementType^ Type { ElementType^ get(); }
        property String^ File { String^ get(); }
        property array<Byte>^ Bytes { array<Byte>^ get(); }
        property Int32^ BytesCount { Int32^ get(); }
    };
}