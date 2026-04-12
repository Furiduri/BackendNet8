using GCatcode.Services.DB.RolServices.Models;
using GCatcode.Services.DB.UserRolServices;
using GCatcode.Services.DB.UserRolServices.Models;

namespace TestUnit.Repository
{
    [TestClass]
    public sealed class TestUserRol : TestRepositoryConfig
    {
        private UserRolService _userRolService;

        [TestInitialize]
        public void Setup()
        {
            _userRolService = new UserRolService(_connection);
        }

        #region Insert Tests

        [TestMethod]
        public void Insert_WithValidData_ReturnsUserRol()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            int testRolId = 1;  // Adjust based on your test data

            var userRol = new UserRolDTO
            {
                UserId = testUserId,
                RolId = (RolesType)testRolId
            };

            // Act
            var result = _userRolService.Insert(userRol);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testUserId, result.UserId);
            Assert.AreEqual(testRolId, (int)result.RolId);
            Assert.IsTrue(result.Available);

            // Cleanup
            _userRolService.Delete(testUserId, testRolId);
        }

        [TestMethod]
        public void Insert_WithExistingRelation_UpdatesAvailability()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            int testRolId = 1;  // Adjust based on your test data

            var userRol = new UserRolDTO
            {
                UserId = testUserId,
                RolId = (RolesType)testRolId
            };

            // First insert
            _userRolService.Insert(userRol);
            // Soft delete
            _userRolService.Delete(testUserId, testRolId);

            // Act - Re-insert should reactivate
            var result = _userRolService.Insert(userRol);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testUserId, result.UserId);
            Assert.AreEqual(testRolId, (int)result.RolId);
            Assert.IsTrue(result.Available);

            // Cleanup
            _userRolService.Delete(testUserId, testRolId);
        }

        [TestMethod]
        public void Insert_WithDuplicateActiveRelation_UpdatesTimestamp()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            int testRolId = 1;  // Adjust based on your test data

            var userRol = new UserRolDTO
            {
                UserId = testUserId,
                RolId = (RolesType)testRolId
            };

            // First insert
            var firstResult = _userRolService.Insert(userRol);

            // Wait a moment to ensure different timestamp
            System.Threading.Thread.Sleep(100);

            // Act - Insert again
            var secondResult = _userRolService.Insert(userRol);

            // Assert
            Assert.IsNotNull(secondResult);
            Assert.AreEqual(testUserId, secondResult.UserId);
            Assert.AreEqual(testRolId, (int)secondResult.RolId);
            Assert.IsTrue(secondResult.Available);

            // Cleanup
            _userRolService.Delete(testUserId, testRolId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Insert_WithNullData_ThrowsException()
        {
            // Arrange
            UserRolDTO nullData = null;

            // Act
            _userRolService.Insert(nullData);

            // Assert is handled by ExpectedException
        }

        #endregion Insert Tests

        #region Delete Tests

        [TestMethod]
        public void Delete_WithValidIds_SoftDeletesRelation()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            int testRolId = 1;  // Adjust based on your test data

            var userRol = new UserRolDTO
            {
                UserId = testUserId,
                RolId = (RolesType)testRolId
            };

            // First insert
            _userRolService.Insert(userRol);

            // Act
            var result = _userRolService.Delete(testUserId, testRolId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testUserId, result.UserId);
            Assert.AreEqual(testRolId, (int)result.RolId);
            Assert.IsFalse(result.Available);
        }

        [TestMethod]
        public void Delete_WithNonExistentRelation_ReturnsNull()
        {
            // Arrange
            int nonExistentUserId = -999;
            int nonExistentRolId = -999;

            // Act
            var result = _userRolService.Delete(nonExistentUserId, nonExistentRolId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Delete_AfterPreviousDelete_ReturnsUpdatedRelation()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            int testRolId = 1;  // Adjust based on your test data

            var userRol = new UserRolDTO
            {
                UserId = testUserId,
                RolId = (RolesType)testRolId
            };

            // Insert and delete
            _userRolService.Insert(userRol);
            _userRolService.Delete(testUserId, testRolId);

            // Act - Delete again
            var result = _userRolService.Delete(testUserId, testRolId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Available);
        }

        #endregion Delete Tests

        #region GetRolsByUserId Tests

        [TestMethod]
        public void GetRolsByUserId_WithValidUserId_ReturnsRoles()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            int testRolId = 1;  // Adjust based on your test data

            var userRol = new UserRolDTO
            {
                UserId = testUserId,
                RolId = (RolesType)testRolId
            };

            // Ensure at least one active relation exists
            _userRolService.Insert(userRol);

            // Act
            var result = _userRolService.GetRolsByUserId(testUserId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
            Assert.IsTrue(result.Any(r => r.RolId == testRolId));

            // Cleanup
            _userRolService.Delete(testUserId, testRolId);
        }

        [TestMethod]
        public void GetRolsByUserId_WithInvalidUserId_ReturnsEmpty()
        {
            // Arrange
            int invalidUserId = -999;

            // Act
            var result = _userRolService.GetRolsByUserId(invalidUserId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void GetRolsByUserId_WithDeletedRelations_ExcludesUnavailableRoles()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            int testRolId = 2;  // Adjust based on your test data

            var userRol = new UserRolDTO
            {
                UserId = testUserId,
                RolId = (RolesType)testRolId
            };

            // Insert and delete
            _userRolService.Insert(userRol);
            _userRolService.Delete(testUserId, testRolId);

            // Act
            var result = _userRolService.GetRolsByUserId(testUserId);

            // Assert
            Assert.IsNotNull(result);
            // Should not include the deleted role
            Assert.IsFalse(result.Any(r => r.RolId == testRolId));
        }

        [TestMethod]
        public void GetRolsByUserId_WithMultipleRoles_ReturnsAllActiveRoles()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            var testRolIds = new List<int> { 1, 2 }; // Adjust based on your test data

            // Insert multiple roles
            foreach (var rolId in testRolIds)
            {
                _userRolService.Insert(new UserRolDTO
                {
                    UserId = testUserId,
                    RolId = (RolesType)rolId
                });
            }

            // Act
            var result = _userRolService.GetRolsByUserId(testUserId);

            // Assert
            Assert.IsNotNull(result);
            var resultList = result.ToList();
            Assert.IsTrue(resultList.Count >= testRolIds.Count);

            foreach (var rolId in testRolIds)
            {
                Assert.IsTrue(resultList.Any(r => r.RolId == rolId));
            }

            // Cleanup
            foreach (var rolId in testRolIds)
            {
                _userRolService.Delete(testUserId, rolId);
            }
        }

        #endregion GetRolsByUserId Tests

        #region Integration Tests

        [TestMethod]
        public void CompleteWorkflow_InsertGetDelete_WorksCorrectly()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            int testRolId = 1;  // Adjust based on your test data

            var userRol = new UserRolDTO
            {
                UserId = testUserId,
                RolId = (RolesType)testRolId
            };

            // Act & Assert - Insert
            var insertedRol = _userRolService.Insert(userRol);
            Assert.IsNotNull(insertedRol);

            // Act & Assert - Get
            var roles = _userRolService.GetRolsByUserId(testUserId);
            Assert.IsTrue(roles.Any(r => r.RolId == testRolId));

            // Act & Assert - Delete
            var deletedRol = _userRolService.Delete(testUserId, testRolId);
            Assert.IsNotNull(deletedRol);

            // Act & Assert - Verify deletion
            var rolesAfterDelete = _userRolService.GetRolsByUserId(testUserId);
            Assert.IsFalse(rolesAfterDelete.Any(r => r.RolId == testRolId));
        }

        [TestMethod]
        public void CompleteWorkflow_InsertDeleteReinsert_RestoresRelation()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            int testRolId = 1;  // Adjust based on your test data

            var userRol = new UserRolDTO
            {
                UserId = testUserId,
                RolId = (RolesType)testRolId
            };

            // Act - Insert, Delete, Re-insert
            _userRolService.Insert(userRol);
            _userRolService.Delete(testUserId, testRolId);
            var reinsertedRol = _userRolService.Insert(userRol);

            // Assert
            Assert.IsNotNull(reinsertedRol);

            var roles = _userRolService.GetRolsByUserId(testUserId);
            Assert.IsTrue(roles.Any(r => r.RolId == testRolId));

            // Cleanup
            _userRolService.Delete(testUserId, testRolId);
        }

        #endregion Integration Tests
    }
}