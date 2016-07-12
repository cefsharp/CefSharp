namespace CefSharp.ModelBinding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Locates model binders for a particular model
    /// </summary>
    public class DefaultModelBinderLocator : IModelBinderLocator
    {
        /// <summary>
        /// Available model binders
        /// </summary>
        private readonly IReadOnlyCollection<IModelBinder> binders;

        /// <summary>
        /// Default model binder to fall back on
        /// </summary>
        private readonly IBinder fallbackBinder;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultModelBinderLocator"/> class.
        /// </summary>
        /// <param name="binders">Available model binders</param>
        /// <param name="fallbackBinder">Fallback binder</param>
        public DefaultModelBinderLocator(IEnumerable<IModelBinder> binders, IBinder fallbackBinder)
        {
            this.fallbackBinder = fallbackBinder;

            if (binders != null)
            {
                this.binders = binders.ToArray();
            }
        }

        /// <summary>
        /// Gets a binder for the given type
        /// </summary>
        /// <param name="modelType">Destination type to bind to</param>
        /// <returns>IModelBinder instance or null if none found</returns>
        public IBinder GetBinderForType(Type modelType)
        {
            if (this.binders == null)
            {
                return this.fallbackBinder;
            }

            foreach (var modelBinder in this.binders)
            {
                if (modelBinder.CanBind(modelType))
                {
                    return modelBinder;
                }
            }

            return this.fallbackBinder;
        }
    }
}
