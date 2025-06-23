namespace JobTracking.DataAccess.Models
{
    public enum UserRole
    {
        User,
        Admin
    }

    public class User
    {
        public int Id { get; set; }
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