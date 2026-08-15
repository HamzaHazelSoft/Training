using AutoMapper;
using UserManagementSystem.DTOs;
using UserManagementSystem.Models;

namespace UserManagementSystem.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<UserDTO, User>().ReverseMap();  //means copying data from user to UserDTO and vice versa
            CreateMap<RegisterDTO, User>().ReverseMap();
        }
    }
}
