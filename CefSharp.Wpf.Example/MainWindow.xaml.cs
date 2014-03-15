using CefSharp.Wpf.Example.Views.Main;
using System.Windows;

namespace CefSharp.Wpf.Example
{
    public partial class MainWindow : Window
    {
        public FrameworkElement Tab1Content { get; set; }
        public FrameworkElement Tab2Content { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Tab1Content = new MainView
            {
                DataContext = new MainViewModel()
            };

            Tab2Content = new MainView
            {
                DataContext = new MainViewModel("http://www.google.com")
            };
        }
    }
}
