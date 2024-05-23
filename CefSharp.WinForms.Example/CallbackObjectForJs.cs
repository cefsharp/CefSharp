using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharp.WinForms.Example
{
    public class CallbackObjectForJs
    {
        public void clickOnBrowser(int x, int y)
        {
            BrowserForm.Instance.ClickOnBrowser(x, y);
        }

        public void log(string msg)
        {
            BrowserForm.Instance.Log(msg);
            BrowserForm.Instance.LogToMainApp(msg);
        }

        public void loginSuccess()
        {
            BrowserForm.Instance.LoginSuccess();
        }

        public void acceptFriendsCompleted()
        {
            BrowserForm.Instance.AcceptFriendsCompleted();
        }

        public void loginFail()
        {
            BrowserForm.Instance.Log("loginFail");
            BrowserForm.Instance.LoginFail();
        }

        public void updateFriendList(string list, string msg)
        {
            BrowserForm.Instance.UpdateFriendList(list, msg);
        }

        public void updateFriendListEmpty(string list)
        {
            BrowserForm.Instance.UpdateFriendListEmpty(list);
        }

        public void updateFriendListForAcceptFriend(string list, string msg)
        {
            BrowserForm.Instance.UpdateFriendListForAcceptFriend(list, msg);
        }

        public void sendMessageComplete(string msg)
        {
            BrowserForm.Instance.SendMessageComplete(msg);
        }

        public void sendInviteComplete(string msg)
        {
            BrowserForm.Instance.SendInviteComplete(msg);
        }

        public void sendMessageToPhoneNumberComplete(string msg)
        {
            BrowserForm.Instance.SendMessageToPhoneNumberComplete(msg);
        }

        public void updateMessagePerFriendStatus(string friendName, string status)
        {
            BrowserForm.Instance.UpdateMessagePerFriendStatus(friendName, status);
        }

        public void updateInvitePerFriendStatus(string friendPhoneNumber, string status)
        {
            BrowserForm.Instance.UpdateInvitePerFriendStatus(friendPhoneNumber, status);
        }

        public void updateSendMessageToPhoneNumberPerFriendStatus(string friendPhoneNumber, string status)
        {
            BrowserForm.Instance.UpdateSendMessageToPhoneNumberPerFriendStatus(friendPhoneNumber, status);
        }
    }
}
