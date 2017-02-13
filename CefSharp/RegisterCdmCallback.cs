// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp
{
    /// <summary>
    /// Provides a simple callback implementation of <see cref="IRegisterCdmCallback"/> for use with Widevine CDM registration.
    /// </summary>
    public class RegisterCdmCallback: IRegisterCdmCallback
    {
        private readonly Action<CdmRegistration> _callback;

        public RegisterCdmCallback(Action<CdmRegistration> callback)
        {
            _callback = callback;
        }

        void IRegisterCdmCallback.OnRegistrationComplete(CdmRegistration registration)
        {
            if (_callback != null)
                _callback(registration);
        }

        void IDisposable.Dispose()
        {
        }
    }
}
