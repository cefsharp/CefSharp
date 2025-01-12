using System.Threading.Tasks;
using System.Windows;
using CefSharp.Example.JavascriptBinding;

namespace CefSharp.Wpf.Example
{
    /// <summary>
    /// Interaction logic for JavascriptCallbackMainWindow.xaml
    /// </summary>
    public partial class JavascriptCallbackMainWindow : Window
    {
        private JavascriptCallbackBoundObject boundObjectOne;
        private JavascriptCallbackBoundObject boundObjectTwo;

        public JavascriptCallbackMainWindow()
        {
            InitializeComponent();
#if ! CEFSHARP_WPF_HWNDHOST
            BrowserTwo.BorderBrush = BrowserOne.BorderBrush = System.Windows.Media.Brushes.Red;
            BrowserTwo.BorderThickness = BrowserOne.BorderThickness = new Thickness(1);
#endif
            boundObjectOne = new JavascriptCallbackBoundObject(BrowserOne);
            boundObjectTwo = new JavascriptCallbackBoundObject(BrowserTwo);

#if NETCOREAPP
            BrowserOne.JavascriptObjectRepository.Register("boundObject", boundObjectOne);
            BrowserTwo.JavascriptObjectRepository.Register("boundObject", boundObjectTwo);
#else
            BrowserOne.JavascriptObjectRepository.Register("boundObject", boundObjectOne, false);
            BrowserTwo.JavascriptObjectRepository.Register("boundObject", boundObjectTwo, false);
#endif
        }

        private void ExecuteCallbackImmediatelyClick(object sender, RoutedEventArgs e)
        {
            boundObjectOne.RunCallback();
            boundObjectTwo.RunCallback();

            BrowserOne.Reload();
            BrowserTwo.Reload();
        }

        private void ExecuteCallbackInThreeSeconds(object sender, RoutedEventArgs e)
        {
            BrowserOne.Address = "custom://cefsharp/SchemeTest.html";
            BrowserTwo.Address = "custom://cefsharp/SchemeTest.html";

            Task.Delay(3000).ContinueWith(t =>
            {
                boundObjectOne.RunCallback();
                boundObjectTwo.RunCallback();
            });

        }
    }
}
