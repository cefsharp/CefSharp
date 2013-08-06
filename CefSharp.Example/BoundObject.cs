﻿using System;

namespace CefSharp.Example
{
    class BoundObject
    {
        public int MyProperty { get; set; }
        public double myProperty { get; private set; } // undef CHANGE_FIRST_CHAR_TO_LOWER to make this property available
        public string MyReadOnlyProperty { get; internal set; }
        public Type MyConvertibleProperty { get; set; }
        public double MyWriteOnlyProperty { set { myProperty = value; } }
        public long[] MyArrayProperty { get; set; }
        public object IntObject { get { return intValue; } set { intValue = Convert.ToInt32(value); } }
        private int intValue = 12345678;
        public BoundObject()
        {
            MyProperty = 42;
            MyReadOnlyProperty = "I'm immutable!";
            MyConvertibleProperty = GetType();
            MyArrayProperty = new long[] { 1, 2, 3 };
        }

        public BoundObject Me { get { return this; } }
        public BoundObject NewMe { get { return new BoundObject(); } }

        public string Repeat(string str, int n)
        {
            string result = String.Empty;
            for (int i = 0; i < n; i++)
            {
                result += str;
            }
            return result;
        }

        public void EchoVoid()
        {
        }

        public Boolean EchoBoolean(Boolean arg0)
        {
            return arg0;
        }

        public Boolean? EchoNullableBoolean(Boolean? arg0)
        {
            return arg0;
        }

        public SByte EchoSByte(SByte arg0)
        {
            return arg0;
        }

        public SByte? EchoNullableSByte(SByte? arg0)
        {
            return arg0;
        }

        public Int16 EchoInt16(Int16 arg0)
        {
            return arg0;
        }

        public Int16? EchoNullableInt16(Int16? arg0)
        {
            return arg0;
        }

        public Int32 EchoInt32(Int32 arg0)
        {
            return arg0;
        }

        public Int32? EchoNullableInt32(Int32? arg0)
        {
            return arg0;
        }

        public Int64 EchoInt64(Int64 arg0)
        {
            return arg0;
        }

        public Int64? EchoNullableInt64(Int64? arg0)
        {
            return arg0;
        }

        public Byte EchoByte(Byte arg0)
        {
            return arg0;
        }

        public Byte? EchoNullableByte(Byte? arg0)
        {
            return arg0;
        }

        public UInt16 EchoUInt16(UInt16 arg0)
        {
            return arg0;
        }

        public UInt16? EchoNullableUInt16(UInt16? arg0)
        {
            return arg0;
        }

        public UInt32 EchoUInt32(UInt32 arg0)
        {
            return arg0;
        }

        public UInt32? EchoNullableUInt32(UInt32? arg0)
        {
            return arg0;
        }

        public UInt64 EchoUInt64(UInt64 arg0)
        {
            return arg0;
        }

        public UInt64? EchoNullableUInt64(UInt64? arg0)
        {
            return arg0;
        }

        public Single EchoSingle(Single arg0)
        {
            return arg0;
        }

        public Single? EchoNullableSingle(Single? arg0)
        {
            return arg0;
        }

        public Double EchoDouble(Double arg0)
        {
            return arg0;
        }

        public Double? EchoNullableDouble(Double? arg0)
        {
            return arg0;
        }

        public Char EchoChar(Char arg0)
        {
            return arg0;
        }

        public Char? EchoNullableChar(Char? arg0)
        {
            return arg0;
        }

        public DateTime EchoDateTime(DateTime arg0)
        {
            return arg0;
        }

        public DateTime? EchoNullableDateTime(DateTime? arg0)
        {
            return arg0;
        }

        public Decimal EchoDecimal(Decimal arg0)
        {
            return arg0;
        }

        public Decimal? EchoNullableDecimal(Decimal? arg0)
        {
            return arg0;
        }

        public String EchoString(String arg0)
        {
            return arg0;
        }

        // This will currently not work, as it causes a collision w/ the EchoString() method.
        //public String echoString(String arg)
        //{
        //    return "Lowercase echo: " + arg;
        //}

        public String lowercaseMethod()
        {
            return "lowercase";
        }
    }
}
