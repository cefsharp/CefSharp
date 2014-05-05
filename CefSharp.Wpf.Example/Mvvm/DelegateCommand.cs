// Copyright © 2010-2014 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.Windows.Input;

namespace CefSharp.Wpf.Example.Mvvm
{
    public class DelegateCommand : ICommand
    {
        private readonly Action commandHandler;
        private readonly Func<bool> canExecuteHandler;

        public DelegateCommand(Action commandHandler, Func<bool> canExecuteHandler = null)
        {
            this.commandHandler = commandHandler;
            this.canExecuteHandler = canExecuteHandler;
        }

        public void Execute(object parameter)
        {
            commandHandler();
        }

        public bool CanExecute(object parameter)
        {
            return
                canExecuteHandler == null ||
                canExecuteHandler();
        }

        #pragma warning disable 0067
        public event EventHandler CanExecuteChanged;
        #pragma warning restore 0067
    }
}
