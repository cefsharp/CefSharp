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
                    auto wrappedObject = unmanagedWrapper->Get();

                    if (wrappedObject == nullptr)
                    {
                        exception = "Binding's CLR object is null.";
                        return true;
                    }

                    auto clrName = toClr(name);
#ifdef CHANGE_FIRST_CHAR_TO_LOWER
                    clrName = unmanagedWrapper->GetPropertyMapping(clrName);
#endif
                    auto prop = wrappedObject->GetType()->GetProperty(clrName, BindingFlags::Instance | BindingFlags::Public);

                    if (prop == nullptr)
                    {
                        exception = toNative("No member named " + clrName + " to read.");
                        return true;
                    }

                    try
                    {
                        auto clrValue = prop->GetValue(wrappedObject, nullptr);
                        retval = clrValue == wrappedObject ? object : convertToCef(clrValue, prop->PropertyType, object);
                    }
                    catch (Exception^ e)
                    {
                        exception = toNative(e->Message);
                     }
                    return true;
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
                    auto wrappedObject = unmanagedWrapper->Get();

                    if (wrappedObject == nullptr)
                    {
                        exception = "Binding's CLR object is null.";
                        return true;
                    }

                    auto clrName = toClr(name);
#ifdef CHANGE_FIRST_CHAR_TO_LOWER
                    clrName = unmanagedWrapper->GetPropertyMapping(clrName);
#endif
                    auto prop = wrappedObject->GetType()->GetProperty(clrName, BindingFlags::Instance | BindingFlags::Public);

                    if (prop == nullptr)
                    {
                        exception = toNative("No member named " + clrName + " to write.");
                        return true;
                    }

                    try
                    { 
                        auto clrValue = value == object ? wrappedObject : convertFromCef(value);
                        prop->SetValue(wrappedObject, BindingHandler::ChangeType(clrValue, prop->PropertyType), nullptr);
                    }
                    catch (Exception^ e)
                    {
                        exception = toNative(e->Message);
                    }
                    return true;
                }

                IMPLEMENT_REFCOUNTING(PropertyAccessor)
            };
        }
    }
}