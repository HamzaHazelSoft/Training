using Microsoft.AspNetCore.Mvc;
using Training.DTOs;
using Training.Helper;

namespace Training.Services.Auth
{
    public interface IAuthService
    {
        public Task<bool> Register(RegisterDTO registerDTO);

        public Task<Response<string>> Login(LoginDTO loginDTO);
    }
}
