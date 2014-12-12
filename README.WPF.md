# README (WPF)

This file provides some basic information about how to use the CefSharp.Wpf NuGet package.

CefSharp is based on CEF (Chromium Embeddable Framework), which is composed of a number of .dll and .pak files. For this reason, it's not enough to just add .NET reference to the CefSharp.Wpf and the CefSharp assemblies; the other .dll/.pak files must also exist in your projects "Output path" (as defined in the  Visual Studio project properties.

This is done automatically by the CefSharp.Wpf NuGet package (or the packages it depend on). However, it is only possible to copy these files into *either* the Debug or Release target of your project. Because it is the default when creating a new project, and the target which is normally used when developing .NET applications, I have chosen to place them in the bin\Debug folder for now. This means that if you want to run your application in Release mode, you need to copy these files from bin\Debug to bin\Release for the moment.

To be able to use CefSharp.Wpf, you basically just have to add a ChromiumWebBrowser like this:

``` xml
<Window x:Class="WpfApplication3.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:cefSharp="clr-namespace:CefSharp.Wpf;assembly=CefSharp.Wpf" Title="MainWindow" Height="350" Width="525">
    <Grid>
        <cefSharp:ChromiumWebBrowser x:Name="Browser" Address="http://www.google.com.au" />
    </Grid>
</Window>
```

...and in the code-behind some code like this:

``` csharp
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Cef.Initialize(new CefSettings());
        }
    }
```
