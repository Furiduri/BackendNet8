namespace GCatcode.Repository.DB.PermissionServices.Models
{
    public class PermissionDTO
    {
        public int PermissionId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Resource { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class UserPermissionResult
    {
        public int PermissionId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Resource { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty; // "Role" o "User"
        public bool IsGranted { get; set; } = true;
    }

    public class UserPermissionInsert
    {
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public bool IsGranted { get; set; } = true;
        public string? Conditions { get; set; }
    }

    public class RolePermissionInsert
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
    }
}
