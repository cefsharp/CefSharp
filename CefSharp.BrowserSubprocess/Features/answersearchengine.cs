using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CefSharp.BrowserSubprocess.Features
{
    /// <summary>
    /// AnswerSearchEngine — searches BOT folder files for answers matching a question.
    ///
    /// Supports 4 file formats:
    /// 1. Q:/A: tagged    → "Q: question\nA: answer"
    /// 2. Heading-based   → "## question\nanswer text"
    /// 3. Numbered        → "1. question\nanswer text"
    /// 4. Separator-based → "question\n---\nanswer text\n==="
    ///
    /// Matching strategies (in priority order):
    /// 1. Exact Q: line match
    /// 2. Heading match
    /// 3. Keyword intersection (highest overlap wins)
    /// 4. Fuzzy substring (longest common subsequence)
    /// </summary>
    public class AnswerSearchEngine
    {
        private readonly string botFolderPath;

        public string BotFolderPath { get { return botFolderPath; } }

        public event Action<string> LogEvent;

        public AnswerSearchEngine(string botFolder)
        {
            botFolderPath = botFolder;
        }

        /// <summary>
        /// Searches all text files in the BOT folder for an answer matching the question.
        /// Returns the best answer found, or null if no match.
        /// </summary>
        public SearchResult Search(string questionText)
        {
            if (string.IsNullOrWhiteSpace(questionText) || string.IsNullOrEmpty(botFolderPath) || !Directory.Exists(botFolderPath))
                return null;

            var cleanQ = CleanQuestion(questionText);
            Log("Searching BOT folder for: \"" + cleanQ + "\"");

            var files = GetTextFiles();
            Log("Found " + files.Count + " text files in BOT folder");

            SearchResult best = null;

            foreach (var file in files)
            {
                try
                {
                    var content = File.ReadAllText(file, DetectEncoding(file));
                    var entries = ParseFile(content, file);

                    foreach (var entry in entries)
                    {
                        float score = ScoreMatch(entry.Question, cleanQ);
                        Log("  " + Path.GetFileName(file) + " — Q: \"" + Truncate(entry.Question, 60) + "\" → score: " + score.ToString("F2"));

                        if (score > 0.3f && (best == null || score > best.Score))
                        {
                            best = new SearchResult
                            {
                                Question = entry.Question,
                                Answer = entry.Answer,
                                SourceFile = file,
                                Score = score
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log("Error reading " + Path.GetFileName(file) + ": " + ex.Message);
                }
            }

            if (best != null)
                Log("BEST MATCH: " + Path.GetFileName(best.SourceFile) + " (score: " + best.Score.ToString("F2") + ")");
            else
                Log("No matching answer found");

            return best;
        }

        /// <summary>
        /// Cleans question text: removes leading numbers, trims, collapses whitespace.
        /// </summary>
        private string CleanQuestion(string raw)
        {
            if (string.IsNullOrEmpty(raw)) return "";
            var text = raw.Trim();
            // Remove leading "1." / "1)" / "Q:" / "Question:" etc.
            text = Regex.Replace(text, @"^[\d]+[.)]\s*", "");
            text = Regex.Replace(text, @"^(Q|Question)[:.]\s*", "", RegexOptions.IgnoreCase);
            // Remove trailing question mark duplicates
            text = text.TrimEnd('?', ' ', '\t');
            // Collapse whitespace
            text = Regex.Replace(text, @"\s+", " ").Trim();
            return text;
        }

        /// <summary>
        /// Gets all text-readable files from the BOT folder recursively.
        /// </summary>
        private List<string> GetTextFiles()
        {
            var textExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { ".txt", ".md", ".csv", ".log", ".json", ".xml", ".html", ".htm" };

            var files = new List<string>();
            try
            {
                files = Directory.GetFiles(botFolderPath, "*.*", SearchOption.AllDirectories)
                    .Where(f => textExtensions.Contains(Path.GetExtension(f)))
                    .ToList();
            }
            catch { }
            return files;
        }

        /// <summary>
        /// Parses a file's content into Q/A entries, auto-detecting the format.
        /// </summary>
        public List<QAEntry> ParseFile(string content, string sourceFile)
        {
            var entries = new List<QAEntry>();

            // Detect format by looking at content patterns
            if (Regex.IsMatch(content, @"(?m)^(Q|Question)\s*[:.]"))
                entries = ParseTaggedFormat(content);
            else if (Regex.IsMatch(content, @"(?m)^#{1,3}\s"))
                entries = ParseHeadingFormat(content);
            else if (Regex.IsMatch(content, @"(?m)^\d+[.)]\s"))
                entries = ParseNumberedFormat(content);
            else if (content.Contains("---") || content.Contains("==="))
                entries = ParseSeparatorFormat(content);
            else
                entries = ParsePlainFormat(content, sourceFile);

            return entries;
        }

        /// <summary>
        /// Format 1: Q:/A: tagged.
        /// Q: What is photosynthesis?
        /// A: It is the process...
        /// </summary>
        private List<QAEntry> ParseTaggedFormat(string content)
        {
            var entries = new List<QAEntry>();
            var matches = Regex.Matches(content, @"(?ms)^(?:Q|Question)\s*[:.]\s*(.+?)(?:\r?\n)+(?:A|Answer)\s*[:.]\s*(.+?)(?=\r?\n\s*\r?\n|\r?\n(?:Q|Question)\s*[:.]|$)");

            foreach (Match m in matches)
            {
                entries.Add(new QAEntry
                {
                    Question = m.Groups[1].Value.Trim(),
                    Answer = m.Groups[2].Value.Trim()
                });
            }

            // Fallback: line-by-line parse for malformed entries
            if (entries.Count == 0)
            {
                var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                string curQ = null;
                var curA = new StringBuilder();

                foreach (var line in lines)
                {
                    var qMatch = Regex.Match(line, @"^(?:Q|Question)\s*[:.]\s*(.+)", RegexOptions.IgnoreCase);
                    var aMatch = Regex.Match(line, @"^(?:A|Answer)\s*[:.]\s*(.*)", RegexOptions.IgnoreCase);

                    if (qMatch.Success)
                    {
                        if (curQ != null && curA.Length > 0)
                            entries.Add(new QAEntry { Question = curQ, Answer = curA.ToString().Trim() });
                        curQ = qMatch.Groups[1].Value.Trim();
                        curA.Clear();
                    }
                    else if (aMatch.Success && curQ != null)
                    {
                        curA.AppendLine(aMatch.Groups[1].Value);
                    }
                    else if (curQ != null && !string.IsNullOrWhiteSpace(line))
                    {
                        curA.AppendLine(line);
                    }
                }

                if (curQ != null && curA.Length > 0)
                    entries.Add(new QAEntry { Question = curQ, Answer = curA.ToString().Trim() });
            }

            return entries;
        }

        /// <summary>
        /// Format 2: Markdown headings.
        /// ## What is photosynthesis?
        /// It is the process...
        /// </summary>
        private List<QAEntry> ParseHeadingFormat(string content)
        {
            var entries = new List<QAEntry>();
            var matches = Regex.Matches(content, @"(?m)^(#{1,3})\s+(.+?)(?:\r?\n)+((?:(?!#{1,3}\s).+?(?:\r?\n|$))*)");

            foreach (Match m in matches)
            {
                var q = m.Groups[2].Value.Trim();
                var a = m.Groups[3].Value.Trim();
                if (!string.IsNullOrEmpty(q) && !string.IsNullOrEmpty(a))
                    entries.Add(new QAEntry { Question = q, Answer = a });
            }

            return entries;
        }

        /// <summary>
        /// Format 3: Numbered questions.
        /// 1. What is photosynthesis?
        /// It is the process...
        /// </summary>
        private List<QAEntry> ParseNumberedFormat(string content)
        {
            var entries = new List<QAEntry>();
            var matches = Regex.Matches(content, @"(?m)^(\d+)[.)]\s+(.+?)(?:\r?\n)+((?:(?!\d+[.)]\s).+?(?:\r?\n|$))*)");

            foreach (Match m in matches)
            {
                var q = m.Groups[2].Value.Trim();
                var a = m.Groups[3].Value.Trim();
                if (!string.IsNullOrEmpty(q) && !string.IsNullOrEmpty(a))
                    entries.Add(new QAEntry { Question = q, Answer = a });
            }

            return entries;
        }

        /// <summary>
        /// Format 4: Separator-based.
        /// What is photosynthesis?
        /// ---
        /// It is the process...
        /// ===
        /// </summary>
        private List<QAEntry> ParseSeparatorFormat(string content)
        {
            var entries = new List<QAEntry>();
            var blocks = Regex.Split(content, @"(?m)^={3,}\s*$", RegexOptions.Multiline);

            foreach (var block in blocks)
            {
                var parts = Regex.Split(block.Trim(), @"(?m)^-{3,}\s*$", RegexOptions.Multiline);
                if (parts.Length >= 2)
                {
                    var q = parts[0].Trim();
                    var a = string.Join("\n", parts.Skip(1)).Trim();
                    if (!string.IsNullOrEmpty(q) && !string.IsNullOrEmpty(a))
                        entries.Add(new QAEntry { Question = q, Answer = a });
                }
            }

            return entries;
        }

        /// <summary>
        /// Fallback: treat entire file as one entry with filename as question.
        /// </summary>
        private List<QAEntry> ParsePlainFormat(string content, string sourceFile)
        {
            var entries = new List<QAEntry>();
            if (!string.IsNullOrWhiteSpace(content))
            {
                entries.Add(new QAEntry
                {
                    Question = Path.GetFileNameWithoutExtension(sourceFile),
                    Answer = content.Trim()
                });
            }
            return entries;
        }

        /// <summary>
        /// Scores how well a stored question matches the searched question.
        /// Returns 0.0 to 1.0.
        /// </summary>
        private float ScoreMatch(string storedQuestion, string searchQuestion)
        {
            if (string.IsNullOrEmpty(storedQuestion) || string.IsNullOrEmpty(searchQuestion))
                return 0f;

            var sq = storedQuestion.ToLowerInvariant().Trim();
            var cq = searchQuestion.ToLowerInvariant().Trim();

            // Exact match
            if (sq == cq) return 1.0f;

            // Contains match (one is substring of the other)
            if (sq.Contains(cq) || cq.Contains(sq)) return 0.85f;

            // Keyword intersection
            var sqWords = Tokenize(sq);
            var cqWords = Tokenize(cq);

            if (sqWords.Count == 0 || cqWords.Count == 0) return 0f;

            var intersection = sqWords.Intersect(cqWords).Count();
            var union = sqWords.Union(cqWords).Count();
            float jaccard = (float)intersection / union;

            // LCS score (longest common subsequence)
            float lcsScore = LcsRatio(sq, cq);

            // Weighted combination: keyword match matters most, then LCS
            float score = jaccard * 0.6f + lcsScore * 0.4f;

            return score;
        }

        private HashSet<string> Tokenize(string text)
        {
            var stopWords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { "the", "a", "an", "is", "are", "was", "were", "what", "who", "when", "where",
              "why", "how", "define", "explain", "describe", "of", "in", "on", "at", "to",
              "for", "and", "or", "not", "this", "that", "with", "from", "by", "as",
              "do", "does", "did", "will", "would", "can", "could", "should", "shall" };

            var words = Regex.Matches(text.ToLowerInvariant(), @"[a-z0-9]+")
                .Cast<Match>()
                .Select(m => m.Value)
                .Where(w => w.Length > 2 && !stopWords.Contains(w));

            return new HashSet<string>(words);
        }

        /// <summary>
        /// Longest Common Subsequence ratio.
        /// </summary>
        private float LcsRatio(string a, string b)
        {
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b)) return 0;

            int maxLen = Math.Min(a.Length, b.Length);
            int[,] dp = new int[maxLen + 1, maxLen + 1];

            // Space-optimized LCS
            var shorter = a.Length <= b.Length ? a : b;
            var longer = a.Length <= b.Length ? b : a;

            int[,] lcs = new int[shorter.Length + 1, longer.Length + 1];

            for (int i = 1; i <= shorter.Length; i++)
            {
                for (int j = 1; j <= longer.Length; j++)
                {
                    if (shorter[i - 1] == longer[j - 1])
                        lcs[i, j] = lcs[i - 1, j - 1] + 1;
                    else
                        lcs[i, j] = Math.Max(lcs[i - 1, j], lcs[i, j - 1]);
                }
            }

            float ratio = (2f * lcs[shorter.Length, longer.Length]) / (a.Length + b.Length);
            return ratio;
        }

        private Encoding DetectEncoding(string filePath)
        {
            try
            {
                var bytes = new byte[4];
                using (var fs = File.OpenRead(filePath))
                {
                    int read = fs.Read(bytes, 0, 4);
                    if (read >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
                        return Encoding.UTF8;
                    if (read >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE)
                        return Encoding.Unicode;
                    if (read >= 2 && bytes[0] == 0xFE && bytes[1] == 0xFF)
                        return Encoding.BigEndianUnicode;
                }
            }
            catch { }
            return Encoding.UTF8;
        }

        private string Truncate(string s, int max)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Length > max ? s.Substring(0, max) + "..." : s;
        }

        private void Log(string msg)
        {
            LogEvent?.Invoke(msg);
        }
    }

    /// <summary>
    /// A single Q/A pair extracted from a file.
    /// </summary>
    public class QAEntry
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }

    /// <summary>
    /// Result of an answer search.
    /// </summary>
    public class SearchResult
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string SourceFile { get; set; }
        public float Score { get; set; }

        public string Preview
        {
            get
            {
                if (string.IsNullOrEmpty(Answer)) return "";
                var len = Math.Min(80, Answer.Length);
                return Answer.Substring(0, len) + (Answer.Length > len ? "..." : "");
            }
        }
    }
}
