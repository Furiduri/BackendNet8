using System.ComponentModel.DataAnnotations;

namespace GCatcode.Repository.DB.RolService
{
    public class RolInsert
    {
        [Required, MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }
    }
}