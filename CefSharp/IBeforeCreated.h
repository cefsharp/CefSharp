#include "stdafx.h"
#pragma once

using namespace System;

namespace CefSharp 
{
    public interface class IBeforeCreated
    {
    public:

      /// called before a browser is created.
      /// return true to prevent the browser from being created.
      bool HandleBeforeCreated(bool popup, String^ url);
    };
}