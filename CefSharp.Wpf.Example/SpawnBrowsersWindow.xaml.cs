using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CefSharp.Wpf.Example
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class SpawnBrowsersWindow : Window
    {
        bool _isRunning;
        bool _requestCancel;
        private const int MAX_SPEED = 1000;

        public SpawnBrowsersWindow()
        {
            InitializeComponent();
            for (int i = 1; i <= 100; i++)
            {
                itemList.Items.Add("Item " + i);
            }
        }

        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (browserContainer.Content is ChromiumWebBrowser)
            {
                ChromiumWebBrowser oldBrowser = browserContainer.Content as ChromiumWebBrowser;
                browserContainer.Content = null;
                oldBrowser.Dispose();
            }
            await Task.Delay(10);

            ChromiumWebBrowser browser = new ChromiumWebBrowser()
            {
                Address = "http://www.google.com"
            };
            browserContainer.Content = browser;
            //browser.ExecuteScriptAsync("document.body.innerHTML = '';");
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_isRunning)
            {
                StartStopTest(false);
            }
            else
            {
                StartStopTest(true);

                for (int i = 0; i < itemList.Items.Count; i++)
                {
                    itemList.SelectedIndex = i;
                    await Task.Delay((int)(MAX_SPEED * speedSlider.Value));
                    if (_requestCancel)
                        break;
                }

                StartStopTest(false);
            }
        }

        private void StartStopTest(bool isStart)
        {
            _requestCancel = !isStart;
            _isRunning = isStart;
            btnTest.Content = isStart ? "Cancel" : "Test";
            itemList.IsEnabled = !_isRunning;
        }

        private void speedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            speedLabel.Content = string.Format("{0}ms", (int)(MAX_SPEED * speedSlider.Value));
        }
    }
}
