using System.Collections.Generic;

namespace EnglishEasyRead.Core.Services
{
    public interface ITextServices
    {
        long SaveText(string name, string body, IEnumerable<string> words);
        string[] GetWords(long textId);
    }
}
