// Copyright © 2018 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

#include "Stdafx.h"

#include "include\cef_values.h"
#include "CefWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        [System::Diagnostics::DebuggerDisplay("{DebuggerDisplay,nq}")]
        private ref class CefValueWrapper : public IValue, public CefWrapper
        {
        private:
            MCefRefPtr<CefValue> _cefValue;

            property String^ DebuggerDisplay
            {
                String^ get()
                {
                    if (IsDisposed)
                    {
                        return "CefValueWrapper";
                    }

                    if (Type == Enums::ValueType::String)
                    {
                        return GetString();
                    }

                    if (Type == Enums::ValueType::Bool)
                    {
                        return GetBool().ToString();
                    }

                    if (Type == Enums::ValueType::Int)
                    {
                        return GetInt().ToString();
                    }

                    if (Type == Enums::ValueType::Double)
                    {
                        return GetDouble().ToString();
                    }

                    return "CefValueWrapper: " + Type.ToString();
                }
            }

        internal:
            CefValueWrapper(CefRefPtr<CefValue> &cefValue) : _cefValue(cefValue)
            {
            }

            !CefValueWrapper()
            {
                _cefValue = nullptr;
            }

            ~CefValueWrapper()
            {
                this->!CefValueWrapper();

                _disposed = true;
            }

        public:
            virtual bool GetBool();
            virtual double GetDouble();
            virtual int GetInt();
            virtual String^ GetString();
            virtual IDictionary<String^, IValue^>^ GetDictionary();
            virtual IList<IValue^>^ GetList();
            virtual Object^ GetObject();

            virtual property Enums::ValueType Type
            {
                Enums::ValueType get()
                {
                    ThrowIfDisposed();

                    return (Enums::ValueType)_cefValue->GetType();
                }
            }
        };
    }
}
