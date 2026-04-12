using Dapper;
using GCatcode.Services.DB.ViewServices.Models;
using GCatcode.Utils.GenericModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GCatcode.Services.DB.ViewServices
{
    public class ViewService : DBService
    {
        public ViewService(SqlConnection connection, IDbTransaction? transaction = null)
            : base(connection, transaction!)
        {
        }

        /// <summary>
        /// Gets all views
        /// </summary>
        public PagesList<ViewDTO> GetAll(int page, int pageSize = 10)
        {
            const string sql = @"
                SELECT ViewId, Name, Route, Icon, Description, ParentViewId, [Order], IsActive
                FROM CL_Views
                WHERE Available = 1
                ORDER BY [Order], Name
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            var items = DbConnection.Query<ViewDTO>(sql, new { Offset = (page - 1) * pageSize, PageSize = pageSize }, transaction: Transaction);
            var totalItems = DbConnection.QueryFirstOrDefault<int>("SELECT COUNT(*) FROM CL_Views WHERE Available = 1", transaction: Transaction);   
            
            return new PagesList<ViewDTO>
            {
                Items = items,
                TotalCount = totalItems,
                PageSize = pageSize,
                Page = page
            };
        }

        /// <summary>
        /// Gets a view by its ID
        /// </summary>
        public ViewDTO? GetById(int viewId)
        {
            const string sql = @"
                SELECT ViewId, Name, Route, Icon, Description, ParentViewId, [Order], IsActive
                FROM CL_Views
                WHERE ViewId = @ViewId AND Available = 1";

            return DbConnection.QueryFirstOrDefault<ViewDTO>(sql, new { ViewId = viewId }, transaction: Transaction);
        }

        /// <summary>
        /// Gets views assigned to a user (based on their roles)
        /// </summary>
        public IEnumerable<ViewDTO> GetByUserId(int userId)
        {
            const string sql = @"
                SELECT DISTINCT 
                    v.ViewId, v.Name, v.Route, v.Icon, v.Description, 
                    v.ParentViewId, v.[Order], v.IsActive
                FROM CL_Views v
                INNER JOIN RL_ViewRoles vr ON v.ViewId = vr.ViewId
                INNER JOIN RL_UserRoles ur ON vr.RolId = ur.RolId
                WHERE ur.UserId = @UserId 
                    AND v.Available = 1 
                    AND v.IsActive = 1
                    AND vr.Available = 1
                    AND ur.Available = 1
                ORDER BY v.[Order], v.Name";

            return DbConnection.Query<ViewDTO>(sql, new { UserId = userId }, transaction: Transaction);
        }

        /// <summary>
        /// Gets views assigned to a role
        /// </summary>
        public IEnumerable<ViewDTO> GetByRoleId(int roleId)
        {
            const string sql = @"
                SELECT v.ViewId, v.Name, v.Route, v.Icon, v.Description, 
                       v.ParentViewId, v.[Order], v.IsActive
                FROM CL_Views v
                INNER JOIN RL_ViewRoles vr ON v.ViewId = vr.ViewId
                WHERE vr.RolId = @RoleId 
                    AND v.Available = 1                    
                    AND vr.Available = 1
                ORDER BY v.[Order], v.Name";

            return DbConnection.Query<ViewDTO>(sql, new { RoleId = roleId }, transaction: Transaction);
        }

        /// <summary>
        /// Creates a new view
        /// </summary>
        public ViewDTO? Add(ViewInsert data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            const string sql = @"
                INSERT INTO CL_Views (Name, Route, Icon, Description, ParentViewId, [Order], IsActive, CreateTime, LastUpdated, Available)
                VALUES (@Name, @Route, @Icon, @Description, @ParentViewId, @Order, @IsActive, GETUTCDATE(), GETUTCDATE(), 1);
                SELECT * FROM CL_Views WHERE ViewId = @@IDENTITY;";

            return DbConnection.QueryFirstOrDefault<ViewDTO>(sql, data, transaction: Transaction);
        }

        /// <summary>
        /// Updates an existing view
        /// </summary>
        public bool Update(ViewUpdate data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            const string sql = @"
                UPDATE CL_Views
                SET Name = @Name,
                    Route = @Route,
                    Icon = @Icon,
                    Description = @Description,
                    ParentViewId = @ParentViewId,
                    [Order] = @Order,
                    IsActive = @IsActive,
                    LastUpdated = GETUTCDATE()
                WHERE ViewId = @ViewId AND Available = 1";

            var result = DbConnection.Execute(sql, data, transaction: Transaction);
            return result > 0;
        }

        /// <summary>
        /// Deletes (soft delete) a view
        /// </summary>
        public bool Delete(int viewId)
        {
            const string sql = @"
                UPDATE CL_Views
                SET Available = 0, LastUpdated = GETUTCDATE()
                WHERE ViewId = @ViewId";

            var result = DbConnection.Execute(sql, new { ViewId = viewId }, transaction: Transaction);
            return result > 0;
        }

        /// <summary>
        /// Assigns a view to a role
        /// </summary>
        public void AssignViewToRole(ViewRoleAssignment viewToRole)
        {
            if (viewToRole == null) throw new ArgumentNullException(nameof(viewToRole));

            const string sql = @"
                IF NOT EXISTS (SELECT 1 FROM RL_ViewRoles WHERE ViewId = @ViewId AND RolId = @RoleId)
                BEGIN
                    INSERT INTO RL_ViewRoles (ViewId, RolId, CreateTime, LastUpdated, Available)
                    VALUES (@ViewId, @RoleId, GETUTCDATE(), GETUTCDATE(), 1);
                END ELSE BEGIN
                    UPDATE RL_ViewRoles
                    SET Available = 1,
                        LastUpdated = GETUTCDATE()
                    WHERE ViewId = @ViewId AND RolId = @RoleId;
                END";

            DbConnection.Execute(sql, viewToRole, transaction: Transaction);
        }

        /// <summary>
        /// Removes the assignment of a view from a role
        /// </summary>
        public void RemoveViewFromRole(ViewRoleAssignment viewToRole)
        {
            if (viewToRole == null) throw new ArgumentNullException(nameof(viewToRole));

            const string sql = @"UPDATE RL_ViewRoles
                SET Available = 0, LastUpdated = GETUTCDATE()   
                WHERE ViewId = @ViewId AND RolId = @RoleId";

            DbConnection.Execute(sql, viewToRole, transaction: Transaction);
        }

        /// <summary>
        /// Assigns multiple roles to a view (replaces existing assignments)
        /// </summary>
        public void AssignRolesToView(ViewAssignmentRoleList assignmentRoleList)
        {
            if (assignmentRoleList == null) throw new ArgumentNullException(nameof(assignmentRoleList));

            // Remove existing assignments
            const string deleteSql = "UPDATE RL_ViewRoles SET Available = 0, LastUpdated = GETUTCDATE() WHERE ViewId = @ViewId";
            DbConnection.Execute(deleteSql, new { ViewId = assignmentRoleList.ViewId }, transaction: Transaction);
            // Insert new assignments
            if (assignmentRoleList.RoleIds.Any())
            {                
                foreach (var roleId in assignmentRoleList.RoleIds)
                {
                    AssignViewToRole(new ViewRoleAssignment { ViewId = assignmentRoleList.ViewId, RoleId = roleId });
                }
            }
        }

        public ViewDTO? GetByRoute(string route)
        {
            const string sql = "SELECT * FROM CL_Views WHERE Route LIKE @Route";
            return DbConnection.QueryFirstOrDefault<ViewDTO>(sql, new { Route = route }, transaction: Transaction);
        }
    }
}
