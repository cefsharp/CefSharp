// Copyright © 2010-2015 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

    using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CefSharp.Internals
{
    internal class JavascriptTypeBinder : Binder
    {
        interface IConvert
        {
            object Convert(object obj);
        }

        class Converter<F, T> : IConvert
        {
            Func<F, T> convert;

            public Converter(Func<F, T> func)
            {
                this.convert = func;
            }

            public object Convert(object obj)
            {
                return convert((F) obj);
            }
        }

        public static readonly JavascriptTypeBinder Singleton = new JavascriptTypeBinder();
        /// <summary>
        /// A Dictionary that stores the Implicit Numeric Conversions Table
        /// </summary>
        /// <see cref="https://msdn.microsoft.com/en-us/library/y5b434w4%28v=vs.140%29.aspx"/>
        static Dictionary<TypeCode, TypeCode[]> implicitConversions = new Dictionary<TypeCode, TypeCode[]>();
        /// <summary>
        /// Explicit Numeric Conversions Table
        /// </summary>
        static Dictionary<Type, Dictionary<Type, IConvert>> explicitConversions = new Dictionary<Type, Dictionary<Type, IConvert>>();

        static JavascriptTypeBinder()
        {
            implicitConversions[TypeCode.SByte] = new TypeCode[] 
            {
                TypeCode.Int16,
                TypeCode.Int32,
                TypeCode.Int64,
                TypeCode.Single,
                TypeCode.Double,
            };
            implicitConversions[TypeCode.Byte] = new TypeCode[] 
            {
                TypeCode.Int16,
                TypeCode.UInt16,
                TypeCode.Int32,
                TypeCode.UInt32,
                TypeCode.Int64,
                TypeCode.UInt64,
                TypeCode.Single,
                TypeCode.Double,
                TypeCode.Decimal,
            };
            implicitConversions[TypeCode.Int16] = new TypeCode[] 
            {
                TypeCode.Int32,
                TypeCode.Int64,
                TypeCode.Single,
                TypeCode.Double,
                TypeCode.Decimal,
            };
            implicitConversions[TypeCode.UInt16] = new TypeCode[]
            {
                TypeCode.Int32,
                TypeCode.UInt32,
                TypeCode.Int64,
                TypeCode.UInt64,
                TypeCode.Single,
                TypeCode.Double,
                TypeCode.Decimal,
            };
            implicitConversions[TypeCode.Int32] = new TypeCode[]
            {
                TypeCode.Int64,
                TypeCode.Single,
                TypeCode.Double,
                TypeCode.Decimal,
            };
            implicitConversions[TypeCode.UInt32] = new TypeCode[] 
            {
                TypeCode.Int64,
                TypeCode.UInt64,
                TypeCode.Single,
                TypeCode.Double,
                TypeCode.Decimal,
            };
            implicitConversions[TypeCode.Int64] = new TypeCode[] 
            {
                TypeCode.Single,
                TypeCode.Double,
                TypeCode.Decimal,
            };
            implicitConversions[TypeCode.Char] = new TypeCode[] 
            {
                TypeCode.UInt16,
                TypeCode.Int32,
                TypeCode.UInt32,
                TypeCode.Int64,
                TypeCode.UInt64,
                TypeCode.Single,
                TypeCode.Double,
                TypeCode.Decimal,
            };
            implicitConversions[TypeCode.Single] = new TypeCode[] 
            {
                TypeCode.Double,
            };
            implicitConversions[TypeCode.UInt64] = new TypeCode[]
            {
                TypeCode.Single,
                TypeCode.Double,
                TypeCode.Decimal,
            };

            // signed to unsigned conversion to silence overflow exception
            AddConverter<int, byte>(v => (byte) v);
            AddConverter<int, char>(v => (char) v); // char is 16bit unsigned
            AddConverter<int, ushort>(v => (ushort) v);
            AddConverter<int, uint>(v => (uint) v);
            AddConverter<int, ulong>(v => (ulong) v);
            AddConverter<long, ulong>(v => (ulong) v);

            AddConverter<double, char>(v => (char) v); // char is 16bit unsigned

            AddConverter<int, bool?>(v => v != 0);
            AddConverter<int, sbyte?>(v => (sbyte) v);
            AddConverter<int, byte?>(v => (byte) v);
            AddConverter<int, short?>(v => (short) v);
            AddConverter<int, ushort?>(v => (ushort) v);
            AddConverter<int, char?>(v => (char) v);
            AddConverter<int, uint?>(v => (uint) v);
            AddConverter<int, long?>(v => (long) v);
            AddConverter<int, ulong?>(v => (ulong) v);
            AddConverter<int, Single?>(v => (Single) v);
            AddConverter<int, decimal?>(v => (decimal) v);
            AddConverter<int, double?>(v => (double) v);

            AddConverter<double, bool?>(v => v != 0);
            AddConverter<double, sbyte?>(v => (sbyte) v);
            AddConverter<double, byte?>(v => (byte) v);
            AddConverter<double, short?>(v => (short) v);
            AddConverter<double, char?>(v => (char) v);
            AddConverter<double, ushort?>(v => (ushort) v);
            AddConverter<double, int?>(v => (int) v);
            AddConverter<double, uint?>(v => (uint) v);
            AddConverter<double, long?>(v => (long) v);
            AddConverter<double, ulong?>(v => (ulong) v);
            AddConverter<double, Single?>(v => (Single) v);
            AddConverter<double, decimal?>(v => (decimal) v);
        }

        private static void AddConverter<F, T>(Func<F, T> func)
        {
            var type = typeof(F);
            if (!explicitConversions.ContainsKey(type))
            {
                explicitConversions[type] = new Dictionary<Type, IConvert>();
            }
            explicitConversions[type][typeof(T)] = new Converter<F, T>(func);
        }

        public override FieldInfo BindToField(BindingFlags bindingAttr, FieldInfo[] match, object value, CultureInfo culture)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }
            // Get a field for which the value parameter can be converted to the specified field type. 
            for (int i = 0; i < match.Length; i++)
            {
                if (ChangeType(value, match[i].FieldType, culture) != null)
                {
                    return match[i];
                }
            }
            return null;
        }
        public override MethodBase BindToMethod(BindingFlags bindingAttr, MethodBase[] match, ref object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] names, out object state)
        {
            // Store the arguments to the method in a state object.
            object[] arguments = new Object[args.Length];
            args.CopyTo(arguments, 0);
            state = arguments;
            if (match == null)
            {
                throw new ArgumentNullException();
            }
            // Find a method that has the same parameters as those of the args parameter. 
            for (int i = 0; i < match.Length; i++)
            {
                // Count the number of parameters that match. 
                int count = 0;
                ParameterInfo[] parameters = match[i].GetParameters();
                // Go on to the next method if the number of parameters do not match. 
                if (args.Length != parameters.Length)
                {
                    continue;
                }
                // Match each of the parameters that the user expects the method to have. 
                for (int j = 0; j < args.Length; j++)
                {
                    // If the names parameter is not null, then reorder args. 
                    if (names != null)
                    {
                        if (names.Length != args.Length)
                        {
                            throw new ArgumentException("names and args must have the same number of elements.");
                        }
                        for (int k = 0; k < names.Length; k++)
                        {
                            if (String.Compare(parameters[j].Name, names[k].ToString()) == 0)
                            {
                                args[j] = arguments[k];
                            }
                        }
                    }
                    // Determine whether the types specified by the user can be converted to the parameter type. 
                    if (ChangeType(args[j], parameters[j].ParameterType, culture) != null)
                    {
                        count += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                // Determine whether the method has been found. 
                if (count == args.Length)
                {
                    return match[i];
                }
            }
            return null;
        }       

        public override object ChangeType(object value, Type type, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var originalType = value.GetType();
            if (originalType.IsArray)
            {
                var from = value as Array;
                if (type.IsArray)
                {
                    var toElemType = type.GetElementType();
                    var array = Array.CreateInstance(toElemType, from.Length);
                    for (int i = 0; i < from.Length; i++)
                    {
                        array.SetValue(ChangeType(from.GetValue(i), toElemType, culture), i);
                    }
                    return array;
                }
                else if (type.IsGenericType && type.GetInterface(typeof(IList).Name) != null)
                {
                    var list = Activator.CreateInstance(type) as IList;
                    var toElemType = type.GetGenericArguments()[0];
                    for (int i = 0; i < from.Length; i++)
                    {
                        list.Add(ChangeType(from.GetValue(i), toElemType, culture));
                    }
                    return list;
                }
                return Convert.ChangeType(value, type);
            }
            else
            {
                if (explicitConversions.ContainsKey(originalType) && explicitConversions[originalType].ContainsKey(type))
                {
                    return explicitConversions[originalType][type].Convert(value);
                }
                else
                {
                    return Convert.ChangeType(value, type);
                }
            }
        }

        public override void ReorderArgumentArray(ref object[] args, object state)
        {
            // Return the args that had been reordered by BindToMethod.
            ((object[]) state).CopyTo(args, 0);
        }

        public override MethodBase SelectMethod(BindingFlags bindingAttr, MethodBase[] match, Type[] types, ParameterModifier[] modifiers)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }
            for (int i = 0; i < match.Length; i++)
            {
                // Count the number of parameters that match. 
                int count = 0;
                ParameterInfo[] parameters = match[i].GetParameters();
                // Go on to the next method if the number of parameters do not match. 
                if (types.Length != parameters.Length)
                {
                    continue;
                }
                // Match each of the parameters that the user expects the method to have. 
                for (int j = 0; j < types.Length; j++)
                {
                    // Determine whether the types specified by the user can be converted to parameter type. 
                    if (CanConvertFrom(types[j], parameters[j].ParameterType))
                    {
                        count += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                // Determine whether the method has been found. 
                if (count == types.Length)
                {
                    return match[i];
                }
            }
            return null;
        }
        public override PropertyInfo SelectProperty(BindingFlags bindingAttr, PropertyInfo[] match, Type returnType, Type[] indexes, ParameterModifier[] modifiers)
        {
            if (match == null)
            {
                throw new ArgumentNullException("match");
            }
            return SelectProperty(match, returnType, indexes, true, false) ??
               SelectProperty(match, returnType, indexes, false, true) ??
               SelectProperty(match, returnType, indexes, false, false);
        }

        private PropertyInfo SelectProperty(PropertyInfo[] match, Type returnType, Type[] indexes, bool matchParameterType, bool matchReturnType)
        {
            for (int i = 0; i < match.Length; i++)
            {
                // Count the number of indexes that match. 
                int count = 0;
                ParameterInfo[] parameters = match[i].GetIndexParameters();
                // Go on to the next property if the number of indexes do not match. 
                if (indexes.Length != parameters.Length)
                {
                    continue;
                }
                // Match each of the indexes that the user expects the property to have. 
                for (int j = 0; j < indexes.Length; j++)
                {
                    // Determine whether the types specified by the user can be converted to index type. 
                    if ((matchParameterType && indexes[j] == parameters[j].ParameterType) || (!matchParameterType && CanConvertFrom(indexes[j], parameters[j].ParameterType)))
                    {
                        count += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                // Determine whether the property has been found. 
                if (count == indexes.Length)
                {
                    // Determine whether the return type can be converted to the properties type. 
                    if ((matchReturnType && returnType == match[i].PropertyType) || (!matchReturnType && CanConvertFrom(returnType, match[i].PropertyType)))
                    {
                        return match[i];
                    }
                }
            }
            return null;
        }

        // Determines whether type1 can be converted to type2. Check only for primitive types. 
        private static bool CanConvertFrom(Type type1, Type type2)
        {
            if (type1.IsPrimitive && type2.IsPrimitive)
            {
                TypeCode typeCode1 = Type.GetTypeCode(type1);
                TypeCode typeCode2 = Type.GetTypeCode(type2);
                // If both type1 and type2 have the same type, return true. Boolean can also be converted to any type
                if (typeCode1 == typeCode2 || typeCode1 == TypeCode.Boolean)
                {
                    return true;
                }
                return implicitConversions.ContainsKey(typeCode1) && implicitConversions[typeCode1].Contains(typeCode2);
            }
            return false;
        }
    }
}
