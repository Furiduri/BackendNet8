using GCatcode.Services.DB.RolServices;
using GCatcode.Services.DB.RolServices.Models;

namespace TestUnit.Repository
{
    [TestClass]
    public sealed class TestRoles : TestRepositoryConfig
    {
        private RolesService _rolesService;

        [TestInitialize]
        public void Setup()
        {
            _rolesService = new RolesService(_connection);
        }

        [TestMethod]
        public void GetById_WithValidId_ReturnsRole()
        {
            // Arrange
            int validRoleId = 1;

            // Act
            var result = _rolesService.GetById(validRoleId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(validRoleId, result.RolId);
        }

        [TestMethod]
        public void GetById_WithInvalidId_ReturnsNull()
        {
            // Arrange
            int invalidRoleId = -1;

            // Act
            var result = _rolesService.GetById(invalidRoleId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Get_WithDefaultParameters_ReturnsRoles()
        {
            // Arrange
            int page = 1;
            int maxItems = 100;

            // Act
            var result = _rolesService.Get(page, null, maxItems);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void Get_WithPagination_ReturnsCorrectPage()
        {
            // Arrange
            int page = 1;
            int maxItems = 5;

            // Act
            var result = _rolesService.Get(page, null, maxItems);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count() <= maxItems);
        }

        [TestMethod]
        public void Add_WithValidData_ReturnsNewRole()
        {
            // Arrange
            var newRole = new RolInsert
            {
                Name = $"TestRole_{Guid.NewGuid()}",
                Description = "Test role description"
            };

            // Act
            var result = _rolesService.Add(newRole);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newRole.Name, result.Name);
            Assert.AreEqual(newRole.Description, result.Description);
            Assert.IsTrue(result.RolId > 0);

            // Cleanup
            _rolesService.Delete(result.RolId);
        }

        [TestMethod]
        public void Update_WithValidData_ReturnsUpdatedRole()
        {
            // Arrange
            var newRole = new RolInsert
            {
                Name = $"TestRole_{Guid.NewGuid()}",
                Description = "Original description"
            };
            var createdRole = _rolesService.Add(newRole);

            var updateData = new RolUpdate
            {
                RolId = createdRole.RolId,
                Name = $"UpdatedRole_{Guid.NewGuid()}",
                Description = "Updated description",
                Available = true
            };

            // Act
            var result = _rolesService.Update(updateData);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(updateData.RolId, result.RolId);
            Assert.AreEqual(updateData.Name, result.Name);
            Assert.AreEqual(updateData.Description, result.Description);

            // Cleanup
            _rolesService.Delete(result.RolId);
        }

        [TestMethod]
        public void Delete_WithValidId_MarksRoleAsUnavailable()
        {
            // Arrange
            var newRole = new RolInsert
            {
                Name = $"TestRole_{Guid.NewGuid()}",
                Description = "Role to be deleted"
            };
            var createdRole = _rolesService.Add(newRole);

            // Act
            var result = _rolesService.Delete(createdRole.RolId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(createdRole.RolId, result.RolId);
            Assert.IsFalse(result.Available);
        }

        [TestMethod]
        public void GetRolesByUserId_WithValidUserId_ReturnsUserRoles()
        {
            // Arrange
            int validUserId = 1;

            // Act
            var result = _rolesService.GetRolesByUserId(validUserId);

            // Assert
            Assert.IsNotNull(result);
            // Note: Assert will depend on test data, adjust as needed
        }

        [TestMethod]
        public void GetRolesByUserId_WithInvalidUserId_ReturnsEmpty()
        {
            // Arrange
            int invalidUserId = -1;

            // Act
            var result = _rolesService.GetRolesByUserId(invalidUserId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Add_WithNullData_ThrowsException()
        {
            // Arrange
            RolInsert? nullData = null;

            // Act
            _rolesService.Add(nullData);

            // Assert is handled by ExpectedException
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Update_WithNullData_ThrowsException()
        {
            // Arrange
            RolUpdate? nullData = null;

            // Act
            _rolesService.Update(nullData);

            // Assert is handled by ExpectedException
        }
    }
}