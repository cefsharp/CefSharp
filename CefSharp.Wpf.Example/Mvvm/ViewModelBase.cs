using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using CefSharp.Example;

namespace CefSharp.Wpf.Example.Mvvm
{
    public class ViewModelBase : ObjectBase, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        private PropertyChangedEventHandler propertyChanged;
        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { if (!IsDisposed) propertyChanged += value; }
            remove { propertyChanged -= value; }
        }

        public bool ChangeAndNotify<T>(ref T field, T value, Expression<Func<T>> memberExpression)
        {
            return propertyChanged.ChangeAndNotify(ref field, value, memberExpression);
        }

        /// <summary>
        /// Is called when a property is changed and raises a <see cref="PropertyChanged"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oldvalue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected virtual void OnPropertyChanged<T>(T oldvalue, T newValue, PropertyChangedEventArgs e)
        {
            var handlers = propertyChanged;
            if (handlers == null)
            {
                return;
            }

            handlers(this, e);
        }

        public static PropertyChangedEventArgs GetArgs<T>(Expression<Func<T, object>> propertyexpression)
        {
            if (propertyexpression == null)
            {
                throw new ArgumentNullException("propertyexpression");
            }

            var body = propertyexpression.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("Lambda must return a property.");
            }

            return new PropertyChangedEventArgs(body.Member.Name);
        }

        /// <summary>
        /// Sets the specified field and calls <see cref="OnPropertyChanged{T}(T,T,PropertyChangedEventArgs)"/> if the value changed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">The field.</param>
        /// <param name="value">The value.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected void Set<T>(ref T field, T value, PropertyChangedEventArgs e)
        {
            var oldvalue = field;

            if (EqualityComparer<T>.Default.Equals(oldvalue, value))
            {
                return;
            }

            field = value;

            OnPropertyChanged(oldvalue, value, e);
        }

        #endregion
    }
}
