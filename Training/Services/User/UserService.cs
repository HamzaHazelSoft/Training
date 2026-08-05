

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training.DTOs;
using Training.Models;
using Training.Repositories;

namespace Training.Services
{
    public class UserService : IUserService
    {

        private readonly IGenericRepository<User> genericRepository;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<User> genericRepository, IMapper mapper)
        {
            this.genericRepository = genericRepository;
            this._mapper = mapper;
        }
        
        public async Task<bool> AddUser(UserDTO userDto)
        {
            if(userDto == null)
                return false;

            var user = _mapper.Map<User>(userDto); //Map the UserDTO to a User entity
            return await genericRepository.AddAsync(user);
        }

        public async Task<User> GetUserById(string id)
        {
            if(string.IsNullOrEmpty(id))
                return null;
            
            return await genericRepository.GetByIdAsync(id);
        }

        // Deletes a user by Id
        public async Task<bool> DeleteUserById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            return await genericRepository.DeleteByIdAsync(id);
        }
        public async Task<bool> UpdateUserById(string id, UserDTO userDto)
        {
            if (string.IsNullOrEmpty(id) || userDto == null)
                return false;

            var user = await genericRepository.GetByIdAsync(id); //Get the user by Id

            if (user == null)
                return false;

            _mapper.Map(userDto, user); //Mapper copies all related fields from userDto to user

            return await genericRepository.UpdateAsync(user);
        }
    }
}
