

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training.DTOs;
using Training.Models;

namespace Training.Services
{
    public class UserService : IUserService
    {

        private readonly TrainingContext _context;
        private readonly IMapper _mapper;

        public UserService(TrainingContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<bool> AddUser(User user)
        {
            _context.Users.Add(user); // Not yet saved . It bind flag of Added
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> GetUserById(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(id)); // Returns the first user that matches the Id,
                                                                                 // or null if no user is found
        }

        // Deletes a user by Id
        public async Task<bool> DeleteUserById(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(id)); //Returns the first user that matches the Id,
                                                                                       //or null if no user is found

            if (user == null)
                return false;

            _context.Users.Remove(user); // Now it modifies the flag of the user to be deleted
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> UpdateUserById(string id, UserDTO userDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (user == null)
                return false;

            _mapper.Map(userDto, user); //Mapper copies all related fields from userDto to user

            _context.Users.Update(user); //Now it modifies the flag of the user to be updated

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
