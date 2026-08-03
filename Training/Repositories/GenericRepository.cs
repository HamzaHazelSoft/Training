using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Training.DTOs;
using Training.Models;

namespace Training.Repositories
{
    public class GenericRepository : IGenericRepository
    {

        private readonly TrainingContext _context;
        private readonly IMapper _mapper;

        public GenericRepository(TrainingContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public PaginationResponse<Student> GetAll(int page)
        {
            int pageSize = 3;
            int offset = (page - 1) * pageSize;

            return new PaginationResponse<Student>
            {
                Data = _context.Students.OrderBy(s => s.RollNumber).Skip(offset).Take(pageSize).ToList(),
                Count = _context.Students.Count(),
                nextPage = _context.Students.Count() > offset + pageSize ? $"/api/Student?page={page + 1}" : null,
                previousPage = page > 1 ? $"/api/Student?page={page - 1}" : null
            };
        }

        public bool Add(Student student)
        {
            _context.Students.Add(student);
            return _context.SaveChanges() > 0;

        }

        public Student GetById(int id)
        {
            Student student = _context.Students.FirstOrDefault(x => x.RollNumber == id);
            return student;
        }

        public bool DeleteById(int id)
        {
            Student student = GetById(id);
            if (student == null)
            {
                return false;
            }
            _context.Remove(student);
            return _context.SaveChanges() > 0;
        }

        public bool UpdateById(int id, StudentDTO student)
        {
            var existingStudent = GetById(id);
            if (existingStudent == null)
            {
                return false;
            }
            _mapper.Map(student, existingStudent); //means copy from source:student to destination:existingStudent
            return _context.SaveChanges() > 0;
        }
    }
}
