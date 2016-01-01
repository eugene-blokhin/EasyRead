using System;
using System.Data.Entity;
using System.Linq;
using EasyRead.Common;
using EasyRead.Core.Model;
using NUnit.Framework;

namespace EasyRead.Core.DataAccess.Tests
{
    [TestFixture]
    public class LoginAuthenticationRepositoryTests
    {
        private IDbContextFactory _contextFactory;

        [SetUp]
        public void BeforeTest()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<EasyReadDbContext>());
            _contextFactory = new TestContextFactory(new EasyReadDbContext("DefaultContext"));
            _contextFactory.GetContext().Database.Initialize(true);
        }

        [TearDown]
        public void AfterTests()
        {
            _contextFactory.GetContext().Dispose();
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void GetByLogin_NullIsPassed()
        {
            var repository = new LoginAuthenticationRepository(_contextFactory);
            repository.GetByLogin(null);
        }

        [Test]
        public void GetByLogin_LoginDoesNotExist()
        {
            var repository = new LoginAuthenticationRepository(_contextFactory);
            var loginAuthentication = repository.GetByLogin("login");
            Assert.IsNull(loginAuthentication);
        }

        [Test]
        public void GetByLogin_LoginExists()
        {
            var userRepository = new UserRepository(_contextFactory);
            var authenticationRepository = new LoginAuthenticationRepository(_contextFactory);

            var user = new User { Name = "name", Email = "email" };
            var userId = userRepository.Create(user);

            var initalLoginAuth = new LoginAuthentication
            {
                UserId = userId,
                LoginName = "login",
                PasswordHash = Enumerable.Range(0, 32).Select(i => (byte)i).ToArray(),
                Salt = Enumerable.Range(0, 16).Select(i => (byte)i).ToArray()
            };

            authenticationRepository.Save(initalLoginAuth);

            var restoredLoginAuthentication = authenticationRepository.GetByLogin(initalLoginAuth.LoginName);

            Assert.NotNull(restoredLoginAuthentication);
            Assert.AreEqual(initalLoginAuth.UserId, restoredLoginAuthentication.UserId);
            Assert.AreEqual(initalLoginAuth.LoginName, restoredLoginAuthentication.LoginName);
            CollectionAssert.AreEqual(initalLoginAuth.Salt, restoredLoginAuthentication.Salt);
            CollectionAssert.AreEqual(initalLoginAuth.PasswordHash, restoredLoginAuthentication.PasswordHash);
        }

        [TestCase(-1, ExpectedException = typeof(ArgumentOutOfRangeException))]
        [TestCase(0, ExpectedException = typeof(ArgumentOutOfRangeException))]
        public void GetByUserId_InvalidUserIdPassed(long userId)
        {
            var repository = new LoginAuthenticationRepository(_contextFactory);
            repository.GetByUserId(userId);
        }

        [Test]
        public void GetByUserId_LoginDoesNotExist()
        {
            var repository = new LoginAuthenticationRepository(_contextFactory);
            var loginAuthentication = repository.GetByUserId(long.MaxValue);
            Assert.IsNull(loginAuthentication);
        }

        [Test]
        public void GetByUserId_LoginExists()
        {
            var userRepository = new UserRepository(_contextFactory);
            var authenticationRepository = new LoginAuthenticationRepository(_contextFactory);

            var user = new User { Name = "name", Email = "email" };
            var userId = userRepository.Create(user);

            var initalLoginAuth = new LoginAuthentication
            {
                UserId = userId,
                LoginName = "login",
                PasswordHash = Enumerable.Range(0, 32).Select(i => (byte)i).ToArray(),
                Salt = Enumerable.Range(0, 16).Select(i => (byte)i).ToArray()
            };

            authenticationRepository.Save(initalLoginAuth);

            var restoredLoginAuthentication = authenticationRepository.GetByUserId(initalLoginAuth.UserId);

            Assert.NotNull(restoredLoginAuthentication);
            Assert.AreEqual(initalLoginAuth.UserId, restoredLoginAuthentication.UserId);
            Assert.AreEqual(initalLoginAuth.LoginName, restoredLoginAuthentication.LoginName);
            CollectionAssert.AreEqual(initalLoginAuth.Salt, restoredLoginAuthentication.Salt);
            CollectionAssert.AreEqual(initalLoginAuth.PasswordHash, restoredLoginAuthentication.PasswordHash);
        }

        [Test, ExpectedException(typeof(ArgumentNullException))]
        public void Save_LoginAuthenticationIsNull()
        {
            new LoginAuthenticationRepository(_contextFactory).Save(null);
        }

        [Test]
        public void Save_LoginNameIsNull()
        {
            var loginAuthentication = new LoginAuthentication
            {
                LoginName = null,
                UserId = 1,
                PasswordHash = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray(),
                Salt = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray()
            };

            var exception = Assert.Throws<Exception<RequiredFieldNotInitialized>>(
                () => new LoginAuthenticationRepository(_contextFactory).Save(loginAuthentication)
            );

            Assert.AreEqual(nameof(loginAuthentication.LoginName), exception.ExceptionData.FieldName);
        }

        [Test]
        public void Save_SaltIsNull()
        {
            var loginAuthentication = new LoginAuthentication
            {
                LoginName = "login",
                UserId = 1,
                PasswordHash = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray(),
                Salt = null
            };

            var exception = Assert.Throws<Exception<RequiredFieldNotInitialized>>(
                () => new LoginAuthenticationRepository(_contextFactory).Save(loginAuthentication)
            );

            Assert.AreEqual(nameof(loginAuthentication.Salt), exception.ExceptionData.FieldName);
        }

        [Test]
        public void Save_PasswordHashIsNull()
        {
            var loginAuthentication = new LoginAuthentication
            {
                LoginName = "login",
                UserId = 1,
                PasswordHash = null,
                Salt = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray()
            };

            var exception = Assert.Throws<Exception<RequiredFieldNotInitialized>>(
                () => new LoginAuthenticationRepository(_contextFactory).Save(loginAuthentication)
            );

            Assert.AreEqual(nameof(loginAuthentication.PasswordHash), exception.ExceptionData.FieldName);
        }

        [Test]
        public void Save_UserIdIsZero()
        {
            var loginAuthentication = new LoginAuthentication
            {
                LoginName = "login",
                UserId = 0,
                PasswordHash = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray(),
                Salt = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray()
            };

            var exception = Assert.Throws<Exception<RequiredFieldNotInitialized>>(
                () => new LoginAuthenticationRepository(_contextFactory).Save(loginAuthentication)
            );

            Assert.AreEqual(nameof(loginAuthentication.UserId), exception.ExceptionData.FieldName);
        }

        [Test]
        public void Save_UserIdIsLessThanZero()
        {
            var loginAuthentication = new LoginAuthentication
            {
                LoginName = "login",
                UserId = -1,
                PasswordHash = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray(),
                Salt = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray()
            };

            var exception = Assert.Throws<Exception<ForeignKeyOutOfRange>>(
                () => new LoginAuthenticationRepository(_contextFactory).Save(loginAuthentication)
            );

            Assert.AreEqual(nameof(loginAuthentication.UserId), exception.ExceptionData.FieldName);
        }

        [Test]
        public void Save_IdIsNotZero()
        {
            var loginAuthentication = new LoginAuthentication
            {
                Id = long.MaxValue,
                LoginName = "login",
                UserId = 0,
                PasswordHash = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray(),
                Salt = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray()
            };

            Assert.Throws<Exception<EntityHasAssignedKey>>(
                () => new LoginAuthenticationRepository(_contextFactory).Save(loginAuthentication)
            );
        }

        [Test]
        public void Save_IdIsZero()
        {
            var userRepository = new UserRepository(_contextFactory);
            var authenticationRepository = new LoginAuthenticationRepository(_contextFactory);

            var user = new User { Name = "name", Email = "email" };
            var userId = userRepository.Create(user);
            
            var loginAuthentication = new LoginAuthentication
            {
                Id = 0,
                LoginName = "login",
                UserId = userId,
                PasswordHash = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray(),
                Salt = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray()
            };

            authenticationRepository.Save(loginAuthentication);

            var savedLoginAuthentication = _contextFactory.GetContext().LoginsAuthentication.First();

            Assert.NotNull(savedLoginAuthentication);
            Assert.AreEqual(loginAuthentication.UserId, savedLoginAuthentication.UserId);
            Assert.AreEqual(loginAuthentication.LoginName, savedLoginAuthentication.LoginName);
            CollectionAssert.AreEqual(loginAuthentication.Salt, savedLoginAuthentication.Salt);
            CollectionAssert.AreEqual(loginAuthentication.PasswordHash, savedLoginAuthentication.PasswordHash);
        }

        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void Update_LoginAuthenticationIsNull()
        {
            new LoginAuthenticationRepository(_contextFactory).Update(null);
        }

        [Test]
        public void Update_LoginNameIsNull()
        {
            var loginAuthentication = new LoginAuthentication
            {
                Id = long.MaxValue,
                LoginName = null,
                UserId = 1,
                PasswordHash = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray(),
                Salt = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray()
            };

            var exception = Assert.Throws<Exception<RequiredFieldNotInitialized>>(
                () => new LoginAuthenticationRepository(_contextFactory).Update(loginAuthentication)
            );

            Assert.AreEqual(nameof(loginAuthentication.LoginName), exception.ExceptionData.FieldName);
        }

        [Test]
        public void Update_SaltIsNull()
        {
            var loginAuthentication = new LoginAuthentication
            {
                Id = long.MaxValue,
                LoginName = "login",
                UserId = 1,
                PasswordHash = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray(),
                Salt = null
            };

            var exception = Assert.Throws<Exception<RequiredFieldNotInitialized>>(
                () => new LoginAuthenticationRepository(_contextFactory).Update(loginAuthentication)
            );

            Assert.AreEqual(nameof(loginAuthentication.Salt), exception.ExceptionData.FieldName);
        }

        [Test]
        public void Update_PasswordHashIsNull()
        {
            var loginAuthentication = new LoginAuthentication
            {
                Id = long.MaxValue,
                LoginName = "login",
                UserId = 1,
                PasswordHash = null,
                Salt = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray()
            };

            var exception = Assert.Throws<Exception<RequiredFieldNotInitialized>>(
                () => new LoginAuthenticationRepository(_contextFactory).Update(loginAuthentication)
            );

            Assert.AreEqual(nameof(loginAuthentication.PasswordHash), exception.ExceptionData.FieldName);
        }

        [Test]
        public void Update_UserIdIsZero()
        {
            var loginAuthentication = new LoginAuthentication
            {
                Id = long.MaxValue,
                LoginName = "login",
                UserId = 0,
                PasswordHash = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray(),
                Salt = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray()
            };

            var exception = Assert.Throws<Exception<RequiredFieldNotInitialized>>(
                () => new LoginAuthenticationRepository(_contextFactory).Update(loginAuthentication)
            );

            Assert.AreEqual(nameof(loginAuthentication.UserId), exception.ExceptionData.FieldName);
        }

        [Test]
        public void Update_UserIdIsLessThanZero()
        {
            var loginAuthentication = new LoginAuthentication
            {
                Id = long.MaxValue,
                LoginName = "login",
                UserId = -1,
                PasswordHash = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray(),
                Salt = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray()
            };

            var exception = Assert.Throws<Exception<ForeignKeyOutOfRange>>(
                () => new LoginAuthenticationRepository(_contextFactory).Update(loginAuthentication)
            );

            Assert.AreEqual(nameof(loginAuthentication.UserId), exception.ExceptionData.FieldName);
        }

        [Test]
        public void Update_IdIsNotZero()
        {
            var userRepository = new UserRepository(_contextFactory);
            var authenticationRepository = new LoginAuthenticationRepository(_contextFactory);

            //Create user
            var user = new User { Name = "name", Email = "email" };
            var userId1 = userRepository.Create(user);

            var user2 = new User { Name = "name2", Email = "email2" };
            var userId2 = userRepository.Create(user2);

            //Save first version login authentication record
            var initialLoginAuthentication = new LoginAuthentication
            {
                Id = 0,
                LoginName = "login",
                UserId = userId1,
                PasswordHash = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray(),
                Salt = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray()
            };
            
            authenticationRepository.Save(initialLoginAuthentication);
            
            //Update login authentication record
            var updatedLoginAuthentication = authenticationRepository.GetByLogin(initialLoginAuthentication.LoginName);
            updatedLoginAuthentication.LoginName = "loginUpd";
            updatedLoginAuthentication.PasswordHash = Enumerable.Range(0, 10).Select(i => (byte) (i + 1)).ToArray();
            updatedLoginAuthentication.Salt = Enumerable.Range(0, 10).Select(i => (byte) (i + 2)).ToArray();
            updatedLoginAuthentication.UserId = userId2;
            
            authenticationRepository.Update(updatedLoginAuthentication);
            
            //Check what's in the DB
            var storedLoginAuthentication = _contextFactory.GetContext().LoginsAuthentication.First();

            Assert.NotNull(storedLoginAuthentication);
            Assert.AreEqual(updatedLoginAuthentication.UserId, storedLoginAuthentication.UserId);
            Assert.AreEqual(updatedLoginAuthentication.LoginName, storedLoginAuthentication.LoginName);
            CollectionAssert.AreEqual(updatedLoginAuthentication.Salt, storedLoginAuthentication.Salt);
            CollectionAssert.AreEqual(updatedLoginAuthentication.PasswordHash, storedLoginAuthentication.PasswordHash);
        }

        [Test]
        public void Update_IdIsZero()
        {
            var loginAuthentication = new LoginAuthentication
            {
                Id = 0,
                LoginName = "login",
                UserId = 0,
                PasswordHash = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray(),
                Salt = Enumerable.Range(0, 10).Select(i => (byte)i).ToArray()
            };

            Assert.Throws<Exception<EntityHasNotAssignedKey>>(
                () => new LoginAuthenticationRepository(_contextFactory).Update(loginAuthentication)
            );
        }
    }
}

