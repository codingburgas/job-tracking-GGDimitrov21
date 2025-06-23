using JobTracking.Application.Contracts;
using JobTracking.DataAccess.Data.Models;
using JobTracking.DataAccess.Persistance;
using JobTracking.Domain.DTOs.Response;
using Microsoft.EntityFrameworkCore;

namespace JobTracking.Application.Implementations
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsers(int page, int pageCount)
        {
            return await _context.Users
                .Skip((page - 1) * pageCount)
                .Take(pageCount)
                .ToListAsync();
        }

        public async Task<UserResponseDTO> GetUser(int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return null;

            return new UserResponseDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Username = user.Username,
                Role = user.Role
            };
        }
    }
}