using System.Threading.Tasks;

namespace EnglishEasyRead.BusinessServices.RemoteServices.TextService
{
    public interface ITextServiceClient
    {
        Task<TextModel> SaveTextAsync(TextModel text);
        Task<TextModel[]> GetAllTexts();
        Task<TextModel> GetTextAsync(long textId);
        Task<string> GetTextBodyAsync(long textId);
        Task<string[]> GetTextWordsAsync(long textId);
    }
}
