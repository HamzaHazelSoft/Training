using Training.DTOs;
using Training.Models;

namespace Training.Services
{
    public interface IUserService
    {
        public Task<bool> AddUser(User User);
        public Task<User> GetUserById(string id);
        public Task<bool> DeleteUserById(string id);
        public Task<bool> UpdateUserById(string id, UserDTO User);

    }
}
