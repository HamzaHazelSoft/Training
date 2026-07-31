using AutoMapper;
using Training.DTOs;
using Training.Models;

namespace Training.Mappings
{
    public class StudentMapping : Profile
    {
        public StudentMapping()
        {
            CreateMap<StudentDTO,Student>(); //means it can copy data from StudentDTO to Student
        }
    }
}
