namespace CefSharp.Example
{
    public class DownloadHandler : IDownloadHandler
    {
        public bool OnBeforeDownload(IBrowser browser, DownloadItem downloadItem, out string downloadPath, out bool showDialog)
        {
            downloadPath = downloadItem.SuggestedFileName;
            showDialog = true;

            return true;
        }

        public bool OnDownloadUpdated(IBrowser browser, DownloadItem downloadItem)
        {
            return false;
        }
    }
}
