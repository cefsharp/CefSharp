// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;

namespace CefSharp.Internals
{
    /// <summary>
    /// Base classes for Feezable settings objects
    /// </summary>
    public class FreezableBase
    {
        private bool frozen;

        public void Freeze()
        {
            frozen = true;
        }

        protected void ThrowIfFrozen([System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            if (frozen)
            {
                throw new Exception(GetType().Name + "." + memberName + " can no longer be modified, settings must be changed before the underlying browser has been created.");
            }
        }
    }
}
