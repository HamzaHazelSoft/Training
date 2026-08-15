using UserManagementSystem.Models;

namespace UserManagementSystem.DTOs
{
    public class RegisterResponseDTO
    {
        public UserDTO UserDTO { get; set; }
        public string PasswordResetToken { get; set; }
    }
}
