namespace CefSharp
{
    public interface IDownloadHandler
    {
        bool OnBeforeDownload(string suggestedName, out string downloadPath, out bool showDialog);
        bool ReceivedData(byte[] data);
        void Complete();
    }
}
