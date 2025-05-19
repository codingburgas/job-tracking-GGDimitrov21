using JobTracking.Application.Contracts;
using JobTracking.Application.Contracts.Base;
using JobTracking.DataAccess.Data.Models;
using JobTracking.Domain.DTOs.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace JobTracking.Application.Implementations;

public class UserService : IUserService
{
    public DependencyProvider Provider { get; set; }

    public UserService(DependencyProvider provider)
    {
        Provider = provider;
    }

    public async Task<List<User>> GetAllUsers(int page, int pageCount)
    {
        return await Provider.Db.Users
            .Skip(page - 1 * pageCount)
            .Take(pageCount)
            .ToListAsync();


    }
    
    public /*async*/ Task<UserResponseDTO> GetUser(int userId)
    {
        /*var result = (from user in Provider.Db.Users)
            where user.Id == userId
                select new UserResponseDTO()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Age = user.Age,
                        Email = user.Email,
                        Password = user.Password
                    }*/
        return /*await*/ Provider.Db.Users
            .Where(u => u.Id == userId)
            .Select(u => new UserResponseDTO()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Age = u.Age,
                Email = u.Email,
                Password = u.Password
            })
            .FirstAsync();
    }
}