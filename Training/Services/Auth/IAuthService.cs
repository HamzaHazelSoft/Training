using Microsoft.AspNetCore.Mvc;
using Training.DTOs;
using Training.Helper;
using Training.Models;

namespace Training.Services.Auth
{
    public interface IAuthService
    {
        public Task<Response<User>> Register(RegisterDTO registerDTO);
        public Task<Response<string>> Login(LoginDTO loginDTO);
        public Task<Response<User>> ConfirmEmail(string userId, string token);
        public Task<Response<User>> SetPassword(SetPasswordDTO passwordDTO);
    }
}
