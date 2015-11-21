using System;
using System.IO;
using System.Linq;
using EnglishEasyRead.WordNet;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main()
        {
            var wordNetClient = new WordNetClient(@"..\..\..\..\res\wordnet_dict");
            var text = File.ReadAllText(@"input.txt");
            var tokens = EnglishEasyRead.TextAnalysis.Tokenizer.tokenize(text);

            var normalizedForms = tokens.Select(t => new {SourceForm = t, NormalizedForms = wordNetClient.GetBaseForms(t)});
            foreach (var normalizedForm in normalizedForms.Take(1000))
            {
                Console.WriteLine("{0} ->", normalizedForm.SourceForm);
                foreach (var form in normalizedForm.NormalizedForms)
                {
                    var indexRecords = wordNetClient.IndexSeek(form);
                    if (indexRecords.Any())
                    {
                        Console.WriteLine(" {0} ->", form);
                        foreach (var indexRecord in indexRecords)
                        {
                            Console.WriteLine("  {0} {1}", ConvertToString(indexRecord.Pos), string.Join("|", indexRecord.SynsetOffsetList));
                        }
                    }
                    else
                    {
                        Console.WriteLine("none");
                    }
                    
                }
            }
        }

        private static string ConvertToString(WordNetProvider.SyntacticCategory category)
        {
            if (category.IsNoun) return "Noun";
            if (category.IsVerb) return "Verb";
            if (category.IsAdjective) return "Adjective";
            if (category.IsAdverb) return "Adverb";
            throw new ArgumentOutOfRangeException(nameof(category));
        }
    }
}
