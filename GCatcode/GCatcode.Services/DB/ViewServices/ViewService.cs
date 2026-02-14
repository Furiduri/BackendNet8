using Dapper;
using GCatcode.Repository.DB.ViewServices.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GCatcode.Repository.DB.ViewServices
{
    public class ViewService : DBService
    {
        public ViewService(SqlConnection connection, IDbTransaction? transaction = null)
            : base(connection, transaction!)
        {
        }

        /// <summary>
        /// Obtiene todas las vistas
        /// </summary>
        public IEnumerable<ViewDTO> GetAll()
        {
            const string sql = @"
                SELECT ViewId, Name, Route, Icon, Description, ParentViewId, [Order], IsActive
                FROM CL_Views
                WHERE Available = 1
                ORDER BY [Order], Name";

            return DbConnection.Query<ViewDTO>(sql, transaction: Transaction);
        }

        /// <summary>
        /// Obtiene una vista por su ID
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
        /// Obtiene todas las vistas con sus roles asignados
        /// </summary>
        public IEnumerable<ViewWithRoles> GetAllWithRoles()
        {
            const string sql = @"
                SELECT 
                    v.ViewId, v.Name, v.Route, v.Icon, v.Description, 
                    v.ParentViewId, v.[Order], v.IsActive,
                    r.RolId, r.Name as RoleName
                FROM CL_Views v
                LEFT JOIN RL_ViewRoles vr ON v.ViewId = vr.ViewId
                LEFT JOIN CL_Roles r ON vr.RolId = r.RolId AND r.Available = 1
                WHERE v.Available = 1
                ORDER BY v.[Order], v.Name";

            var viewsDict = new Dictionary<int, ViewWithRoles>();

            DbConnection.Query<ViewWithRoles, RoleInfo, ViewWithRoles>(
                sql,
                (view, role) =>
                {
                    if (!viewsDict.TryGetValue(view.ViewId, out var existingView))
                    {
                        existingView = view;
                        existingView.Roles = new List<RoleInfo>();
                        viewsDict.Add(view.ViewId, existingView);
                    }

                    if (role != null && role.RolId > 0)
                    {
                        existingView.Roles.Add(role);
                    }

                    return existingView;
                },
                splitOn: "RolId",
                transaction: Transaction
            );

            return viewsDict.Values;
        }

        /// <summary>
        /// Obtiene las vistas asignadas a un usuario (según sus roles)
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
                    AND ur.Available = 1
                ORDER BY v.[Order], v.Name";

            return DbConnection.Query<ViewDTO>(sql, new { UserId = userId }, transaction: Transaction);
        }

        /// <summary>
        /// Obtiene las vistas asignadas a un rol
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
                ORDER BY v.[Order], v.Name";

            return DbConnection.Query<ViewDTO>(sql, new { RoleId = roleId }, transaction: Transaction);
        }

        /// <summary>
        /// Crea una nueva vista
        /// </summary>
        public ViewDTO? Add(ViewInsert data)
        {
            const string sql = @"
                INSERT INTO CL_Views (Name, Route, Icon, Description, ParentViewId, [Order], IsActive, CreateTime, LastUpdated, Available)
                OUTPUT INSERTED.ViewId, INSERTED.Name, INSERTED.Route, INSERTED.Icon, INSERTED.Description, 
                       INSERTED.ParentViewId, INSERTED.[Order], INSERTED.IsActive
                VALUES (@Name, @Route, @Icon, @Description, @ParentViewId, @Order, @IsActive, GETUTCDATE(), GETUTCDATE(), 1)";

            return DbConnection.QueryFirstOrDefault<ViewDTO>(sql, data, transaction: Transaction);
        }

        /// <summary>
        /// Actualiza una vista existente
        /// </summary>
        public bool Update(ViewUpdate data)
        {
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
        /// Elimina (soft delete) una vista
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
        /// Asigna una vista a un rol
        /// </summary>
        public void AssignViewToRole(int viewId, int roleId)
        {
            const string sql = @"
                IF NOT EXISTS (SELECT 1 FROM RL_ViewRoles WHERE ViewId = @ViewId AND RolId = @RoleId)
                BEGIN
                    INSERT INTO RL_ViewRoles (ViewId, RolId)
                    VALUES (@ViewId, @RoleId);
                END";

            DbConnection.Execute(sql, new { ViewId = viewId, RoleId = roleId }, transaction: Transaction);
        }

        /// <summary>
        /// Remueve la asignación de una vista a un rol
        /// </summary>
        public void RemoveViewFromRole(int viewId, int roleId)
        {
            const string sql = @"
                DELETE FROM RL_ViewRoles 
                WHERE ViewId = @ViewId AND RolId = @RoleId";

            DbConnection.Execute(sql, new { ViewId = viewId, RoleId = roleId }, transaction: Transaction);
        }

        /// <summary>
        /// Asigna múltiples roles a una vista (reemplaza asignaciones existentes)
        /// </summary>
        public void AssignRolesToView(int viewId, IEnumerable<int> roleIds)
        {
            // Eliminar asignaciones existentes
            const string deleteSql = "DELETE FROM RL_ViewRoles WHERE ViewId = @ViewId";
            DbConnection.Execute(deleteSql, new { ViewId = viewId }, transaction: Transaction);

            // Insertar nuevas asignaciones
            if (roleIds.Any())
            {
                const string insertSql = "INSERT INTO RL_ViewRoles (ViewId, RolId) VALUES (@ViewId, @RoleId)";
                foreach (var roleId in roleIds)
                {
                    DbConnection.Execute(insertSql, new { ViewId = viewId, RoleId = roleId }, transaction: Transaction);
                }
            }
        }
    }
}
