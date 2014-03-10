using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CefSharp.Wpf
{
    internal class ParameterCommand : DelegateCommand
    {
        public static DependencyProperty ParameterProperty = DependencyProperty.Register( "Parameter", typeof( object ), typeof( ParameterCommand ), new PropertyMetadata( true, OnParameterChange ) );
        public object Parameter
        {
            get { return (object) GetValue( ParameterProperty ); }
            set { SetValue( ParameterProperty, value ); }
        }

        private static void OnParameterChange( DependencyObject sender, DependencyPropertyChangedEventArgs args )
        {
            ParameterCommand owner = (ParameterCommand) sender;
            object oldvalue = args.OldValue;
            object newvalue = args.NewValue;

            owner.OnParameterChange( oldvalue, newvalue );
        }

        protected virtual void OnParameterChange( object oldValue, object newValue )
        {
            RaiseCanExecuteChanged();
        }

        public ParameterCommand( Action commandHandler )
            : base( commandHandler )
        {
            
        }
    }
}
