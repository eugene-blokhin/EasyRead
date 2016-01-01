using System;
using EasyRead.Core.Repositories;

namespace EasyRead.Core.Services.User
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

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new ServiceException<ModelValidationError>(new ModelValidationError(nameof(user.Email), "Field cannot be null or empty."));
            if (string.IsNullOrWhiteSpace(user.Name))
                throw new ServiceException<ModelValidationError>(new ModelValidationError(nameof(user.Name), "Field cannot be null or empty."));
            if (user.Id != default(long))
                throw new ServiceException<ModelValidationError>(new ModelValidationError(nameof(user.Id), "Field must have value 0 for the creation operation."));

            return _userRepository.Create(user);
        }

        public Model.User GetUser(long userId)
        {
            if (userId <= 0) throw new ArgumentOutOfRangeException(nameof(userId));

            return _userRepository.GetById(userId);
        }
    }

    public class ModelValidationError : ServiceExceptionData
    {
        public ModelValidationError(string field, string error)
        {
            Error = error;
            Field = field;
        }

        public string Error { get; }
        public string Field { get; }
    }
}
