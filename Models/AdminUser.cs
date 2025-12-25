using System.ComponentModel.DataAnnotations;

namespace FileGallery.Models
{
    public class AdminUser
    {
        [Key]
        public int AdminId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string PasswordHash { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? FullName { get; set; }
        
        [StringLength(200)]
        public string? Email { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public DateTime? LastLoginDate { get; set; }
    }
    
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        
        public bool RememberMe { get; set; }
    }
}