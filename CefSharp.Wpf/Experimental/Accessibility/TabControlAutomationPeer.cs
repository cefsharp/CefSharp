using System.Collections.Generic;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Media;

namespace CefSharp.Wpf.Experimental.Accessibility
{
    /// <summary>
    /// Default TabControl's AutomationPeer doesn’t know anything about the controls within it, since they’re loaded dynamically.
    /// The purpose of this class is to fix this behavior.
    /// </summary>
    /// <remarks>
    /// Taken from https://www.colinsalmcorner.com/post/genericautomationpeer--helping-the-coded-ui-framework-find-your-custom-controls
    /// </remarks>
    public class TabControlAutomationPeer : UIElementAutomationPeer
    {
        public TabControlAutomationPeer(UIElement owner) : base(owner)
        {
        }

        protected override List<AutomationPeer> GetChildrenCore()
        {
            var list = base.GetChildrenCore();
            list.AddRange(GetChildPeers(Owner));
            return list;
        }

        private List<AutomationPeer> GetChildPeers(UIElement element)
        {
            var list = new List<AutomationPeer>();

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var child = VisualTreeHelper.GetChild(element, i) as UIElement;
                if (child != null)
                {
                    var childPeer = CreatePeerForElement(child);
                    if (childPeer != null)
                    {
                        list.Add(childPeer);
                    }
                    else
                    {
                        list.AddRange(GetChildPeers(child));
                    }
                }
            }

            return list;
        }
    }
}
