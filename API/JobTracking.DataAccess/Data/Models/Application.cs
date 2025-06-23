using JobTracking.DataAccess.Data.Base;
using JobTracking.DataAccess.Data.Models;
using JobTracking.Domain.Enums;

namespace JobTracking.DataAccess.Data.Models
{
    public class Application : IEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string? UpdatedBy { get; set; }
        public int JobListingId { get; set; }
        public int UserId { get; set; }
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;
        public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;

        // Navigation properties
        public JobListing JobListing { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}