using EasyRead.Core.Model;

namespace EasyRead.Core.Repositories
{
    public interface IUserRepository
    {
        User GetById(long userId);
        long Create(User user);
    }
}

