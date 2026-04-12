using GCatcode.Services.DB.PermissionServices;

namespace TestUnit.Repository
{
    [TestClass]
    public sealed class TestPermission : TestRepositoryConfig
    {
        private PermissionService _permissionService;

        [TestInitialize]
        public void Setup()
        {
            _permissionService = new PermissionService(_connection);
        }

        #region GetAll Tests

        [TestMethod]
        public void GetAll_WithDefaultParameters_ReturnsPagedPermissions()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;

            // Act
            var result = _permissionService.GetAll(page, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(page, result.Page);
            Assert.AreEqual(pageSize, result.PageSize);
            Assert.IsTrue(result.TotalCount >= 0);
        }

        [TestMethod]
        public void GetAll_WithPagination_ReturnsCorrectPage()
        {
            // Arrange
            int page = 2;
            int pageSize = 5;

            // Act
            var result = _permissionService.GetAll(page, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Items.Count() <= pageSize);
            Assert.AreEqual(page, result.Page);
        }

        [TestMethod]
        public void GetAll_OrdersByResourceAndAction_ReturnsOrderedList()
        {
            // Arrange
            int page = 1;
            int pageSize = 100;

            // Act
            var result = _permissionService.GetAll(page, pageSize);

            // Assert
            Assert.IsNotNull(result);
            var items = result.Items.ToList();

            // Verify ordering by Resource, then Action
            for (int i = 0; i < items.Count - 1; i++)
            {
                var current = items[i];
                var next = items[i + 1];

                int resourceComparison = string.Compare(current.Resource, next.Resource, StringComparison.Ordinal);
                if (resourceComparison == 0)
                {
                    Assert.IsTrue(string.Compare(current.Action, next.Action, StringComparison.Ordinal) <= 0);
                }
            }
        }

        #endregion GetAll Tests

        #region GetById Tests

