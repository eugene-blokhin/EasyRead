using System;
using System.Linq;
using EasyRead.Common;
using EasyRead.Core.Model;
using EasyRead.Core.Repositories;

namespace EasyRead.Core.DataAccess
{
    public sealed class LoginAuthenticationRepository : ILoginAuthenticationRepository
    {
        private readonly IDbContextFactory _contextFactory;

        public LoginAuthenticationRepository(IDbContextFactory contextFactory)
        {
            if (contextFactory == null) throw new ArgumentNullException(nameof(contextFactory));
            _contextFactory = contextFactory;
        }

        public LoginAuthentication GetByUserId(long userId)
        {
            if (userId <= 0) throw new ArgumentOutOfRangeException();
            return _contextFactory.GetContext().LoginsAuthentication.Find(userId);
        }

        public LoginAuthentication GetByLogin(string login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));

            return _contextFactory.GetContext().LoginsAuthentication
                .FirstOrDefault(authentication => authentication.LoginName == login);
        }

        public void Save(LoginAuthentication loginAuthentication)
        {
            if(loginAuthentication == null) throw new ArgumentNullException(nameof(loginAuthentication));
            if(loginAuthentication.Id != default(long)) throw new Exception<EntityHasAssignedKey>();
            CheckFields(loginAuthentication);

            var context = _contextFactory.GetContext();
            context.LoginsAuthentication.Add(loginAuthentication);
            context.SaveChanges();
        }

        public void Update(LoginAuthentication loginAuthentication)
        {
            if (loginAuthentication == null) throw new ArgumentNullException(nameof(loginAuthentication));
            if (loginAuthentication.Id == default(long)) throw new Exception<EntityHasNotAssignedKey>();
            CheckFields(loginAuthentication);

            var context = _contextFactory.GetContext();
            context.LoginsAuthentication.Find(loginAuthentication.Id);
            context.SaveChanges();
        }

        private static void CheckFields(LoginAuthentication loginAuthentication)
        {
            if (loginAuthentication.LoginName == null)
                throw new Exception<RequiredFieldNotInitialized>(new RequiredFieldNotInitialized(nameof(loginAuthentication.LoginName)));
            if (loginAuthentication.Salt == null)
                throw new Exception<RequiredFieldNotInitialized>(new RequiredFieldNotInitialized(nameof(loginAuthentication.Salt)));
            if (loginAuthentication.PasswordHash == null)
                throw new Exception<RequiredFieldNotInitialized>(new RequiredFieldNotInitialized(nameof(loginAuthentication.PasswordHash)));
            if (loginAuthentication.UserId == default(long))
                throw new Exception<RequiredFieldNotInitialized>(new RequiredFieldNotInitialized(nameof(loginAuthentication.UserId)));
            if (loginAuthentication.UserId < 0)
                throw new Exception<ForeignKeyOutOfRange>(new ForeignKeyOutOfRange(nameof(loginAuthentication.UserId)));
        }
    }
}

