
#include <include/cef_runnable.h>

using namespace System;
using namespace System::Threading::Tasks;
using namespace System::Runtime::InteropServices;




typedef void(*Continue)(Task^ task);
typedef void(*Functor)();


CefRefPtr<CefTask> GetCefTaskFromTask(Task^ task)
{
    auto intPtr = Marshal::GetFunctionPointerForDelegate(gcnew Action(task, &Task::RunSynchronously));

    return NewCefRunnableFunction(static_cast<Functor>(intPtr.ToPointer()));
}