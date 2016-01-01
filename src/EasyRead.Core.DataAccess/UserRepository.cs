using System;
using EasyRead.Common;
using EasyRead.Core.Model;
using EasyRead.Core.Repositories;

namespace EasyRead.Core.DataAccess
{
    public interface IDbContextFactory
    {
        EasyReadDbContext GetContext();
    }
    
    public sealed class UserRepository : IUserRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public UserRepository(IDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public User GetById(long userId)
        {
            if(userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));

            return _contextFactory.GetContext().Users.Find(userId);
        }

        public long Create(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if(user.Id != 0) throw new Exception<EntityHasAssignedKey>();
            if (user.Name == null) throw new Exception<RequiredFieldNotInitialized>(new RequiredFieldNotInitialized(nameof(user.Name)));
            if (user.Email == null) throw new Exception<RequiredFieldNotInitialized>(new RequiredFieldNotInitialized(nameof(user.Email)));

            var context = _contextFactory.GetContext();
            var createdUser = context.Users.Add(user);
            context.SaveChanges();

            return createdUser.Id;
        }
    }
}


