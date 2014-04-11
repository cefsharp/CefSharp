using System;
using System.Net;

namespace CefSharp.Wpf.Example.Mvvm
{
	/// <summary>
	/// TODO: Move this to a more appropriate location
	/// </summary>
	public class DefaultRequestHandler : IRequestHandler
	{
		public virtual bool OnBeforeBrowse(IWebBrowser browser, IRequest request, NavigationType navigationType, bool isRedirect)
		{
			return false;
		}

		public virtual bool OnBeforeResourceLoad(IWebBrowser browser, IRequestResponse requestResponse)
		{
			return false;
		}

		public virtual void OnResourceResponse(IWebBrowser browser, string url, int status, string statusText, string mimeType, WebHeaderCollection headers)
		{
			
		}

		public virtual bool GetDownloadHandler(IWebBrowser browser, out IDownloadHandler handler)
		{
			throw new NotImplementedException();
		}

		public virtual bool GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme, ref string username, ref string password)
		{
			var authDialog = new AuthDialog();
			if (authDialog.ShowDialog() == true)
			{
				username = authDialog.UserName;
				password = authDialog.Password;

				return true;
			}

			return false;
		}
	}
}
