using GCatcode.Services.DB.ViewServices;
using GCatcode.Services.DB.ViewServices.Models;

namespace TestUnit.Repository
{
    [TestClass]
    public sealed class TestView : TestRepositoryConfig
    {
        private ViewService _viewService;

        [TestInitialize]
        public void Setup()
        {
            _viewService = new ViewService(_connection);
        }

        #region GetAll Tests

        [TestMethod]
        public void GetAll_WithDefaultParameters_ReturnsPagedViews()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;

            // Act
            var result = _viewService.GetAll(page, pageSize);

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
            var result = _viewService.GetAll(page, pageSize);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Items.Count() <= pageSize);
            Assert.AreEqual(page, result.Page);
        }

        #endregion GetAll Tests

        #region GetById Tests

        [TestMethod]
        public void GetById_WithValidId_ReturnsView()
        {
            // Arrange
            int validViewId = 1;

            // Act
            var result = _viewService.GetById(validViewId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(validViewId, result.ViewId);
        }

        [TestMethod]
        public void GetById_WithInvalidId_ReturnsNull()
        {
            // Arrange
            int invalidViewId = -1;

            // Act
            var result = _viewService.GetById(invalidViewId);

            // Assert
            Assert.IsNull(result);
        }

        #endregion GetById Tests

        #region GetByUserId Tests

        [TestMethod]
        public void GetByUserId_WithValidUserId_ReturnsUserViews()
        {
            // Arrange
            int validUserId = 1;

            // Act
            var result = _viewService.GetByUserId(validUserId);

            // Assert
            Assert.IsNotNull(result);
            // Note: Result count depends on test data
        }

        [TestMethod]
        public void GetByUserId_WithInvalidUserId_ReturnsEmpty()
        {
            // Arrange
            int invalidUserId = -999;

            // Act
            var result = _viewService.GetByUserId(invalidUserId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
        }

        #endregion GetByUserId Tests

        #region GetByRoleId Tests

        [TestMethod]
        public void GetByRoleId_WithValidRoleId_ReturnsRoleViews()
        {
            // Arrange
            int validRoleId = 1;

            // Act
            var result = _viewService.GetByRoleId(validRoleId);

            // Assert
            Assert.IsNotNull(result);
            // Note: Result count depends on test data
        }

        [TestMethod]
        public void GetByRoleId_WithInvalidRoleId_ReturnsEmpty()
        {
            // Arrange
            int invalidRoleId = -999;

            // Act
            var result = _viewService.GetByRoleId(invalidRoleId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Any());
        }

        #endregion GetByRoleId Tests

        #region Add Tests

        [TestMethod]
        public void Add_WithValidData_ReturnsNewView()
        {
            // Arrange
            var newView = new ViewInsert
            {
                Name = $"TestView_{Guid.NewGuid()}",
                Route = $"/test/view/{Guid.NewGuid()}",
                Icon = "test-icon",
                Description = "Test view description",
                ParentViewId = null,
                Order = 100,
                IsActive = true
            };

            // Act
            var result = _viewService.Add(newView);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(newView.Name, result.Name);
            Assert.AreEqual(newView.Route, result.Route);
            Assert.IsTrue(result.ViewId > 0);

            // Cleanup
            _viewService.Delete(result.ViewId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Add_WithNullData_ThrowsException()
        {
            // Arrange
            ViewInsert nullData = null;

            // Act
            _viewService.Add(nullData);

            // Assert is handled by ExpectedException
        }

        #endregion Add Tests

        #region Update Tests

        [TestMethod]
        public void Update_WithValidData_ReturnsTrue()
        {
            // Arrange
            var newView = new ViewInsert
            {
                Name = $"TestView_{Guid.NewGuid()}",
                Route = $"/test/original/{Guid.NewGuid()}",
                Icon = "original-icon",
                Description = "Original description",
                ParentViewId = null,
                Order = 100,
                IsActive = true
            };
            var createdView = _viewService.Add(newView);

            var updateData = new ViewUpdate
            {
                ViewId = createdView.ViewId,
                Name = $"UpdatedView_{Guid.NewGuid()}",
                Route = $"/test/updated/{Guid.NewGuid()}",
                Icon = "updated-icon",
                Description = "Updated description",
                ParentViewId = null,
                Order = 200,
                IsActive = false
            };

            // Act
            var result = _viewService.Update(updateData);

            // Assert
            Assert.IsTrue(result);

            var updatedView = _viewService.GetById(createdView.ViewId);
            Assert.IsNotNull(updatedView);
            Assert.AreEqual(updateData.Name, updatedView.Name);
            Assert.AreEqual(updateData.Route, updatedView.Route);

            // Cleanup
            _viewService.Delete(createdView.ViewId);
        }

        [TestMethod]
        public void Update_WithInvalidId_ReturnsFalse()
        {
            // Arrange
            var updateData = new ViewUpdate
            {
                ViewId = -1,
                Name = "Invalid Update",
                Route = "/invalid",
                Icon = "icon",
                Description = "Description",
                ParentViewId = null,
                Order = 1,
                IsActive = true
            };

            // Act
            var result = _viewService.Update(updateData);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion Update Tests

        #region Delete Tests

        [TestMethod]
        public void Delete_WithValidId_ReturnsTrue()
        {
            // Arrange
            var newView = new ViewInsert
            {
                Name = $"TestView_{Guid.NewGuid()}",
                Route = $"/test/delete/{Guid.NewGuid()}",
                Icon = "delete-icon",
                Description = "View to be deleted",
                ParentViewId = null,
                Order = 100,
                IsActive = true
            };
            var createdView = _viewService.Add(newView);

            // Act
            var result = _viewService.Delete(createdView.ViewId);

            // Assert
            Assert.IsTrue(result);

            // Verify soft delete
            var deletedView = _viewService.GetById(createdView.ViewId);
            Assert.IsNull(deletedView); // Should be null because Available = 0
        }

        [TestMethod]
        public void Delete_WithInvalidId_ReturnsFalse()
        {
            // Arrange
            int invalidViewId = -999;

            // Act
            var result = _viewService.Delete(invalidViewId);

            // Assert
            Assert.IsFalse(result);
        }

        #endregion Delete Tests

        #region GetByRoute Tests

        [TestMethod]
        public void GetByRoute_WithValidRoute_ReturnsView()
        {
            // Arrange
            var uniqueRoute = $"/test/route/{Guid.NewGuid()}";
            var newView = new ViewInsert
            {
                Name = $"TestView_{Guid.NewGuid()}",
                Route = uniqueRoute,
                Icon = "route-icon",
                Description = "View with unique route",
                ParentViewId = null,
                Order = 100,
                IsActive = true
            };
            var createdView = _viewService.Add(newView);

            // Act
            var result = _viewService.GetByRoute(uniqueRoute);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(uniqueRoute, result.Route);

            // Cleanup
            _viewService.Delete(createdView.ViewId);
        }

        [TestMethod]
        public void GetByRoute_WithInvalidRoute_ReturnsNull()
        {
            // Arrange
            string invalidRoute = "/nonexistent/route/99999";

            // Act
            var result = _viewService.GetByRoute(invalidRoute);

            // Assert
            Assert.IsNull(result);
        }

        #endregion GetByRoute Tests

        #region AssignViewToRole Tests

        [TestMethod]
        public void AssignViewToRole_WithValidData_AssignsSuccessfully()
        {
            // Arrange
            var newView = new ViewInsert
            {
                Name = $"TestView_{Guid.NewGuid()}",
                Route = $"/test/assign/{Guid.NewGuid()}",
                Icon = "assign-icon",
                Description = "View for role assignment",
                ParentViewId = null,
                Order = 100,
                IsActive = true
            };
            var createdView = _viewService.Add(newView);
            int validRoleId = 1; // Adjust based on your test data

            var assignment = new ViewRoleAssignment
            {
                ViewId = createdView.ViewId,
                RoleId = validRoleId
            };

            // Act
            _viewService.AssignViewToRole(assignment);

            // Assert
            var roleViews = _viewService.GetByRoleId(validRoleId);
            Assert.IsTrue(roleViews.Any(v => v.ViewId == createdView.ViewId));

            // Cleanup
            _viewService.RemoveViewFromRole(assignment);
            _viewService.Delete(createdView.ViewId);
        }

        #endregion AssignViewToRole Tests

        #region RemoveViewFromRole Tests

        [TestMethod]
        public void RemoveViewFromRole_WithValidData_RemovesSuccessfully()
        {
            // Arrange
            var newView = new ViewInsert
            {
                Name = $"TestView_{Guid.NewGuid()}",
                Route = $"/test/remove/{Guid.NewGuid()}",
                Icon = "remove-icon",
                Description = "View for role removal",
                ParentViewId = null,
                Order = 100,
                IsActive = true
            };
            var createdView = _viewService.Add(newView);
            int validRoleId = 1; // Adjust based on your test data

            var assignment = new ViewRoleAssignment
            {
                ViewId = createdView.ViewId,
                RoleId = validRoleId
            };

            _viewService.AssignViewToRole(assignment);

            // Act
            _viewService.RemoveViewFromRole(assignment);

            // Assert
            var roleViews = _viewService.GetByRoleId(validRoleId);
            Assert.IsFalse(roleViews.Any(v => v.ViewId == createdView.ViewId));

            // Cleanup
            _viewService.Delete(createdView.ViewId);
        }

        #endregion RemoveViewFromRole Tests

        #region AssignRolesToView Tests

        [TestMethod]
        public void AssignRolesToView_WithMultipleRoles_AssignsSuccessfully()
        {
            // Arrange
            var newView = new ViewInsert
            {
                Name = $"TestView_{Guid.NewGuid()}",
                Route = $"/test/multi-assign/{Guid.NewGuid()}",
                Icon = "multi-icon",
                Description = "View for multiple role assignments",
                ParentViewId = null,
                Order = 100,
                IsActive = true
            };
            var createdView = _viewService.Add(newView);

            var assignmentList = new ViewAssignmentRoleList
            {
                ViewId = createdView.ViewId,
                RoleIds = new List<int> { 1, 2 } // Adjust based on your test data
            };

            // Act
            _viewService.AssignRolesToView(assignmentList);

            // Assert
            foreach (var roleId in assignmentList.RoleIds)
            {
                var roleViews = _viewService.GetByRoleId(roleId);
                Assert.IsTrue(roleViews.Any(v => v.ViewId == createdView.ViewId));
            }

            // Cleanup
            _viewService.Delete(createdView.ViewId);
        }

        [TestMethod]
        public void AssignRolesToView_WithEmptyList_ClearsAssignments()
        {
            // Arrange
            var newView = new ViewInsert
            {
                Name = $"TestView_{Guid.NewGuid()}",
                Route = $"/test/clear/{Guid.NewGuid()}",
                Icon = "clear-icon",
                Description = "View for clearing assignments",
                ParentViewId = null,
                Order = 100,
                IsActive = true
            };
            var createdView = _viewService.Add(newView);

            // First assign a role
            _viewService.AssignViewToRole(new ViewRoleAssignment
            {
                ViewId = createdView.ViewId,
                RoleId = 1
            });

            var assignmentList = new ViewAssignmentRoleList
            {
                ViewId = createdView.ViewId,
                RoleIds = new List<int>()
            };

            // Act
            _viewService.AssignRolesToView(assignmentList);

            // Assert
            var roleViews = _viewService.GetByRoleId(1);
            Assert.IsFalse(roleViews.Any(v => v.ViewId == createdView.ViewId));

            // Cleanup
            _viewService.Delete(createdView.ViewId);
        }

        #endregion AssignRolesToView Tests
    }
}