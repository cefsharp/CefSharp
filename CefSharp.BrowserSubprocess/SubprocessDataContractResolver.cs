using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using CefSharp.Internals;

namespace CefSharp.BrowserSubprocess
{
    public class SubprocessDataContractResolver : DataContractResolver
    {
        public SubprocessDataContractResolver()
        {
            typesByName = new Dictionary<string, Type>();
            typesByName.Add(typeof(JavascriptObject).Name, typeof(JavascriptObjectWrapper));
            typesByName.Add(typeof(JavascriptProperty).Name, typeof(JavascriptPropertyWrapper));
            typesByName.Add(typeof(JavascriptMethod).Name, typeof(JavascriptMethodWrapper));
        }

        private Dictionary<string, Type> typesByName;

        public override bool TryResolveType(Type type, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
        {
            typeName = null;
            typeNamespace = null;
            return false;
        }

        public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
        {
            Type result;
            if (!typesByName.TryGetValue(typeName, out result))
            {
                result = knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, null);
            }
            return result;
        }
    }
}
