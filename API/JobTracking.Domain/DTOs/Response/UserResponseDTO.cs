using JobTracking.Domain.Enums;

namespace JobTracking.Domain.DTOs.Response;

public class UserResponseDTO
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string MiddleName { get; set; }
    public string Username { get; set; }
    public UserRole Role { get; set; }
}