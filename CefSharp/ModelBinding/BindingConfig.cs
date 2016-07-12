namespace CefSharp.ModelBinding
{
    /// <summary>
    /// Configurations that controls the behavior of the binder at runtime.
    /// </summary>
    public class BindingConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BindingConfig"/> class.
        /// </summary>
        public BindingConfig()
        {
            this.Overwrite = true;
        }

        /// <summary>
        /// Default binding configuration.
        /// </summary>
        public static readonly BindingConfig Default = new BindingConfig() { Overwrite = true, IgnoreErrors = true } ;

        /// <summary>
        /// Gets or sets whether binding error should be ignored and the binder should continue with the next property.
        /// </summary>
        /// <remarks>Setting this property to <see langword="true" /> means that no <see cref="ModelBindingException"/> will be thrown if an error occurs.</remarks>
        /// <value><see langword="true" />If the binder should ignore errors, otherwise <see langword="false" />.</value>
        public bool IgnoreErrors { get; set; }

        /// <summary>
        /// Gets or sets whether the binder is allowed to overwrite properties that does not have a default value.
        /// </summary>
        /// <value><see langword="true" /> if the binder is allowed to overwrite non-default values, otherwise <see langword="false" />.</value>
        public bool Overwrite { get; set; }
    }
}