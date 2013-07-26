﻿using CefSharp.Example;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace CefSharp.Wpf.Example
{
    public partial class MainWindow : Window, IExampleView
    {
        // file
        public event EventHandler ShowDevToolsActivated;
        public event EventHandler CloseDevToolsActivated;
        public event EventHandler ExitActivated;

        // edit
        public event EventHandler UndoActivated;
        public event EventHandler RedoActivated;
        public event EventHandler CutActivated;
        public event EventHandler CopyActivated;
        public event EventHandler PasteActivated;
        public event EventHandler DeleteActivated;
        public event EventHandler SelectAllActivated;

        // test
        public event EventHandler TestResourceLoadActivated;
        public event EventHandler TestSchemeLoadActivated;
        public event EventHandler TestExecuteScriptActivated;
        public event EventHandler TestEvaluateScriptActivated;
        public event EventHandler TestBindActivated;
        public event EventHandler TestConsoleMessageActivated;
        public event EventHandler TestTooltipActivated;
        public event EventHandler TestPopupActivated;
        public event EventHandler TestLoadStringActivated;
        public event EventHandler TestCookieVisitorActivated;

        // navigation
        public event Action<object, string> UrlActivated;
        public event EventHandler BackActivated;
        public event EventHandler ForwardActivated;

        private readonly IDictionary<object, EventHandler> handlers;

        public static CefSharp.Wpf.WebView web_view;

        public MainWindow()
        {
            InitializeComponent();

            //Fix WPF XAML-Designer

            /*
            <cs:WebView x:Name="web_view"
                        Opacity="{Binding ElementName=opacitySlider, Path=Value}">
                <cs:WebView.LayoutTransform>
                    <TransformGroup>
                        <RotateTransform Angle="{Binding Value, ElementName=angleSlider}" />
                    </TransformGroup>
                </cs:WebView.LayoutTransform>
            </cs:WebView>
            */

            web_view = new WebView() { Name = "web_view" };

            RotateTransform myRotateTransform = new RotateTransform();
            myRotateTransform.Angle = angleSlider.Value;
            TransformGroup myTransformGroup = new TransformGroup();
            myTransformGroup.Children.Add(myRotateTransform);

            Binding b = new Binding("Value");
            b.Source = angleSlider;
            BindingOperations.SetBinding(myRotateTransform, RotateTransform.AngleProperty, b);

            web_view.RenderTransform = myTransformGroup;

            opacitySlider.SetBinding(Slider.ValueProperty, new Binding("Opacity") { Mode = BindingMode.TwoWay, Source = web_view });

            // Finally add CefSharp.Wpf.WebView web_view to Grid->MainDock
            MainDock.Children.Add(web_view);


            var presenter = new ExamplePresenter(web_view, this,
                invoke => Dispatcher.BeginInvoke(invoke));

            handlers = new Dictionary<object, EventHandler>
            {
                // file
                { showDevToolsMenuItem, ShowDevToolsActivated},
                { closeDevToolsMenuItem, CloseDevToolsActivated},
                { exitMenuItem, ExitActivated },

                // edit
                { undoMenuItem, UndoActivated },
                { redoMenuItem, RedoActivated },
                { cutMenuItem, CutActivated },
                { copyMenuItem, CopyActivated },
                { pasteMenuItem, PasteActivated },
                { deleteMenuItem, DeleteActivated },
                { selectAllMenuItem, SelectAllActivated },

                // test
                { testResourceLoadMenuItem, TestResourceLoadActivated },
                { testSchemeLoadMenuItem, TestSchemeLoadActivated },
                { testExecuteScriptMenuItem, TestExecuteScriptActivated },
                { testEvaluateScriptMenuItem, TestEvaluateScriptActivated },
                { testBindMenuItem, TestBindActivated },
                { testConsoleMessageMenuItem, TestConsoleMessageActivated },
                { testTooltipMenuItem, TestTooltipActivated },
                { testPopupMenuItem, TestPopupActivated },
                { testLoadStringMenuItem, TestLoadStringActivated },
                { testCookieVisitorMenuItem, TestCookieVisitorActivated },

                // navigation
                { backButton, BackActivated },
                { forwardButton, ForwardActivated },
            };
        }

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void SetAddress(string address)
        {
            urlTextBox.Text = address;
        }

        public void SetCanGoBack(bool can_go_back)
        {
            backButton.IsEnabled = can_go_back;
        }

        public void SetCanGoForward(bool can_go_forward)
        {
            forwardButton.IsEnabled = can_go_forward;
        }

        public void SetIsLoading(bool is_loading)
        {

        }

        public void ExecuteScript(string script)
        {
            web_view.ExecuteScript(script);
        }

        public object EvaluateScript(string script)
        {
            return web_view.EvaluateScript(script);
        }

        public void DisplayOutput(string output)
        {
            outputLabel.Content = output;
        }

        private void control_Activated(object sender, RoutedEventArgs e)
        {
            EventHandler handler;
            if (handlers.TryGetValue(sender, out handler) &&
                handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void urlTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
            {
                return;
            }

            var handler = UrlActivated;
            if (handler != null)
            {
                handler(this, urlTextBox.Text);
            }
        }
    }
}
