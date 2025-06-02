using System.Collections.Immutable;
using JobTracking.Application.Contracts;
using JobTracking.Application.Contracts.Base;
using JobTracking.DataAccess.Data.Models;
using JobTracking.Domain.DTOs.Response;
using JobTracking.Domain.Filters;
using JobTracking.Domain.Filters.Base;
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

    public async Task<List<UserResponseDTO>> GetFilteredUsers(string? username)
    {
        var query = Provider.Db.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(username))
        {
            query = query.Where(u => u.FirstName.Contains(username));
        }
        
        var result = await query
            .Select(u => new UserResponseDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    Email = u.Email,
                    Password = u.Password //??
                }).ToListAsync();

        return result;
    }

    public async Task<IQueryable<UserResponseDTO>> GetUsers(BaseFilter<UserFilter> filter)
    {
        IQueryable<User> query = Provider.Db.Users;
        
        UserFilter? userFilter = filter.Filters;

        if (userFilter != null)
        {
            if (string.IsNullOrEmpty(userFilter.FirstName))
            {
                query = query.Where(u => u.FirstName.Contains(userFilter.FirstName));
            }

            if (string.IsNullOrEmpty(userFilter.LastName))
            {
                query = query.Where(u => u.LastName.Contains(userFilter.LastName));
            }
            
            if (userFilter.Age is not null)
            {
                query = query.Where(u => u.Age == userFilter.Age);
            }
        }
        query = query.Skip(filter.PageSize * (filter.Page - 1)).Take(filter.PageSize);

        return query.Select(u => new UserResponseDTO()
        {
            Id = u.Id,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Age = u.Age
        });
    }
}