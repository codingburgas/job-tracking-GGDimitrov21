namespace JobTracking.Domain.DTOs.Request;
    
public class CreateJobListingDto
{
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class UpdateJobListingDto
{
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // "Active" or "Inactive"
}