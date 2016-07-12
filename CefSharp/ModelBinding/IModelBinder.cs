namespace CefSharp.ModelBinding
{
    using System;

    /// <summary>
    /// Provides a way to bind an incoming request, via the context, to a model type
    /// </summary>
    public interface IModelBinder : IBinder
    {
        /// <summary>
        /// Whether the binder can bind to the given model type
        /// </summary>
        /// <param name="modelType">Required model type</param>
        /// <returns>True if binding is possible, false otherwise</returns>
        bool CanBind(Type modelType);
    }
}