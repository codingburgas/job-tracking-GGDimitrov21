using JobTracking.DataAccess.Persistance;
using Microsoft.EntityFrameworkCore;

namespace JobTracking.Application.Contracts.Base;

public class DependencyProvider
{
    public DependencyProvider(DbContext dbContext)
    {
        Db = dbContext;
    }

    public DbContext Db { get; set; }
}