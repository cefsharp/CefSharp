using System;
using System.ComponentModel;

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// A special <see cref="Guid"/> type converter that is registered to allow for easy conversion between Javascript & .NET bridge.
    /// </summary>
    internal class BinderGuidConverter : GuidConverter
    {
        /// <summary>
        /// Guid to string? That is okay!
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(Guid) || base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// String to Guid? You know it!
        /// </summary>
        /// <param name="context"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(Guid) || base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts a <see cref="Guid"/> instance into a string using <see cref="Guid.ToString"/> with the format specifier "N"
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value">the <see cref="Guid"/> structure that will be converted to a string.</param>
        /// <returns>The <see cref="Guid"/> converted to a string.</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is Guid guid)
            {
                return guid.ToString("N");
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Allows for conversions from a string to a <see cref="Guid"/>.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="culture"></param>
        /// <param name="value">the string that will be parsed</param>
        /// <param name="destinationType"></param>
        /// <returns>The parsed structure or <see cref="Guid.Empty"/> on failure</returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(Guid) && value is string stringValue)
            {
                if (string.IsNullOrWhiteSpace(stringValue))
                {
                    // this seems most appropriate 
                    return Guid.Empty;
                }
                return Guid.TryParse(stringValue, out var result) ? result : Guid.Empty;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }


        /// <summary>
        /// Adds the converter for handling bidirectional conversion between <see cref="string"/> and <see cref="Guid"/> objects to the global TypeDescriptor.
        /// It is safe to call this multiple times as subsequent calls just overwrite a previously created instance.
        /// </summary>
        internal static void Register()
        {
           TypeDescriptor.AddAttributes(typeof(string), new TypeConverterAttribute(typeof(BinderGuidConverter)));
        }
    }
}
