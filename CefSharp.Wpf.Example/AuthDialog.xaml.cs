using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CefSharp.Example;

namespace CefSharp.Wpf.Example
{
	/// <summary>
	/// Interaction logic for AuthDialog.xaml
	/// </summary>
	public partial class AuthDialog : Window, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private string userName;
		public string UserName
		{
			get { return userName; }
			set { PropertyChanged.ChangeAndNotify(ref userName, value, () => UserName); }
		}

		private string password;
		public string Password
		{
			get { return password; }
			set { PropertyChanged.ChangeAndNotify(ref password, value, () => Password); }
		}

		public AuthDialog()
		{
			InitializeComponent();

			CommandBindings.Add(new CommandBinding(ApplicationCommands.Close, CloseDialog));

			DataContext = this;
		}

		private void CloseDialog(object sender, ExecutedRoutedEventArgs e)
		{
			if (e.Parameter != null)
			{
				DialogResult = bool.Parse(e.Parameter.ToString());
			}
			Close();
		}

		private void PasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
		{
			var passwordBox = (PasswordBox)sender;
			Password = passwordBox.Password;
		}
	}
}
