// Copyright © 2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// Represents a bindable member of a type, which can be a property or a field.
    /// </summary>
    public class BindingMemberInfo
    {
        PropertyInfo propertyInfo;
        FieldInfo fieldInfo;

        /// <summary>
        /// Gets a reference to the MemberInfo that this BindingMemberInfo represents. This can be a property or a field.
        /// </summary>
        public MemberInfo MemberInfo
        {
            get { return this.propertyInfo ?? (MemberInfo)this.fieldInfo; }
        }

        /// <summary>
        /// Gets the name of the property or field represented by this BindingMemberInfo.
        /// </summary>
        public string Name
        {
            get { return this.MemberInfo.Name; }
        }

        /// <summary>
        /// Gets the data type of the property or field represented by this BindingMemberInfo.
        /// </summary>
        public Type PropertyType
        {
            get
            {
                if (this.propertyInfo != null)
                {
                    return this.propertyInfo.PropertyType;
                }
                else
                {
                    return this.fieldInfo.FieldType;
                }
            }
        }

        /// <summary>
        /// Constructs a BindingMemberInfo instance for a property.
        /// </summary>
        /// <param name="propertyInfo">The bindable property to represent.</param>
        public BindingMemberInfo(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }

            this.propertyInfo = propertyInfo;
        }

        /// <summary>
        /// Constructs a BindingMemberInfo instance for a field.
        /// </summary>
        /// <param name="fieldInfo">The bindable field to represent.</param>
        public BindingMemberInfo(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
            {
                throw new ArgumentNullException("fieldInfo");
            }

            this.fieldInfo = fieldInfo;
        }

        /// <summary>
        /// Sets the value from a specified object associated with the property or field represented by this BindingMemberInfo.
        /// </summary>
        /// <param name="destinationObject">The object whose property or field should be assigned.</param>
        /// <param name="newValue">The value to assign in the specified object to this BindingMemberInfo's property or field.</param>
        public void SetValue(object destinationObject, object newValue)
        {
            if (this.propertyInfo != null)
            {
                this.propertyInfo.SetValue(destinationObject, newValue, null);
            }
            else
            {
                this.fieldInfo.SetValue(destinationObject, newValue);
            }
        }

        /// <inherit-doc/>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var other = obj as BindingMemberInfo;

            if (other == null)
            {
                return false;
            }

            return this.MemberInfo.Equals(other.MemberInfo);
        }

        /// <summary>
        /// Compares two BindingMemberInfo's with eachother on their respective values rather then their reference
        /// </summary>
        /// <param name="obj">the other BindingMemberInfo</param>
        /// <returns>true when they are equal and false otherwise</returns>
        public bool Equals(BindingMemberInfo obj)
        {
            if (obj == null)
            {
                return false;
            }

            return this.MemberInfo.Equals(obj.MemberInfo);
        }

        /// <inherit-doc/>
        public override int GetHashCode()
        {
            return this.MemberInfo.GetHashCode();
        }

        /// <summary>
        /// Returns an enumerable sequence of bindable properties for the specified type.
        /// </summary>
        /// <typeparam name="T">The type to enumerate.</typeparam>
        /// <returns>Bindable properties.</returns>
        public static IEnumerable<BindingMemberInfo> Collect<T>()
        {
            return Collect(typeof(T));
        }

        /// <summary>
        /// Returns an enumerable sequence of bindable properties for the specified type.
        /// </summary>
        /// <param name="type">The type to enumerate.</param>
        /// <returns>Bindable properties.</returns>
        public static IEnumerable<BindingMemberInfo> Collect(Type type)
        {
            var fromProperties = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite)
                .Where(property => !property.GetIndexParameters().Any())
                .Select(property => new BindingMemberInfo(property));

            var fromFields = type.GetFields(BindingFlags.Public | BindingFlags.Instance).Where(f => !f.IsInitOnly)
                .Select(field => new BindingMemberInfo(field));

            return fromProperties.Concat(fromFields);
        }
    }
}
