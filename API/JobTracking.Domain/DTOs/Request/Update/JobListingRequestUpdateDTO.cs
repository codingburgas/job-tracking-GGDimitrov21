namespace JobTracking.Domain.DTOs.Request.Update;

public class UpdateJobListingDto
{
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // "Active" or "Inactive"
}