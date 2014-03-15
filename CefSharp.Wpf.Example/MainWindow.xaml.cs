using CefSharp.Wpf.Example.Views.Main;
using System.Windows;

namespace CefSharp.Wpf.Example
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Content.Content = new MainView
            {
                DataContext = new MainViewModel()
            };

            Content2.Content = new MainView
            {
                DataContext = new MainViewModel("http://www.google.com")
            }; ;
        }
    }
}
