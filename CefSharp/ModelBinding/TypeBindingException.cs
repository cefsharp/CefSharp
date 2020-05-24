using System;
using System.Linq;

namespace CefSharp.ModelBinding
{

    /// <summary>
    /// An attribute set on <see cref="BindingFailureCode"/> fields to provide context during an exception
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    internal class BindingFailureContextAttribute : Attribute
    {
        /// <summary>
        /// Create a new instance of <see cref="BindingFailureContextAttribute"/>
        /// </summary>
        /// <param name="context">A string that expands upon an error code. Helpful for debugging.</param>
        public BindingFailureContextAttribute(string context)
        {
            Value = context;
        }
        /// <summary>
        /// The context you're looking for.
        /// </summary>
        public string Value { get; }
    }

    /// <summary>
    /// An enumeration of error codes for why the binding process failed.
    /// </summary>
    /// <remarks>
    /// The inline documentation mirrors the context attribute so it's possible to inspect failure context during development.
    /// Because XML documentation isn't going to be present at runtime in a Production release, we provide the attribute as well.
    /// </remarks>
    public enum BindingFailureCode
    {
        /// <summary>
        /// A default value for when no error code is provided.
        /// </summary>
        /// <remarks>
        /// We use "Unavailable" rather than "None" due to the fact "None" is misleading and can lead to improper error handling.
        /// </remarks>
        [BindingFailureContext("No failure code is available for this exception")]
        Unavailable,
        /// <summary>
        /// It was inferred the source object was an <see cref="Enum"/>. field, however the destination type is not an <see cref="Enum"/>.
        /// </summary>
        [BindingFailureContext("It was inferred the source object was an Enum field, however the destination type is not an Enum.")]
        NoEnumAtDestinationType,
        /// <summary>
        /// No field exist in the destination enumeration that matches the source integral value.
        /// </summary>
        [BindingFailureContext("No field exist in the destination enumeration that matches the source integral value.")]
        NumberNotDefinedInEnum,
        /// <summary>
        /// No field exist in the destination enumeration that matches the source string.
        /// </summary>
        [BindingFailureContext("No field exist in the destination enumeration that matches the source string.")]
        StringNotDefinedInEnum,
        /// <summary>
        /// The destination enumeration contains no fields.
        /// </summary>
        [BindingFailureContext("The destination enumeration contains no fields.")]
        DestinationEnumEmpty,
        /// <summary>
        /// A string could not be parsed to an underlying integral type compatible with an enum.
        /// </summary>
        [BindingFailureContext("A string could not be parsed to an underlying integral type compatible with an enum.")]
        EnumIntegralNotFound,
        /// <summary>
        /// A provided source object was null or an empty collection on a non-nullable destination type.
        /// </summary>
        [BindingFailureContext("A provided source object was null on a non-nullable destination type.")]
        SourceObjectNullOrEmpty,
        /// <summary>
        /// The underlying type for the source object cannot be assigned to the destination type or lacks a destination altogether.
        /// </summary>
        [BindingFailureContext("The underlying type for the source object cannot be assigned to the destination type or lacks a destination altogether.")]
        SourceNotAssignable,
        /// <summary>
        ///  The Javascript object member does not correspond to any member on the destination type
        /// </summary>
        [BindingFailureContext("The Javascript object member {0} does not correspond to any member on the destination type. Are your style conventions correct?")]
        MemberNotFound,
        /// <summary>
        ///  The source type cannot be serialized to a type that is safe for Javascript to use.
        /// </summary>
        [BindingFailureContext("The source type {0} cannot be serialized to a type that is safe for Javascript to use. {1}")]
        UnsupportedJavascriptType
    }

    /// <summary>
    /// An exception that is thrown whenever data cannot be properly marshaled.
    /// </summary>
    public class TypeBindingException : Exception
    {
        /// <summary>
        /// The underlying type that was inferred for the source object that needed to be bound 
        /// </summary>
        public Type SourceObjectType { get; }
        public Type DestinationType { get; }
        public string Context { get; }
        public BindingFailureCode Code { get; }

        /// <summary>
        /// Creates a new <see cref="TypeBindingException"/> using a backing failure code for which context can be derived.
        /// </summary>
        /// <param name="sourceObjectType">the inferred type for the object that was meant to be bound.</param>
        /// <param name="destinationType">the destination type the object attempted to be marshaled to.</param>
        /// <param name="code">a failure code that defines the reason the binding process failed.</param>
        /// <param name="formatValues">if present, any values here will be used to format the context string.</param>
        public TypeBindingException(Type sourceObjectType, Type destinationType, BindingFailureCode code, params object[] formatValues)
        {
            SourceObjectType = sourceObjectType;
            DestinationType = destinationType;
            Code = code;
            Context = FindFailureContext(code);
            if (formatValues != null && formatValues.Length > 0)
            {
                Context = string.Format(Context, formatValues);
            }
        }

        /// <summary>
        /// Attempts to find the <see cref="BindingFailureContextAttribute"/> value of a given <see cref="BindingFailureCode"/> field.
        /// </summary>
        /// <param name="value">the field to find context on.</param>
        /// <returns>the value set on the <see cref="BindingFailureContextAttribute"/>, otherwise a default non-null value.</returns>
        private static string FindFailureContext(Enum value)
        {
            var enumType = value.GetType();
            var name = Enum.GetName(enumType, value);
            return enumType.GetField(name).GetCustomAttributes(false).OfType<BindingFailureContextAttribute>().SingleOrDefault()?.Value ?? "No context is available this error code.";
        }

        /// <summary>
        /// Creates a new <see cref="TypeBindingException"/> without a backing failure code.
        /// </summary>
        /// <param name="sourceObjectType">the inferred type for the object that was meant to be bound.</param>
        /// <param name="destinationType">the destination type the object attempted to be marshaled to.</param>
        /// <param name="context">in lieu of a failure code, provide a explanation as to why the binding process failed.</param>
        /// <remarks>
        /// the <see cref="Code"/> property will automatically be set to <see cref="BindingFailureCode.Unavailable"/>
        /// </remarks>
        public TypeBindingException(Type sourceObjectType, Type destinationType, string context)
        {
            SourceObjectType = sourceObjectType;
            DestinationType = destinationType;
            Context = context;
            Code = BindingFailureCode.Unavailable;
        }
    }
}
