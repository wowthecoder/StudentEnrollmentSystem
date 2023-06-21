using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentEnrollmentSystem.Authentication;
using StudentEnrollmentSystem.Services;

namespace StudentEnrollmentSystem.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private IAdminServices _adminServices;
        public AdminController(IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var status = await _adminServices.RegisterAdmin(model);
            if (status.Status.Equals("Error"))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, status);
            }
            return Ok(status);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminDTO>>> GetAllAdmins()
        {
            var output = await _adminServices.GetAllAdmins();
            return Ok(output);
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<ActionResult<AdminDTO>> GetAdmin(string username)
        {
            try
            {
                var admin = await _adminServices.GetAdmin(username);
                return Ok(admin);
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

        [HttpPatch]
        public async Task<IActionResult> UpdateAdmin(AdminDTO admin)
        {
            try
            {
                var status = await _adminServices.UpdateAdmin(admin);
                if (status.Status.Equals("Error"))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, status);
                }
                return Ok(status);
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

        [HttpDelete]
        [Route("{username}")]
        public async Task<IActionResult> HardDeleteAdmin(string username)
        {
            try
            {
                var status = await _adminServices.HardDeleteAdmin(username);
                if (status.Status.Equals("Error"))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, status);
                }
                return Ok(status);
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
    }
}
