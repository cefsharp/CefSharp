using System.Collections.Generic;
using System.Diagnostics;

namespace CefSharp.Example
{
    public class WebPluginInfoVisitor : IWebPluginInfoVisitor
    {
        public bool Visit(IWebPluginInfo info, int count, int total)
        {
            Debug.WriteLine("Plugin ,Name={0} ,Version={1} ,Description={2} ,Path={3}", info.Name, info.Version, info.Description, info.Path);
            return true;
        }
    }
}