// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using Xunit;

namespace CefSharp.Test
{
    public class SkipIfRunOnAppVeyorFact : FactAttribute
    {
        public SkipIfRunOnAppVeyorFact()
        {
            if(Environment.GetEnvironmentVariable("APPVEYOR") == "True")
            {
                Skip = "Running on Appveyor - Test Skipped";
            }
        }
    }
}
