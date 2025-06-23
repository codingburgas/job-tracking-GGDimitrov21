using JobTracking.Domain.DTOs;
using JobTracking.Application.Contracts;
using JobTracking.DataAccess.Models;
using JobTracking.DataAccess.Data;
using JobTracking.DataAccess.Persistance;
using JobTracking.Domain.DTOs.Request;
using JobTracking.Domain.DTOs.Response;
using Microsoft.EntityFrameworkCore;

namespace JobTracking.Application.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly ApplicationDbContext _context;

        public ApplicationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationResponseDTO.ApplicationDto>> GetApplicationsForUser(int userId)
        {
            return await _context.Applications
                .Where(a => a.UserId == userId)
                .Include(a => a.JobListing)
                .Select(a => new ApplicationResponseDTO.ApplicationDto
                {
                    Id = a.Id,
                    JobListingId = a.JobListingId,
                    JobTitle = a.JobListing.Title,
                    CompanyName = a.JobListing.Company,
                    UserId = a.UserId,
                    Username = a.User.Username, // Assuming User is loaded or accessible via navigation
                    ApplicationDate = a.ApplicationDate,
                    Status = a.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ApplicationResponseDTO.ApplicationDto>> GetAllApplications()
        {
            return await _context.Applications
                .Include(a => a.JobListing)
                .Include(a => a.User)
                .Select(a => new ApplicationResponseDTO.ApplicationDto
                {
                    Id = a.Id,
                    JobListingId = a.JobListingId,
                    JobTitle = a.JobListing.Title,
                    CompanyName = a.JobListing.Company,
                    UserId = a.UserId,
                    Username = a.User.Username,
                    ApplicationDate = a.ApplicationDate,
                    Status = a.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<ApplicationResponseDTO.ApplicationDto?> SubmitApplication(int userId, CreateApplicationDto dto)
        {
            // Check if job listing exists
            var jobListing = await _context.JobListings.FindAsync(dto.JobListingId);
            if (jobListing == null)
            {
                Console.WriteLine($"Job listing with ID {dto.JobListingId} not found.");
                return null;
            }

            // Check if user exists
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                Console.WriteLine($"User with ID {userId} not found.");
                return null;
            }

            // Check if user has already applied for this job
            if (await HasUserApplied(userId, dto.JobListingId))
            {
                Console.WriteLine($"User {userId} has already applied for job listing {dto.JobListingId}.");
                return null;
            }

            var newApplication = new DataAccess.Models.Application
            {
                JobListingId = dto.JobListingId,
                UserId = userId,
                ApplicationDate = DateTime.UtcNow,
                Status = ApplicationStatus.Submitted // Default status
            };

            _context.Applications.Add(newApplication);
            await _context.SaveChangesAsync();

            // Populate DTO with related data for response
            return new ApplicationResponseDTO.ApplicationDto
            {
                Id = newApplication.Id,
                JobListingId = newApplication.JobListingId,
                JobTitle = jobListing.Title,
                CompanyName = jobListing.Company,
                UserId = newApplication.UserId,
                Username = user.Username,
                ApplicationDate = newApplication.ApplicationDate,
                Status = newApplication.Status.ToString()
            };
        }

        public async Task<ApplicationResponseDTO.ApplicationDto?> UpdateApplicationStatus(int applicationId, UpdateApplicationStatusDto dto)
        {
            var application = await _context.Applications
                .Include(a => a.JobListing)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == applicationId);

            if (application == null)
            {
                Console.WriteLine($"Application with ID {applicationId} not found.");
                return null;
            }

            // Basic validation
            if (string.IsNullOrWhiteSpace(dto.Status))
            {
                Console.WriteLine("Status is required for updating application status.");
                return null;
            }

            if (Enum.TryParse(dto.Status, true, out ApplicationStatus newStatus))
            {
                application.Status = newStatus;
            }
            else
            {
                Console.WriteLine($"Invalid status provided: {dto.Status}");
                return null; // Or throw an exception for invalid status
            }

            await _context.SaveChangesAsync();

            return new ApplicationResponseDTO.ApplicationDto
            {
                Id = application.Id,
                JobListingId = application.JobListingId,
                JobTitle = application.JobListing.Title,
                CompanyName = application.JobListing.Company,
                UserId = application.UserId,
                Username = application.User.Username,
                ApplicationDate = application.ApplicationDate,
                Status = application.Status.ToString()
            };
        }

        public async Task<bool> HasUserApplied(int userId, int jobListingId)
        {
            return await _context.Applications
                .AnyAsync(a => a.UserId == userId && a.JobListingId == jobListingId);
        }
    }
}