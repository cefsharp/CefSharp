// Copyright © 2010 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using Newtonsoft.Json;
using Share;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CefSharp.WinForms.Example
{
    public partial class BrowserForm : Form
    {
        /// <summary>
        /// Biến này dùng cho mục đích test, không phải lập trình nghiệp vụ
        /// </summary>
        private bool isShowForm = true;

        /// <summary>
        /// Biến này dùng cho mục đích test, không phải lập trình nghiệp vụ
        /// </summary>
        private bool isCloseForm = false;

        /// <summary>
        /// Dùng cho việc testing
        /// </summary>
        private Thread threadTimerGetSignal;

        private BrowserTabUserControl browser;
        public string CurrentCmd;
        public string Proxy { get; set; }
        public string ApiUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string AccountName { get; set; }
        public static BrowserForm Instance { get; set; }
        private const string DefaultUrlForAddedTabs = "https://chat.zalo.me/";

        private bool multiThreadedMessageLoopEnabled;

        public BrowserForm(bool multiThreadedMessageLoopEnabled)
        {
            Instance = this;
            InitializeComponent();

            Visible = true;
            ShowInTaskbar = true;
            Opacity = 1;

            var bitness = Environment.Is64BitProcess ? "x64" : "x86";
            Text = "CefSharp.WinForms.Example - " + bitness;
            WindowState = FormWindowState.Maximized;

            //Only perform layout when control has completly finished resizing
            ResizeBegin += (s, e) => SuspendLayout();
            ResizeEnd += (s, e) => ResumeLayout(true);

            this.multiThreadedMessageLoopEnabled = multiThreadedMessageLoopEnabled;
        }

        public IContainer Components
        {
            get
            {
                if (components == null)
                {
                    components = new Container();
                }

                return components;
            }
        }

        /// <summary>
        /// Used to add a Popup browser as a Tab
        /// </summary>
        /// <param name="browserHostControl"></param>
        public void AddTab(Control browserHostControl, string url)
        {
            browserTabControl.SuspendLayout();

            var tabPage = new TabPage(url)
            {
                Dock = DockStyle.Fill
            };

            tabPage.Controls.Add(browserHostControl);

            browserTabControl.TabPages.Add(tabPage);

            //Make newly created tab active
            browserTabControl.SelectedTab = tabPage;

            browserTabControl.ResumeLayout(true);
        }

        /// <summary>
        /// mở 1 tab
        /// </summary>
        /// <param name="url"></param>
        /// <param name="insertIndex"></param>
        private void AddTab(string url, int? insertIndex = null)
        {
            browserTabControl.SuspendLayout();

            browser = new BrowserTabUserControl(AddTab, url, multiThreadedMessageLoopEnabled)
            {
                Dock = DockStyle.Fill,
            };

            browser.Browser.FrameLoadEnd += (sender, args) =>
            {
                if (args.Frame.IsMain)
                {
                    Task.Factory.StartNew(RunJsOnMainFrame);
                }
            };

            var tabPage = new TabPage(url)
            {
                Dock = DockStyle.Fill
            };

            //This call isn't required for the sample to work.
            //It's sole purpose is to demonstrate that #553 has been resolved.
            browser.CreateControl();

            tabPage.Controls.Add(browser);

            if (insertIndex == null)
            {
                browserTabControl.TabPages.Add(tabPage);
            }
            else
            {
                browserTabControl.TabPages.Insert(insertIndex.Value, tabPage);
            }

            //Make newly created tab active
            browserTabControl.SelectedTab = tabPage;

            browserTabControl.ResumeLayout(true);
        }

        /// <summary>
        /// chạy code javascript vào page
        /// </summary>
        private void RunJsOnMainFrame()
        {
            browser.Browser.GetMainFrame().EvaluateScriptAsync(Properties.Resources.JQueryStript).ContinueWith(t =>
            {
                browser.Browser.GetMainFrame().EvaluateScriptAsync(
                    //CefSharp.WinForms.Example.Properties.Resources.MainJS
                    ReadFile("main.js")
                    ).ContinueWith(t1 =>
                {
                    var zaloFriendInviteStatus = typeof(ZaloFriendInviteStatus).GetAllPublicConstantValues<string>();
                    var zaloFriendMessageStatus = typeof(ZaloFriendMessageStatus).GetAllPublicConstantValues<string>();
                    var script = string.Format("bot.init('{0}', {1}, {2})", PhoneNumber, JsonConvert.SerializeObject(zaloFriendInviteStatus), JsonConvert.SerializeObject(zaloFriendMessageStatus));

                    browser.Browser.GetMainFrame().EvaluateScriptAsync(script).ContinueWith(t2 =>
                    {
                        browser.Browser.GetMainFrame().EvaluateScriptAsync("(function(){return typeof jQuery != 'undefined' && typeof bot != 'undefined'})()").ContinueWith(t3 =>
                        {
                            var response = t3.Result;

                            if (response.Success && response.Result != null)
                            {
                                if (response.Result.ToStr().IndexOf("True") >= 0)
                                {
                                    HandleCmd();
                                }
                            }
                        });
                    });
                });
            });
        }

        /// <summary>
        /// đọc code javascript từ file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string ReadFile(string fileName)
        {
            return File.ReadAllText(Path.Combine(Application.StartupPath, fileName));
        }

        /// <summary>
        /// xóa tab
        /// </summary>
        /// <param name="windowHandle"></param>
        public void RemoveTab(IntPtr windowHandle)
        {
            var parentControl = FromChildHandle(windowHandle);
            if (!parentControl.IsDisposed)
            {
                if (parentControl.Parent is TabPage tabPage)
                {
                    browserTabControl.TabPages.Remove(tabPage);
                }
                else if (parentControl.Parent is Panel panel)
                {
                    var browserTabUserControl = (BrowserTabUserControl)panel.Parent;

                    var tab = (TabPage)browserTabUserControl.Parent;
                    browserTabControl.TabPages.Remove(tab);
                }
            }
        }

        /// <summary>
        /// gọi api, truyền message log về ứng dụng chính
        /// </summary>
        /// <param name="msg"></param>
        public async Task LogToMainApp(string msg)
        {
            await PostToMainApp("/api/js/log", new { message = msg });
        }

        /// <summary>
        /// được gọi từ js, thông báo hoàn thành login, dùng cho login tự động, và login thủ công
        /// </summary>
        public async void LoginSuccess()
        {
            await PostToMainApp("/api/js/loginSuccess", new { phoneNumber = PhoneNumber });
            if (isCloseForm)
            {
                Close();
            }
        }

        /// <summary>
        /// được gọi từ js, thông báo thực hiện lệnh chấp nhận kết bạn hoàn thất
        /// </summary>
        public async void AcceptFriendsCompleted()
        {
            await PostToMainApp("/api/js/AcceptFriendsCompleted", new { phoneNumber = PhoneNumber });
            if (isCloseForm)
            {
                Close();
            }
        }

        /// <summary>
        /// được gọi từ js, cập nhật trạng thái việc gửi 1 tin nhắn
        /// </summary>
        /// <param name="friendName"></param>
        public async void UpdateMessagePerFriendStatus(string friendName, string status)
        {
            await PostToMainApp("/api/js/UpdateMessagePerFriendStatus", new { phoneNumber = PhoneNumber, friendName, status });
        }

        /// <summary>
        /// hàm này được gọi từ js, chập nhật trạng thái việc gửi 1 lời mời kết bạn
        /// </summary>
        /// <param name="friendPhoneNumber"></param>
        /// <param name="status"></param>
        public async void UpdateInvitePerFriendStatus(string friendPhoneNumber, string status)
        {
            await PostToMainApp("/api/js/UpdateInvitePerFriendStatus", new { phoneNumber = PhoneNumber, friendPhoneNumber, status });
        }

        public async void UpdateSendMessageToPhoneNumberPerFriendStatus(string friendPhoneNumber, string status)
        {
            await PostToMainApp("/api/js/UpdateSendMessageToPhoneNumberPerFriendStatus", new { phoneNumber = PhoneNumber, friendPhoneNumber, status });
        }

        /// <summary>
        /// được gọi từ js, thông báo login fail, dùng cho login tự động và login thủ công
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task LoginFail(string msg = "")
        {
            await PostToMainApp("/api/js/loginFail", new { phoneNumber = PhoneNumber, message = msg });
            if (isCloseForm)
            {
                Close();
            }
        }

        /// <summary>
        /// được gọi từ js, truyền danh sách bạn bè về ứng dụng chính
        /// </summary>
        /// <param name="list"></param>
        /// <param name="msg"></param>
        public async void UpdateFriendList(string list, string msg)
        {
            await PostToMainApp("/api/js/UpdateFriendList", new { listFriend = list, phoneNumber = PhoneNumber, message = msg });
            if (isCloseForm)
            {
                Close();
            }
        }

        /// <summary>
        /// được gọi từ js, truyền danh sách bạn bè về ứng dụng chính, nhưng danh sách rỗng
        /// </summary>
        /// <param name="list"></param>
        public async void UpdateFriendListEmpty(string list)
        {
            await PostToMainApp("/api/js/UpdateFriendListEmpty", new { listFriend = list, phoneNumber = PhoneNumber });
            if (isCloseForm)
            {
                Close();
            }
        }

        /// <summary>
        /// được gọi từ js, truyền danh sách kết bạn về ứng dụng chính
        /// </summary>
        /// <param name="list"></param>
        /// <param name="msg"></param>
        public async void UpdateFriendListForAcceptFriend(string list, string msg)
        {
            await PostToMainApp("/api/js/UpdateFriendListForAcceptFriend", new { listFriend = list, phoneNumber = PhoneNumber, message = msg });
            if (isCloseForm)
            {
                Close();
            }
        }

        /// <summary>
        /// được gọi từ js, thông báo việc gửi tin nhắn hoàn thành
        /// </summary>
        /// <param name="msg"></param>
        public async void SendMessageComplete(string msg)
        {
            await PostToMainApp("/api/js/SendMessageComplete", new { phoneNumber = PhoneNumber, message = msg });
            if (isCloseForm)
            {
                Close();
            }
        }

        /// <summary>
        /// được gọi từ js, thông báo việc gởi lời mời kết bạn hoàn thành
        /// </summary>
        /// <param name="msg"></param>
        public async void SendInviteComplete(string msg)
        {
            await PostToMainApp("/api/js/SendInviteComplete", new { phoneNumber = PhoneNumber, message = msg });
            if (isCloseForm)
            {
                Close();
            }
        }

        public async void SendMessageToPhoneNumberComplete(string msg)
        {
            await PostToMainApp("/api/js/SendMessageToPhoneNumberComplete", new { phoneNumber = PhoneNumber, message = msg });
            if (isCloseForm)
            {
                Close();
            }
        }

        /// <summary>
        /// hàm sử dụng chung, khi gọi về ứng dụng chính thông qua web api, phương thức POST
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task PostToMainApp(string path, object data)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(ApiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(path, content);
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        /// <summary>
        /// hàm sử dụng chung, khi gọi về ứng dụng chính thông qua web api, phương thức GET
        /// </summary>
        /// <param name="path"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private async Task<string> GetToMainApp(string path, string param)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(ApiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(path + "?" + param);

                if (response.IsSuccessStatusCode)
                {
                    var cmd = await response.Content.ReadAsStringAsync();
                    return cmd;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }

            return string.Empty;
        }

        private async void BrowserForm_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Proxy))
            {
                Log(string.Format("Proxy: {0}", Proxy));
            }

            Text = string.Format("Tài khoản: {0} ( {1} )", AccountName, PhoneNumber);

            if (NetworkInterface.GetIsNetworkAvailable() && new Ping().Send(new IPAddress(new byte[] { 8, 8, 8, 8 }), 2000).Status == IPStatus.Success)
            {
                AddTab(DefaultUrlForAddedTabs);

                threadTimerGetSignal = new Thread(new ThreadStart(HandleSignal));
                threadTimerGetSignal.Start();
            }
            else
            {
                var msg = "Không có kết nối internet";
                if (CurrentCmd == Cmd.ShowFriends || CurrentCmd == Cmd.GetFriendsForSendMessage)
                {
                    UpdateFriendList("", msg);
                }

                if (CurrentCmd == Cmd.SendMessage)
                {
                    SendMessageComplete(msg);
                }

                if (CurrentCmd == Cmd.ShowLogin || CurrentCmd == Cmd.AutoLogin)
                {
                    await LoginFail(msg);
                }
            }
        }

        /// <summary>
        /// dùng cho việc testing
        /// </summary>
        private async void HandleSignal()
        {
            do
            {
                var msg = await GetToMainApp("/api/js/GetSignal", "phoneNumber=" + PhoneNumber);
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    msg = ProcessStringResultFromApi(msg);
                    if (msg.Contains(Cmd.ShowBrowser))
                    {
                        ShowForm();
                    }
                }
                Thread.Sleep(1000);
            }
            while (true);
        }

        /// <summary>
        /// xử lý các lệnh truyền từ ứng dụng chính qua
        /// </summary>
        private void HandleCmd()
        {
            if (CurrentCmd == Cmd.ShowFriends)
            {
                if (isShowForm)
                {
                    ShowForm();
                }

                var script = string.Format("bot.getFriends()");
                browser.Browser.GetMainFrame().ExecuteJavaScriptAsync(script);
            }

            if (CurrentCmd == Cmd.GetFriendsForSendMessage)
            {
                if (isShowForm)
                {
                    ShowForm();
                }
                var script = string.Format("bot.getFriends()");
                browser.Browser.GetMainFrame().ExecuteJavaScriptAsync(script);
            }

            if (CurrentCmd == Cmd.AcceptFriend)
            {
                if (isShowForm)
                {
                    ShowForm();
                }
                HandleAcceptFriend();
            }

            if (CurrentCmd == Cmd.ShowFriendsForAcceptFriend)
            {
                if (isShowForm)
                {
                    ShowForm();
                }
                var script = string.Format("bot.getFriendsForAcceptFriends()");
                browser.Browser.GetMainFrame().ExecuteJavaScriptAsync(script);
            }

            if (CurrentCmd == Cmd.SendMessage)
            {
                if (isShowForm)
                {
                    ShowForm();
                }
                HandleSendMessage();
            }

            if (CurrentCmd == Cmd.SendInvites)
            {
                if (isShowForm)
                {
                    ShowForm();
                }
                HandleSendInvite();
            }

            if (CurrentCmd == Cmd.SendMessageToPhoneNumbers)
            {
                if (isShowForm)
                {
                    ShowForm();
                }

                HandleSendMessageToPhoneNumber();
            }

            if (CurrentCmd == Cmd.ShowLogin)
            {
                ShowForm();
                var script = string.Format("bot.checkLogin(false)");
                browser.Browser.GetMainFrame().ExecuteJavaScriptAsync(script);
            }

            if (CurrentCmd == Cmd.AutoLogin)
            {
                if (isShowForm)
                {
                    ShowForm();
                }
                var script = string.Format("bot.checkLogin(true)");
                browser.Browser.GetMainFrame().ExecuteJavaScriptAsync(script);
            }
        }

        /// <summary>
        /// xử lý lệnh chấp nhận lời mời kết bạn
        /// </summary>
        private async void HandleAcceptFriend()
        {
            var friendsStr = await GetToMainApp("/api/js/GetFriendsForAccept", "phoneNumber=" + PhoneNumber);
            friendsStr = ProcessStringResultFromApi(friendsStr);
            Log(friendsStr);

            var friends = friendsStr.Split(new[] { "<name>", "</name>" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var listFriends = new List<string>();
            friends.ForEach(i =>
            {
                listFriends.Add(string.Format("'{0}'", i));
            });

            var jsFriendArr = string.Format("[{0}]", string.Join(",", listFriends));

            Log("Friends: " + jsFriendArr);

            var script = string.Format("bot.acceptFriends({0})", jsFriendArr);
            browser.Browser.GetMainFrame().ExecuteJavaScriptAsync(script);
        }

        /// <summary>
        /// xử lý lệnh gửi tin nhắn
        /// </summary>
        private async void HandleSendMessage()
        {
            //lấy nội dung tin nhắn
            var msg = await GetToMainApp("/api/js/GetMessage", "phoneNumber=" + PhoneNumber);
            msg = ProcessStringResultFromApi(msg);
            Log("Tin nhắn:" + msg);

            //lấy danh sách bạn bè checked bởi người dùng
            var friendsStr = await GetToMainApp("/api/js/GetFriendsForSendMessage", "phoneNumber=" + PhoneNumber);
            friendsStr = ProcessStringResultFromApi(friendsStr);
            var friends = friendsStr.Split(new[] { "<name>", "</name>" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            Log("Friends: " + string.Join(",", friends));

            var listFriends = new List<string>();
            friends.ForEach(i =>
            {
                listFriends.Add(string.Format("'{0}'", i.Replace("'", "%27")));
            });

            var jsFriendArr = string.Format("[{0}]", string.Join(",", listFriends));

            //lấy thời gian giữa 2 lần gửi tin nhắn
            var delaySecondStr = await GetToMainApp("/api/js/GetDelaySecondForSendMessage", "phoneNumber=" + PhoneNumber);
            delaySecondStr = ProcessStringResultFromApi(delaySecondStr);

            var delaySecond = 0;
            int.TryParse(delaySecondStr, out delaySecond);

            var script = string.Format("bot.sendMessage('{0}', {1}, {2})", msg, jsFriendArr, delaySecond);
            browser.Browser.GetMainFrame().ExecuteJavaScriptAsync(script);
        }

        /// <summary>
        /// xử lý lệnh gửi lời mời kết bạn
        /// </summary>
        private async void HandleSendInvite()
        {
            //lấy nội dung tin nhắn
            var msg = await GetToMainApp("/api/js/GetInviteMessage", "phoneNumber=" + PhoneNumber);
            msg = ProcessStringResultFromApi(msg);
            Log("Tin nhắn:" + msg);

            //lấy danh sách bạn bè checked bởi người dùng
            var friendsStr = await GetToMainApp("/api/js/GetFriendsForSendInvite", "phoneNumber=" + PhoneNumber);
            friendsStr = ProcessStringResultFromApi(friendsStr);
            var friends = friendsStr.Split(new[] { "<PhoneNumber>", "</PhoneNumber>" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            Log("Friends: " + string.Join(",", friends));

            var listFriends = new List<string>();
            friends.ForEach(i =>
            {
                listFriends.Add(string.Format("'{0}'", i.Replace("'", "%27")));
            });

            var jsFriendArr = string.Format("[{0}]", string.Join(",", listFriends));

            //lấy thời gian giữa 2 lần gửi lời mời
            var delaySecondStr = await GetToMainApp("/api/js/GetDelaySecondForSendInvite", "phoneNumber=" + PhoneNumber);
            delaySecondStr = ProcessStringResultFromApi(delaySecondStr);

            var delaySecond = 0;
            int.TryParse(delaySecondStr, out delaySecond);

            var script = string.Format("bot.sendInvite('{0}', {1}, {2})", msg, jsFriendArr, delaySecond);

            browser.Browser.GetMainFrame().ExecuteJavaScriptAsync(script);
        }

        private async void HandleSendMessageToPhoneNumber()
        {
            //lấy nội dung tin nhắn
            var msg = await GetToMainApp("/api/js/GetSendMessageToPhoneNumberContent", "phoneNumber=" + PhoneNumber);
            msg = ProcessStringResultFromApi(msg);
            Log("Tin nhắn:" + msg);

            //lấy danh sách bạn bè checked bởi người dùng
            var friendsStr = await GetToMainApp("/api/js/GetFriendsForSendMessageToPhoneNumber", "phoneNumber=" + PhoneNumber);
            friendsStr = ProcessStringResultFromApi(friendsStr);
            var friends = friendsStr.Split(new[] { "<PhoneNumber>", "</PhoneNumber>" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            Log("Friends: " + string.Join(",", friends));

            var listFriends = new List<string>();
            friends.ForEach(i =>
            {
                listFriends.Add(string.Format("'{0}'", i.Replace("'", "%27")));
            });

            var jsFriendArr = string.Format("[{0}]", string.Join(",", listFriends));

            //lấy thời gian giữa 2 lần gửi lời mời
            var delaySecondStr = await GetToMainApp("/api/js/GetDelaySecondForSendMessageToPhoneNumber", "phoneNumber=" + PhoneNumber);
            delaySecondStr = ProcessStringResultFromApi(delaySecondStr);

            var delaySecond = 0;
            int.TryParse(delaySecondStr, out delaySecond);

            var script = string.Format("bot.sendMessageToPhoneNumber('{0}', {1}, {2})", msg, jsFriendArr, delaySecond);

            browser.Browser.GetMainFrame().ExecuteJavaScriptAsync(script);
        }

        /// <summary>
        /// kết quả string gởi từ api về, sẽ có dấu " ở đầu và cuối tring, hàm này bỏ dấu " ở đầu và cuối string
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private string ProcessStringResultFromApi(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg)) return msg;
            msg = msg.Remove(0, 1);
            msg = msg.Remove(msg.Length - 1, 1);
            return msg;
        }

        /// <summary>
        /// hiển thị browser, chỉ dùng khi kiểm tra lỗi, hoặc thực hiện lệnh login thủ công, còn các lện khác không sử dụng hàm này
        /// </summary>
        private void ShowForm()
        {
            this.InvokeIfNeeded(() =>
            {
                WindowState = FormWindowState.Maximized;
                ShowInTaskbar = true;
                Opacity = 1000;
                Show();
                Activate();
                Focus();
                BringToFront();
            });
        }

        /// <summary>
        /// ghi log ra textbox
        /// </summary>
        /// <param name="msg"></param>
        public void Log(string msg)
        {
            this.InvokeIfNeeded(() =>
            {
                if (!string.IsNullOrWhiteSpace(tbxLog.Text) && tbxLog.Text.Length > 5000) tbxLog.Text = string.Empty;
                tbxLog.Text += msg + Environment.NewLine;
                tbxLog.SelectionStart = tbxLog.Text.Length;
                tbxLog.ScrollToCaret();
            });
        }

        public void ClickOnBrowser(int x, int y)
        {
            browser.Browser.GetBrowserHost().SendMouseClickEvent(x, y, MouseButtonType.Left, false, 1, CefEventFlags.None);
            Thread.Sleep(15);
            browser.Browser.GetBrowserHost().SendMouseClickEvent(x, y, MouseButtonType.Left, true, 1, CefEventFlags.None);

            Log("Clicked at " + x + " | " + y);
        }

        private void BrowserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (threadTimerGetSignal != null && threadTimerGetSignal.IsAlive)
                {
                    threadTimerGetSignal.Abort();
                }
            }
            catch { }
        }
    }
}
