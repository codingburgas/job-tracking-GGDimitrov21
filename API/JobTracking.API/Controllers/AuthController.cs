using JobTracking.Domain.DTOs;
using JobTracking.Application.Contracts;
using JobTracking.Domain.DTOs.Request;
using Microsoft.AspNetCore.Mvc;

namespace JobTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.Register(request);
            if (response == null)
            {
                return Conflict("Registration failed. Username might already exist or invalid data.");
            }
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.Login(request);
            if (response == null)
            {
                return Unauthorized("Invalid username or password.");
            }
            return Ok(response);
        }
    }
}