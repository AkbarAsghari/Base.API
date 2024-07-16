using API.Infrastructure.Enums;
using API.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace API.Infrastructure.DataAcess
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly DbContext db;
        private readonly DbSet<TEntity> dbset;
        private readonly IHttpContextAccessor _accessor;
        private readonly bool softDelete;
        private bool? ignoreQueryFilters;
        private bool? asNoTracking;

        public Repository(DbContext _db, IHttpContextAccessor accessor)
        {
            db = _db;
            dbset = db.Set<TEntity>();
            _accessor = accessor;
            ignoreQueryFilters = null;
            asNoTracking = null;
            softDelete = typeof(TEntity).GetInterface(typeof(ISoftDeleteEntity).Name) != null ? true : false;
        }

        private IQueryable<TEntity> GetQuery(bool asNoTrackingByDefault, Expression<Func<TEntity, bool>> filter = null,
                                                       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                        int? start = null, int? count = null)
        {
            IQueryable<TEntity> query = dbset;

            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            if (orderBy != null)
                query = orderBy(query);

            if (start is not null)
                query = query.Skip(start.Value);

            if (count is not null)
                query = query.Take(count.Value);

            switch (asNoTracking, asNoTrackingByDefault)
            {
                case (null, true):
                case (true, true):
                case (true, false):
                    query = query.AsNoTracking();
                    break;
                case (null, false):
                case (false, true):
                case (false, false):
                    query = query.AsTracking();
                    break;
            }

            if (ignoreQueryFilters.HasValue && ignoreQueryFilters == true)
                query = query.IgnoreQueryFilters();

            asNoTracking = null;
            ignoreQueryFilters = null;

            return query;
        }

        public IRepository<TEntity> AsNoTracking()
        {
            asNoTracking = true;
            return this;
        }
        public IRepository<TEntity> AsTracking()
        {
            asNoTracking = false;
            return this;
        }
        public IRepository<TEntity> IgnoreQueryFilters()
        {
            ignoreQueryFilters = true;
            return this;
        }

        public async Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter = null,
                                                       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                        int? start = null, int? count = null)
        {
            return await GetQuery(true, filter, orderBy, include, start, count).ToListAsync();
        }

        public IQueryable<TEntity> GetEntities(Expression<Func<TEntity, bool>> filter = null,
                                                       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                        int? start = null, int? count = null)
        {
            return GetQuery(true, filter, orderBy, include, start, count);
        }

        public async Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            return await GetQuery(false, filter, include: include).SingleOrDefaultAsync(filter);
        }

        public async Task<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            return await GetQuery(false, filter, orderBy, include).FirstOrDefaultAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter,
           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null)
        {
            return await GetQuery(true, filter, include: include).AnyAsync(filter);
        }

        public bool Insert(TEntity entity)
        {
            var result = dbset.Add(entity);
            if (result.State == EntityState.Added)
                return true;
            return false;

        }

        public void InsertRange(IEnumerable<TEntity> entities)
        {
            dbset.AddRange(entities);
        }

        public bool Update(TEntity entityToUpdate, UpdatePropertyTypeEnum UpdatePropertyType = UpdatePropertyTypeEnum.Include, Expression<Func<TEntity, object>> property = null)
        {
            dbset.Attach(entityToUpdate);
            try
            {
                db.Entry<TEntity>(entityToUpdate).State = EntityState.Modified;

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(TEntity entityToDelete)
        {
            if (softDelete)
            {
                (entityToDelete as ISoftDeleteEntity)!.IsDeleted = true;
                //(entityToDelete as BaseEntity)!.UpdateDate = DateTime.UtcNow;
                return Update(entityToDelete);
            }

            var result = dbset.Remove(entityToDelete);
            if (result.State == EntityState.Deleted)
                return true;
            return false;
        }

        public bool DeleteRange(IEnumerable<TEntity> entitiesToDelete)
        {
            if (softDelete)
            {
                foreach (var entity in entitiesToDelete)
                {
                    (entity as ISoftDeleteEntity)!.IsDeleted = true;
                    //(entity as BaseEntity)!.UpdateDate = DateTime.UtcNow;
                    Update(entity);
                }
                return true;
            }

            dbset.RemoveRange(entitiesToDelete);
            return true;
        }

        public async Task<bool> InsertAsync(TEntity entity)
        {
            var result = await dbset.AddAsync(entity);
            if (result.State == EntityState.Added)
                return true;
            return false;
        }
    }
}
