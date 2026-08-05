using Training.DTOs;
using Training.Helper;
using Training.Models;

namespace Training.Services
{
    public interface IUserService
    {
        public Task<PaginationResponse<User>> GetAllUsers(PaginationRequest paginationRequest);
        public Task<bool> AddUser(UserDTO userDto);
        public Task<User> GetUserById(string id);
        public Task<bool> DeleteUserById(string id);
        public Task<bool> UpdateUserById(string id, UserDTO userDto);

    }
}
