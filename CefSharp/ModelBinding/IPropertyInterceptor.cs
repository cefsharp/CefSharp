using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// Provides the capability intercepting get/set property calls made from javascript as part of the
    /// JavascriptBinding (JSB) implementation.
    /// </summary>
    public interface IPropertyInterceptor
    {
        /// <summary>
        /// Called before the get property is invokved. You are now responsible for evaluating
        /// the property and returning the result.
        /// </summary>
        /// <param name="propertyGetter">A Func that represents the property to be called</param>
        /// <param name="propertName">Name of the property to be called</param>
        /// <returns>The property result</returns>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// public object IPropertyInterceptor.InterceptGet(Func<object> propertyGetter, string propertyName)
        /// {
        ///    object result = propertyGetter();
        ///    Debug.WriteLine("InterceptGet " + propertyName);
        ///    return result;
        /// }
        /// ]]>
        /// </code>
        /// </example>
        object InterceptGet(Func<object> propertyGetter, string propertName);

        /// <summary>
        /// Called before the set property is invokved. You are now responsible for evaluating
        /// the property.
        /// </summary>
        /// <param name="propertySetter">A Func that represents the property to be called</param>
        /// <param name="parameter">paramater to be set to property</param>
        /// <param name="propertName">Name of the property to be called</param>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// public object IPropertyInterceptor.InterceptSet(Action<object> propertySetter, object parameter, string propertName)
        /// {
        ///    Debug.WriteLine("InterceptSet " + propertName);
        ///    propertySetter(parameter);
        /// }
        /// ]]>
        /// </code>
        /// </example>
        void InterceptSet(Action<Object> propertySetter, object parameter, string propertName);
    }
}
