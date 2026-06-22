using System;
using System.Net;
using System.Threading;

namespace CefSharp.BrowserSubprocess.Features
{
    public class AutoReconnect : IDisposable
    {
        private Thread monitorThread;
        private volatile bool running;
        private volatile bool connected = true;

        public string TargetUrl { get; set; } = "";
        public int CheckIntervalMs { get; set; } = 5000;
        public int RetryIntervalMs { get; set; } = 3000;
        public int MaxRetries { get; set; } = 50;
        public int TimeoutMs { get; set; } = 4000;

        public bool IsConnected { get { return connected; } }
        public bool IsRunning { get { return running; } }
        public int TotalReconnects { get; private set; }
        public int ConsecutiveFailures { get; private set; }

        public event Action<string> LogEvent;
        public event Action ConnectionLost;
        public event Action ConnectionRestored;
        public event Action<int> RetryAttempt;

        public void Start()
        {
            if (running) return;
            running = true;

            monitorThread = new Thread(MonitorLoop)
            {
                IsBackground = true,
                Name = "AutoReconnect"
            };
            monitorThread.Start();

            Log("Auto-reconnect started" + (string.IsNullOrEmpty(TargetUrl) ? " (waiting for target URL)" : " monitoring: " + TargetUrl));
        }

        public void Stop()
        {
            running = false;
        }

        public void SetTargetFromUrl(string pageUrl)
        {
            try
            {
                if (!string.IsNullOrEmpty(pageUrl) && Uri.TryCreate(pageUrl, UriKind.Absolute, out var uri))
                {
                    TargetUrl = uri.Scheme + "://" + uri.Host;
                }
            }
            catch { }
        }

        private void MonitorLoop()
        {
            while (running)
            {
                try
                {
                    Thread.Sleep(CheckIntervalMs);

                    if (string.IsNullOrEmpty(TargetUrl))
                        continue;

                    bool reachable = CheckConnectivity();

                    if (reachable && !connected)
                    {
                        connected = true;
                        ConsecutiveFailures = 0;
                        TotalReconnects++;
                        Log("Connection RESTORED to " + TargetUrl + " (total reconnects: " + TotalReconnects + ")");
                        ConnectionRestored?.Invoke();
                    }
                    else if (!reachable && connected)
                    {
                        connected = false;
                        Log("Connection LOST to " + TargetUrl + " — entering retry mode");
                        ConnectionLost?.Invoke();
                        RetryLoop();
                    }
                }
                catch (ThreadInterruptedException) { break; }
                catch (Exception ex)
                {
                    Log("Monitor error: " + ex.Message);
                }
            }
        }

        private void RetryLoop()
        {
            int retries = 0;
            while (running && !connected && retries < MaxRetries)
            {
                retries++;
                ConsecutiveFailures = retries;
                Log("Retry attempt " + retries + "/" + MaxRetries + "...");
                RetryAttempt?.Invoke(retries);

                Thread.Sleep(RetryIntervalMs);

                if (CheckConnectivity())
                {
                    connected = true;
                    ConsecutiveFailures = 0;
                    TotalReconnects++;
                    Log("Connection RESTORED after " + retries + " retries (total: " + TotalReconnects + ")");
                    ConnectionRestored?.Invoke();
                    return;
                }
            }

            if (!connected)
            {
                Log("Failed to reconnect after " + MaxRetries + " attempts — still monitoring");
                ConsecutiveFailures = 0;
            }
        }

        private bool CheckConnectivity()
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(TargetUrl);
                request.Method = "HEAD";
                request.Timeout = TimeoutMs;
                request.AllowAutoRedirect = true;

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    return (int)response.StatusCode < 500;
                }
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse resp && (int)resp.StatusCode < 500)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public string GetStatus()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("=== Auto-Reconnect Status ===");
            sb.AppendLine("  Running:              " + running);
            sb.AppendLine("  Connected:            " + connected);
            sb.AppendLine("  Target URL:           " + (string.IsNullOrEmpty(TargetUrl) ? "(not set)" : TargetUrl));
            sb.AppendLine("  Check Interval:       " + CheckIntervalMs + "ms");
            sb.AppendLine("  Retry Interval:       " + RetryIntervalMs + "ms");
            sb.AppendLine("  Max Retries:          " + MaxRetries);
            sb.AppendLine("  Total Reconnects:     " + TotalReconnects);
            sb.AppendLine("  Consecutive Failures: " + ConsecutiveFailures);
            return sb.ToString();
        }

        private void Log(string msg)
        {
            LogEvent?.Invoke("[" + DateTime.Now.ToString("HH:mm:ss") + "] [RECONNECT] " + msg);
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
