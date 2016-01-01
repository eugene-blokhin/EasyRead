using System.Threading.Tasks;

namespace EasyRead.BusinessServices.RemoteServices.DictionaryApi
{
    public interface IDictionaryApiClient
    {
        Task<string[]> ExtractLemmasAsync(string text);
    }
}

