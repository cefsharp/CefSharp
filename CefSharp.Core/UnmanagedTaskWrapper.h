
#include <include/cef_runnable.h>

using namespace System;
using namespace System::Threading::Tasks;
using namespace System::Runtime::InteropServices;



typedef void(*Functor)();

CefRefPtr<CefTask> GetCefTaskFromTask(Task^ task)
{
    return NewCefRunnableFunction((Functor)(void*)Marshal::GetFunctionPointerForDelegate(gcnew Action(task, &Task::RunSynchronously)));
}