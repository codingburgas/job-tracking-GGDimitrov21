using JobTracking.DataAccess.Persistance;

namespace JobTracking.Application.Contracts.Base;

public class DependencyProvider
{
    public DependencyProvider(AppDbContext dbContext)
    {
        Db = dbContext;
    }

    public AppDbContext Db { get; set; }
}