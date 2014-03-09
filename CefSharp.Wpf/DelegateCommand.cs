using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace CefSharp.Wpf
{
    internal class DelegateCommand : DependencyObject, ICommand
    {
        private readonly Action _commandHandler;

        public event EventHandler CanExecuteChanged;

        public void SetBinding( DependencyProperty dp, BindingBase binding )
        {
            BindingOperations.SetBinding( this, dp,binding); 
        }

        public static DependencyProperty CanExecuteValueProperty = DependencyProperty.Register("CanExecuteValue", typeof(bool), typeof(DelegateCommand), new PropertyMetadata(true, OnCanExecutePropertyChange));
        public bool CanExecuteValue 
        {
            get { return (bool)GetValue(CanExecuteValueProperty); }
            set { SetValue( CanExecuteValueProperty, value ); }
        }

        private static void OnCanExecutePropertyChange( DependencyObject sender, DependencyPropertyChangedEventArgs args )
        {
            DelegateCommand owner = (DelegateCommand)sender;
            bool oldvalue = (bool)args.OldValue;
            bool newvalue = (bool)args.NewValue;

            owner.OnCanExecutePropertyChange( oldvalue, newvalue );
        }

        protected virtual void OnCanExecutePropertyChange( bool oldValue, bool newValue )
        {
            RaiseCanExecuteChanged();
        }

        public DelegateCommand(Action commandHandler )
        {
            _commandHandler = commandHandler;
        }

        public void Execute(object parameter)
        {
            _commandHandler();
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteValue;
        }
        
        private void RaiseCanExecuteChanged()
        {
            var handlers = CanExecuteChanged;
            if (handlers != null)
            {
                handlers(this, new EventArgs());
            }
        }
    }
}