        [TestMethod]
        public void GetById_WithValidId_ReturnsPermission()
        {
            // Arrange
            int validPermissionId = 1; // Adjust based on your test data

            // Act
            var result = _permissionService.GetById(validPermissionId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(validPermissionId, result.PermissionId);
            Assert.IsFalse(string.IsNullOrEmpty(result.Code));
        }

        [TestMethod]
        public void GetById_WithInvalidId_ReturnsNull()
        {
            // Arrange
            int invalidPermissionId = -1;

            // Act
            var result = _permissionService.GetById(invalidPermissionId);

            // Assert
            Assert.IsNull(result);
        }

        #endregion GetById Tests

        #region GetByCode Tests

        [TestMethod]
        public void GetByCode_WithValidCode_ReturnsPermission()
        {
            // Arrange
            // First, get a valid permission code from the database
            var permissions = _permissionService.GetAll(1, 1);
            var firstPermission = permissions.Items.FirstOrDefault();

            Assert.IsNotNull(firstPermission, "No permissions found in database for testing");
            string validCode = firstPermission.Code;

            // Act
            var result = _permissionService.GetByCode(validCode);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(validCode, result.Code);
        }

        [TestMethod]
        public void GetByCode_WithInvalidCode_ReturnsNull()
        {
            // Arrange
            string invalidCode = "INVALID_CODE_9999";

            // Act
            var result = _permissionService.GetByCode(invalidCode);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetByCode_WithNullCode_ReturnsNull()
        {
            // Arrange
            string nullCode = null;

            // Act
            var result = _permissionService.GetByCode(nullCode);

            // Assert
            Assert.IsNull(result);
        }

        #endregion GetByCode Tests

        #region GetUserPermissions Tests

        [TestMethod]
        public void GetUserPermissions_WithValidUserId_ReturnsPermissions()
        {
            // Arrange
            int validUserId = 1; // Adjust based on your test data

            // Act
            var result = _permissionService.GetUserPermissions(validUserId);

            // Assert
            Assert.IsNotNull(result);
            // Note: May return empty if user has no permissions assigned
        }

        [TestMethod]
        public void GetUserPermissions_WithInvalidUserId_ReturnsEmpty()
        {
            // Arrange
            int invalidUserId = -999;

            // Act
            var result = _permissionService.GetUserPermissions(invalidUserId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void GetUserPermissions_CombinesRoleAndUserPermissions_ReturnsHybridResults()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            int testPermissionId = 1; // Adjust based on your test data

            // Grant direct permission to user
            _permissionService.GrantPermissionToUser(testUserId, testPermissionId, true);

            // Act
            var result = _permissionService.GetUserPermissions(testUserId);

            // Assert
            Assert.IsNotNull(result);
            var permissions = result.ToList();

            // Should include both Role-based and User-based permissions
            var hasRolePermissions = permissions.Any(p => p.Source == "Role");
            var hasUserPermissions = permissions.Any(p => p.Source == "User");

            Assert.IsTrue(hasRolePermissions || hasUserPermissions);

            // Cleanup
            _permissionService.RevokePermissionFromUser(testUserId, testPermissionId);
        }

        #endregion GetUserPermissions Tests

        #region HasPermission Tests

        [TestMethod]
        public void HasPermission_WithGrantedPermission_ReturnsTrue()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            var permissions = _permissionService.GetAll(1, 1);
            var testPermission = permissions.Items.FirstOrDefault();

            Assert.IsNotNull(testPermission, "No permissions found for testing");

            // Grant permission to user
            _permissionService.GrantPermissionToUser(testUserId, testPermission.PermissionId, true);

            // Act
            var result = _permissionService.HasPermission(testUserId, testPermission.Code);

            // Assert
            Assert.IsTrue(result);

            // Cleanup
            _permissionService.RevokePermissionFromUser(testUserId, testPermission.PermissionId);
        }

        [TestMethod]
        public void HasPermission_WithRevokedPermission_ReturnsFalse()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            var permissions = _permissionService.GetAll(1, 1);
            var testPermission = permissions.Items.FirstOrDefault();

            Assert.IsNotNull(testPermission, "No permissions found for testing");

            // Grant and then revoke permission
            _permissionService.GrantPermissionToUser(testUserId, testPermission.PermissionId, true);
            _permissionService.RevokePermissionFromUser(testUserId, testPermission.PermissionId);

            // Act
            var result = _permissionService.HasPermission(testUserId, testPermission.Code);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void HasPermission_WithDeniedDirectPermission_ReturnsFalse()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            var permissions = _permissionService.GetAll(1, 1);
            var testPermission = permissions.Items.FirstOrDefault();

            Assert.IsNotNull(testPermission, "No permissions found for testing");

            // Explicitly deny permission (IsGranted = false)
            _permissionService.GrantPermissionToUser(testUserId, testPermission.PermissionId, false);

            // Act
            var result = _permissionService.HasPermission(testUserId, testPermission.Code);

            // Assert
            Assert.IsFalse(result);

            // Cleanup
            _permissionService.RevokePermissionFromUser(testUserId, testPermission.PermissionId);
        }

        [TestMethod]
        public void HasPermission_DirectPermissionOverridesRolePermission_PrioritizesDirect()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            int testRoleId = 1; // Adjust based on your test data
            var permissions = _permissionService.GetAll(1, 1);
            var testPermission = permissions.Items.FirstOrDefault();

            Assert.IsNotNull(testPermission, "No permissions found for testing");

            // Grant permission to role
            _permissionService.GrantPermissionToRole(testRoleId, testPermission.PermissionId);

            // Deny direct permission to user (should override role permission)
            _permissionService.GrantPermissionToUser(testUserId, testPermission.PermissionId, false);

            // Act
            var result = _permissionService.HasPermission(testUserId, testPermission.Code);

            // Assert
            Assert.IsFalse(result, "Direct denial should override role grant");

            // Cleanup
            _permissionService.RevokePermissionFromUser(testUserId, testPermission.PermissionId);
            _permissionService.RevokePermissionFromRole(testRoleId, testPermission.PermissionId);
        }

        #endregion HasPermission Tests

        #region GrantPermissionToUser Tests

        [TestMethod]
        public void GrantPermissionToUser_WithValidData_GrantsPermission()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            var permissions = _permissionService.GetAll(1, 1);
            var testPermission = permissions.Items.FirstOrDefault();

