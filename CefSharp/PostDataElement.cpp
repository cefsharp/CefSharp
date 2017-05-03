#include "Stdafx.h"

#include "PostDataElement.h"

namespace CefSharp
{
    ElementType^ CefPostDataElementWrapper::Type::get()
    {
        CefPostDataElement::Type type = _wrappedPostDataElement->GetType();
        
        switch (type)
        {
        case PDE_TYPE_EMPTY:
            return ElementType::Empty;
        case PDE_TYPE_BYTES:
            return ElementType::Bytes;
        case PDE_TYPE_FILE:
            return ElementType::File;
        }
    }

    String^ CefPostDataElementWrapper::File::get()
    {
        return toClr(_wrappedPostDataElement->GetFile());
    }

    array<Byte>^ CefPostDataElementWrapper::Bytes::get()
    {
        size_t byteCount = _wrappedPostDataElement->GetBytesCount();
        unsigned char* byteBuffer = new unsigned char[byteCount];

        _wrappedPostDataElement->GetBytes(byteCount, byteBuffer);

        array<Byte>^ clrByteArray = gcnew array<Byte>(byteCount);
        System::Runtime::InteropServices::Marshal::Copy(IntPtr(byteBuffer), clrByteArray, 0, byteCount);
        delete[] byteBuffer;

        return clrByteArray;
    }

    Int32^ CefPostDataElementWrapper::BytesCount::get()
    {
        return gcnew Int32(_wrappedPostDataElement->GetBytesCount());
    }
}