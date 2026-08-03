using Training.DTOs;
using Training.Models;

namespace Training.Services
{
    public interface IUserService
    {
        public IEnumerable<User> GetAllUsers();
        public bool AddUser(User User);
        public User GetUserById(string id);
        public bool DeleteUserById(string id);
        public bool UpdateUserById(string id, UserDTO User);

    }
}
