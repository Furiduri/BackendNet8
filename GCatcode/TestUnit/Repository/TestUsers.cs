using GCatcode.Services.DB.UserServices;
using GCatcode.Utils;

namespace TestUnit.Repository
{
    [TestClass]
    public sealed class TestUsers : TestRepositoryConfig
    {
        public TestUsers() : base()
        {
        }

        [TestMethod]
        public void TestGetUsers()
        {
            using (var transaction = _connection.BeginTransaction())
            {
                UserService userService = new UserService(_connection, transaction);
                var users = userService.Get();
                Assert.IsNotNull(users);
                Assert.IsTrue(users.Any());
            }
        }

        [TestMethod]
        public void TestGetUserById()
        {
            UserService userService = new UserService(_connection);
            var user = userService.GetById(1);
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void TestGetUserByName()
        {
            UserService userService = new UserService(_connection);
            var user = userService.GetByUserName("Dev");
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void TestGetUserByEmail()
        {
            UserService userService = new UserService(_connection);
            var user = userService.GetByEmail("dev@local.com");
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void TestAddUser()
        {
            using (var transaction = _connection.BeginTransaction())
            {
                UserService userService = new UserService(_connection, transaction);
                var userInsert = new UserInsert
                {
                    UserName = "TestUser_" + Guid.NewGuid().ToString().Substring(0, 8),
                    Password = Argon2Helper.EncryptToBase64("TestPassword2.5!"),
                    Email = "TestEmail@test.com"
                };
                var user = userService.Add(userInsert);
                Assert.IsNotNull(user);
                var userWhitPassword = userService.GetUpdateById(user.UserId);
                Assert.IsTrue(Argon2Helper.VerifyPassword(Argon2Helper.DecryptToBase64(userInsert.Password), userWhitPassword.Password));
            }
        }
    }
}