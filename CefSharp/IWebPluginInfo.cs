using System;

namespace CefSharp
{
    /// <summary>
    /// Wrapper for the CEF3 CefWebPluginInfo
    /// </summary>
    public interface IWebPluginInfo
    {
        string Name { get; }

        string Description { get; }

        string Path { get; }

        string Version { get; }
    }
}
