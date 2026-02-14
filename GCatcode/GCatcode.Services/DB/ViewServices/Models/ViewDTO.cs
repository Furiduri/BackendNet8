namespace GCatcode.Repository.DB.ViewServices.Models
{
    public class ViewDTO
    {
        public int ViewId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? ParentViewId { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
    }

    public class ViewInsert
    {
        public string Name { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? ParentViewId { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class ViewUpdate
    {
        public int ViewId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? ParentViewId { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
    }

    public class ViewWithRoles
    {
        public int ViewId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? ParentViewId { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public List<RoleInfo> Roles { get; set; } = new();
    }

    public class RoleInfo
    {
        public int RolId { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class ViewRoleAssignment
    {
        public int ViewId { get; set; }
        public int RoleId { get; set; }
    }
}
