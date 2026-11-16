using Microsoft.EntityFrameworkCore;

namespace FxferService.Infrastructure.Persistence.Batch;

public interface IDbContextBase
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;

    int SaveChanges();
}
