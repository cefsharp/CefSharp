using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace CefSharp.BrowserSubprocess.Features
{
    public class ResourceMonitor : IDisposable
    {
        private Thread monitorThread;
        private volatile bool running;
        private PerformanceCounter cpuCounter;
        private PerformanceCounter ramCounter;

        public int IntervalMs { get; set; } = 2000;
        public ResourceSnapshot LatestSnapshot { get; private set; }

        public event Action<ResourceSnapshot> DataUpdated;
        public event Action<string> LogEvent;

        public void Start()
        {
            if (running) return;
            running = true;

            try
            {
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                ramCounter = new PerformanceCounter("Memory", "Available MBytes");
                cpuCounter.NextValue();
            }
            catch (Exception ex)
            {
                Log("Failed to init perf counters: " + ex.Message);
            }

            monitorThread = new Thread(MonitorLoop)
            {
                IsBackground = true,
                Name = "ResourceMonitor"
            };
            monitorThread.Start();
            Log("Resource monitor started");
        }

        public void Stop()
        {
            running = false;
        }

        private void MonitorLoop()
        {
            while (running)
            {
                try
                {
                    Thread.Sleep(IntervalMs);

                    var snap = new ResourceSnapshot
                    {
                        Timestamp = DateTime.Now
                    };

                    try
                    {
                        snap.CpuPercent = cpuCounter != null ? cpuCounter.NextValue() : 0;
                    }
                    catch { snap.CpuPercent = -1; }

                    try
                    {
                        snap.AvailableRamMB = ramCounter != null ? ramCounter.NextValue() : 0;
                        var proc = Process.GetCurrentProcess();
                        snap.ProcessWorkingSetMB = proc.WorkingSet64 / (1024.0 * 1024.0);
                        snap.ProcessPrivateMemMB = proc.PrivateMemorySize64 / (1024.0 * 1024.0);
                    }
                    catch { }

                    try
                    {
                        long bytesSent = 0, bytesRecv = 0;
                        foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
                        {
                            if (ni.OperationalStatus == OperationalStatus.Up &&
                                ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                            {
                                var stats = ni.GetIPv4Statistics();
                                bytesSent += stats.BytesSent;
                                bytesRecv += stats.BytesReceived;
                            }
                        }
                        snap.NetworkBytesSent = bytesSent;
                        snap.NetworkBytesReceived = bytesRecv;
                    }
                    catch { }

                    try
                    {
                        var proc = Process.GetCurrentProcess();
                        snap.ProcessId = proc.Id;
                        snap.ProcessName = proc.ProcessName;
                        snap.ThreadCount = proc.Threads.Count;
                        snap.HandleCount = proc.HandleCount;
                    }
                    catch { }

                    LatestSnapshot = snap;
                    DataUpdated?.Invoke(snap);
                }
                catch (ThreadInterruptedException) { break; }
                catch { }
            }
        }

        public string GetFormattedStatus()
        {
            var snap = LatestSnapshot;
            if (snap == null) return "No data yet";

            var sb = new StringBuilder();
            sb.AppendLine("=== Resource Monitor ===");
            sb.AppendLine("  CPU Usage:         " + snap.CpuPercent.ToString("F1") + "%");
            sb.AppendLine("  Available RAM:     " + snap.AvailableRamMB.ToString("F0") + " MB");
            sb.AppendLine("  Process Memory:    " + snap.ProcessWorkingSetMB.ToString("F1") + " MB (working set)");
            sb.AppendLine("  Process Private:   " + snap.ProcessPrivateMemMB.ToString("F1") + " MB");
            sb.AppendLine("  Network Sent:      " + FormatBytes(snap.NetworkBytesSent));
            sb.AppendLine("  Network Received:  " + FormatBytes(snap.NetworkBytesReceived));
            sb.AppendLine("  Process:           " + snap.ProcessName + " (PID " + snap.ProcessId + ")");
            sb.AppendLine("  Threads:           " + snap.ThreadCount);
            sb.AppendLine("  Handles:           " + snap.HandleCount);
            sb.AppendLine("  Last Update:       " + snap.Timestamp.ToString("HH:mm:ss"));
            return sb.ToString();
        }

        private string FormatBytes(long bytes)
        {
            if (bytes < 1024) return bytes + " B";
            if (bytes < 1048576) return (bytes / 1024.0).ToString("F1") + " KB";
            if (bytes < 1073741824) return (bytes / 1048576.0).ToString("F1") + " MB";
            return (bytes / 1073741824.0).ToString("F2") + " GB";
        }

        private void Log(string msg)
        {
            LogEvent?.Invoke("[" + DateTime.Now.ToString("HH:mm:ss") + "] [RESMON] " + msg);
        }

        public void Dispose()
        {
            Stop();
            cpuCounter?.Dispose();
            ramCounter?.Dispose();
        }
    }

    public class ResourceSnapshot
    {
        public DateTime Timestamp { get; set; }
        public float CpuPercent { get; set; }
        public float AvailableRamMB { get; set; }
        public double ProcessWorkingSetMB { get; set; }
        public double ProcessPrivateMemMB { get; set; }
        public long NetworkBytesSent { get; set; }
        public long NetworkBytesReceived { get; set; }
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public int ThreadCount { get; set; }
        public int HandleCount { get; set; }
    }
}
