using System.Collections.ObjectModel;
using CefSharp.Wpf.Example.Views.BrowserTab;

namespace CefSharp.Wpf.Example.Views.Main
{
    public class MainViewModel
    {
        public ObservableCollection<object> BrowserViewModels { get; private set; }

        public MainViewModel()
        {
            BrowserViewModels = new ObservableCollection<object>
            {
                new BrowserTabViewModel { ShowSidebar = true },
                new BrowserTabViewModel()
            };
        }

        //private void OnTabControlSelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (e.AddedItems.Count != 1) return;

        //    var selectedtab = (TabItem)e.AddedItems[0];
        //    if ((string)selectedtab.Header != "+")
        //    {
        //        return;
        //    }

        //    var tabItem = CreateTabItem();
        //    TabControl.Items.Insert(TabControl.Items.Count - 1, tabItem);
        //}

        //private static TabItem CreateTabItem()
        //{
        //    var tabItem = new TabItem
        //    {
        //        Width = 150,
        //        Height = 20,
        //        IsSelected = true,
        //        Content = CreateNewTab()
        //    };
        //    tabItem.SetBinding(HeaderedContentControl.HeaderProperty, new Binding("Content.DataContext.Title")
        //    {
        //        RelativeSource = RelativeSource.Self
        //    });
        //    return tabItem;
        //}

        //private static MainView CreateNewTab()
        //{
        //    return new MainView
        //    {
        //        DataContext = new MainViewModel("http://www.google.com")
        //    };
        //}

    }
}