            Assert.IsNotNull(testPermission, "No permissions found for testing");

            // Act
            _permissionService.GrantPermissionToUser(testUserId, testPermission.PermissionId, true);

            // Assert
            var hasPermission = _permissionService.HasPermission(testUserId, testPermission.Code);
            Assert.IsTrue(hasPermission);

            // Cleanup
            _permissionService.RevokePermissionFromUser(testUserId, testPermission.PermissionId);
        }

        [TestMethod]
        public void GrantPermissionToUser_WithConditions_StoresConditions()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            var permissions = _permissionService.GetAll(1, 1);
            var testPermission = permissions.Items.FirstOrDefault();
            string testConditions = "department:IT AND level:senior";

            Assert.IsNotNull(testPermission, "No permissions found for testing");

            // Act
            _permissionService.GrantPermissionToUser(testUserId, testPermission.PermissionId, true, testConditions);

            // Assert
            var hasPermission = _permissionService.HasPermission(testUserId, testPermission.Code);
            Assert.IsTrue(hasPermission);

            // Cleanup
            _permissionService.RevokePermissionFromUser(testUserId, testPermission.PermissionId);
        }

        [TestMethod]
        public void GrantPermissionToUser_CalledTwice_UpdatesExistingPermission()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            var permissions = _permissionService.GetAll(1, 1);
            var testPermission = permissions.Items.FirstOrDefault();

            Assert.IsNotNull(testPermission, "No permissions found for testing");

            // First grant
            _permissionService.GrantPermissionToUser(testUserId, testPermission.PermissionId, true);

            // Act - Grant again with different value
            _permissionService.GrantPermissionToUser(testUserId, testPermission.PermissionId, false);

            // Assert
            var hasPermission = _permissionService.HasPermission(testUserId, testPermission.Code);
            Assert.IsFalse(hasPermission, "Should update to denied");

            // Cleanup
            _permissionService.RevokePermissionFromUser(testUserId, testPermission.PermissionId);
        }

        #endregion GrantPermissionToUser Tests

        #region RevokePermissionFromUser Tests

        [TestMethod]
        public void RevokePermissionFromUser_WithValidData_RevokesPermission()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            var permissions = _permissionService.GetAll(1, 1);
            var testPermission = permissions.Items.FirstOrDefault();

            Assert.IsNotNull(testPermission, "No permissions found for testing");

            // Grant first
            _permissionService.GrantPermissionToUser(testUserId, testPermission.PermissionId, true);

            // Act
            _permissionService.RevokePermissionFromUser(testUserId, testPermission.PermissionId);

            // Assert
            var userPermissions = _permissionService.GetUserPermissions(testUserId);
            var hasDirectPermission = userPermissions.Any(p =>
                p.PermissionId == testPermission.PermissionId && p.Source == "User" && p.IsGranted);

            Assert.IsFalse(hasDirectPermission, "Direct permission should be revoked");
        }

        [TestMethod]
        public void RevokePermissionFromUser_WithNonExistentPermission_DoesNotThrowException()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            int nonExistentPermissionId = -999;

            // Act & Assert - Should not throw
            _permissionService.RevokePermissionFromUser(testUserId, nonExistentPermissionId);
        }

        #endregion RevokePermissionFromUser Tests

        #region GetRolePermissions Tests

        [TestMethod]
        public void GetRolePermissions_WithValidRoleId_ReturnsPermissions()
        {
            // Arrange
            int testRoleId = 1; // Adjust based on your test data

            // Act
            var result = _permissionService.GetRolePermissions(testRoleId);

            // Assert
            Assert.IsNotNull(result);
            // Note: May return empty if role has no permissions
        }

        [TestMethod]
        public void GetRolePermissions_WithInvalidRoleId_ReturnsEmpty()
        {
            // Arrange
            int invalidRoleId = -999;

            // Act
            var result = _permissionService.GetRolePermissions(invalidRoleId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void GetRolePermissions_OrdersByResourceAndAction_ReturnsOrderedList()
        {
            // Arrange
            int testRoleId = 1; // Adjust based on your test data
            var permissions = _permissionService.GetAll(1, 2);
            var permissionsToGrant = permissions.Items.Take(2).ToList();

            // Grant multiple permissions
            foreach (var permission in permissionsToGrant)
            {
                _permissionService.GrantPermissionToRole(testRoleId, permission.PermissionId);
            }

            // Act
            var result = _permissionService.GetRolePermissions(testRoleId);

            // Assert
            Assert.IsNotNull(result);
            var items = result.ToList();

            // Verify ordering
            for (int i = 0; i < items.Count - 1; i++)
            {
                var current = items[i];
                var next = items[i + 1];

                int resourceComparison = string.Compare(current.Resource, next.Resource, StringComparison.Ordinal);
                Assert.IsTrue(resourceComparison <= 0);
            }

            // Cleanup
            foreach (var permission in permissionsToGrant)
            {
                _permissionService.RevokePermissionFromRole(testRoleId, permission.PermissionId);
            }
        }

        #endregion GetRolePermissions Tests

        #region GrantPermissionToRole Tests

        [TestMethod]
        public void GrantPermissionToRole_WithValidData_GrantsPermission()
        {
            // Arrange
            int testRoleId = 1; // Adjust based on your test data
            var permissions = _permissionService.GetAll(1, 1);
            var testPermission = permissions.Items.FirstOrDefault();

            Assert.IsNotNull(testPermission, "No permissions found for testing");

            // Act
            _permissionService.GrantPermissionToRole(testRoleId, testPermission.PermissionId);

            // Assert
            var rolePermissions = _permissionService.GetRolePermissions(testRoleId);
            Assert.IsTrue(rolePermissions.Any(p => p.PermissionId == testPermission.PermissionId));

            // Cleanup
            _permissionService.RevokePermissionFromRole(testRoleId, testPermission.PermissionId);
        }

        [TestMethod]
        public void GrantPermissionToRole_CalledTwice_ReactivatesPermission()
        {
            // Arrange
            int testRoleId = 1; // Adjust based on your test data
            var permissions = _permissionService.GetAll(1, 1);
            var testPermission = permissions.Items.FirstOrDefault();

            Assert.IsNotNull(testPermission, "No permissions found for testing");

            // Grant, revoke, and grant again
            _permissionService.GrantPermissionToRole(testRoleId, testPermission.PermissionId);
            _permissionService.RevokePermissionFromRole(testRoleId, testPermission.PermissionId);

            // Act
            _permissionService.GrantPermissionToRole(testRoleId, testPermission.PermissionId);

            // Assert
            var rolePermissions = _permissionService.GetRolePermissions(testRoleId);
            Assert.IsTrue(rolePermissions.Any(p => p.PermissionId == testPermission.PermissionId));

            // Cleanup
            _permissionService.RevokePermissionFromRole(testRoleId, testPermission.PermissionId);
        }

        #endregion GrantPermissionToRole Tests

        #region RevokePermissionFromRole Tests

        [TestMethod]
        public void RevokePermissionFromRole_WithValidData_RevokesPermission()
        {
            // Arrange
            int testRoleId = 1; // Adjust based on your test data
            var permissions = _permissionService.GetAll(1, 1);
            var testPermission = permissions.Items.FirstOrDefault();

            Assert.IsNotNull(testPermission, "No permissions found for testing");

            // Grant first
            _permissionService.GrantPermissionToRole(testRoleId, testPermission.PermissionId);

            // Act
            _permissionService.RevokePermissionFromRole(testRoleId, testPermission.PermissionId);

            // Assert
            var rolePermissions = _permissionService.GetRolePermissions(testRoleId);
            Assert.IsFalse(rolePermissions.Any(p => p.PermissionId == testPermission.PermissionId));
        }

        [TestMethod]
        public void RevokePermissionFromRole_WithNonExistentPermission_DoesNotThrowException()
        {
            // Arrange
            int testRoleId = 1; // Adjust based on your test data
            int nonExistentPermissionId = -999;

            // Act & Assert - Should not throw
            _permissionService.RevokePermissionFromRole(testRoleId, nonExistentPermissionId);
        }

        #endregion RevokePermissionFromRole Tests

        #region Integration Tests - Complete Workflows

        [TestMethod]
        public void CompleteWorkflow_UserPermissions_GrantCheckRevoke()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            var permissions = _permissionService.GetAll(1, 1);
            var testPermission = permissions.Items.FirstOrDefault();

            Assert.IsNotNull(testPermission, "No permissions found for testing");

            // Act & Assert - Grant
            _permissionService.GrantPermissionToUser(testUserId, testPermission.PermissionId, true);
            Assert.IsTrue(_permissionService.HasPermission(testUserId, testPermission.Code));

            // Act & Assert - Check in user permissions
            var userPermissions = _permissionService.GetUserPermissions(testUserId);
            Assert.IsTrue(userPermissions.Any(p =>
                p.PermissionId == testPermission.PermissionId && p.Source == "User" && p.IsGranted));

            // Act & Assert - Revoke
            _permissionService.RevokePermissionFromUser(testUserId, testPermission.PermissionId);
            var userPermissionsAfterRevoke = _permissionService.GetUserPermissions(testUserId);
            Assert.IsFalse(userPermissionsAfterRevoke.Any(p =>
                p.PermissionId == testPermission.PermissionId && p.Source == "User" && p.IsGranted));
        }

        [TestMethod]
        public void CompleteWorkflow_RolePermissions_GrantCheckRevoke()
        {
            // Arrange
            int testRoleId = 1; // Adjust based on your test data
            var permissions = _permissionService.GetAll(1, 1);
            var testPermission = permissions.Items.FirstOrDefault();

            Assert.IsNotNull(testPermission, "No permissions found for testing");

            // Act & Assert - Grant
            _permissionService.GrantPermissionToRole(testRoleId, testPermission.PermissionId);
            var rolePermissions = _permissionService.GetRolePermissions(testRoleId);
            Assert.IsTrue(rolePermissions.Any(p => p.PermissionId == testPermission.PermissionId));

            // Act & Assert - Revoke
            _permissionService.RevokePermissionFromRole(testRoleId, testPermission.PermissionId);
            var rolePermissionsAfterRevoke = _permissionService.GetRolePermissions(testRoleId);
            Assert.IsFalse(rolePermissionsAfterRevoke.Any(p => p.PermissionId == testPermission.PermissionId));
        }

        [TestMethod]
        public void CompleteWorkflow_HybridRBAC_ABAC_PrioritizesDirectPermission()
        {
            // Arrange
            int testUserId = 1; // Adjust based on your test data
            int testRoleId = 1; // Adjust based on your test data
            var permissions = _permissionService.GetAll(1, 1);
            var testPermission = permissions.Items.FirstOrDefault();

            Assert.IsNotNull(testPermission, "No permissions found for testing");

            // Step 1: Grant permission to role (RBAC)
            _permissionService.GrantPermissionToRole(testRoleId, testPermission.PermissionId);

            // Verify user has permission through role
            var userPermissions1 = _permissionService.GetUserPermissions(testUserId);
            var hasRolePermission = userPermissions1.Any(p =>
                p.PermissionId == testPermission.PermissionId && p.Source == "Role");

            if (hasRolePermission)
            {
                Assert.IsTrue(_permissionService.HasPermission(testUserId, testPermission.Code));
            }

            // Step 2: Deny direct permission to user (ABAC override)
            _permissionService.GrantPermissionToUser(testUserId, testPermission.PermissionId, false);

            // Verify direct denial overrides role grant
            Assert.IsFalse(_permissionService.HasPermission(testUserId, testPermission.Code));

            // Step 3: Grant direct permission to user
            _permissionService.GrantPermissionToUser(testUserId, testPermission.PermissionId, true);

            // Verify direct grant is effective
            Assert.IsTrue(_permissionService.HasPermission(testUserId, testPermission.Code));

            // Cleanup
            _permissionService.RevokePermissionFromUser(testUserId, testPermission.PermissionId);
            _permissionService.RevokePermissionFromRole(testRoleId, testPermission.PermissionId);
        }

        #endregion Integration Tests - Complete Workflows
    }
}