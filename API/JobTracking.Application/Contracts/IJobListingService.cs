using JobTracking.Domain.DTOs;
using JobTracking.DataAccess.Models;
using JobTracking.Domain.DTOs.Request;
using JobTracking.Domain.DTOs.Response;

namespace JobTracking.Application.Contracts
{
    public interface IJobListingService
    {
        Task<IEnumerable<JobListingResponseDTO.JobListingDto>> GetAllJobListings();
        Task<JobListingResponseDTO.JobListingDto?> GetJobListingById(int id);
        Task<JobListingResponseDTO.JobListingDto?> CreateJobListing(CreateJobListingDto dto);
        Task<JobListingResponseDTO.JobListingDto?> UpdateJobListing(int id, UpdateJobListingDto dto);
        Task<bool> DeleteJobListing(int id);
    }
}