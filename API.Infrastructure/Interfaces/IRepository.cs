using API.Infrastructure.Enums;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace API.Infrastructure.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter = null,
                                                      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                       int? start = null, int? count = null);
        IQueryable<TEntity> GetEntities(Expression<Func<TEntity, bool>> filter = null,
                                                      Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                       int? start = null, int? count = null);
        Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> filter = null,
                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

        bool Insert(TEntity entity);
        Task<bool> InsertAsync(TEntity entity);

        void InsertRange(IEnumerable<TEntity> entities);

        bool Update(TEntity entityToUpdate, UpdatePropertyTypeEnum UpdatePropertyType = UpdatePropertyTypeEnum.Include, Expression<Func<TEntity, object>> property = null);

        IRepository<TEntity> IgnoreQueryFilters();

        IRepository<TEntity> AsNoTracking();

        IRepository<TEntity> AsTracking();

        bool DeleteRange(IEnumerable<TEntity> entitiesToDelete);
        bool Delete(TEntity entityToDelete);
    }
}
