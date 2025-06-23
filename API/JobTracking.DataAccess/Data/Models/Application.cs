namespace JobTracking.DataAccess.Models
{
    public enum ApplicationStatus
    {
        Submitted,
        ApprovedForInterview,
        Rejected
    }

    public class Application
    {
        public int Id { get; set; }
        public int JobListingId { get; set; }
        public int UserId { get; set; }
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;

        // Navigation properties
        public JobListing JobListing { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}