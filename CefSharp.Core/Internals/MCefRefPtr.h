// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#pragma once

namespace CefSharp
{
    namespace Internals
    {
        // This class appears to be required in order
        // to continue reference counting in managed classes
        // that the compiler won't let contain CefRefPtr<...>
        template <typename T>
        ref class MCefRefPtr sealed
        {
        private:
            T* _ptr;

        public:
            MCefRefPtr() : _ptr(NULL) {}

            MCefRefPtr(T* p) : _ptr(p)
            {
                if (_ptr)
                {
                    _ptr->AddRef();
                }
            }

            MCefRefPtr(const MCefRefPtr<T>% r) : _ptr(r._ptr)
            {
                if (_ptr)
                {
                    _ptr->AddRef();
                }
            }

            MCefRefPtr(const CefRefPtr<T>& r) : _ptr(r.get())
            {
                if (_ptr)
                {
                    _ptr->AddRef();
                }
            }

            !MCefRefPtr()
            {
                if (_ptr)
                {
                    _ptr->Release();
                    // Be paranoid about preventing a double release
                    // from this managed instance.
                    _ptr = nullptr;
                }
            }

            ~MCefRefPtr()
            {
                // Normally, we would invoke the finalizer method here
                // via !classname, however... the overloaded -> operator
                // prevents that from being feasible.
                if (_ptr)
                {
                    _ptr->Release();
                    // Be paranoid about preventing a double release
                    // from this managed instance.
                    _ptr = nullptr;
                }
            }

            T* get()
            {
                return _ptr;
            }

            /*
            * commented out for now as this operator interferes with the
            * return statement of MCefRefPtr<T>% operator=(T* p)
            operator T*()
            {
            return _ptr;
            }
            */

            T* operator->()
            {
                return _ptr;
            }

            MCefRefPtr<T>% operator=(T* p)
            {
                // AddRef first so that self assignment should work
                if (p)
                {
                    p->AddRef();
                }
                if (_ptr)
                {
                    _ptr->Release();
                }

                _ptr = p;
                return *this;
            }

            MCefRefPtr<T>% operator=(const MCefRefPtr<T>% r)
            {
                return %this = r._ptr;
            }

            void swap(T** pp)
            {
                T* p = _ptr;
                _ptr = *pp;
                *pp = p;
            }

            void swap(MCefRefPtr<T>% r)
            {
                swap(%r._ptr);
            }

            virtual bool Equals(Object^ obj) override
            {
                if (obj->GetType() == GetType())
                {
                    MCefRefPtr^ other = safe_cast<MCefRefPtr^>(obj);
                    return (*other)._ptr == _ptr;
                }

                return false;
            }

            virtual int GetHashCode() override
            {
                return (int)_ptr;
            }
        };
    }
}