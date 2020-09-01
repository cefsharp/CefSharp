using System.Threading.Tasks;

namespace CefSharp.DevTools.Network
{
    public class Network
    {
        private DevToolsClient client;

        public Network(DevToolsClient client)
        {
            this.client = client;
        }

        public async Task<bool> ClearBrowserCacheAsync()
        {
            var result = await client.ExecuteDevToolsMethodAsync("Network.clearBrowserCache");

            return result.Success;
        }
    }
}
