

using Microsoft.AspNetCore.Mvc;
using Training.DTOs;
using Training.Models;
using Training.Repositories;

namespace Training.Services
{
    public class StudentService : IStudentService
    {

        private readonly IGenericRepository _genericRepository;
        public StudentService(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }   
        public PaginationResponse<Student> GetAllStudents(int page)
        {
            //business logic
            return _genericRepository.GetAll(page);

        }
        public bool AddStudent(Student student)
        {
            if(student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }
            return _genericRepository.Add(student);
        }

        public Student GetStudentById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0.", nameof(id));
            }
            var result = _genericRepository.GetById(id);
            return result; 
        }

        public bool DeleteStudentById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0.", nameof(id));
            }
            return _genericRepository.DeleteById(id);
        }

        public bool UpdateStudentById(int id, StudentDTO student)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Id must be greater than 0.", nameof(id));
            }
            if(student == null)
            {
                throw new KeyNotFoundException("Student not found");
            }

            return _genericRepository.UpdateById(id, student);

        }
    }
}
