using System.Threading.Tasks;

namespace EnglishEasyRead.BusinessServices
{
    public interface ITextBusinessService
    {
        Task<long> SubmitTextAsync(TextModel text);
        Task<TextInfo[]> GetAvailableAsync();
    }
}