using System;
using System.Linq;
using EnglishEasyRead.WordNet;

namespace EnglishEasyRead.Dictionary
{
    public class TextAnalysisService : ITextAnalysisService
    {
        private readonly IWordNetFacade _wordNetFacade;
        private readonly IMorpher _morpher;

        public TextAnalysisService(IWordNetFacade wordNetFacade, IMorpher morpher)
        {
            if (wordNetFacade == null) throw new ArgumentNullException(nameof(wordNetFacade));
            if (morpher == null) throw new ArgumentNullException(nameof(morpher));

            _wordNetFacade = wordNetFacade;
            _morpher = morpher;
        }
        
        public string[] ExtractLemmasFromText(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            return Tokenizer.Tokenize(text)
                .SelectMany(token => _morpher.GetPossibleLemmas(token))
                .Distinct()
                .Where(lemma => _wordNetFacade.IndexSeek(lemma).Length > 0)
                .ToArray();
        }
    }
}