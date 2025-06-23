namespace JobTracking.Domain.DTOs.Response;

public class ApplicationResponseDTO
{
    public class ApplicationDto
    {
        public int Id { get; set; }
        public int JobListingId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime ApplicationDate { get; set; }
        public string Status { get; set; } = string.Empty; // String representation of ApplicationStatus
    }
}