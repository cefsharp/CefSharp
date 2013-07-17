// Copyright © 2010-2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

#include "Settings.h"
#include "include\cef_version.h"

using namespace System::Reflection;

namespace CefSharp
{
    public ref class Cef
    {
    public:
        void Initialize(Settings^ settings);

        // TODO: Questionable as to whether this should be here or in ClientAdapter.h or elsewhere...
        void NavigateTo(Uri^ uri);

        static property String^ CefSharpVersion
        {
            String^ get()
            {
                Assembly^ assembly = Assembly::GetAssembly(Cef::typeid);
                return assembly->GetName()->Version->ToString();
            }
        }

        static property String^ CefVersion
        {
            String^ get()
            {
                return String::Format("r{0}", CEF_REVISION);
            }
        }

        static property String^ ChromiumVersion
        {
            String^ get()
            {
                // TODO: Get rid of C4965 which we get here for the current Chromium version... :)
                return String::Format("{0}.{1}.{2}.{3}",
                    CHROME_VERSION_MAJOR, CHROME_VERSION_MINOR,
                    CHROME_VERSION_BUILD, CHROME_VERSION_PATCH);
            }
        }
    };
}