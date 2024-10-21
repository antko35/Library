using System.Linq.Expressions;

namespace Library.Core.Abstractions.IRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task Delete(object id);
        void Delete(TEntity entityToDelete);
        //Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "");
        Task<IEnumerable<TEntity>> Get(string filter = null, string orderBy = null, string includeProperties = null);
        Task<TEntity?> GetByID(object id);
        Task Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
    }
}