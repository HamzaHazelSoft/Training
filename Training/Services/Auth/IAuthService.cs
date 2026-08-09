using Microsoft.AspNetCore.Mvc;
using UserManagementSystem.DTOs;
using UserManagementSystem.Models;

namespace UserManagementSystem.Services.Auth
{
    public interface IAuthService
    {
        public Task<RegisterResponseDTO<UserDTO>> Register(RegisterDTO registerDTO);
        public Task<string> Login(LoginDTO loginDTO);
        public Task<UserDTO> ConfirmEmail(string userId, string token);
        public Task<UserDTO> SetPassword(SetPasswordDTO passwordDTO);
    }
}
