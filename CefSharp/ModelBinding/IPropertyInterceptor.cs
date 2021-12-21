using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// Provides the capability intercept Net property calls made from javascript as part of the
    /// JavascriptBinding (JSB) implementation.
    /// </summary>
    public interface IPropertyInterceptor
    {
        /// <summary>
        /// Intercept the get property
        /// </summary>
        /// <param name="property"></param>
        /// <param name="parameters"></param>
        /// <param name="propertName"></param>
        /// <returns></returns>
        object GetIntercept(Func<object> property, string propertName);

        /// <summary>
        /// Intercept the set property
        /// </summary>
        /// <param name="property"></param>
        /// <param name="parameters"></param>
        /// <param name="propertName"></param>
        /// <returns></returns>
        void SetIntercept(Action<Object> property, object parameter, string propertName);
    }
}


