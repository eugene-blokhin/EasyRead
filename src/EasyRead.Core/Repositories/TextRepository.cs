using System;
using System.Collections.Generic;
using System.Linq;
using EasyRead.Core.Model;
using ServiceStack.Common;
using ServiceStack.Common.Extensions;
using ServiceStack.Redis;

namespace EasyRead.Core.Repositories
{
    public sealed class TextRepository : ITextRepository
    {
        private readonly IRedisClient _redisClient;

        private string GetWordsSetId(long textId) => UrnId.Create("TextWords", textId.ToString());
        private string TextBodiesHashes { get; } = "TextBodies";

        public TextRepository(IRedisClient redisClient)
        {
            if (redisClient == null) throw new ArgumentNullException(nameof(redisClient));

            _redisClient = redisClient;
        }
        
        public void SetWords(Text text, IEnumerable<string> words)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (words == null) throw new ArgumentNullException(nameof(words));
            
            EnsureSaved(text);
            var wordsSet = _redisClient.Sets[GetWordsSetId(text.Id)];
            words.ForEach(wordsSet.Add);
        }

        public string[] GetAllWords(Text text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            return text.Id != default(long)
                ? _redisClient.Sets[GetWordsSetId(text.Id)].GetAll().ToArray()
                : new string[0];
        }

        public void SetBody(Text text, string body)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (body == null) throw new ArgumentNullException(nameof(body));

            EnsureSaved(text);
            var bodyHash = _redisClient.Hashes[TextBodiesHashes];
            bodyHash[text.Id.ToString()] = body;
        }

        public string GetBody(Text text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            return text.Id != default(long)
                ? _redisClient.Hashes[TextBodiesHashes][text.Id.ToString()]
                : null;
        }

        public Text SaveText(Text text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            var redisTexts = _redisClient.As<Text>();
            text.Id = redisTexts.GetNextSequence();
            return InjectRepository(redisTexts.Store(text));
        }

        public Text[] GetAllTexts()
        {
            var redisTexts = _redisClient.As<Text>();
            return redisTexts.GetAll().ToArray();
        }

        public Text GetTextById(long id)
        {
            var redisTexts = _redisClient.As<Text>();
            return InjectRepository(redisTexts.GetById(id));
        }

        private void EnsureSaved(Text text)
        {
            if (text.Id == default(long))
            {
                SaveText(text);
            }
        }

        private Text InjectRepository(Text text)
        {
            text.TextRepository = this;
            return text;
        }
    }
}
