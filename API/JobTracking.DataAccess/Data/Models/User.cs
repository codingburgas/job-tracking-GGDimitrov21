using JobTracking.DataAccess.Data.Base;
using JobTracking.DataAccess.Data.Models;
using JobTracking.Domain.Enums;

namespace JobTracking.DataAccess.Data.Models
{
    

    public class User : IEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Store hashed passwords
        public UserRole Role { get; set; }

        // Navigation property for applications submitted by this user
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}