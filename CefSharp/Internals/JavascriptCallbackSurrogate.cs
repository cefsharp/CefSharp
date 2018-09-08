// Copyright © 2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Serialization;

namespace CefSharp.Internals
{
    internal sealed class JavascriptCallbackSurrogate : IDataContractSurrogate
    {
        private readonly IJavascriptCallbackFactory callbackFactory;

        public JavascriptCallbackSurrogate(IJavascriptCallbackFactory callbackFactory)
        {
            this.callbackFactory = callbackFactory;
        }

        public Type GetDataContractType(Type type)
        {
            if (type == typeof(JavascriptCallback))
            {
                return typeof(IJavascriptCallback);
            }
            return type;
        }

        public object GetObjectToSerialize(object obj, Type targetType)
        {
            return obj;
        }

        public object GetDeserializedObject(object obj, Type targetType)
        {
            var result = obj;
            var dto = obj as JavascriptCallback;
            if (dto != null)
            {
                result = callbackFactory.Create(dto);
            }
            return result;
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
