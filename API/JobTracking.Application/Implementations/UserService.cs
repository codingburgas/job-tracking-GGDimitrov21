using JobTracking.Application.Contracts;
using JobTracking.Application.Contracts.Base;
using JobTracking.DataAccess.Data.Models;
using JobTracking.DataAccess.Persistence;
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

        // public async Task<UserResponseDTO> GetUser(int userId)
        // {
        //     var user = await _context.Users.FindAsync(userId);
        //
        //     if (user == null)
        //         return null;
        //
        //     return new UserResponseDTO
        //     {
        //         Id = user.Id,
        //         FirstName = user.FirstName,
        //         MiddleName = user.MiddleName,
        //         LastName = user.LastName,
        //         Username = user.Username,
        //         Role = user.Role
        //     };
        // }
        protected DependencyProvider Provider { get; set; }
        public Task<UserResponseDTO?> GetUser(int userId)
        {
            return Provider.Db.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserResponseDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    MiddleName = u.MiddleName,
                    LastName = u.LastName,
                    Username = u.Username,
                    Role = u.Role
                })
                .FirstOrDefaultAsync();
        }
        

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

    }
}