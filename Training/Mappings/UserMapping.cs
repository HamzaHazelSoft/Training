using AutoMapper;
using UserManagementSystem.DTOs;
using UserManagementSystem.Models;

namespace UserManagementSystem.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<UserDTO, User>()
                .ForMember(x => x.UserName, opt => opt.Ignore());

            CreateMap<User, UserDTO>(); //means copying data from user to UserDTO

        }
    }
}
