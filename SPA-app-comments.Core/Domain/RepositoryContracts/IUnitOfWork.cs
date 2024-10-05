using Microsoft.EntityFrameworkCore;

namespace SPA_app_comments.Core.Domain.RepositoryContracts
{
    public interface IUnitOfWork<out TContext> : IDisposable where TContext : DbContext
    {
        TContext DbContext { get; }
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync();
    }
}
