using JobTracking.Application.Contracts;
using JobTracking.Domain.DTOs.Request;
using JobTracking.Domain.DTOs.Request.Create;
using JobTracking.Domain.DTOs.Request.Update;
using JobTracking.Domain.DTOs.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobTracking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobListingsController : ControllerBase
    {
        private readonly IJobListingService _jobListingService;

        public JobListingsController(IJobListingService jobListingService)
        {
            _jobListingService = jobListingService;
        }

        // GET: api/JobListings (accessible by all authenticated users)
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<JobListingResponseDTO.JobListingDto>>> GetJobListings()
        {
            var jobListings = await _jobListingService.GetAllJobListings();
            return Ok(jobListings);
        }

        // GET: api/JobListings/5 (accessible by all authenticated users)
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<JobListingResponseDTO.JobListingDto>> GetJobListing(int id)
        {
            var jobListing = await _jobListingService.GetJobListingById(id);
            if (jobListing == null)
            {
                return NotFound();
            }
            return Ok(jobListing);
        }

        // POST: api/JobListings (Admin only)
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<JobListingResponseDTO.JobListingDto>> CreateJobListing([FromBody] CreateJobListingDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var jobListing = await _jobListingService.CreateJobListing(dto);
            if (jobListing == null)
            {
                return BadRequest("Could not create job listing. Please check input data.");
            }
            return CreatedAtAction(nameof(GetJobListing), new { id = jobListing.Id }, jobListing);
        }

        // PUT: api/JobListings/5 (Admin only)
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateJobListing(int id, [FromBody] UpdateJobListingDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedJobListing = await _jobListingService.UpdateJobListing(id, dto);
            if (updatedJobListing == null)
            {
                return NotFound($"Job Listing with ID {id} not found or invalid data provided.");
            }
            return Ok(updatedJobListing);
        }

        // DELETE: api/JobListings/5 (Admin only)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteJobListing(int id)
        {
            var result = await _jobListingService.DeleteJobListing(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}