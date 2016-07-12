// Copyright © 2010-2016 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

namespace CefSharp.ModelBinding
{
    /// <summary>
    /// Configurations that controls the behavior of the binder at runtime.
    /// </summary>
    public class BindingConfig
    {
        /// <summary>
        /// Default binding configuration.
        /// </summary>
        public static readonly BindingConfig Default = new BindingConfig { IgnoreErrors = true } ;

        /// <summary>
        /// Gets or sets whether binding error should be ignored and the binder should continue with the next property.
        /// </summary>
        /// <remarks>Setting this property to <see langword="true" /> means that no <see cref="ModelBindingException"/> will be thrown if an error occurs.</remarks>
        /// <value><see langword="true" />If the binder should ignore errors, otherwise <see langword="false" />.</value>
        public bool IgnoreErrors { get; set; }
    }
}