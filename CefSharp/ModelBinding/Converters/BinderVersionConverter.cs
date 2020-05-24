using System;
using System.ComponentModel;

namespace CefSharp.ModelBinding.Converters
{
    /// <summary>
    ///A converter for <see cref="Version"/>
    /// </summary>
    public class BinderVersionConverter : TypeConverter
    {
        /// <summary>
        /// Allow for <see cref="Version"/> to <see cref="string"/>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(Version) || base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Allow for <see cref="string"/> to <see cref="Version"/>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Version) || base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts a <see cref="Version"/> instance into a <see cref="string"/> 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value">the <see cref="Version"/> instance that will be converted to a string.</param>
        /// <returns>The <see cref="Version"/> converted to a string.</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is Version version)
            {
                return version.ToString();
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Allows for conversions from a <see cref="string"/> to a <see cref="Version"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value">the string that will be parsed</param>
        /// <param name="destinationType"></param>
        /// <returns>The parsed version</returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(Version) || !(value is string stringValue))
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
            return Version.TryParse(stringValue, out var v) ? v : base.ConvertTo(context, culture, value, destinationType);
        }


        /// <summary>
        /// Adds the converter for handling bidirectional conversion between <see cref="string"/> and <see cref="Version"/> objects to the global TypeDescriptor.
        /// It is safe to call this multiple times as subsequent calls just overwrite a previously created instance.
        /// </summary>
        internal static void Register()
        {
            TypeDescriptor.AddAttributes(typeof(string), new TypeConverterAttribute(typeof(BinderVersionConverter)));
        }
    }
}
