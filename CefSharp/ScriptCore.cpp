#include "Stdafx.h"
#include "include/cef_runnable.h"
#include "ScriptCore.h"
#include "ScriptException.h"

namespace CefSharp
{
    bool ScriptCore::TryGetMainFrame(CefRefPtr<CefBrowser> browser, CefRefPtr<CefFrame>& frame)
    {
        if (browser != nullptr)
        {
            frame = browser->GetMainFrame();
            return frame != nullptr;
        }
        else
        {
            return false;
        }
    }

    void ScriptCore::UIT_Execute(CefRefPtr<CefBrowser> browser, CefString script)
    {
        CefRefPtr<CefFrame> mainFrame;
        if (TryGetMainFrame(browser, mainFrame))
        {
            mainFrame->ExecuteJavaScript(script, "about:blank", 0);
        }
    }

    void ScriptCore::_UIT_Evaluate(ScriptCore* const _this, CefRefPtr<CefBrowser> browser, CefString script)
    {
        CefRefPtr<CefFrame> mainFrame;
        if (TryGetMainFrame(browser, mainFrame))
        {
            CefRefPtr<CefV8Context> context = mainFrame->GetV8Context();

            if (context.get() && context->Enter())
            {
                CefRefPtr<CefV8Value> result;
                CefRefPtr<CefV8Exception> exception;

                bool success = context->Eval(script, result, exception);
                if (success)
                {
                   try
                    {
                        _this->_result = convertFromCef(result);
                    }
                    catch (Exception^ ex)
                    {
                        _this->_exceptionMessage = ex->Message;
                    }
                }
                else if (exception.get())
                {
                    _this->_exceptionMessage = toClr(exception->GetMessage());
                }
                else
                {
                    _this->_exceptionMessage = "Failed to evaluate script";
                }

                context->Exit();
            }
        }
        else
        {
            _this->_exceptionMessage = "Failed to obtain reference to main frame";
        }

        SetEvent(_this->_event);
    }

    void ScriptCore::UIT_Evaluate(CefRefPtr<CefBrowser> browser, CefString script)
    {
        if (IsCrossDomainCallRequired()) {
            msclr::call_in_appdomain(GetAppDomainId(), &_UIT_Evaluate, this, browser, script);
        } else {
            _UIT_Evaluate(this, browser, script);
        }
    }

    void ScriptCore::_Execute(ScriptCore* const _this, CefRefPtr<CefBrowser> browser, CefString script)
    {
        if (CefCurrentlyOn(TID_UI))
        {
            _this->UIT_Execute(browser, script);
        }
        else
        {
            CefPostTask(TID_UI, NewCefRunnableMethod(_this, &ScriptCore::UIT_Execute,
                browser, script));
        }
    }

    void ScriptCore::Execute(CefRefPtr<CefBrowser> browser, CefString script)
    {
        if (IsCrossDomainCallRequired()) {
            msclr::call_in_appdomain(GetAppDomainId(), &_Execute, this, browser, script);
        } else {
            _Execute(this, browser, script);
        }
    }

    gcroot<Object^> ScriptCore::Evaluate(CefRefPtr<CefBrowser> browser, CefString script, double timeout)
    {
        AutoLock lock_scope(this);

        if (IsCrossDomainCallRequired()) {
            return msclr::call_in_appdomain(GetAppDomainId(), &_Evaluate, this, browser, script, timeout);
        } else {
            return _Evaluate(this, browser, script, timeout);
        }
    }

    gcroot<Object^> ScriptCore::_Evaluate(ScriptCore* const _this, CefRefPtr<CefBrowser> browser, CefString script, double timeout)
    {
        _this->_result = nullptr;
        _this->_exceptionMessage = nullptr;

        if (CefCurrentlyOn(TID_UI))
        {
            _UIT_Evaluate(_this, browser, script);
        }
        else
        {
            CefPostTask(TID_UI, NewCefRunnableMethod(_this, &ScriptCore::UIT_Evaluate,
                browser, script));
        }

        switch (WaitForSingleObject(_this->_event, timeout))
        {
        case WAIT_TIMEOUT:
            throw gcnew ScriptException("Script timed out");
        case WAIT_ABANDONED:
        case WAIT_FAILED:
            throw gcnew ScriptException("Script error");
        }

        if (_this->_exceptionMessage)
        {
            throw gcnew ScriptException(_this->_exceptionMessage);
        }
        else
        {
            return _this->_result;
        }
    }
}