using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CefSharp.BrowserSubprocess.Features
{
    public class JSInjectionEngine : IDisposable
    {
        private readonly string persistPath;
        private List<JSInjectionRule> rules = new List<JSInjectionRule>();
        private readonly object syncLock = new object();

        public event Action<string> LogEvent;
        public event Action<string> InjectRequested;

        public JSInjectionEngine()
        {
            persistPath = Path.Combine(Path.GetTempPath(), "seb_js_rules_v8.json");
            LoadRules();
        }

        public IReadOnlyList<JSInjectionRule> Rules
        {
            get { lock (syncLock) return rules.ToList().AsReadOnly(); }
        }

        public List<string> GetScriptsForUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return new List<string>();

            lock (syncLock)
            {
                return rules
                    .Where(r => r.Enabled && MatchesPattern(url, r.UrlPattern))
                    .Select(r => r.Script)
                    .ToList();
            }
        }

        public void InjectNow(string script)
        {
            if (string.IsNullOrEmpty(script)) return;
            Log("Manual inject: " + script.Substring(0, Math.Min(60, script.Length)) + "...");
            InjectRequested?.Invoke(script);
        }

        public void AddRule(string urlPattern, string script, string name = "")
        {
            lock (syncLock)
            {
                rules.Add(new JSInjectionRule
                {
                    Id = Guid.NewGuid().ToString("N").Substring(0, 8),
                    Name = string.IsNullOrEmpty(name) ? "Rule_" + (rules.Count + 1) : name,
                    UrlPattern = urlPattern,
                    Script = script,
                    Enabled = true
                });
            }
            SaveRules();
            Log("Added JS rule: " + urlPattern);
        }

        public void RemoveRule(string id)
        {
            lock (syncLock)
            {
                rules.RemoveAll(r => r.Id == id);
            }
            SaveRules();
        }

        public void ToggleRule(string id)
        {
            lock (syncLock)
            {
                var rule = rules.FirstOrDefault(r => r.Id == id);
                if (rule != null)
                    rule.Enabled = !rule.Enabled;
            }
            SaveRules();
        }

        public void UpdateRule(string id, string urlPattern, string script, string name)
        {
            lock (syncLock)
            {
                var rule = rules.FirstOrDefault(r => r.Id == id);
                if (rule != null)
                {
                    rule.UrlPattern = urlPattern;
                    rule.Script = script;
                    rule.Name = name;
                }
            }
            SaveRules();
        }

        public void ClearRules()
        {
            lock (syncLock) rules.Clear();
            SaveRules();
        }

        private bool MatchesPattern(string url, string pattern)
        {
            if (string.IsNullOrEmpty(pattern) || pattern == "*")
                return true;

            try
            {
                string regexPattern = "^" + System.Text.RegularExpressions.Regex.Escape(pattern)
                    .Replace("\\*", ".*")
                    .Replace("\\?", ".") + "$";

                return System.Text.RegularExpressions.Regex.IsMatch(url, regexPattern,
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }
            catch
            {
                return url.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }

        public void SaveRules()
        {
            try
            {
                lock (syncLock)
                {
                    var json = JsonConvert.SerializeObject(rules, Formatting.Indented);
                    File.WriteAllText(persistPath, json, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                Log("Failed to save JS rules: " + ex.Message);
            }
        }

        private void LoadRules()
        {
            try
            {
                if (File.Exists(persistPath))
                {
                    var json = File.ReadAllText(persistPath, Encoding.UTF8);
                    var loaded = JsonConvert.DeserializeObject<List<JSInjectionRule>>(json);
                    if (loaded != null)
                    {
                        lock (syncLock) rules = loaded;
                    }
                }
            }
            catch (Exception ex)
            {
                Log("Failed to load JS rules: " + ex.Message);
            }
        }

        private void Log(string msg)
        {
            LogEvent?.Invoke("[" + DateTime.Now.ToString("HH:mm:ss") + "] [JS-INJECT] " + msg);
        }

        public void Dispose()
        {
            SaveRules();
        }
    }

    [Serializable]
    public class JSInjectionRule
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UrlPattern { get; set; }
        public string Script { get; set; }
        public bool Enabled { get; set; }
    }
}
