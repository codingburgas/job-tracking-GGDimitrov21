using JobTracking.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace JobTracking.Application.Contracts.Base;

public class DependencyProvider
{
    public DependencyProvider(AppDbContext appDbContext)
    {
        Db = appDbContext;
    }

    public AppDbContext Db { get; set; }
}