namespace JobTracking.Domain.DTOs.Request.Update;

public class UpdateApplicationStatusDto
{
    public string Status { get; set; } = string.Empty; // "Submitted", "ApprovedForInterview", "Rejected"
}