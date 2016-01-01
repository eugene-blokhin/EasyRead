using System.Threading.Tasks;

namespace EasyRead.BusinessServices
{
    public interface ITextBusinessService
    {
        Task<long> SubmitTextAsync(TextModel text);
        Task<TextInfo[]> GetAvailableAsync();
    }
}
