using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp.DevTools.Page
{
    public partial class Viewport
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Viewport()
        {
            Scale = 1.0;
        }
    }
}

namespace CefSharp.DevTools.Network
{
    public partial class NetworkClient
    {
        /// <summary>
        /// Fetches the resource and returns the content.
        /// </summary>
        /// <param name = "frameId">Frame id to get the resource for. Mandatory for frame targets, and should be omitted for worker targets.</param>
        /// <param name = "url">URL of the resource to get content for.</param>
        /// <param name = "options">Options for the request.</param>
        /// <returns>returns System.Threading.Tasks.Task&lt;LoadNetworkResourceResponse&gt;</returns>
        /// <remarks>
        /// This overload of LoadNetworkResourceAsync exists to avoid a breaking change as optional params are now always at the end
        /// where previously they weren't marked as optional when at the beginning.
        /// </remarks>
        public System.Threading.Tasks.Task<LoadNetworkResourceResponse> LoadNetworkResourceAsync(string frameId, string url, CefSharp.DevTools.Network.LoadNetworkResourceOptions options)
        {
            var dict = new System.Collections.Generic.Dictionary<string, object>();
            if (!(string.IsNullOrEmpty(frameId)))
            {
                dict.Add("frameId", frameId);
            }

            dict.Add("url", url);
            dict.Add("options", options.ToDictionary());
            return _client.ExecuteDevToolsMethodAsync<LoadNetworkResourceResponse>("Network.loadNetworkResource", dict);
        }
    }
}
