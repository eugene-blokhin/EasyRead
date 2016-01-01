using System;
using EnglishEasyRead.Core.Repositories;

namespace EnglishEasyRead.Core.Services.User
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            if (userRepository == null) throw new ArgumentNullException(nameof(userRepository));

            _userRepository = userRepository;
        }

        public long CreateUser(Model.User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(user.Email)) throw new ArgumentException("Email field cannot be null or empty.");
            if (string.IsNullOrWhiteSpace(user.Name)) throw new ArgumentException("Name field cannot be null or empty.");
            if (user.Id != default(long)) throw new ArgumentException("The instance already has an non-zero identifier.");

            return _userRepository.Create(user);
        }

        public Model.User GetUser(long userId)
        {
            if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));

            return _userRepository.GetById(userId);
        }
    }
}