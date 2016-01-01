namespace EasyRead.Dictionary
{
    public interface ITextAnalysisService
    {
        string[] ExtractLemmasFromText(string text);
    }
}

