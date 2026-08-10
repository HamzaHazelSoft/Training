

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;
using UserManagementSystem.DTOs;
using UserManagementSystem.Models;
using UserManagementSystem.Repositories;
using static UserManagementSystem.Helper.Constant;
using UserManagementSystem.Services.Mail;

namespace UserManagementSystem.Services.Users.Implementation
{
    public class UserService : IUserService
    {

        private readonly IGenericRepository<User> _genericRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;

        public UserService(IGenericRepository<User> genericRepository,
            IMapper mapper,UserManager<User> userManager,
            IMailService mailService)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _userManager = userManager;
            _mailService = mailService;
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

            _genericRepository.Delete(user);

            int affectedRows = await _genericRepository.SaveChangesAsync();
            return affectedRows > 0;
        }
        public async Task<bool> UpdateUserById(string id, UserDTO userDto)
        {
            if (userDto == null)
                return false;

            var user = await GetUser(id);

            if (user == null)
                return false;

            await ValidateUsernameAsync(id, userDto.UserName);

            bool emailChanged = await HandleEmailChangeAsync(user, id, userDto.Email);

            UpdateUserEntity(user, userDto, id);

            await _genericRepository.SaveChangesAsync();

            if (emailChanged)
            {
                _ = _mailService.SendConfirmationEmailAsync(user);
            }

            return true;
        }
        private async Task ValidateUsernameAsync(string userId, string username)
        {
            var existingNameUser = await _userManager.FindByNameAsync(username);

            if (existingNameUser != null && !string.Equals(existingNameUser.Id, userId, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(MessageConstants.UsernameAlreadyTaken);
            }
        }
        private async Task<bool> HandleEmailChangeAsync(User user,string userId,string newEmail)
        {
            bool emailChanged = !string.Equals(user.Email,newEmail,StringComparison.OrdinalIgnoreCase);

            if (!emailChanged)
                return false;

            var existingEmailUser = await _userManager.FindByEmailAsync(newEmail);

            if (existingEmailUser != null && existingEmailUser.Id != userId)
            {
                throw new InvalidOperationException(MessageConstants.UserAlreadyExists);
            }

            user.EmailConfirmed = false;

            return true;
        }

        private void UpdateUserEntity(User user,UserDTO userDto,string id)
        {
            userDto.Id = id;

            _mapper.Map(userDto, user);

            user.UserName = userDto.UserName;
            user.NormalizedUserName =_userManager.NormalizeName(userDto.UserName);

            user.Email = userDto.Email;
            user.NormalizedEmail = _userManager.NormalizeEmail(userDto.Email);
        }
    }
}
