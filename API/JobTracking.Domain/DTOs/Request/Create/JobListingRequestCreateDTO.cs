namespace JobTracking.Domain.DTOs.Request.Create;
    
public class CreateJobListingDto
{
    public string Title { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

