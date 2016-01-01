using System.Collections.Generic;
using EasyRead.Core.Repositories;

namespace EasyRead.Core.Model
{
    public class Text
    {
        public ITextRepository TextRepository { private get; set; }

        public long Id { get; set; }
        public string Name { get; set; }
        
        public void SetWords(IEnumerable<string> words)
        {
            TextRepository.SetWords(this, words);
        }

        public string[] GetAllWords()
        {
            return TextRepository.GetAllWords(this);
        }

        public void SetBody(string body)
        {
            TextRepository.SetBody(this, body);
        }

        public string GetBody()
        {
            return TextRepository.GetBody(this);
        }
    }
}

