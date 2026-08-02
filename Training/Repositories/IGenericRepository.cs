using Training.DTOs;
using Training.Models;

namespace Training.Repositories
{
    public interface IGenericRepository
    {
        public IEnumerable<Student> GetAll(int page);
        public bool Add(Student student);
        public Student GetById(int id);
        public bool DeleteById(int id);
        public bool UpdateById(int id, StudentDTO student);

    }
}
