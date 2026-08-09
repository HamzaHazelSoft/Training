

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;
using UserManagementSystem.DTOs;
using UserManagementSystem.Models;
using UserManagementSystem.Repositories;
using static UserManagementSystem.Helper.Constant;

namespace UserManagementSystem.Services.Users.Implementation
{
    public class UserService : IUserService
    {

        private readonly IGenericRepository<User> _genericRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<User> genericRepository,
            IMapper mapper,
            UserManager<User> userManager)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _userManager = userManager;
        }
        //UserService private helper
        private async Task<User> GetUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            return await _genericRepository.GetByIdAsync(id);
        }

        public async Task<PaginationResponseDTO<UserDTO>> GetUsers(PaginationRequestDTO paginationRequest)
        {

            if (paginationRequest.CurrentPage <= 0)
                throw new ArgumentException(MessageConstants.InvalidCurrentPage);

            if (paginationRequest.PageSize <= 0 )
                throw new ArgumentException(MessageConstants.InvalidPageSize);


            var result =  await _genericRepository.GetAsync(paginationRequest);

            return new PaginationResponseDTO<UserDTO>
            {
                Total = result.Total,
                PageSize = result.PageSize,
                CurrentPage = result.CurrentPage,
                Items = _mapper.Map<List<UserDTO>>(result.Items)
            };
        }  

        public async Task<UserDTO> GetUserById(string id)
        {
            var user = await GetUser(id);

            if (user == null)
                return null;

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<bool> DeleteUserById(string id)
        {
            var user = await GetUser(id);

            if (user == null)
                return false;

            return await _genericRepository.DeleteAsync(user);
        }
        public async Task<bool> UpdateUserById(string id, UserDTO userDto)
        {
            
            if (userDto == null)
                return false;

            var user = await GetUser(id);
            if (user == null)
                return false;

            var existingUser = await _userManager.FindByNameAsync(userDto.UserName);

            if (existingUser != null && existingUser.Id != id)
                throw new InvalidOperationException(MessageConstants.UsernameAlreadyTaken);

            userDto.Id = id;

            _mapper.Map(userDto, user);
            user.UserName = userDto.UserName;
            user.NormalizedUserName = _userManager.NormalizeName(userDto.UserName);

            return await _genericRepository.UpdateAsync(user);

        }
    }
}
