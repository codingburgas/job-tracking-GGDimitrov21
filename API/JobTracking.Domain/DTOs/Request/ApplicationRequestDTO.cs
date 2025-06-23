namespace JobTracking.Domain.DTOs.Request;

public class CreateApplicationDto
{
    public int JobListingId { get; set; }
}

public class UpdateApplicationStatusDto
{
    public string Status { get; set; } = string.Empty; // "Submitted", "ApprovedForInterview", "Rejected"
}