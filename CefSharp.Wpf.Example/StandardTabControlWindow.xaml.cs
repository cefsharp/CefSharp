using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace CefSharp.Wpf.Example
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class StandardTabControlWindow : Window
    {
        public List<ChromiumWebBrowser> Tabs { get; }

        public StandardTabControlWindow()
        {
            InitializeComponent();

            Tabs = Enumerable.Range(1, 10).Select(x => new ChromiumWebBrowser
            {
                Address = "google.com"
            }).ToList();

            DataContext = this;

        }
    }
}
