using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace CefSharp.Wpf
{
    public class MappingConverter : DependencyObject, IValueConverter
    {
        #region IValueConverter Members

        public static readonly DependencyProperty FallbackValueProperty = DependencyProperty.Register(
            "FallbackValue", typeof (object), typeof (MappingConverter), new PropertyMetadata(default(object)));

        public object FallbackValue
        {
            get { return (object) GetValue(FallbackValueProperty); }
            set { SetValue(FallbackValueProperty, value); }
        }

        public ObservableCollection<Mapping> Mappings = new ObservableCollection<Mapping>();

        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            var mapping = Mappings.FirstOrDefault(p => Equals(p.From, value) );

            if (mapping == null)
            {
                return FallbackValue;
            }
            else
            {
                return mapping.To;
            }
        }

        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class Mapping : DependencyObject
    {
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register(
            "From", typeof (object), typeof (Mapping), new PropertyMetadata(default(object)));

        public object From
        {
            get { return (object) GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }

        public static readonly DependencyProperty ToProperty = DependencyProperty.Register(
            "To", typeof (object), typeof (Mapping), new PropertyMetadata(default(object)));

        public object To
        {
            get { return (object) GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }
    }
}
