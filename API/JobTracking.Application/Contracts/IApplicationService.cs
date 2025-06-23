using JobTracking.Domain.DTOs;
using JobTracking.DataAccess.Models;
using JobTracking.Domain.DTOs.Request;
using JobTracking.Domain.DTOs.Response;

namespace JobTracking.Application.Contracts
{
    public interface IApplicationService
    {
        Task<IEnumerable<ApplicationResponseDTO.ApplicationDto>> GetApplicationsForUser(int userId);
        Task<IEnumerable<ApplicationResponseDTO.ApplicationDto>> GetAllApplications(); // For admin
        Task<ApplicationResponseDTO.ApplicationDto?> SubmitApplication(int userId, CreateApplicationDto dto);
        Task<ApplicationResponseDTO.ApplicationDto?> UpdateApplicationStatus(int applicationId, UpdateApplicationStatusDto dto);
        Task<bool> HasUserApplied(int userId, int jobListingId);
    }
}