using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.DTOs;
using UserManagementSystem.Models;

namespace UserManagementSystem.Services.Auth
{
    public interface IAuthService
    {
        public Task<UserDTO> Register(RegisterDTO registerDTO);
        public Task<LoginResponseDTO> Login(LoginDTO loginDTO);
        public Task<RegisterResponseDTO> ConfirmEmail(string userId, string token);
        public Task<UserDTO> SetPassword(SetPasswordDTO passwordDTO);
    }
}
