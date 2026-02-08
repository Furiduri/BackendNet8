using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GCatcode.DataBase.Models
{
    [Index(nameof(Token), IsUnique = true)]
    [Index(nameof(UserId))]
    [Index(nameof(ExpiryDate), nameof(IsRevoked))]
    public class RefreshToken : BaseModel
    {
        [Key]
        public int RefreshTokenId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required, MaxLength(500)]
        public string Token { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public bool IsRevoked { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [MaxLength(50)]
        public string CreatedByIp { get; set; }

        public DateTime? RevokedAt { get; set; }

        [MaxLength(50)]
        public string RevokedByIp { get; set; }

        [MaxLength(500)]
        public string ReplacedByToken { get; set; }

        // Navigation property
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        // Computed properties
        [NotMapped]
        public bool IsExpired => DateTime.UtcNow >= ExpiryDate;

        [NotMapped]
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}