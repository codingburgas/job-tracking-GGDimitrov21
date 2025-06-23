using JobTracking.Domain.Enums;
using JobTracking.Domain.DTOs.Response;

namespace JobTracking.Domain.DTOs.Request.Create
{
    public class UserRequestDTO
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }

        // Factory method for creating a new user
        public static UserRequestDTO CreateUserDto(UserResponseDTO response)
        {
            return new UserRequestDTO
            {
                FirstName = response.FirstName,
                MiddleName = response.MiddleName,
                LastName = response.LastName,
                Age = response.Age,
                Email = response.Email,
                Password = response.Password,
                Username = response.Username,
                Role = response.Role
            };
        }

        
    }
}