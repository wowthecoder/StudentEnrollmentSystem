using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Models;
using StudentEnrollmentSystem.Services;

namespace StudentEnrollmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private ICourseServices _courseServices;

        public CoursesController(ICourseServices courseServices)
        {
            _courseServices = courseServices;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            try 
            {
                var output = await _courseServices.GetAllCourses();
                return Ok(output);
            }
            catch (NotFoundException nfex) 
            {
                return NotFound(nfex.Message);
            }
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(long id)
        {
            try
            {
                return await _courseServices.GetCourse(id);
            }
            catch (NotFoundException nfex) 
            {
                return NotFound(nfex.Message);
            }
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(long id, Course course)
        {
            try
            {
                await _courseServices.UpdateCourse(id, course);
                return NoContent();
            }
            catch (BadRequestException brex)
            {
                return BadRequest(brex.Message);
            }
            catch (NotFoundException nfex)
            {
                return NotFound();
            }
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            try
            {
                var newCourse = await _courseServices.CreateCourse(course);
                return CreatedAtAction("GetCourse", new { id = newCourse.Id }, newCourse);
            }
            catch (ProblemException pex)
            {
                return Problem(pex.Message);
            }
        }

        //Soft Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteCourse(long id)
        {
            try
            {
                await _courseServices.SoftDeleteCourse(id);
                return NoContent();
            }
            catch (NotFoundException nfex)
            {
                return NotFound(nfex.Message);
            }
        }

        //Hard Delete
        [HttpDelete("hard/{id}")]
        public async Task<IActionResult> HardDeleteCourse(long id)
        {
            try
            {
                await _courseServices.HardDeleteCourse(id);
                return NoContent();
            }
            catch (NotFoundException nfex)
            {
                return NotFound(nfex.Message);
            }
        }
    }
}
