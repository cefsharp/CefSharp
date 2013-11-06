// Copyright © 2013 The CefSharp Project. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using namespace System::ServiceModel;

namespace CefSharp
{
    namespace Internals
    {
        namespace JavascriptBinding
        {
            // TODO: Should be internal (a.k.a. private in C++), but I couldn't manage to get InternalsVisibleTo functioning
            // properly between C++ projects...
            [ServiceContract]
            public interface class IJavascriptProxy
            {
                [OperationContract]
                Object^ EvaluateScript(int frameId, String^ script, double timeout);

				[OperationContract]
				void Terminate();
            };
        }
    }
}