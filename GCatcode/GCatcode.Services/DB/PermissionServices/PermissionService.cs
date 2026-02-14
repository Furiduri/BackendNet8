using Dapper;
using GCatcode.Repository.DB.PermissionServices.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GCatcode.Repository.DB.PermissionServices
{
    public class PermissionService : DBService
    {
        public PermissionService(SqlConnection connection, IDbTransaction? transaction = null)
            : base(connection, transaction!)
        {
        }

        /// <summary>
        /// Obtiene todos los permisos disponibles en el sistema
        /// </summary>
        public IEnumerable<PermissionDTO> GetAll()
        {
            const string sql = @"
                SELECT PermissionId, Code, Resource, Action, Description
                FROM CL_Permissions
                WHERE Available = 1
                ORDER BY Resource, Action";

            return DbConnection.Query<PermissionDTO>(sql, transaction: Transaction);
        }

        /// <summary>
        /// Obtiene un permiso por su ID
        /// </summary>
        public PermissionDTO? GetById(int permissionId)
        {
            const string sql = @"
                SELECT PermissionId, Code, Resource, Action, Description
                FROM CL_Permissions
                WHERE PermissionId = @PermissionId AND Available = 1";

            return DbConnection.QueryFirstOrDefault<PermissionDTO>(sql, new { PermissionId = permissionId }, transaction: Transaction);
        }

        /// <summary>
        /// Obtiene un permiso por su código
        /// </summary>
        public PermissionDTO? GetByCode(string code)
        {
            const string sql = @"
                SELECT PermissionId, Code, Resource, Action, Description
                FROM CL_Permissions
                WHERE Code = @Code AND Available = 1";

            return DbConnection.QueryFirstOrDefault<PermissionDTO>(sql, new { Code = code }, transaction: Transaction);
        }

        /// <summary>
        /// Obtiene todos los permisos de un usuario (combinando roles + permisos directos)
        /// Implementa el enfoque híbrido RBAC + ABAC
        /// </summary>
        public IEnumerable<UserPermissionResult> GetUserPermissions(int userId)
        {
            const string sql = @"
                -- Permisos heredados de roles (RBAC)
                SELECT DISTINCT 
                    p.PermissionId,
                    p.Code,
                    p.Resource,
                    p.Action,
                    'Role' as Source,
                    CAST(1 AS BIT) as IsGranted
                FROM CL_Permissions p
                INNER JOIN RL_RolePermissions rp ON p.PermissionId = rp.PermissionId
                INNER JOIN RL_UserRoles ur ON rp.RolId = ur.RolId
                WHERE ur.UserId = @UserId 
                    AND p.Available = 1 
                    AND rp.Available = 1 
                    AND ur.Available = 1

                UNION

                -- Permisos específicos del usuario (ABAC - Overrides)
                SELECT 
                    p.PermissionId,
                    p.Code,
                    p.Resource,
                    p.Action,
                    'User' as Source,
                    up.IsGranted
                FROM CL_Permissions p
                INNER JOIN RL_UserPermissions up ON p.PermissionId = up.PermissionId
                WHERE up.UserId = @UserId 
                    AND p.Available = 1 
                    AND up.Available = 1

                ORDER BY Resource, Action";

            return DbConnection.Query<UserPermissionResult>(sql, new { UserId = userId }, transaction: Transaction);
        }

        /// <summary>
        /// Verifica si un usuario tiene un permiso específico
        /// Prioriza los permisos directos sobre los heredados de roles
        /// </summary>
        public bool HasPermission(int userId, string permissionCode)
        {
            const string sql = @"
                DECLARE @HasDirectPermission BIT = 0;
                DECLARE @DirectPermissionGranted BIT = 0;

                -- Verificar si existe permiso directo
                SELECT TOP 1 
                    @HasDirectPermission = 1,
                    @DirectPermissionGranted = up.IsGranted
                FROM RL_UserPermissions up
                INNER JOIN CL_Permissions p ON up.PermissionId = p.PermissionId
                WHERE up.UserId = @UserId 
                    AND p.Code = @PermissionCode
                    AND up.Available = 1 
                    AND p.Available = 1;

                -- Si tiene permiso directo, retornar ese valor
                IF @HasDirectPermission = 1
                    SELECT @DirectPermissionGranted as HasPermission;
                ELSE
                BEGIN
                    -- Si no, verificar permisos heredados de roles
                    IF EXISTS (
                        SELECT 1
                        FROM CL_Permissions p
                        INNER JOIN RL_RolePermissions rp ON p.PermissionId = rp.PermissionId
                        INNER JOIN RL_UserRoles ur ON rp.RolId = ur.RolId
                        WHERE ur.UserId = @UserId 
                            AND p.Code = @PermissionCode
                            AND p.Available = 1 
                            AND rp.Available = 1 
                            AND ur.Available = 1
                    )
                        SELECT CAST(1 AS BIT) as HasPermission;
                    ELSE
                        SELECT CAST(0 AS BIT) as HasPermission;
                END";

            return DbConnection.QuerySingle<bool>(sql, new { UserId = userId, PermissionCode = permissionCode }, transaction: Transaction);
        }

        /// <summary>
        /// Asigna un permiso directo a un usuario (ABAC)
        /// </summary>
        public void GrantPermissionToUser(int userId, int permissionId, bool isGranted = true, string? conditions = null)
        {
            const string sql = @"
                IF EXISTS (SELECT 1 FROM RL_UserPermissions WHERE UserId = @UserId AND PermissionId = @PermissionId AND Available = 1)
                BEGIN
                    UPDATE RL_UserPermissions 
                    SET IsGranted = @IsGranted, 
                        Conditions = @Conditions,
                        LastUpdated = GETUTCDATE()
                    WHERE UserId = @UserId AND PermissionId = @PermissionId;
                END
                ELSE
                BEGIN
                    INSERT INTO RL_UserPermissions (UserId, PermissionId, IsGranted, Conditions, CreateTime, LastUpdated, Available)
                    VALUES (@UserId, @PermissionId, @IsGranted, @Conditions, GETUTCDATE(), GETUTCDATE(), 1);
                END";

            DbConnection.Execute(sql, new { UserId = userId, PermissionId = permissionId, IsGranted = isGranted, Conditions = conditions }, transaction: Transaction);
        }

        /// <summary>
        /// Revoca un permiso directo de un usuario
        /// </summary>
        public void RevokePermissionFromUser(int userId, int permissionId)
        {
            const string sql = @"
                UPDATE RL_UserPermissions 
                SET Available = 0, LastUpdated = GETUTCDATE()
                WHERE UserId = @UserId AND PermissionId = @PermissionId";

            DbConnection.Execute(sql, new { UserId = userId, PermissionId = permissionId }, transaction: Transaction);
        }

        /// <summary>
        /// Obtiene los permisos de un rol
        /// </summary>
        public IEnumerable<PermissionDTO> GetRolePermissions(int roleId)
        {
            const string sql = @"
                SELECT p.PermissionId, p.Code, p.Resource, p.Action, p.Description
                FROM CL_Permissions p
                INNER JOIN RL_RolePermissions rp ON p.PermissionId = rp.PermissionId
                WHERE rp.RolId = @RoleId 
                    AND p.Available = 1 
                    AND rp.Available = 1
                ORDER BY p.Resource, p.Action";

            return DbConnection.Query<PermissionDTO>(sql, new { RoleId = roleId }, transaction: Transaction);
        }

        /// <summary>
        /// Asigna un permiso a un rol
        /// </summary>
        public void GrantPermissionToRole(int roleId, int permissionId)
        {
            const string sql = @"
                IF NOT EXISTS (SELECT 1 FROM RL_RolePermissions WHERE RolId = @RoleId AND PermissionId = @PermissionId)
                BEGIN
                    INSERT INTO RL_RolePermissions (RolId, PermissionId, CreateTime, LastUpdated, Available)
                    VALUES (@RoleId, @PermissionId, GETUTCDATE(), GETUTCDATE(), 1);
                END
                ELSE
                BEGIN
                    UPDATE RL_RolePermissions 
                    SET Available = 1, LastUpdated = GETUTCDATE()
                    WHERE RolId = @RoleId AND PermissionId = @PermissionId;
                END";

            DbConnection.Execute(sql, new { RoleId = roleId, PermissionId = permissionId }, transaction: Transaction);
        }

        /// <summary>
        /// Revoca un permiso de un rol
        /// </summary>
        public void RevokePermissionFromRole(int roleId, int permissionId)
        {
            const string sql = @"
                UPDATE RL_RolePermissions 
                SET Available = 0, LastUpdated = GETUTCDATE()
                WHERE RolId = @RoleId AND PermissionId = @PermissionId";

            DbConnection.Execute(sql, new { RoleId = roleId, PermissionId = permissionId }, transaction: Transaction);
        }
    }
}
