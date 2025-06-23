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
    public class JobListingService : IJobListingService
    {
        private readonly ApplicationDbContext _context;

        public JobListingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<JobListingResponseDTO.JobListingDto>> GetAllJobListings()
        {
            return await _context.JobListings
                .Select(jl => new JobListingResponseDTO.JobListingDto
                {
                    Id = jl.Id,
                    Title = jl.Title,
                    Company = jl.Company,
                    Description = jl.Description,
                    PublishDate = jl.PublishDate,
                    Status = jl.Status.ToString()
                })
                .ToListAsync();
        }

        public async Task<JobListingResponseDTO.JobListingDto?> GetJobListingById(int id)
        {
            var jobListing = await _context.JobListings.FindAsync(id);
            if (jobListing == null) return null;

            return new JobListingResponseDTO.JobListingDto
            {
                Id = jobListing.Id,
                Title = jobListing.Title,
                Company = jobListing.Company,
                Description = jobListing.Description,
                PublishDate = jobListing.PublishDate,
                Status = jobListing.Status.ToString()
            };
        }

        public async Task<JobListingResponseDTO.JobListingDto?> CreateJobListing(CreateJobListingDto dto)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Company) || string.IsNullOrWhiteSpace(dto.Description))
            {
                Console.WriteLine("Title, Company, and Description are required for creating a job listing.");
                return null;
            }

            var newJob = new JobListing
            {
                Title = dto.Title,
                Company = dto.Company,
                Description = dto.Description,
                PublishDate = DateTime.UtcNow,
                Status = JobStatus.Active // Default status is Active
            };

            _context.JobListings.Add(newJob);
            await _context.SaveChangesAsync();

            return new JobListingResponseDTO.JobListingDto
            {
                Id = newJob.Id,
                Title = newJob.Title,
                Company = newJob.Company,
                Description = newJob.Description,
                PublishDate = newJob.PublishDate,
                Status = newJob.Status.ToString()
            };
        }

        public async Task<JobListingResponseDTO.JobListingDto?> UpdateJobListing(int id, UpdateJobListingDto dto)
        {
            var jobListing = await _context.JobListings.FindAsync(id);
            if (jobListing == null) return null;

            // Basic validation
            if (string.IsNullOrWhiteSpace(dto.Title) || string.IsNullOrWhiteSpace(dto.Company) || string.IsNullOrWhiteSpace(dto.Description) || string.IsNullOrWhiteSpace(dto.Status))
            {
                Console.WriteLine("Title, Company, Description, and Status are required for updating a job listing.");
                return null;
            }

            jobListing.Title = dto.Title;
            jobListing.Company = dto.Company;
            jobListing.Description = dto.Description;

            if (Enum.TryParse(dto.Status, true, out JobStatus newStatus))
            {
                jobListing.Status = newStatus;
            }
            else
            {
                Console.WriteLine($"Invalid status provided: {dto.Status}");
                return null; // Or throw an exception for invalid status
            }

            await _context.SaveChangesAsync();

            return new JobListingResponseDTO.JobListingDto
            {
                Id = jobListing.Id,
                Title = jobListing.Title,
                Company = jobListing.Company,
                Description = jobListing.Description,
                PublishDate = jobListing.PublishDate,
                Status = jobListing.Status.ToString()
            };
        }

        public async Task<bool> DeleteJobListing(int id)
        {
            var jobListing = await _context.JobListings.FindAsync(id);
            if (jobListing == null) return false;

            _context.JobListings.Remove(jobListing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}