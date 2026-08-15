namespace UserManagementSystem.DTOs
{
    public class LoginResponseDTO
    {
        public string JwtToken { get; set; }
        public UserDTO UserDTO { get; set; }

    }
}
