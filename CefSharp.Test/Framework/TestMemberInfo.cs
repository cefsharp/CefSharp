// Copyright © 2020 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.


using System;
using System.Reflection;

namespace CefSharp.Test.Framework
{
    public class TestMemberInfo : MemberInfo
    {
        private readonly string name;

        public TestMemberInfo(string name)
        {
            this.name = name;
        }

        public override MemberTypes MemberType
        {
            get { throw new NotImplementedException(); }
        }

        public override string Name => name;

        public override Type DeclaringType
        {
            get { throw new NotImplementedException(); }
        }

        public override Type ReflectedType
        {
            get { throw new NotImplementedException(); }
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }
    }
}
