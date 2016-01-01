using EnglishEasyRead.Core.Model;

namespace EnglishEasyRead.Core.Repositories
{
    public interface IUserRepository
    {
        User GetById(long userId);
        long Create(User user);
    }
}
