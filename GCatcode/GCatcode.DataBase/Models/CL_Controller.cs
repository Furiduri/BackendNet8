using System.ComponentModel.DataAnnotations;

namespace GCatcode.DataBase.Models
{
    public class CL_Controller : BaseModel
    {
        [Key]
        public int ControllerId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } 

        [Required, MaxLength(200)]
        public string ControllerPath { get; set; } 

        [MaxLength(500)]
        public string Description { get; set; }
        public IEnumerable<RL_ViewController> ViewControllers { get; internal set; }
    }
}