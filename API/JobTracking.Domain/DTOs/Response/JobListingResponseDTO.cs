namespace JobTracking.Domain.DTOs.Response;

public class JobListingResponseDTO
{
    public class JobListingDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime PublishDate { get; set; }
        public string Status { get; set; } = string.Empty; // String representation of JobStatus
    }
}