using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using CefSharp.Internals;

namespace CefSharp.BrowserSubprocess
{
    public class SubprocessSurrogates : IDataContractSurrogate
    {
        private Dictionary<Type, Type> surrogates;

        public SubprocessSurrogates()
        {
            surrogates = new Dictionary<Type, Type>();
            surrogates.Add(typeof(JavascriptObject), typeof(JavascriptObjectWrapper));
            surrogates.Add(typeof(JavascriptProperty), typeof(JavascriptPropertyWrapper));
            surrogates.Add(typeof(JavascriptMethod), typeof(JavascriptMethodWrapper));
        }

        public Type GetDataContractType(Type type)
        {
            Type surrogate;
            if (surrogates.TryGetValue(type, out surrogate))
            {
                return surrogate;
            }
            return type;
        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            throw new NotImplementedException();
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            return obj;
        }

        public object GetCustomDataToExport(MemberInfo memberInfo, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public object GetCustomDataToExport(Type clrType, Type dataContractType)
        {
            throw new NotImplementedException();
        }

        public void GetKnownCustomDataTypes(Collection<Type> customDataTypes)
        {
            throw new NotImplementedException();
        }

        public Type GetReferencedTypeOnImport(string typeName, string typeNamespace, object customData)
        {
            throw new NotImplementedException();
        }

        public CodeTypeDeclaration ProcessImportedType(CodeTypeDeclaration typeDeclaration, CodeCompileUnit compileUnit)
        {
            throw new NotImplementedException();
        }
    }
}
