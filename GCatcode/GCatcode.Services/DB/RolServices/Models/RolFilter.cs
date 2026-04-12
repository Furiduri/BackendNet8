namespace GCatcode.Services.DB.RolServices.Models
{
    public class RolFilter
    {
        public int? RolId { get; set; } = null;

        public string? Name { get; set; } = null;
        public string? Description { get; set; } = null;
        public bool? Available { get; set; } = null;
        public DateTime? LastUpdated { get; set; } = null;
    }
}