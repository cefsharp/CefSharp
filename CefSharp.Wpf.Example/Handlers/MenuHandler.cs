using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CefSharp.Wpf.Example.Handlers
{

    public class MenuHandler : IMenuHandler
    {

        public bool OnBeforeContextMenu(IWebBrowser browser, IContextMenuParams parameters)
        {

            Console.WriteLine("Context menu opened");
            Console.WriteLine(parameters.MisspelledWord);

            return true;
        }


    }
}
