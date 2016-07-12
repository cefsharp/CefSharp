namespace CefSharp.ModelBinding
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;

    /// <summary>
    /// Provides wiring up of a model binder when cast to a destination type
    /// </summary>
    public class DynamicModelBinderAdapter : DynamicObject
    {
        private readonly IModelBinderLocator locator;
        private readonly IDictionary<string, object> objectDictionary;
        private readonly BindingConfig configuration;
        private readonly string[] blacklistedProperties;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicModelBinderAdapter"/> class.
        /// </summary>
        /// <param name="locator">Model binder locator</param>
        /// <param name="context">Nancy context</param>
        /// <param name="configuration">The <see cref="BindingConfig"/> that should be applied during binding.</param>
        /// <param name="blacklistedProperties">Blacklisted property names</param>
        public DynamicModelBinderAdapter(IModelBinderLocator locator, IDictionary<string, object> objectDictionary, BindingConfig configuration, params string[] blacklistedProperties)
        {
            if (locator == null)
            {
                throw new ArgumentNullException("locator");
            }

            if (objectDictionary == null)
            {
                throw new ArgumentNullException("objectDictionary");
            }

            if (configuration == null)
            {
                throw new ArgumentNullException("configuration");
            }

            this.locator = locator;
            this.objectDictionary = objectDictionary;
            this.configuration = configuration;
            this.blacklistedProperties = blacklistedProperties;
        }

        /// <summary>
        /// Provides implementation for type conversion operations. Classes derived from the <see cref="T:System.Dynamic.DynamicObject"/> class can override this method to specify dynamic behavior for operations that convert an object from one type to another.
        /// </summary>
        /// <returns>
        /// true if the operation is successful; otherwise, false. If this method returns false, the run-time binder of the language determines the behavior. (In most cases, a language-specific run-time exception is thrown.)
        /// </returns>
        /// <param name="binder">Provides information about the conversion operation. The binder.Type property provides the type to which the object must be converted. For example, for the statement (String)sampleObject in C# (CType(sampleObject, Type) in Visual Basic), where sampleObject is an instance of the class derived from the <see cref="T:System.Dynamic.DynamicObject"/> class, binder.Type returns the <see cref="T:System.String"/> type. The binder.Explicit property provides information about the kind of conversion that occurs. It returns true for explicit conversion and false for implicit conversion.</param><param name="result">The result of the type conversion operation.</param>
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            var instanceType = binder.Type;

            var modelBinder = this.locator.GetBinderForType(instanceType);

            if (modelBinder == null)
            {
                throw new ModelBindingException(instanceType);
            }

            result = modelBinder.Bind(this.objectDictionary, instanceType, this.configuration, this.blacklistedProperties);

            return result != null || base.TryConvert(binder, out result);
        }
    }
}