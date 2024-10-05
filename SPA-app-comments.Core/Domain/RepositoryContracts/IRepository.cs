using SPA_app_comments.Core.Domain.Entities.Base;
using System.Linq.Expressions;

namespace SPA_app_comments.Core.Domain.RepositoryContracts
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public IEnumerable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        public TEntity? GetById(long id);
        public TEntity Insert(TEntity entity);
        public TEntity Update(TEntity entity);
        public bool Delete(long id);
        public bool Delete(TEntity entity);
    }
}
