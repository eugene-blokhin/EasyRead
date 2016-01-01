using System;
using System.Collections.Generic;
using EasyRead.Core.Model;
using EasyRead.Core.Repositories;

namespace EasyRead.Core.Services
{
    public sealed class TextServices : ITextServices
    {
        private readonly ITextRepository _textRepository;

        public TextServices(ITextRepository textRepository)
        {
            if (textRepository == null) throw new ArgumentNullException(nameof(textRepository));

            _textRepository = textRepository;
        }

        public long SaveText(string name, string body, IEnumerable<string> words)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (body == null) throw new ArgumentNullException(nameof(body));
            if (words == null) throw new ArgumentNullException(nameof(words));
            
            var text = new Text {Name = name, TextRepository = _textRepository};
            text.SetBody(body);
            text.SetWords(words);

            _textRepository.SaveText(text);

            return text.Id;
        }

        public string[] GetWords(long textId)
        {
            var text = _textRepository.GetTextById(textId);
            return text.GetAllWords();
        }
    }
}
