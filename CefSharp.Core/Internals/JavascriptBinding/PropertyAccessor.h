#include "Stdafx.h"
#include "UnmanagedWrapper.h"

namespace CefSharp
{
    namespace Internals
    {
        namespace JavascriptBinding
        {
            private class PropertyAccessor : CefV8Accessor
            {
                ///
                // Handle retrieval the accessor value identified by |name|. |object| is the
                // receiver ('this' object) of the accessor. If retrieval succeeds set
                // |retval| to the return value. If retrieval fails set |exception| to the
                // exception that will be thrown. Return true if accessor retrieval was
                // handled.
                ///
                /*--cef()--*/
                virtual bool Get(const CefString& name, const CefRefPtr<CefV8Value> object, CefRefPtr<CefV8Value>& retval,
                    CefString& exception) override
                {
                    auto unmanagedWrapper = static_cast<UnmanagedWrapper*>(object->GetUserData().get());
                    auto clrName = toClr(name);
                    clrName = unmanagedWrapper->GetPropertyMapping(clrName);
                    PropertyInfo^ property;

                    if (unmanagedWrapper->Properties->TryGetValue(clrName, property))
                    {
                        auto wrappedObject = unmanagedWrapper->Get();

                        if (wrappedObject == nullptr)
                        {
                            exception = "Binding's CLR object is null.";
                            return true;
                        }

                        try
                        {
                            auto clrValue = property->GetValue(wrappedObject, nullptr);

                            retval = convertToCef(clrValue, nullptr);
                            return true;
                        }
                        catch (Exception^ e)
                        {
                            exception = toNative(e->Message);
                            return true;
                        }
                    }
                    else
                    {
                        // Will probably never get here in reality, since V8 knows the name of the properties that exist on this
                        // object and will only call us for existent properties.
                        return false;
                    }
                }

                ///
                // Handle assignment of the accessor value identified by |name|. |object| is
                // the receiver ('this' object) of the accessor. |value| is the new value
                // being assigned to the accessor. If assignment fails set |exception| to the
                // exception that will be thrown. Return true if accessor assignment was
                // handled.
                ///
                /*--cef()--*/
                virtual bool Set(const CefString& name, const CefRefPtr<CefV8Value> object, const CefRefPtr<CefV8Value> value,
                    CefString& exception) override
                {
                    auto unmanagedWrapper = static_cast<UnmanagedWrapper*>(object->GetUserData().get());
                    auto clrName = toClr(name);
                    PropertyInfo^ property;

                    if (unmanagedWrapper->Properties->TryGetValue(clrName, property))
                    {
                        auto wrappedObject = unmanagedWrapper->Get();

                        if (wrappedObject == nullptr)
                        {
                            exception = "Binding's CLR object is null.";
                            return true;
                        }

                        auto clrValue = convertFromCef(value);
                        property->SetValue(wrappedObject, clrValue, nullptr);

                        return true;
                    }
                    else
                    {
                        // Will probably never get here in reality, since V8 knows the name of the properties that exist on this
                        // object and will only call us for existent properties.
                        return false;
                    }
                }

                IMPLEMENT_REFCOUNTING(PropertyAccessor)
            };
        }
    }
}