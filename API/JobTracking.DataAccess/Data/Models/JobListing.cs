namespace JobTracking.DataAccess.Models
{
    public enum JobStatus
    {
        Active,
        Inactive
    }

    public class JobListing
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PublishDate { get; set; } = DateTime.UtcNow;
        public JobStatus Status { get; set; } = JobStatus.Active;

        // Navigation property for applications submitted for this job listing
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}