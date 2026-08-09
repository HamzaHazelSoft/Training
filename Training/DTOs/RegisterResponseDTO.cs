using UserManagementSystem.Models;

namespace UserManagementSystem.DTOs
{
    public class RegisterResponseDTO<T>
    {
        public T Entity { get; set; }
        public string PasswordResetToken { get; set; }
    }
}
