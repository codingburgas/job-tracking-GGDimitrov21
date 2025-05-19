using JobTracking.Application.Implementations;
using JobTracking.DataAccess.Data.Models;
using JobTracking.Domain.DTOs.Response;

namespace JobTracking.Application.Contracts;

public interface IUserService
{
    public Task<List<User>> GetAllUsers(int page, int pageCount);
    public Task<UserResponseDTO> GetUser(int userId);
}