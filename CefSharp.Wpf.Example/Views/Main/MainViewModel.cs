using System;
using System.ComponentModel;
using CefSharp.Example;
using CefSharp.Wpf.Example.Mvvm;

namespace CefSharp.Wpf.Example.Views.Main
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string address;
        public string Address
        {
            get { return address; }
            set { PropertyChanged.ChangeAndNotify(ref address, value, () => Address); }
        }

        public DelegateCommand GoCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel()
        {
            Address = ExamplePresenter.DefaultUrl;

            GoCommand = new DelegateCommand(Go, () => !String.IsNullOrWhiteSpace(Address));
        }

        private void Go()
        {

        }
    }
}
