using System.ComponentModel.DataAnnotations;

namespace GCatcode.DataBase.Models
{
    public class CL_View : BaseModel
    {
        [Key]
        public int ViewId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } 

        [Required, MaxLength(200)]
        public string Route { get; set; }

        [MaxLength(100)]
        public string Icon { get; set; } 

        [MaxLength(500)]
        public string Description { get; set; }

        public int? ParentViewId { get; set; } 

        public int Order { get; set; } 

        public bool IsActive { get; set; } = true;

        public IEnumerable<RL_ViewRol> ViewRols { get; internal set; }
        public IEnumerable<RL_ViewController> ViewControllers { get; internal set; }
    }
}