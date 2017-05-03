#pragma once

#include "Stdafx.h"
#include "PostData.h"

namespace CefSharp
{
    Int32^ CefPostDataWrapper::ElementCount::get()
    {
        return gcnew Int32(_wrappedData->GetElementCount());
    }

    IList<CefPostDataElementWrapper^>^ CefPostDataWrapper::GetElements()
    {
        CefPostData::ElementVector ev;

        _wrappedData->GetElements(ev);

        IList<CefPostDataElementWrapper^>^ elementList = gcnew List<CefPostDataElementWrapper^>();

        for (CefPostData::ElementVector::iterator it = ev.begin(); it != ev.end(); ++it)
        {
            CefPostDataElement* elem = it->get();

            elementList->Add(gcnew CefPostDataElementWrapper(elem));
        }

        return elementList;
    }
}