using JobTracking.Domain.Filters.Base;

namespace JobTracking.Domain.Filters;

public class UserFilter : IFilter
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int? Age { get; set; }

}