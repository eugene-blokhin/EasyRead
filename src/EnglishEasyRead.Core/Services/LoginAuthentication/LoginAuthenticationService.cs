using System;
using System.Linq;
using System.Text.RegularExpressions;
using EasyReader.Common;
using EnglishEasyRead.Core.Repositories;

namespace EnglishEasyRead.Core.Services.LoginAuthentication
{
    public sealed class LoginAuthenticationService
    {
        public const int SALT_LENGTH = 32;

        private readonly ILoginAuthenticationRepository _repository;
        private readonly IPasswordValidator _passwordValidator;
        private readonly ISaltedHashGenerator _saltedHashGenerator;

        public LoginAuthenticationService(ILoginAuthenticationRepository repository, IPasswordValidator passwordValidator, ISaltedHashGenerator saltedHashGenerator)
        {
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (passwordValidator == null) throw new ArgumentNullException(nameof(passwordValidator));
            if (saltedHashGenerator == null) throw new ArgumentNullException(nameof(saltedHashGenerator));

            _repository = repository;
            _passwordValidator = passwordValidator;
            _saltedHashGenerator = saltedHashGenerator;
        }

        public void SetPassword(long userId, string login, string password)
        {
            if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));

            if (login == null) throw new ArgumentNullException(nameof(login));
            if (!Regex.IsMatch(login, @"^[a-z0-9\-]{4,12}$")) throw new Exception<LoginDoesNotMeetRequrements>();

            if (password == null) throw new ArgumentNullException(nameof(password));
            _passwordValidator.Validate(password);

            byte[] salt;
            var saltedHash = _saltedHashGenerator.GenerateSaltedHash(password, out salt, SALT_LENGTH);

            var loginAuthentication = _repository.GetByUserId(userId) ?? new Model.LoginAuthentication {UserId = userId};
            loginAuthentication.LoginName = login;
            loginAuthentication.PasswordHash = saltedHash;
            loginAuthentication.Salt = salt;

            if (loginAuthentication.Id == 0)
            {
                _repository.Save(loginAuthentication);
            }
            else
            {
                _repository.Update(loginAuthentication);
            }
        }

        public long? GetUserId(string login, string password)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));
            if (password == null) throw new ArgumentNullException(nameof(password));

            var loginAuthentication = _repository.GetByLogin(login);

            if (loginAuthentication != null)
            {
                var calculatedHash = _saltedHashGenerator.GenerateSaltedHash(password, loginAuthentication.Salt);
                var passwordIsCorrect = calculatedHash.SequenceEqual(loginAuthentication.PasswordHash);

                return passwordIsCorrect
                    ? loginAuthentication.UserId
                    : (long?) null;
            }

            return null;
        }
    }
}
