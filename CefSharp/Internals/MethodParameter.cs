using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CefSharp.Internals
{
    [DataContract]
    public class MethodParameter
    {
        public String Name { get; set; }

        public Type ParameterType { get; set; }

        public Boolean IsParamArray { get; set; }

        public Int32 Position { get; set; }
    }
}
