using JobTracking.Domain.DTOs;
using JobTracking.Application.Contracts;
using JobTracking.DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using JobTracking.Domain.DTOs.Request;
using JobTracking.Domain.DTOs.Response;

namespace JobTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // All application endpoints require authentication
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("User ID not found in token.");
        }

        private string GetCurrentUserRole()
        {
            return User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
        }

        // GET: api/Applications/my (for regular users)
        [HttpGet("my")]
        [Authorize(Roles = "User")] // Only users can get their own applications
        public async Task<ActionResult<IEnumerable<ApplicationResponseDTO.ApplicationDto>>> GetMyApplications()
        {
            var userId = GetCurrentUserId();
            var applications = await _applicationService.GetApplicationsForUser(userId);
            return Ok(applications);
        }

        // GET: api/Applications (for admins)
        [HttpGet]
        [Authorize(Roles = "Admin")] // Only admins can get all applications
        public async Task<ActionResult<IEnumerable<ApplicationResponseDTO.ApplicationDto>>> GetAllApplications()
        {
            var applications = await _applicationService.GetAllApplications();
            return Ok(applications);
        }

        // POST: api/Applications (for regular users)
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> SubmitApplication([FromBody] CreateApplicationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var application = await _applicationService.SubmitApplication(userId, dto);

            if (application == null)
            {
                // This could be due to job not found or user already applied
                return BadRequest("Failed to submit application. You might have already applied for this job or the job listing does not exist.");
            }

            return CreatedAtAction(nameof(GetMyApplications), new { }, application); // Return to my applications list
        }

        // PUT: api/Applications/5/status (for admins to update status)
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateApplicationStatus(int id, [FromBody] UpdateApplicationStatusDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedApplication = await _applicationService.UpdateApplicationStatus(id, dto);
            if (updatedApplication == null)
            {
                return NotFound($"Application with ID {id} not found or invalid status provided.");
            }
            return Ok(updatedApplication);
        }
    }
}