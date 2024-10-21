using Library.Persistence.Repositories;
using Library.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Library.Core.Entities;
using Library.Core.Abstractions.IRepository;
using System.Text;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entity
{
    internal LibraryDbContext context;
    internal DbSet<TEntity> dbSet;

    public GenericRepository(LibraryDbContext context)
    {
        this.context = context;
        this.dbSet = context.Set<TEntity>();
    }

    /*public virtual async Task<IEnumerable<TEntity>> Get(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "")
    {
        IQueryable<TEntity> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
    }*/
    public virtual async Task<IEnumerable<TEntity>> Get(
    string filter = null,
    string orderBy = null,
    string includeProperties = null)
    {
        var tableName = typeof(TEntity).Name.Replace("Entity", string.Empty)+'s';
        var sql = new StringBuilder($"SELECT * FROM public.\"{tableName}\"" );

        if (!string.IsNullOrEmpty(filter))
        {
            sql.Append($" WHERE {filter}");
        }

        if (!string.IsNullOrEmpty(orderBy))
        {
            sql.Append($" ORDER BY {orderBy}");
        }

        return await dbSet.FromSqlRaw(sql.ToString()).ToListAsync();
    }


    public virtual async Task<TEntity?> GetByID(object id)
    {
        
        return await dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == (Guid)id);
    }

    public virtual async Task Insert(TEntity entity)
    {
        await dbSet.AddAsync(entity);
    }

    public virtual async Task Delete(object id)
    {
        TEntity entityToDelete = await dbSet.FindAsync(id);
        Delete(entityToDelete);
    }
    public virtual void Delete(TEntity entityToDelete)
    {
        if (context.Entry(entityToDelete).State == EntityState.Detached)
        {
            dbSet.Attach(entityToDelete);
        }
        dbSet.Remove(entityToDelete);
    }

    public virtual void Update(TEntity entityToUpdate)
    {
        dbSet.Update(entityToUpdate); 
        /*dbSet.Attach(entityToUpdate);
        context.Entry(entityToUpdate).State = EntityState.Modified;*/
    }
}
