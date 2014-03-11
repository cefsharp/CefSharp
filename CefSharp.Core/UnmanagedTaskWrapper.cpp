
#include "Stdafx.h"
#include "UnmanagedTaskWrapper.h"


namespace CefSharp
{
    void CefTaskWrapper::Execute()
    {
        _scheduler->ExecuteTask( *this );
    };
}