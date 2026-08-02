using Training.DTOs;
using Training.Models;

namespace Training.Services
{
    public interface IStudentService
    {
        public IEnumerable<Student> GetAllStudents();
  
        public bool AddStudent(Student student);
        public Student GetStudentById(int id);
        public bool DeleteStudentById(int id);
        public bool UpdateStudentById(int id, StudentDTO student);

    }
}
