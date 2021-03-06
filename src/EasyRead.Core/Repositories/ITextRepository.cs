﻿using System.Collections.Generic;
using EasyRead.Core.Model;

namespace EasyRead.Core.Repositories
{
    public interface ITextRepository
    {
        void SetWords(Text text, IEnumerable<string> words);
        string[] GetAllWords(Text text);
        void SetBody(Text text, string body);
        string GetBody(Text text);
        Text SaveText(Text text);
        Text[] GetAllTexts();
        Text GetTextById(long id);
    }
}

