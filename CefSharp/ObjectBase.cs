using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace CefSharp
{
    // TODO: Why do we need this one?
    public class ObjectBase : INotifyPropertyChanged, IDisposable
    {
        #region IDisposable

        ~ObjectBase()
        {
            DoDispose(false);
            IsDisposed = true;
        }

        public void Dispose()
        {
            DoDispose(true);
            IsDisposed = true;
        }

        protected virtual void DisposeMember<T>(ref T field)
            where T : class, IDisposable
        {
            var oldvalue = field;
            field = null;

            if (oldvalue != null)
            {
                oldvalue.Dispose();
            }
        }

        protected virtual void DoDispose(bool isDisposing)
        {
            propertyChanged = null;
        }

        public bool IsDisposed { get; private set; }

        #endregion

        #region INotifyPropertyChanged

        private PropertyChangedEventHandler propertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add { if (!IsDisposed) propertyChanged += value; }
            remove { propertyChanged -= value; }
        }

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
