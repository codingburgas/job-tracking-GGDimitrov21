using JobTracking.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JobTracking.DataAccess.Data.Models;
using JobTracking.DataAccess.Persistence;
using JobTracking.Domain.DTOs.Request;
using JobTracking.Domain.DTOs.Response;
using JobTracking.Domain.Enums; // For password hashing

namespace JobTracking.Application.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponseDTO.AuthResponseDto?> Register(RegisterRequestDto request)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                Console.WriteLine("Username and password are required for registration.");
                return null;
            }

            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
            {
                Console.WriteLine($"Username '{request.Username}' already exists.");
                return null;
            }

            // Hash password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var newUser = new User
            {
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Username = request.Username,
                PasswordHash = passwordHash,
                Role = UserRole.User // New registrations are always 'User' role
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            var token = GenerateJwtToken(newUser);

            return new AuthResponseDTO.AuthResponseDto
            {
                UserId = newUser.Id,
                Username = newUser.Username,
                Token = token,
                Role = newUser.Role.ToString()
            };
        }

        public async Task<AuthResponseDTO.AuthResponseDto?> Login(LoginRequestDto request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                Console.WriteLine($"Login failed for username '{request.Username}'. Invalid credentials.");
                return null; // Invalid credentials
            }

            var token = GenerateJwtToken(user);

            return new AuthResponseDTO.AuthResponseDto
            {
                UserId = user.Id,
                Username = user.Username,
                Token = token,
                Role = user.Role.ToString()
            };
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(2), // Token valid for 2 hours
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}