using System.Windows.Controls;

namespace CefSharp.Wpf.Example.Views.Main
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }
    }
}
