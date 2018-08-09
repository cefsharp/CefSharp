// Copyright © 2010-2018 The CefSharp Authors. All rights reserved.
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
        private ref class CefValueWrapper : public ICefValue, public CefWrapper
        {
        private:
            MCefRefPtr<CefValue> _cefValue;

        internal:
            CefValueWrapper(CefRefPtr<CefValue> &cefValue) : _cefValue(cefValue)
            {
            }

            !CefValueWrapper()
            {
                _cefValue = NULL;
            }

            ~CefValueWrapper()
            {
                this->!CefValueWrapper();

                _disposed = true;
            }

        public: 
            virtual Enums::CefValueType GetCefValueType();
            virtual bool GetBool();
            virtual double GetDouble();
            virtual int GetInt();
            virtual String^ GetString();
            virtual Dictionary<String^, ICefValue^>^ GetDictionary();
            virtual List<ICefValue^>^ GetList();
        };
    }
}