namespace CefSharp.ModelBinding
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents an exception when attempting to bind to a model
    /// </summary>
    public class ModelBindingException : Exception
    {
        private const string ExceptionMessage = "Unable to bind to type: {0}";

        /// <summary>
        /// Gets all failures
        /// </summary>
        public virtual IEnumerable<PropertyBindingException> PropertyBindingExceptions { get; private set; }

        /// <summary>
        /// Gets the model type, which caused the exception
        /// </summary>
        public virtual Type BoundType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the ModelBindingException class with a specified model type,
        /// property name and the original exception, which caused the problem
        /// </summary>
        /// <param name="boundType">the model type to bind to</param>
        /// <param name="propertyBindingExceptions">the original exceptions, thrown while binding the property</param>
        /// <param name="innerException">The inner exception.</param>
        public ModelBindingException(Type boundType, IEnumerable<PropertyBindingException> propertyBindingExceptions = null, Exception innerException = null)
            : base(string.Format(ExceptionMessage, boundType), innerException)
        {
            if (boundType == null)
            {
                throw new ArgumentNullException("boundType");
            }
            this.PropertyBindingExceptions = propertyBindingExceptions ?? new List<PropertyBindingException>();
            this.BoundType = boundType;
        }
#if !NETSTANDARD1_6
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBindingException" /> class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected ModelBindingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
#endif
    }
}