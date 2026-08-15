

using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Serilog.Context;
using System.Reflection.Metadata;
using UserManagementSystem.DTOs;
using UserManagementSystem.Helper;
using UserManagementSystem.Models;
using UserManagementSystem.Repositories;
using UserManagementSystem.Services.Mail;
using static UserManagementSystem.Helper.Constant;

namespace UserManagementSystem.Services.Users.Implementation
{
    public class UserService : IUserService
    {

        private readonly IGenericRepository<User> _genericRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;
        public UserService(IGenericRepository<User> genericRepository,
            IMapper mapper,UserManager<User> userManager,
            IMailService mailService, RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,ILogger<UserService> logger
            )
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _userManager = userManager;
            _mailService = mailService;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }
        //UserService private helper
        private async Task<User> GetUser(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            return await _genericRepository.GetByIdAsync(id);
        }

        //Pagination request handles here
        public async Task<PaginationResponseDTO<UserDTO>> GetUsers(PaginationRequestDTO paginationRequest)
        {
            _logger.LogInformation("User retrieval started");
            if (paginationRequest.CurrentPage <= 0)
                throw new Exception(MessageConstants.InvalidCurrentPage);

            if (paginationRequest.PageSize <= 0 )
                throw new Exception(MessageConstants.InvalidPageSize);


            var result =  await _genericRepository.GetAsync(paginationRequest);

            _logger.LogInformation("Users retrieved successfully. Total: {Total}, Page: {CurrentPage}", result.Total, result.CurrentPage);

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
            using var _uid = LogContext.PushProperty("UserId", id);
            _logger.LogInformation("User retrieval started.");

            var user = await GetUser(id);
            if (user == null)
            {
                _logger.LogWarning("User retrieval failed - user not found.");
                return null;
            }

            _logger.LogInformation("User retrieved successfully.");
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<bool> DeleteUserById(string id)
        {
            using var _uid = LogContext.PushProperty("UserId", id);
            _logger.LogInformation("User deletion started.");

            var user = await GetUser(id);

            if (user == null)
            {
                _logger.LogWarning("User deletion failed - user not found.");
                return false;
            }

            _genericRepository.Delete(user);
            _logger.LogInformation("User deleted successfully");
            return await _genericRepository.SaveChangesAsync();
        }

        public async Task<bool> UpdateUserById(string id, RegisterDTO registerDto)
        {
            using var _uid = LogContext.PushProperty("UserId", id);
            _logger.LogInformation("User update started. UserName: {UserName}", registerDto.UserName);

            var user = await GetUser(id);

            if (user == null) throw new Exception(MessageConstants.UserNotFound);

            //Validating UserName
            await ValidateUsernameAsync(id, registerDto.UserName);

            bool emailChanged = await HandleEmailChangeAsync(user, id, registerDto.Email);

            //Updating User
            UpdateUserEntity(user, registerDto, id);

            //Updating Roles
            await UpdateUserRolesAsync(user, registerDto.Roles);

            await _genericRepository.SaveChangesAsync();

            if (emailChanged)
            {
                _logger.LogInformation("User email changed. NewEmail: {Email}",user.Email);
                _ = _mailService.SendConfirmationEmailAsync(user);
            }

            _logger.LogInformation("User updated successfully");

            return true;
        }

        // Checks whether the new username is already used by another user.
        private async Task ValidateUsernameAsync(string userId, string username)
        {
            var existingNameUser = await _userManager.FindByNameAsync(username);

            if (existingNameUser != null && !string.Equals(existingNameUser.Id, userId, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception(MessageConstants.UsernameAlreadyTaken);
            }
        }

        // Checks email change, validates its uniqueness, and resets confirmation status.
        private async Task<bool> HandleEmailChangeAsync(User user,string userId,string newEmail)
        {
            bool emailChanged = !string.Equals(user.Email,newEmail,StringComparison.OrdinalIgnoreCase);

            if (!emailChanged)
                return false;

            var existingEmailUser = await _userManager.FindByEmailAsync(newEmail);

            if (existingEmailUser != null && existingEmailUser.Id != userId)
            {
                throw new Exception(MessageConstants.UserAlreadyExists);
            }

            user.EmailConfirmed = false;

            return true;
        }

        // Maps the DTO values to the user and updates normalized username/email.
        private void UpdateUserEntity(User user,RegisterDTO registerDto, string id)
        {
            registerDto.Id = id;

            _mapper.Map(registerDto, user);

            user.NormalizedUserName =_userManager.NormalizeName(registerDto.UserName);
            user.NormalizedEmail = _userManager.NormalizeEmail(registerDto.Email);
        }

        // Synchronizes the user's roles by validating the provided role IDs,
        // then removing roles that are no longer assigned and adding newly assigned roles.
        private async Task UpdateUserRolesAsync(User user, List<string> roleIds)
        {
            var currentRoles = await _userManager.GetRolesAsync(user);

            var newRoles = new List<string>();

            foreach (var roleId in roleIds)
            {
                var role = await _roleManager.FindByIdAsync(roleId);

                if (role == null)
                {
                    _logger.LogWarning("User role update failed because role was not found. UserId: {UserId}, RoleId: {RoleId}",user.Id,roleId);
                    throw new Exception(MessageConstants.InvalidRolesEntered);
                }

                newRoles.Add(role.Name!);
            }

            var rolesToRemove = currentRoles.Except(newRoles).ToList();
            var rolesToAdd = newRoles.Except(currentRoles).ToList();

            if (rolesToRemove.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            }

            if (rolesToAdd.Any())
            {
                await _userManager.AddToRolesAsync(user, rolesToAdd);
            }
        }
    }
}
