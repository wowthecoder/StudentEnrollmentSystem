using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentEnrollmentSystem.Authentication;
using StudentEnrollmentSystem.Models;
using StudentEnrollmentSystem.Services;

namespace StudentEnrollmentSystem.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IEnrollmentServices _enrollServices;

        public EnrollmentsController(IEnrollmentServices enrollServices)
        {
            _enrollServices = enrollServices;
        }

        // GET: api/Enrollments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enrollment>>> GetEnrollments()
        {
            try
            {
                var output = await _enrollServices.GetAllEnrollments();
                return Ok(output);
            }
            catch (NotFoundException nfex)
            {
                return NotFound(nfex.Message);
            }
        }

        // GET: api/Enrollments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Enrollment>> GetEnrollment(long id)
        {
            try
            {
                return await _enrollServices.GetEnrollment(id);
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

        // PATCH: api/Enrollments/approve/5
        [HttpPatch("approve/{id}")]
        public async Task<IActionResult> ApproveEnrollment(long id)
        {
            try
            {
                await _enrollServices.ApproveEnrollment(id);
                return NoContent();
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

        // PATCH: api/Enrollments/reject/5
        [HttpPatch("reject/{id}")]
        public async Task<IActionResult> RejectEnrollment(long id)
        {
            try
            {
                await _enrollServices.RejectEnrollment(id);
                return NoContent();
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

        // POST: api/Enrollments
        /*[HttpPost]
        public async Task<ActionResult<Enrollment>> PostEnrollment(Enrollment enrollment)
        {
          if (_context.Enrollments == null)
          {
              return Problem("Entity set 'StudentEnrollContext.Enrollments'  is null.");
          }
            _context.Enrollments.Add(enrollment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEnrollment", new { id = enrollment.Id }, enrollment);
        }*/

        // DELETE: api/Enrollments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnrollment(long id)
        {
            try
            {
                await _enrollServices.HardDeleteEnrollment(id);
                return NoContent();
            }
            catch (NotFoundException nfex)
            {
                return NotFound(nfex.Message);
            }
        }
    }
}
