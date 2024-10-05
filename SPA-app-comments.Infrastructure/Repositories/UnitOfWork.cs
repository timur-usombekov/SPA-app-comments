using Microsoft.EntityFrameworkCore;
using SPA_app_comments.Core.Domain.RepositoryContracts;

namespace SPA_app_comments.Infrastructure.Repositories
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        private bool _disposed = false;
        private readonly Dictionary<Type, object> _repositories = new();
        public TContext DbContext { get; }

        ~UnitOfWork() =>
            Dispose(false);
        public UnitOfWork(TContext context) =>
            DbContext = context;

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);

            if (!_repositories.ContainsKey(type))
                _repositories[type] = new Repository<TEntity>(DbContext);

            return (Repository<TEntity>)_repositories[type];
        }
        public async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _repositories.Clear();
                    DbContext.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
