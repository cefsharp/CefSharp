//
//  vcclr_local.h - modified version of vcclr.h from c:\Program Files (x86)\Microsoft Visual Studio 11.0\VC\include\vcclr.h.
//  Modifications indicated below.
//
//	Copyright (C) Microsoft Corporation
//	All rights reserved.
//

#if _MSC_VER > 1000
#pragma once
#endif

#if !defined(_INC_VCCLR)
#define _INC_VCCLR
#ifndef RC_INVOKED

// Deliberately disabled, since this causes mscorlib.dll to be referenced twice from two different locations, breaking
// IntelliSense whenever this header file is included.
//#using <mscorlib.dll>
#include <gcroot.h>

#pragma warning(push)
#pragma warning(disable:4400)

#ifdef __cplusplus_cli
typedef cli::interior_ptr<const System::Char> __const_Char_ptr;
typedef cli::interior_ptr<const System::Byte> __const_Byte_ptr;
typedef cli::interior_ptr<System::Byte> _Byte_ptr;
typedef const System::String^ __const_String_handle;
#define _NULLPTR nullptr
#else
typedef const System::Char* __const_Char_ptr;
typedef const System::Byte* __const_Byte_ptr;
typedef System::Byte* _Byte_ptr;
typedef const System::String* __const_String_handle;
#define _NULLPTR 0
#endif


//
// get an interior gc pointer to the first character contained in a System::String object
//
inline __const_Char_ptr PtrToStringChars(__const_String_handle s) {

    _Byte_ptr bp = const_cast<_Byte_ptr>(reinterpret_cast<__const_Byte_ptr>(s));
    if( bp != _NULLPTR ) {
        unsigned offset = System::Runtime::CompilerServices::RuntimeHelpers::OffsetToStringData;
        bp += offset;
    }
    return reinterpret_cast<__const_Char_ptr>(bp);
}

#pragma warning(pop)

#undef _NULLPTR

#endif /* RC_INVOKED */
#endif //_INC_VCCLR
