using EnglishEasyRead.Core.Model;

namespace EnglishEasyRead.Core.Repositories
{
    public interface ILoginAuthenticationRepository
    {
        LoginAuthentication GetByUserId(long userId);
        LoginAuthentication GetByLogin(string login);
        void Save(LoginAuthentication loginAuthentication);
        void Update(LoginAuthentication loginAuthentication);
    }
}
