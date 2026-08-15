using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.DTOs
{
    public class SetPasswordDTO
    {
        [Required]
        public string PasswordResetToken { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
