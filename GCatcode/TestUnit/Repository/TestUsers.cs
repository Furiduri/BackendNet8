using GCatcode.Repository.DB.UserServices;

namespace TestUnit.Repository
{
    [TestClass]
    public class TestUsers : TestRepositoryConfig
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
            }
        }

        [TestMethod]
        public void TestGetUserById()
        {
            UserService userService = new UserService(_connection);
            var user = userService.Get(1);
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
        public void TestAddUser()
        {
            using (var transaction = _connection.BeginTransaction())
            {
                UserService userService = new UserService(_connection, transaction);
                var user = userService.Add(new UserInsert
                {
                    UserName = "TestUser",
                    Password = "TestPassword2.5",
                    Email = "TestEmail@test.com"
                });
                Assert.IsNotNull(user);
            }
        }
    }
}