using System;
using System.Windows.Input;

namespace CefSharp.Wpf
{
    internal class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> commandHandler;
        private readonly Func<T, bool> canExecuteHandler;

        public DelegateCommand(Action<T> commandHandler, Func<T, bool> canExecuteHandler = null)
        {
            this.commandHandler = commandHandler;
            this.canExecuteHandler = canExecuteHandler;
        }

        public void Execute(object parameter)
        {
            commandHandler((T)parameter);
        }

        public bool CanExecute(object parameter)
        {
            return
                canExecuteHandler == null ||
                canExecuteHandler((T)parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}