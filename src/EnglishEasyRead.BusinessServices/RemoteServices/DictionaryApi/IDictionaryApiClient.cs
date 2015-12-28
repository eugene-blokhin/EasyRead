using System.Threading.Tasks;

namespace EnglishEasyRead.BusinessServices.RemoteServices.DictionaryApi
{
    public interface IDictionaryApiClient
    {
        Task<string[]> ExtractLemmasAsync(string text);
    }
}
