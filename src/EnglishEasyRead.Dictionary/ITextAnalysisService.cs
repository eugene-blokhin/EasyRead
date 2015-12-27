namespace EnglishEasyRead.Dictionary
{
    public interface ITextAnalysisService
    {
        string[] ExtractLemmasFromText(string text);
    }
}
