using System.Linq;
using System.Text.RegularExpressions;

namespace EnglishEasyRead.Dictionary
{
    public static class Tokenizer
    {
        public static string[] Tokenize(string text)
        {
            return Regex.Matches(text, @"([a-z]+)", RegexOptions.IgnoreCase)
                .Cast<Match>()
                .Where(m => m.Success)
                .Select(m => m.Groups[1].Value)
                .Select(t => t.ToLower())
                .ToArray();
        }
    }
}