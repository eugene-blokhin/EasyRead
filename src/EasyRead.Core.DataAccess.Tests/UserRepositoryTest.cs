using System;
using System.Data.Entity;
using EasyRead.Common;
using EasyRead.Core.Model;
using NUnit.Framework;

namespace EasyRead.Core.DataAccess.Tests
{
    [TestFixture]
    public class UserRepositoryTest
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

        [TestCase(-1, "name", "email", ExpectedException = typeof (Exception<EntityHasAssignedKey>))]
        [TestCase(1, "name", "email", ExpectedException = typeof (Exception<EntityHasAssignedKey>))]
        [TestCase(0, null, "email", ExpectedException = typeof (Exception<RequiredFieldNotInitialized>))]
        [TestCase(0, "name", null, ExpectedException = typeof (Exception<RequiredFieldNotInitialized>))]
        [TestCase(0, "name", "email")]
        public void CreateUser_General(long id, string name, string email)
        {
            var user = new User {Id = id, Name = name, Email = email};

            var repository = new UserRepository(_contextFactory);

            var userId = repository.Create(user);
            Assert.Greater(userId, 0);
        }

        [Test, ExpectedException(typeof (ArgumentNullException))]
        public void CreateUser_NullArgumentPassed()
        {
            var repository = new UserRepository(_contextFactory);
            repository.Create(null);
        }

        [Test]
        public void GetById_UserExists()
        {
            var repository = new UserRepository(_contextFactory);
            
            var savedUser = new User { Name = "name", Email = "email" };
            var userId = repository.Create(savedUser);
            var retrievedUser = repository.GetById(userId);

            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual(userId, retrievedUser.Id);
            Assert.AreEqual(savedUser.Name, retrievedUser.Name);
            Assert.AreEqual(savedUser.Email, retrievedUser.Email);
        }

        [Test]
        public void GetById_UserDoesNotExist()
        {
            var repository = new UserRepository(_contextFactory);

            var retrievedUser = repository.GetById(123);
            Assert.IsNull(retrievedUser);
        }

        [TestCase(-1, ExpectedException = typeof(ArgumentOutOfRangeException))]
        [TestCase(0, ExpectedException = typeof(ArgumentOutOfRangeException))]
        public void GetById_IdIsLessOrEqualToZero(long id)
        {
            var repository = new UserRepository(_contextFactory);
            repository.GetById(id);
        }
    }
}


