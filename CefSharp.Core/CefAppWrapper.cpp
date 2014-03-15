#include "Stdafx.h"

#include "Wrappers\CefAppWrapper.h"

namespace CefSharp
{
    CefAppWrapper::CefAppWrapper()
    {        
        cefApp = new CefAppUnmanagedWrapper(this);
    };

    void CefAppWrapper::DoDispose( bool disposing )
    {
        cefApp = nullptr;

        CefAppBase::DoDispose(disposing);
    };


    int CefAppWrapper::Run(array<String^>^ args)
    {
        FindParentProcessId(args);

        auto hInstance = Process::GetCurrentProcess()->Handle;

        CefMainArgs cefMainArgs((HINSTANCE)hInstance.ToPointer());

        auto ptr = cefApp.get();
        return CefExecuteProcess(cefMainArgs, ptr);
    }
}