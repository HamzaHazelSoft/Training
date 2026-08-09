using UserManagementSystem.DTOs;
using UserManagementSystem.Models;

namespace UserManagementSystem.Services
{
    public interface IUserService
    {
        public Task<PaginationResponseDTO<UserDTO>> GetUsers(PaginationRequestDTO paginationRequest);
        public Task<UserDTO> GetUserById(string id);
        public Task<bool> DeleteUserById(string id);
        public Task<bool> UpdateUserById(string id, UserDTO userDto);

    }
}
