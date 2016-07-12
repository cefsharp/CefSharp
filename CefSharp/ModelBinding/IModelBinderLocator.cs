namespace CefSharp.ModelBinding
{
    using System;
using System.Collections.Generic;

    /// <summary>
    /// Locates model binders for a particular model
    /// </summary>
    public interface IModelBinderLocator
    {
        /// <summary>
        /// Gets a binder for the given type
        /// </summary>
        /// <param name="modelType">Destination type to bind to</param>
        /// <param name="context">The <see cref="NancyContext"/> instance of the current request.</param>
        /// <returns>IModelBinder instance or null if none found</returns>
        IBinder GetBinderForType(Type modelType);
    }
}