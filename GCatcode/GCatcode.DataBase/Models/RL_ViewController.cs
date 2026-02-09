using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCatcode.DataBase.Models
{
    public class RL_ViewController
    {
        [Required]
        public int ViewId { get; set; }

        [Required]
        public int ControllerId { get; set; }

        [ForeignKey(nameof(ViewId))]
        public CL_View View { get; set; }

        [ForeignKey(nameof(ControllerId))]
        public CL_Controller Controller { get; set; }
    }
}