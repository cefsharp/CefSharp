// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System.Collections.Generic;
using System.Dynamic;

namespace CefSharp
{
    public class DynamicDictionary : DynamicObject
    {
        private readonly Dictionary<string, object> dictionary;

        public DynamicDictionary(Dictionary<string, object> dictionary)
        {
            this.dictionary = dictionary;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            
            if(dictionary.TryGetValue(binder.Name, out result))
            {
                if (result != null && result.GetType() == typeof(Dictionary<string, object>))
                {
                    result = new DynamicDictionary((Dictionary<string, object>)result);
                }
                return true;
            }

            return false;
        }

        public static implicit operator DynamicDictionary(Dictionary<string, object> dictionary)
        {
            return new DynamicDictionary(dictionary);
        }

        public static implicit operator Dictionary<string, object>(DynamicDictionary dynamicDictionary)
        {
            return dynamicDictionary.dictionary;
        }
    }
}
