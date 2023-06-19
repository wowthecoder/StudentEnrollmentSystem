using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Models;
using StudentEnrollmentSystem.Services;

namespace StudentEnrollmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private IStudentServices _studentservice;

        public StudentsController(IStudentServices studentServices)
        {
            _studentservice = studentServices;
        }
        
        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            var output = await _studentservice.GetAllStudents();
            return Ok(output);
        }
        
        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDTO>> GetStudent(long id)
        {
            try
            {
                return await _studentservice.GetStudent(id);
            }
            catch (NotFoundException nfex) 
            {
                return NotFound(nfex.Message);
            }
        }

        //GET: view self enrollments
        [HttpGet("viewenroll/{id}")]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetSelfEnrollments(long id)
        {
            try
            {
                var output = await _studentservice.GetSelfEnrollments(id);
                return Ok(output);
            }
            catch (NotFoundException nfex) 
            {
                return NotFound(nfex.Message);
            }
        }

        // PUT: To update student profile
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(long id, StudentDTO studentDTO)
        {
            try
            {
                await _studentservice.UpdateStudent(id, studentDTO);
                return NoContent();
            }
            catch (BadRequestException brex)
            {
                return BadRequest(brex.Message);
            }
            catch (NotFoundException nfex)
            {
                return NotFound(nfex.Message);
            }
        }

        // POST: to create student profile
        [HttpPost]
        public async Task<ActionResult<StudentDTO>> PostStudent(StudentDTO studentDTO)
        {
            try
            {
                //student is a DTO object
                var student = await _studentservice.CreateStudentProfile(studentDTO);
                return CreatedAtAction("GetStudent", new { id = student.Id }, student);
            }
            catch (ProblemException pex)
            {
                return Problem(pex.Message);
            }
            catch (BadRequestException brex)
            {
                return BadRequest(brex.Message);
            }
        }

        //To enroll: api/students/enroll
        [HttpPatch("enroll/{studentId}/{courseId}")]
        public async Task<ActionResult<StudentDTO>> EnrollCourse(long studentId, long courseId)
        {
            try
            {
                var student = await _studentservice.EnrollCourse(studentId, courseId);
                return Ok(student);
            }
            catch (NotFoundException nfex)
            {
                return NotFound(nfex.Message);
            }
            catch (ProblemException pex)
            {
                return Problem(pex.Message);
            }
        }

        [HttpPatch("withdraw/{studentId}/{courseId}")]
        public async Task<ActionResult<Student>> WithdrawCourse(long studentId, long courseId)
        {
            try
            {
                var student = await _studentservice.WithdrawCourse(studentId, courseId);
                return Ok(student);
            }
            catch (NotFoundException nfex)
            {
                return NotFound(nfex.Message);
            }
        }
        
        //Soft Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteStudent(long id)
        {
            try
            {
                await _studentservice.SoftDeleteProfile(id);
                return NoContent();
            }
            catch (NotFoundException nfex)
            {
                return NotFound(nfex.Message);
            }
        }

        //Hard Delete
        [HttpDelete("hard/{id}")]
        public async Task<IActionResult> HardDeleteStudent(long id)
        {
            try
            {
                await _studentservice.HardDeleteProfile(id);
                return NoContent();
            }
            catch (NotFoundException nfex)
            {
                return NotFound(nfex.Message);
            }
        } 
        
    }
}
