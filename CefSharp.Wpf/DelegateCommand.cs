using System;
using System.Threading.Tasks;
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

        #region CanExecuteValue

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

        #endregion

        #region IsBusy

        public static DependencyProperty IsBusyProperty = DependencyProperty.Register( "IsBusy", typeof( bool ), typeof( DelegateCommand ), new PropertyMetadata( false ) );
        public bool IsBusy
        {
            get { return (bool) GetValue( IsBusyProperty ); }
            set { SetValue( IsBusyProperty, value ); }
        }

        #endregion

        public DelegateCommand( Action commandHandler )
        {
            _commandHandler = commandHandler;
        }

        public void Execute(object parameter)
        {
            OnTaskBegin();

            Task.Factory.StartNew(_commandHandler)
                .ContinueWith(OnTaskCompleted);
        }

        private void OnTaskBegin()
        {
            SetCurrentValue( IsBusyProperty, true );
        }

        private void OnTaskCompleted( Task task )
        {
            SetCurrentValue( IsBusyProperty, false );
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteValue;
        }
        
        protected void RaiseCanExecuteChanged()
        {
            var handlers = CanExecuteChanged;
            if (handlers != null)
            {
                handlers(this, new EventArgs());
            }
        }
    }
}
