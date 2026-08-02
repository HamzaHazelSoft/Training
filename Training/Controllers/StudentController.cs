using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training.DTOs;
using Training.Models;
using Training.Services;

namespace Training.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public IActionResult GetAll(int page=1)
        {
            var students = _studentService.GetAllStudents(page);
            return Ok(students); 
        }

        [HttpPost]
        public IActionResult Create([FromBody]Student student)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool status = _studentService.AddStudent(student);
            return Ok($"Student  {(status ? "added successfully" : "could not be added")}");
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Student student = _studentService.GetStudentById(id);
            return student != null ? Ok(student) : Ok(new { message = "Student not found" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool status = _studentService.DeleteStudentById(id);
            return status == true ? Ok(new { message = $"Student {id} deleted successfully" }) : Ok(new { message = "Student not found" });
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, StudentDTO student)
        {
            bool status = _studentService.UpdateStudentById(id,student);
            return status == true ? Ok(new { message = $"Student {id} updated successfully" }) : Ok(new { message = "Student not found" });
        }
    }
}
