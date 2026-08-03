using AutoMapper;
using Training.DTOs;
using Training.Models;

namespace Training.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<UserDTO,User>(); //means it can copy data from UserDTO to User. Note: Matching Data
        }
    }
}
