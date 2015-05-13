using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CefSharp.Internals
{
    internal class JavascriptTypeBinder : Binder
    {
        private class BinderState
        {
            public object[] args;
        }
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
                // Add this to JavascriptTypeBinder.explicitConversions
                var type = typeof(F);
                if (!explicitConversions.ContainsKey(type))
                    explicitConversions[type] = new Dictionary<Type, IConvert>();
                explicitConversions[type][typeof(T)] = this;
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
            implicitConversions[TypeCode.SByte] = new TypeCode[] {
                TypeCode.Int16,
                TypeCode.Int32,
                TypeCode.Int64,
                TypeCode.Single,
                TypeCode.Double,
            };
            implicitConversions[TypeCode.Byte] = new TypeCode[] {
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
            implicitConversions[TypeCode.Int16] = new TypeCode[] {
                TypeCode.Int32,
                TypeCode.Int64,
                TypeCode.Single,
                TypeCode.Double,
                TypeCode.Decimal,
            };
            implicitConversions[TypeCode.UInt16] = new TypeCode[] {
                TypeCode.Int32,
                TypeCode.UInt32,
                TypeCode.Int64,
                TypeCode.UInt64,
                TypeCode.Single,
                TypeCode.Double,
                TypeCode.Decimal,
            };
            implicitConversions[TypeCode.Int32] = new TypeCode[] {
                TypeCode.Int64,
                TypeCode.Single,
                TypeCode.Double,
                TypeCode.Decimal,
            };
            implicitConversions[TypeCode.UInt32] = new TypeCode[] {
                TypeCode.Int64,
                TypeCode.UInt64,
                TypeCode.Single,
                TypeCode.Double,
                TypeCode.Decimal,
            };
            implicitConversions[TypeCode.Int64] = new TypeCode[] {
                TypeCode.Single,
                TypeCode.Double,
                TypeCode.Decimal,
            };
            implicitConversions[TypeCode.Char] = new TypeCode[] {
                TypeCode.UInt16,
                TypeCode.Int32,
                TypeCode.UInt32,
                TypeCode.Int64,
                TypeCode.UInt64,
                TypeCode.Single,
                TypeCode.Double,
                TypeCode.Decimal,
            };
            implicitConversions[TypeCode.Single] = new TypeCode[] {
                TypeCode.Double,
            };
            implicitConversions[TypeCode.UInt64] = new TypeCode[] {
                TypeCode.Single,
                TypeCode.Double,
                TypeCode.Decimal,
            };

            // signed to unsigned conversion to silence overflow exception
            new Converter<int, byte>((v) => (byte) v);
            new Converter<int, char>((v) => (char) v); // char is 16bit unsigned
            new Converter<int, ushort>((v) => (ushort) v);
            new Converter<int, uint>((v) => (uint) v);
            new Converter<int, ulong>((v) => (ulong) v);
            new Converter<long, ulong>((v) => (ulong) v);

            new Converter<double, char>((v) => (char) v); // char is 16bit unsigned

            new Converter<int, bool?>((v) => v != 0);
            new Converter<int, sbyte?>((v) => (sbyte) v);
            new Converter<int, byte?>((v) => (byte) v);
            new Converter<int, short?>((v) => (short) v);
            new Converter<int, ushort?>((v) => (ushort) v);
            new Converter<int, char?>((v) => (char) v);
            new Converter<int, uint?>((v) => (uint) v);
            new Converter<int, long?>((v) => (long) v);
            new Converter<int, ulong?>((v) => (ulong) v);
            new Converter<int, Single?>((v) => (Single) v);
            new Converter<int, decimal?>((v) => (decimal) v);
            new Converter<int, double?>((v) => (double) v);

            new Converter<double, bool?>((v) => v != 0);
            new Converter<double, sbyte?>((v) => (sbyte) v);
            new Converter<double, byte?>((v) => (byte) v);
            new Converter<double, short?>((v) => (short) v);
            new Converter<double, char?>((v) => (char) v);
            new Converter<double, ushort?>((v) => (ushort) v);
            new Converter<double, int?>((v) => (int) v);
            new Converter<double, uint?>((v) => (uint) v);
            new Converter<double, long?>((v) => (long) v);
            new Converter<double, ulong?>((v) => (ulong) v);
            new Converter<double, Single?>((v) => (Single) v);
            new Converter<double, decimal?>((v) => (decimal) v);
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
            BinderState myBinderState = new BinderState();
            object[] arguments = new Object[args.Length];
            args.CopyTo(arguments, 0);
            myBinderState.args = arguments;
            state = myBinderState;
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
                                args[j] = myBinderState.args[k];
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
            ((BinderState) state).args.CopyTo(args, 0);
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
                    else
                    {
                        continue;
                    }
                }
            }
            return null;
        }

        // Determines whether type1 can be converted to type2. Check only for primitive types. 
        private bool CanConvertFrom(Type type1, Type type2)
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
