using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using StudentEnrollmentSystem.Authentication;
using System.Text;
using StudentEnrollmentSystem.Services;
using Microsoft.AspNetCore.Authorization;

namespace StudentEnrollmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private IAuthServices _authServices;

        public AuthenticateController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var token = await _authServices.Login(model);
                return Ok(token);
            }
            catch (UnauthorizedException unex)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("register-student")]
        public async Task<IActionResult> RegisterStudent([FromBody] RegisterModel model)
        {
            var status = await _authServices.RegisterStudent(model);
            if (status.Status.Equals("Error"))
            {
                return StatusCode(StatusCodes.Status500InternalServerError, status);
            }
            return Ok(status);
        }

        [HttpPost]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            try
            {
                var status = await _authServices.ChangePassword(model);
                if (status.Status.Equals("Error"))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, status);
                }
                return Ok(status);
            }
            catch (NotFoundException nfex)
            {
                return NotFound(nfex.Message);
            }
        }
    }
}
