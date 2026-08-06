

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Training.DTOs;
using Training.Helper;
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

        public async Task<PaginationResponse<User>> GetUsers(PaginationRequest paginationRequest)
        {

            //Checking Invalid credentials
            var property = typeof(User).GetProperty(paginationRequest.SortBy,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance); // Finds a property named "paginationRequest.SortBy"
                                                                                        // regardless of their casing .Restrict property should be public
                                                                                        // and should be instance omit static member
            if (property == null)
                throw new Exception();

            if (paginationRequest.CurrentPage <= 0)
                paginationRequest.CurrentPage = 1;

            if (paginationRequest.PageSize <= 0 || paginationRequest.PageSize > 50)
                paginationRequest.PageSize = 10;

            if (string.IsNullOrWhiteSpace(paginationRequest.SortBy))
                paginationRequest.SortBy = "Id";


            return await genericRepository.GetAsync(paginationRequest);
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
