using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Service.Dtos;
using Service.Exceptions;
using Service.Services;
using Service.Services.Interfaces;

namespace CourseApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService student)
        {
            _studentService = student;
        }

        [HttpPost("")]
        public IActionResult Create([FromForm] StudentCreateDto student)
        {
            return StatusCode(201, new { id = _studentService.Create(student) });
        }

        [HttpGet("")]
        public ActionResult<List<StudentGetDto>> GetAll(int page = 1, int size = 10)
        {
            return Ok(_studentService.GetAll(page,size));
        }


        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            var data = _studentService.GetById(id);
            return StatusCode(200, data);
        }

        [HttpPut("{id}")]
        public ActionResult Update(StudentUpdateDto updateDto, int id)
        {
            _studentService.Update(updateDto, id);
            return NoContent();
        }



        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _studentService.Delete(id);
            return NoContent();
        }
    }
}
