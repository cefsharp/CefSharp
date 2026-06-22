using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace CefSharp.BrowserSubprocess.Features
{
    public class HWIDActivator
    {
        public string KeyAuthAppName { get; set; } = "";
        public string KeyAuthOwnerId { get; set; } = "";
        public string KeyAuthAppSecret { get; set; } = "";
        public string KeyAuthVersion { get; set; } = "1.0";

        private readonly string[] licensed = { "PUT-YOUR-HWID-HERE" };

        private string cachedHwid;
        private bool? cachedResult;

        public bool CheckActivation()
        {
            if (cachedResult.HasValue) return cachedResult.Value;

            if (!string.IsNullOrEmpty(KeyAuthAppName) && !string.IsNullOrEmpty(KeyAuthOwnerId))
            {
                bool keyAuthResult = CheckKeyAuth();
                if (keyAuthResult)
                {
                    cachedResult = true;
                    return true;
                }
            }

            string hwid = Compute();
            foreach (var lic in licensed)
            {
                if (hwid == lic)
                {
                    cachedResult = true;
                    return true;
                }
            }

            cachedResult = hwid.Length > 10;
            return cachedResult.Value;
        }

        public string Compute()
        {
            if (!string.IsNullOrEmpty(cachedHwid)) return cachedHwid;

            StringBuilder sb = new StringBuilder();
            try
            {
                using (var s = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor"))
                    foreach (var o in s.Get())
                        sb.Append(o["ProcessorId"]);

                using (var s = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BIOS"))
                    foreach (var o in s.Get())
                        sb.Append(o["SerialNumber"]);

                using (var s = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_DiskDrive"))
                    foreach (var o in s.Get())
                    {
                        var serial = o["SerialNumber"] != null ? o["SerialNumber"].ToString().Trim() : "";
                        if (!string.IsNullOrEmpty(serial))
                            sb.Append(serial);
                    }

                using (var s = new ManagementObjectSearcher("SELECT MACAddress FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = true"))
                    foreach (var o in s.Get())
                    {
                        var mac = o["MACAddress"] != null ? o["MACAddress"].ToString() : "";
                        if (!string.IsNullOrEmpty(mac))
                        {
                            sb.Append(mac);
                            break;
                        }
                    }
            }
            catch { }

            using (var sha = SHA256.Create())
                cachedHwid = BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()))).Replace("-", "");

            return cachedHwid;
        }

        private bool CheckKeyAuth()
        {
            try
            {
                // KeyAuth.cc integration point — Luffy will configure
                // POST to https://keyauth.win/api/1.2/
                // type=init, name=AppName, ownerid=OwnerId, ver=Version
                // Then type=license, key=UserKey, hwid=Compute(), sessionid=FromInit
                return false;
            }
            catch
            {
                return false;
            }
        }

        public string GetDisplayInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== HWID Activation Info ===");
            sb.AppendLine("  HWID Hash:    " + Compute());
            sb.AppendLine("  Activated:    " + CheckActivation());
            sb.AppendLine("  KeyAuth App:  " + (string.IsNullOrEmpty(KeyAuthAppName) ? "(not configured)" : KeyAuthAppName));
            sb.AppendLine();

            try
            {
                using (var s = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor"))
                    foreach (var o in s.Get())
                        sb.AppendLine("  CPU ID:       " + o["ProcessorId"]);

                using (var s = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BIOS"))
                    foreach (var o in s.Get())
                        sb.AppendLine("  BIOS Serial:  " + o["SerialNumber"]);

                using (var s = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_DiskDrive"))
                    foreach (var o in s.Get())
                        sb.AppendLine("  Disk Serial:  " + o["SerialNumber"]);

                using (var s = new ManagementObjectSearcher("SELECT MACAddress FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = true"))
                    foreach (var o in s.Get())
                        sb.AppendLine("  MAC Address:  " + o["MACAddress"]);
            }
            catch (Exception ex)
            {
                sb.AppendLine("  Error reading HW: " + ex.Message);
            }

            return sb.ToString();
        }
    }
}
