using System.ComponentModel.DataAnnotations;

namespace GCatcode.DataBase.Models
{
    public class CL_View : BaseModel
    {
        [Key]
        public int ViewId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Route { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Icon { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        public int? ParentViewId { get; set; } 

        public int Order { get; set; } 

        public bool IsActive { get; set; } = true;

        public IEnumerable<RL_ViewRol> ViewRols { get; internal set; } = new List<RL_ViewRol>();
    }
}